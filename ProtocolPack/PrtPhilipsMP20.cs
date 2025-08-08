using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using GlobalClass;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
//using ViewPack;
using System.Threading;
using System.Data.SQLite;
using System.Linq;
using ProtocolPack.IntelliVue;

//----------------------------------------------------------------------------------------------
//PhilipsMP20 Protocol
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class PrtPhilipsMP20 : PrtServerBase
    {
        //----------------------------------------------------------------------------------------------
        // 直连模式
        private static UdpClient Udp = null;
        private static List<PrtPhilipsMP20> ALL = new List<PrtPhilipsMP20>();
        private static Thread ThrUdp; //接收线程
        
        // 级联模式
        /// <summary>
        /// 是否级联模式
        /// </summary>
        public bool CascadeMode { get; private set; }
        /// <summary>
        /// 通信SOCKET对象
        /// </summary>
        public override Socket TCP { get; protected set; } = null;
        
        private CircularBuffer ReceivedBuffer = new CircularBuffer(10 * 1024);

        private static IPEndPoint
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0); //Any IP any port from remote point

        private static DateTime LastTime = DateTime.Now;
        
        private Dictionary<string, string> KeyGroupNames;
        internal short[] NIBP_Para = new short[3] { 0, 0, 0 };
        internal short[] ART_Para = new short[3] { 0, 0, 0 };

        private void InitMeasureKeyGroupNames()
        {
            KeyGroupNames = new Dictionary<string, string>();
            KeyGroupNames.Add("NOM_TEMP","T1");
            KeyGroupNames.Add("NOM_ECG_CARD_BEAT_RATE","HR");
            KeyGroupNames.Add("NOM_PULS_OXIM_SAT_O2", "SpO2"); //血氧
            KeyGroupNames.Add("NOM_PLETH_PULS_RATE", "PR"); //脉搏
            KeyGroupNames.Add("NOM_PRESS_BLD_NONINV_SYS", "Sys"); // 无创血压（收缩压）
            KeyGroupNames.Add("NOM_PRESS_BLD_NONINV_DIA", "Dia"); // 无创血压（舒张压）
            KeyGroupNames.Add("NOM_PRESS_BLD_NONINV_MEAN", "Mean"); 
            KeyGroupNames.Add("NOM_PRESS_BLD_ART_SYS", "ART_Sys"); //动脉高血压（收缩压） 
            KeyGroupNames.Add("NOM_PRESS_BLD_ART_MEAN", "ART_Mean"); //无创平均压
            KeyGroupNames.Add("NOM_PRESS_BLD_ART_DIA", "ART_Dia");
        }

        //事件定义
        public delegate void OnGetParamater(int GroupID, String GroupName, short[] Values);
        
        //----------------------------------------------------------------------------------------------
        public static void Initialize()
        {
            if (Udp == null)
            {
                Udp = new UdpClient(24005);    
            }
            ThrUdp = new Thread(UdpRun);
            ThrUdp.Start();
        }

        //----------------------------------------------------------------------------------------------
        private static void UdpRun()
        {
            //接收数据
            while (true)
            {
                //接收数据
                if (Udp.Client.Available > 0)
                {
                    Log.d(DateTime.Now + " ---- 收到数据" + Udp.Available.ToString());
                    Byte[] RBuf = Udp.Receive(ref RemoteIpEndPoint);
                    //保存数据，用于模拟（测试）
                    /*
                    DBConnect Conn = DBConnect.SYS;
                    Conn.BeginTransaction();
                    Conn.AddPara("@dTime", DateTime.Now);
                    Conn.AddPara("@Data", RBuf);
                    Conn.AddPara("@Len", RBuf.Length);
                    Conn.ExecSQL("Insert into Philips (dTime, Data, Len) Values(@dTime, @Data, @Len)");
                    Conn.Commit();
                    */
                    //int ID = (int)(((RemoteIpEndPoint.Address.Address & 0xFF000000) >> 24) - MIN_ID);
                    //if (ID >= ALL.Count) continue;
                    //ALL[ID].OnReceived(RBuf, RBuf.Length);
                    PrtPhilipsMP20 Item = GetItemByEndPoint(RemoteIpEndPoint);
                    Item.OnReceived(RBuf);
                    //ALL[0].OnReceived(RemoteIpEndPoint, RBuf, RBuf.Length);
                }

                //发送获取数据请求
                if (GLB.PassMs(LastTime) >= 10000)
                {
                    LastTime = DateTime.Now;
                    for (int i = 0; i < ALL.Count; i++)
                        ALL[i].GetDataTick();
                }

                //----检查连接状态
                for (int i = 0; i < ALL.Count; i++)
                    ALL[i].CheckConnect();
                //
                Thread.Sleep(100);
            }
        }

        //----------------------------------------------------------------------------------------------
        private static PrtPhilipsMP20 GetItemByEndPoint(IPEndPoint RemoteIpEndPoint)
        {
            for (int i = 0; i < ALL.Count; i++)
                if (GLB.Same(ALL[i].IP, RemoteIpEndPoint.Address.ToString()))
                    return ALL[i];
            return new PrtPhilipsMP20(RemoteIpEndPoint.Address.MapToIPv4().ToString());
        }

        //----------------------------------------------------------------------------------------------

        private enum ConnectState
        {
            OffLine,
            Connect
        };

        private ConnectState State = ConnectState.OffLine;
        private String IP = "LocalHost";
        private DateTime LastRecTime = DateTime.Now.AddMinutes(-3);


        //----------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ipv4">设备地址</param>
        public PrtPhilipsMP20(string Ipv4)
            : base(0)
        {
            ALL.Add(this);
            IP = Ipv4;
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(Ipv4), 24105);
            IP4 = IP;
            InitMeasureKeyGroupNames();
            Initialize();
        }

        public PrtPhilipsMP20(string IpAddr, bool CascadeSwitch, string CascadeAddr, int CascadePort) : base(0)
        {
            ALL.Add(this);
            IP = IpAddr;
            RemoteIpEndPoint = CascadeSwitch ?new IPEndPoint(IPAddress.Parse(CascadeAddr), CascadePort) : new IPEndPoint(IPAddress.Parse(IpAddr), 24105);
            CascadeMode = CascadeSwitch;
            IP4 = IP;
            InitMeasureKeyGroupNames();
            if (CascadeSwitch)
            {
                InitializeCascade();
            }
            else
            {
                Initialize();
            }
        }

        private void InitializeCascade()
        {
            State = ConnectState.OffLine;
            Thread.Sleep(60);
            Log.d(string.Format("连接级联平台：{0}:{1}", RemoteIpEndPoint.Address, RemoteIpEndPoint.Port));
            IPAddress address = RemoteIpEndPoint.Address;
            if (TCP != null)
            {
                TCP.Close();
            }
            TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 设置接收超时时间
            TCP.ReceiveTimeout = 10000;
            TCP.LingerState = new LingerOption(true, 0);
            TCP.BeginConnect(address, RemoteIpEndPoint.Port, new AsyncCallback(Connected), null);
        }
        /// <summary>
        /// 连接建立完成
        /// </summary>
        /// <param name="ar"></param>
        public void Connected(IAsyncResult ar)
        {
            try
            {
                TCP.EndConnect(ar);
                if (this.onConnected != null) this.onConnected();
                Log.d(string.Format("连接上级平台{0}成功", TCP.RemoteEndPoint));
                State = ConnectState.Connect;

                // 连接建立完毕，创建数据处理线程，开始处理数据
                StateObject state = new StateObject();
                state.Worker = this;
                TCP.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceivedCallback), state);
            }
            catch (SocketException sex)
            {
                Log.d(string.Format("套接字错误：code: {0}, message: {1}", sex.ErrorCode, sex.Message));
                State = ConnectState.OffLine;
                InitializeCascade();
            }
            catch (Exception ex)
            {
                Log.d("连接失败：" + ex.Message);
                Log.d("重新连接");
                State = ConnectState.OffLine;
                InitializeCascade();
            }
        }
        /// <summary>
        /// 数据接收到的处理
        /// </summary>
        /// <param name="ar"></param>
        public static void ReceivedCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            PrtPhilipsMP20 worker = state.Worker as PrtPhilipsMP20;

            try
            {
                if (worker == null || worker.TCP == null) return;

                int bytesReceived = worker.TCP.EndReceive(ar);
                if (bytesReceived > 0)
                {
                    worker.Process(state.Buffer, bytesReceived);
                    worker.TCP.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceivedCallback), state);

                }
                else
                {
                    Log.d(string.Format("级联服务远端关闭了数据连接"));
                    worker.State = ConnectState.OffLine;
                    worker.InitializeCascade();
                }
            }
            catch(ObjectDisposedException)
            {
                Log.d(string.Format("连接已关闭"));
                worker.State = ConnectState.OffLine;
                worker.InitializeCascade();
            }
            catch(SocketException sex)
            {
                Log.d(string.Format("套接字错误：code: {0}, message: {1}", sex.ErrorCode, sex.Message));
                worker.State = ConnectState.OffLine;
                worker.InitializeCascade();
            }
            catch(Exception ex)
            {
                Log.d(string.Format("未知错误：{0}", ex.Message));
                Log.d(string.Format("错误堆栈：{0}", ex.StackTrace));
                // Log.d(string.Format("当前报文明文：{0}", Encoding.GetEncoding("GB2312").GetString(state.Buffer)));
                worker.State = ConnectState.OffLine;
                worker.InitializeCascade();
            }
        }
        
        /// <summary>
        /// 处理协议数据包 该数据包经过封装,格式为: [0x02,ip...,0x1F,DATA,0x03,0x04]
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private void Process(byte[] buffer, int count)
        {
            byte STX = 0x02;
            byte ETX = 0x03;
            byte EOT = 0x04;
            byte US = 0x1F;

            int iStart = 0;
            int iEnd = 0;

            //将接受到的数据放入缓存区
            this.ReceivedBuffer.WriteBuffer(buffer, 0, count);
            // 提取数据包
            for(int i = 0; i < this.ReceivedBuffer.DataCount; i++)
            {
                if (this.ReceivedBuffer[i] == STX)
                {
                    iStart = i + 1;
                    continue;
                }

                if (this.ReceivedBuffer[i] == EOT)
                {
                    if (i == 0) continue;
                    if (this.ReceivedBuffer[i -1] == ETX)
                    {
                        iEnd = i;
                        var package = new byte[iEnd+1];
                        this.ReceivedBuffer.ReadBuffer(package, 0, iEnd+1);
                        var index = Array.IndexOf(package,US);
                        byte[] ipPart = new byte[index-1];
                        

                        // ip
                        Array.Copy(package, 1, ipPart, 0, index-1);
                        var ip = Encoding.ASCII.GetString(ipPart);
                        if (!IP4.Trim().Equals(ip)) continue;  // 与设备IP不匹配的数据包直接丢弃
                        // data
                        byte[] dataPart = new byte[package.Length - index - 3];
                        Array.Copy(package, index+1, dataPart, 0, package.Length - index-3);
                        AnalysisDataExportProtocol(dataPart);
                    }

                }
            }
            // 清除处理过的数据
            this.ReceivedBuffer.Clear(iEnd+1);

        }
        
        

        //----------------------------------------------------------------------------------------------
        public void GetDataTick()
        {
            if (State == ConnectState.Connect)
                Udp.Send(FramePhilipsMP20.EXTENDED_POLL_DATA_REQUEST,
                    FramePhilipsMP20.EXTENDED_POLL_DATA_REQUEST.Length, IP, 24105);
        }

        //----------------------------------------------------------------------------------------------
        private void OnReceived(Byte[] Buf)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Buf.Length; i++)
            {
                sb.Append(Buf[i].ToString("x")+" ");
            }
            Log.d("IP=" + IP + "，接收到数据:"+sb);
            
            if (Buf[0] == 0x00 && Buf[1] == 0x00 && Buf[2] == 0x01 && Buf[3] == 0x00)
            {
                //CONNECT INDICATION EVENT 接收到连接广播包
                Log.d("IP=" + IP + "，接收到连接广播包");
                if (State == ConnectState.OffLine)
                {
                    Log.d("IP=" + IP4 + "开始建立逻辑连接 ");
                    Udp.Send(FramePhilipsMP20.AssocReq, FramePhilipsMP20.AssocReq.Length, IP, 24105);
                }

                return;
            }

            // if (Buf[0] == 0x0E)
            // {
            //     //接收到连接回复信息，发送确认连接包
            //     Log.d("IP=" + IP + "，接收到连接回复信息");
            //     Udp.Send(FramePhilipsMP20.MdsResult, FramePhilipsMP20.MdsResult.Length, IP, 24105);
            //     return;
            // }

            if (Buf[0] == 0xE1)
            {
                AnalysisDataExportProtocol(Buf);
                // onGetParaGroup("NIBP", new short[] { 110, 86, 95});
            }
        }
        
        /***
         * 数据导出协议解析
         */
        private void AnalysisDataExportProtocol(byte[] bytes)
        {
            var headerBytes = bytes.Take(14).ToArray();
            var header = ProtocolHelper.Bytes2Structure<RemoteHeader>(headerBytes);
            var dataBytes = bytes.Skip(14).ToArray();
            // 包长度匹配判断
            var roiVapduLen = header.RoiVapdu.length.ToHostOrderShort();
            if (header.ROapdus.ro_type.ToHostOrderShort() != 0x05 && roiVapduLen != dataBytes.Length)
            {
                Log.d("接收到飞利浦"+IP+"消息,报文长度异常");
                return;
            }

            // MDS CREATE EVENT
            if (header.ROapdus.ro_type.ToHostOrderShort() == 0x01 &&
                header.RoiVapdu.command_type.ToHostOrderShort() == 0x01)
            {
                Log.d("接收到飞利浦"+IP+"MDS CREATE EVENT消息");
                onConnected();
                LastRecTime = DateTime.Now;
                // SEND MDS CREATE EVENT RESULT   
                var eventReportBytes = dataBytes.Take(14).ToArray();
                var eventReportArgument = ProtocolHelper.Bytes2Structure<EventReportResultArgument>(eventReportBytes);
                var mdsCreateEventResult = GetMdsCreateEventResult(header, eventReportArgument);
                Udp.Send(mdsCreateEventResult, mdsCreateEventResult.Length, IP, 24105);
            }
            // EXTENDED POLL DATA REQUEST
            else if (header.ROapdus.ro_type.ToHostOrderShort() == 0x02 &&
                     header.RoiVapdu.command_type.ToHostOrderShort() == 0x07)
            {
                Log.d("接收到飞利浦"+IP+"EXTENDED POLL DATA Result消息");

                // 数据解析
                LastRecTime = DateTime.Now;
                HandlerPollMdibDataReply(dataBytes);
            }
            // SINGLE POLL DATA RESULT (LINKED)
            else if (header.ROapdus.ro_type.ToHostOrderShort() == 0x05)
            {
                Log.d("接收到飞利浦"+IP+"SINGLE POLL DATA RESULT (LINKED)消息");
                LastRecTime = DateTime.Now;
                var dataBytesPack = bytes.Skip(16).ToArray();
                HandlerPollMdibDataReply(dataBytesPack);
            }
            else
            {
                Log.d("接收到飞利浦"+IP+"其他消息!");
            }
        }

        /**
        * 获取MdsCreateEventResult
        */
        private byte[] GetMdsCreateEventResult(RemoteHeader reqHeader, EventReportResultArgument eventReportArgument)
        {
            // header设置
            RemoteHeader header = new RemoteHeader();
            header.InitFixedValue();
            header.ROapdus.ro_type = new byte[] { 0x00, 0x02 };
            header.ROapdus.length = new byte[] { 0x00, 0x14 };
            header.RoiVapdu.invoke_id = reqHeader.RoiVapdu.invoke_id;
            header.RoiVapdu.command_type = new byte[] { 0x00, 0x01 };
            header.RoiVapdu.length = new byte[] { 0x00, 0x0e };

            // result设置,长度14
            EventReportResultArgument reportResult = new EventReportResultArgument();
            reportResult.managed_object = eventReportArgument.managed_object;
            var respTime = eventReportArgument.event_time.ToHostOrderInt() + 200 * 8;
            reportResult.event_time = IntToByteArr(respTime); //未取到开机时间，此处使用report event_time+200ms
            reportResult.event_type = new byte[] { 0x0d, 0x06 };
            reportResult.length = new byte[] { 0x00, 0x00 };

            MDSCreateEventResult result = new MDSCreateEventResult();
            result.header = header;
            result.eventReportResult = reportResult;


            return ProtocolHelper.Structure2Byte(result);
        }
        
        private void HandlerPollMdibDataReply(byte[] dataBytes)
        {
            // skip ActionResult
            var totalReplyPack = dataBytes.Skip(10).ToArray();
            // take pollMdibDataReply bytes
            var pollMdibDataReply = totalReplyPack.Take(26).ToArray();
            var mdibDataReply = ProtocolHelper.Bytes2Structure<PollMdibDataReply>(pollMdibDataReply);


            // skip pollMdibDataReply
            var totalSinglePack = totalReplyPack.Skip(26).ToArray();
            if (mdibDataReply.length.ToHostOrderShort() != totalSinglePack.Length)
            {
                Log.d("PollMdibDataReply包异常,SingleContextPoll长度不符合!");
                return;
            }

            // SingleContextPoll
            for (int i = 0, cpIndex = 0; i < mdibDataReply.count.ToHostOrderShort(); i++)
            {
                var singlePollItemPack = totalSinglePack.Skip(cpIndex).Take(6).ToArray();
                var singleContextPoll = ProtocolHelper.Bytes2Structure<SingleContextPoll>(singlePollItemPack);
                // ObservationPoll
                var totalObservationPollPack =
                    totalSinglePack.Skip(cpIndex + 6).Take(singleContextPoll.length.ToHostOrderShort()).ToArray();
                for (int j = 0, opIndex = 0; j < singleContextPoll.count.ToHostOrderShort(); j++)
                {
                    var observationPollPack = totalObservationPollPack.Skip(opIndex).Take(6).ToArray();
                    var observationPoll = ProtocolHelper.Bytes2Structure<ObservationPoll>(observationPollPack);
                    // AVAType
                    var totalAvaTypePack =
                        totalObservationPollPack.Skip(opIndex + 6).Take(observationPoll.length.ToHostOrderShort())
                            .ToArray();
                    
                    for (int k = 0, atIndex = 0; k < observationPoll.count.ToHostOrderShort(); k++)
                    {
                        var avaTypePack = totalAvaTypePack.Skip(atIndex).Take(4).ToArray();
                        var avaType = ProtocolHelper.Bytes2Structure<AVAType>(avaTypePack);
                        var valueBytes = totalAvaTypePack.Skip(atIndex + 4).Take(avaType.length.ToHostOrderShort())
                            .ToArray();
                        // 参数名和值处理
                        var attribute_id = avaType.attribute_id.ToHostOrderShort();
                        //Numeric Observed Value
                        if (attribute_id == 0x0950)
                        {
                            GetObsValue(valueBytes);
                        }
                        // Compound Numeric Observed Value
                        else if (attribute_id == 0x094b)
                        {
                            var cmpArr = valueBytes.Take(4).ToArray();
                            var nuObsValueCmp = ProtocolHelper.Bytes2Structure<NuObsValueCmp>(cmpArr);
                            var nuObsValueBytes = valueBytes.Skip(4).ToArray();
                            for (int l = 0, obsIndex = 0; l < nuObsValueCmp.count.ToHostOrderShort(); l++)
                            {
                                var obsBytes = nuObsValueBytes.Skip(obsIndex).Take(10).ToArray();
                                GetObsValue(obsBytes);
                                obsIndex += 10;
                            }
                        }
                        atIndex = atIndex + 4 + avaType.length.ToHostOrderShort();
                    }

                    opIndex = opIndex + 6 + observationPoll.length.ToHostOrderShort();
                }

                cpIndex = cpIndex + 6 + singleContextPoll.length.ToHostOrderShort();
            }
        }
        
        private void GetObsValue(byte[] valueBytes)
        {
            var nuObsVal = ProtocolHelper.Bytes2Structure<NuObsValue>(valueBytes);
            var physio_id = nuObsVal.physio_id.ToHostOrderShort();
            if (PhysiologicalIdentifier.physiologicalParam.ContainsKey(physio_id))
            {
                var paramName = PhysiologicalIdentifier.physiologicalParam[physio_id];
                var mantissa = nuObsVal.GeteMantissaValue();
                // 无效值
                if (mantissa == 0x7fffff || mantissa == 0x800000 || mantissa == 0x7ffffe || mantissa == 0x800002)
                {
                    return;
                }

                var paramValue = mantissa * Math.Pow(10, nuObsVal.exponent);
                short result;
                if (paramValue >= short.MinValue && paramValue <= short.MaxValue)
                {
                    result = (short)paramValue;
                }
                else
                {
                    // 处理溢出情况
                    Log.d("飞利浦参数["+paramName+"]值溢出 short 类型的范围,值为:"+paramValue);
                    result = 0;
                }
                //显示参数值
                if (KeyGroupNames.TryGetValue(paramName, out var ob))
                {
                    if (ob.ToUpper().Equals("SYS"))
                    {
                        NIBP_Para[0] = result;
                        onGetParaGroup("NIBP", NIBP_Para);
                    }
                    else if (ob.ToUpper().Equals("DIA"))
                    {
                        NIBP_Para[1] = result;
                        onGetParaGroup("NIBP", NIBP_Para);
                    }
                    else if (ob.ToUpper().Equals("MEAN"))
                    {
                        NIBP_Para[2] = result;
                        onGetParaGroup("NIBP", NIBP_Para);
                    }
                    else if ((ob.ToUpper().Equals("ART_SYS")))
                    {
                        ART_Para[0] = result;
                        onGetParaGroup("ABP", ART_Para);
                    }
                    else if ((ob.ToUpper().Equals("ART_MEAN")))
                    {
                        ART_Para[2] = result;
                        onGetParaGroup("ABP", ART_Para);
                    }
                    else if ((ob.ToUpper().Equals("ART_DIA")))
                    {
                        ART_Para[1] = result;
                        onGetParaGroup("ABP", ART_Para);
                    }
                    else
                    {
                        onGetParaGroup(ob, new [] { result });
                    }
                }
            }
            else
            {
                Log.d("{physio_id} is not exit PhysiologicalIdentifier", physio_id.ToString());
            }
        }
        
        private byte[] IntToByteArr(int val)
        {
            byte[] result = new byte[4];
            result[3] = (byte)(val & 0xff);
            result[2] = (byte)((val & 0xff00) >> 8);
            result[1] = (byte)((val & 0xff0000) >> 16);
            result[0] = (byte)((val & 0xff000000) >> 24);
            return result;
        }
        
        private void CheckConnect()
        {
            if (GLB.PassMs(LastRecTime) >= 120 * 1000)
            {
                State = ConnectState.OffLine;
                if (OnDisConnected != null)
                {
                    OnDisConnected(null);
                }
                
            }
            else
            {
                State = ConnectState.Connect;
                onConnected();
            }
        }
    }
}
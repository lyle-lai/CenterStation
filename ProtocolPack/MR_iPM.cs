using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using GlobalClass;
using System.Diagnostics;

namespace ProtocolPack
{
    /// <summary>
    /// 迈瑞iPM10监护仪通讯协议
    /// </summary>
    public class MR_iPM : PrtServerBase
    {
        #region 静态变量
        public static byte Head = (byte)11;
        public static byte[] Over = new byte[2]
        {
            (byte)28,
            (byte)13
        };
        public static List<KeyValuePair<string, string>> KeyGroupName;

        static MR_iPM()
        {
            KeyGroupName = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("173^NIBP_PR", "脉率");//无创血压脉率
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("101^HR", "HR");//心电心率
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("102^PVCs", "PVCs");//PVC
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("105^ST-I", "ST-I");//PVC
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("106^ST-II", "ST-II");//PVC
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("107^ST-III", "ST-III");//PVC
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("108^ST-aVR", "ST-aVR");//PVC
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("151^RR", "RR");//呼吸频率
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("160^SpO2", "SpO2");//血氧
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("161^PR", "PR");//脉搏
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("161^PI", "PI");//血流灌注指数、灌流指标
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("170^Sys", "Sys");//无创高血压（收缩压）SBP
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("171^Dia", "Dia");//无创低血压（舒张压）
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("172^Mean", "Mean");//无创平均压
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("173^NIBP_PR", "NIBP_PR");//无创血压 脉率
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("200^T1 ", "T1");//第一通道温度T1
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("201^T2", "T2");//第一通道温度T2
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("202^TD", "TD");//两个通道的温度差TD
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("250^EtCO2", "ETCO2");//EtCO2
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("254^FiO2", "FIO2");//FiO2
            KeyGroupName.Add(keyValuePair);

            //keyValuePair = new KeyValuePair<string, string>("500^Sys", "动脉收缩压");//动脉高血压（收缩压）
            keyValuePair = new KeyValuePair<string, string>("500^Sys", "ART_Sys");//动脉高血压（收缩压）
            KeyGroupName.Add(keyValuePair);

            //keyValuePair = new KeyValuePair<string, string>("501^Mean", "动脉平均压");//无创平均压
            keyValuePair = new KeyValuePair<string, string>("501^Mean", "ART_Mean");//无创平均压
            KeyGroupName.Add(keyValuePair);

            //keyValuePair = new KeyValuePair<string, string>("502^Dia", "动脉舒张压");//动脉低血压（舒张压）
            keyValuePair = new KeyValuePair<string, string>("502^Dia", "ART_Dia");//动脉低血压（舒张压）
            KeyGroupName.Add(keyValuePair);

            keyValuePair = new KeyValuePair<string, string>("566^Mean", "IBP_Mean");//中心静脉压
            KeyGroupName.Add(keyValuePair);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 监护仪设备IP
        /// </summary>
        public string DeviceIP { get; set; }
        /// <summary>
        /// 监护仪设备端口
        /// </summary>
        public int DevicePort { get; set; }
        #endregion

        #region 私有变量
        /// <summary>
        /// 无创血压的参数，由于当前的ViewParaGroup对象无法对单个血压参数进行绘图设置
        /// 所以这里定义一个静态的无创血压参数数组，数组的大小固定为3，第一个元素表示
        /// 高压，第二参数表示低压，第三个参数表示平均压
        /// </summary>
        internal short[] NIBP_Para = new short[3] { 0, 0, 0 };

        internal short[] ART_Para = new short[3] { 0, 0, 0 };
        /// <summary>
        /// 是否工作
        /// </summary>
        private bool isRunning;
        /// <summary>
        /// 心跳线程
        /// </summary>
        private Thread HeartBeatThread;

        /// <summary>
        /// Socket连接对象
        /// </summary>
        private Socket TCP;

        /// <summary>
        /// 接收缓存区
        /// </summary>
        private byte[] ReceivedBuffer = new byte[10240];
        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public MR_iPM(string ip, int port)
            : base(0)
        {
            this.IP4 = ip;
            this.DeviceIP = ip;
            this.DevicePort = port;

            Connect();


        }

        /// <summary>
        /// 连接监护仪
        /// </summary>
        public void Connect()
        {
            IPAddress address = IPAddress.Parse(DeviceIP);
            if (null != TCP)
            {
                TCP.Close();
                TCP.Dispose();
            }

            TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 设置接收超时时间
            TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
            TCP.BeginConnect(address, DevicePort, new AsyncCallback(this.Connected), (object)null);

        }

        /// <summary>
        /// 链接建立完成
        /// </summary>
        /// <param name="ar"></param>
        public void Connected(IAsyncResult ar)
        {
            try
            {
                TCP.EndConnect(ar);
                if (this.onConnected != null) this.onConnected();
                Log.d(string.Format("连接监护仪{0}成功", this.DeviceIP));
                State = ConnectState.Connect;

                // 连接建立完毕，创建数据处理线程并开始处理数据
                this.isRunning = true;
                this.ReceiveAsync();

                // 心跳启动
                if (null == HeartBeatThread)
                {
                    HeartBeatThread = new Thread(new ThreadStart(this.HeartBeat));
                    HeartBeatThread.Start();
                }
            }
            catch (Exception ex)
            {
                Log.d("连接失败：" + ex.Message);
                Log.d("重新连接中...");
                Connect();
            }
        }

        /// <summary>
        /// 开始异步接收
        /// </summary>
        public void ReceiveAsync()
        {
            if (null == TCP) return;
            try
            {
                StateObject state = new StateObject();
                state.Worker = this;
                TCP.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception ex)
            {
                Log.d("接收异常：" + ex.Message);
                Connect();
            }
        }

        /// <summary>
        /// 静态方法，处理TCP发送回调
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            MR_iPM worker = state.Worker as MR_iPM;
            try
            {
                int bytesReceived = worker.TCP.EndReceive(ar);
                if (bytesReceived > 0)
                {
                    worker.Process(state.Buffer, bytesReceived);
                    worker.TCP.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    Log.d("监护仪关闭了数据连接");
                    worker.State = ConnectState.OffLine;
                    worker.Connect();
                }
            }
            catch (Exception ex)
            {
                Log.d("接收错误：" + ex.Message);
                Log.d("重新建立连接");
                worker.State = ConnectState.OffLine;
                worker.Connect();
            }
        }

        /// <summary>
        /// 处理接收到的数据包
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private bool Process(byte[] buffer, int count)
        {
            try
            {

                byte SP = (byte)'\v';
                byte SM = (byte)'\r';

                int iStart = 0;
                int iEnd = 0;
                Array.Copy((Array)buffer, 0, (Array)this.ReceivedBuffer, 0, count);
                // 迈瑞监护仪中的ZMW波形数据是二进制数据包，再使用System.Text.Encoding将字节转成字符串
                // 时会存在长度和值的变化，所以这里无法将字节转字符串再进行分割，当前使用字节循环来进行分割
                for (int i = 0; i < count; i++)
                {
                    if (this.ReceivedBuffer[i] == SP)
                    {
                        // 消息分割
                        iStart = i + 1;
                        continue;
                    }
                    if (this.ReceivedBuffer[i] == SM)
                    {
                        iEnd = i;
                        int iSize = iEnd - iStart;
                        byte[] message = new byte[iSize];
                        Array.Copy((Array)this.ReceivedBuffer, iStart, message, 0, iSize);
                        //byte[] message = new byte[] {
                        //       90,77,87,124,204,208,132,147,130,128,128,200,128,128,210,128,210,128,209,128,128,208,128,208,128,208,128,128,208,128,208,128,208,128,207,128,128,207,128,207,128,207,128,128,206,128,206,128,206,128,206,128,128,206,128,205,128,205,128,128,205,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,206,128,208,128,212,128,128,217,128,222,128,227,128,128,231,128,234,128,237,128,240,128,128,242,128,243,128,245,128,128,246,128,247,128,248,128,248,128,128,249,128,249,128,249,128,128,248,128,248,128,247,128,246,128,128,244,128,243,128,241,128,128,239,128,238,128,236,128,235,128,128,234,128,233,128,233,128,128,233,128,233,128,234,128,235,128,128,235,128,236,128,236,128,128,235,128,235,128,234,128,233,128,128,232,128,231,128,229,128,128,227,128,225,128,224,128,222,128,128,220,128,219,128,217,128,128,216,128,215,128,214,128,213,128,128,212,128,212,128,211,128,128,211,128,210,128,210,128,128,128
                        //       // //90,77,87,124,223,220,132,255,129,128,128,192,128,245,245,244,243,243,242,128,242,242,241,241,241,241,241,128,241,241,241,241,241,241,241,128,241,241,241,241,241,241,241,128,241,241,241,241,241,241,241,128,241,241,241,242,242,242,242,128,242,242,242,243,243,243,243,128,243,243,244,244,244,244,244,128,244,245,245,245,245,245,246,128,246,246,246,246,247,247,247,128,247,248,248,248,248,248,249,128,249,249,249,250,250,250,250,128,251,251,251,251,251,252,252,128,252,252,252,253,253,253,253,128,254,254,254,255,255,255,255,128,255,128,128,128,128,129,129,254,129,130,130,130,130,131,131,255,131,132,132,132,133,133,133,255,134,134,134,128,128,128,128,135
                        //       // //90,77,87,124,201,211,132,128,129,129,128,137,128,143,142,142,141,141,140,128,140,140,140,139,139,138,138,128,137,137,137,137,137,136,136,128,135,135,134,134,134,134,133,128,133,133,133,132,132,132,132,128,132,131,131,131,131,130,130,128,130,130,129,129,129,129,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,128,129,131,132,133,134,136,138,128,140,142,144,147,149,151,154,128,156,159,162,164,166,169,171,128,173,176,178,180,183,185,187,128,189,191,193,195,198,200,202,128,204,205,207,209,210,212,213,128,215,215,216,217,217,218,218,128,218,218,217,217,217,217,216,128,215,214,213,213,211,210,209,128,208,206,205,203,201,200,198,128,196,195,193,192,190,188,187,128,185,183,182,180,179,178,176,128,175,174,173,171,170,168,167,128,166,165,164,163,162,161,160,128,159,159,159,159,158,158,158,128,158,157,157,157,157,157,157,128,157,158,158,159,159,160,160,128,161,161,161,161,162,162,162,128,162,162,162,162,162,162,161,128,161,161,161,160,160,160,160,128,160,159,159,158,158,157,157,128,156,156,155,155,155,155,154,128,154,154,153,153,152,152,152,128,152,151,151,150,150,149,149,128,148,148,147,147,147,146,146,128,145,145,145,145,144,144,144,128,144,144,144,144,144,128,128,128
                        //       // //90,77,87,124,158,212,132,206,129,129,128,130,128,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,254,254,255,128,129,129,130,130,131,132,132,255,132,131,131,130,130,129,129,255,255,255,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,252,251,250,128,249,250,130,137,145,152,159,252,162,155,148,140,133,254,248,159,249,250,252,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,254,255,128,129,129,240,130,131,132,132,133,134,135,255,135,135,136,136,136,137,137,255,138,138,138,138,137,137,136,255,135,135,134,133,133,132,130,255,129,129,128,254,254,253,253,135,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,128,128,128
                        //       // //90,77,87,124,151,202,132,205,129,129,128,129,128,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,254,255,129,192,131,131,132,133,134,135,135,255,135,134,134,133,132,131,130,255,128,255,253,253,253,253,253,129,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,252,251,250,248,128,246,249,133,144,155,165,176,252,180,170,159,148,137,254,246,159,248,249,251,252,252,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,255,128,129,130,131,248,133,134,135,136,137,138,139,255,139,140,141,142,142,143,143,255,144,144,144,144,144,143,142,255,140,139,138,137,136,135,133,255,132,130,129,255,254,253,253,135,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,253,253,128,253,253,253,253,253,128,128,128
                        //        };
                        string flag = Encoding.ASCII.GetString(message.Take(4).ToArray());
                        if (flag.ToUpper().Equals("ZMW|"))
                        {
                            DECODED_WAVE_DATA data = MR_ZMW.Parse(message);
                            if (null != data)
                            {
                                // 归一化处理
                                if (data.ChannelID == WAVE_ID.WAVE_ECG_CH1)
                                {
                                    byte MID = (byte)0x80;
                                    float test = 0.0f;
                                    for (int p = 0; p < data.DataBuffer.Length; p++)
                                    {
                                        float temp = data.DataBuffer[p];
                                        data.DataBuffer[p] = (byte)((temp - MID) + (temp / MID));
                                        test = (temp - test) > test ? (temp - test) : test;
                                    }
                                    onGetWaveGroup("ECG.ECG", data.DataBuffer, 0, data.SampleRate);
                                }
                                if (data.ChannelID == WAVE_ID.WAVE_PLETH)
                                {
                                    onGetWaveGroup("SPO2.Pleth", data.DataBuffer, 0, data.SampleRate);
                                }
                                if (data.ChannelID == WAVE_ID.WAVE_RESP)
                                {
                                    //Log.d("RESP: {0}", BitConverter.ToString(data.DataBuffer));
                                    byte MID = (byte)0x80;
                                    for (int p = 0; p < data.DataBuffer.Length; p++)
                                    {
                                        float temp = data.DataBuffer[p];
                                        data.DataBuffer[p] = (byte)((temp - MID) + (temp / MID));
                                    }
                                    onGetWaveGroup("RESP.RESP", data.DataBuffer, 0, data.SampleRate);
                                }
                                if (data.ChannelID == WAVE_ID.IBP_ART)
                                {
                                    //Debug.WriteLine("IBP.ART波形接入");
                                    //Log.d("RESP: {0}", BitConverter.ToString(data.DataBuffer));
                                    //byte MID = (byte)0x80;
                                    byte MID = (byte)0x50;
                                    for (int p = 0; p < data.DataBuffer.Length; p++)
                                    {
                                        float temp = data.DataBuffer[p];
                                        data.DataBuffer[p] = (byte)((temp - MID) + (temp / MID));
                                    }
                                    onGetWaveGroup("IBP.ART", data.DataBuffer, 0, data.SampleRate);
                                }
                            }
                            //MR_ZMW.TestCalCheckSum();
                            //Log.d("波形来源：" + data.ChannelID.ToString());
                        }
                        if (flag.ToUpper().StartsWith("OBX|"))
                        {
                            MR_PDS_OBX obx = MR_PDS_OBX.Parse(message);
                            if ("NM".Equals(obx.ValueType.ToUpper()))
                            {
                                ////解析床头监视器的的体征参数
                                foreach (KeyValuePair<string, string> pair in KeyGroupName)
                                {
                                    if (pair.Key.ToUpper().Equals(obx.ObservationIdentifier.ToUpper()))
                                    {

                                        if (pair.Value.ToUpper().Equals("SYS"))
                                        {
                                            this.NIBP_Para[0] = Convert.ToInt16(obx.ObservationResults);
                                            onGetParaGroup("NIBP", this.NIBP_Para);
                                        }
                                        else if (pair.Value.ToUpper().Equals("DIA"))
                                        {
                                            this.NIBP_Para[1] = Convert.ToInt16(obx.ObservationResults);
                                            onGetParaGroup("NIBP", this.NIBP_Para);
                                        }
                                        else if (pair.Value.ToUpper().Equals("MEAN"))
                                        {
                                            this.NIBP_Para[2] = Convert.ToInt16(obx.ObservationResults);
                                            onGetParaGroup("NIBP", this.NIBP_Para);
                                        }
                                        else if ((pair.Value.ToUpper().Equals("ART_SYS")))
                                        {
                                            this.ART_Para[0] = Convert.ToInt16(obx.ObservationResults);
                                            onGetParaGroup("ABP", this.ART_Para);
                                        }
                                        else if ((pair.Value.ToUpper().Equals("ART_MEAN")))
                                        {
                                            this.ART_Para[2] = Convert.ToInt16(obx.ObservationResults);
                                            onGetParaGroup("ABP", this.ART_Para);
                                        }
                                        else if ((pair.Value.ToUpper().Equals("ART_DIA")))
                                        {
                                            this.ART_Para[1] = Convert.ToInt16(obx.ObservationResults);
                                            onGetParaGroup("ABP", this.ART_Para);
                                        }
                                        else
                                        {
                                            onGetParaGroup(pair.Value, new short[1] { Convert.ToInt16(obx.ObservationResults) });
                                        }
                                    }
                                }
                            }
                        }

                        iStart = i + 1;
                    }
                }
                LastDataTicks = DateTime.Now;
                byte[] numArray = new byte[count];
                int length = count;

                //Array.Copy((Array)this.ReceivedBuffer, 0, (Array)numArray, 0, length);
                ////string message = System.Text.Encoding.ASCII.GetString(numArray);

                //foreach (string str in message.Split('\v'))
                //{
                //    if (str.Contains("ORU^R01|161|") || str.Contains("ORU^R01|157|"))
                //    {
                //        if (str.Contains("ORU^R01|157|"))
                //        {
                //            string[] ms = str.Split('\r');
                //            // 处理波形包
                //            foreach(string zmw in ms)
                //            {
                //                if (zmw.StartsWith("ZMW|"))
                //                {
                //                    Log.d("处理波形数据包");
                //                    int size = zmw.Length;
                //                    DECODED_WAVE_DATA data = MR_ZMW.Parse(Encoding.ASCII.GetBytes(zmw));
                //                    Log.d("波形来源：" + data.ChannelID.ToString());
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Log.d("数据接收或发送错误：" + ex.Message);
                Debug.WriteLine("数据接收或发送错误：" + ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 心跳线程函数
        /// </summary>
        private void HeartBeat()
        {
            List<byte> byteList = new List<byte>();
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes("MSH|^~\\&|||||||ORU^R01|106|P|2.3.1|");
            byteList.Add(MR_iPM.Head);
            byteList.AddRange((IEnumerable<byte>)((IEnumerable<byte>)bytes).ToList<byte>());
            byteList.AddRange((IEnumerable<byte>)MR_iPM.Over);
            int i = 0;
            while (isRunning)
            {
                try
                {
                    Thread.Sleep(1000); // 心跳速率1/秒
                    if (null == TCP || !TCP.Connected) continue;
                    TCP.Send(byteList.ToArray());
                    i++;
                    if (i >= 10)
                    {
                        QueryDeviceParam();
                        i = 0;
                    }
                }
                catch (Exception ex)
                {
                    Log.d(ex.Message);
                    Debug.WriteLine("心跳异常：" + ex.Message);
                }
            }
            Log.d("心跳结束出了");
        }

        /// <summary>
        /// 查询一次，所有参数，生理报警，技术报警
        /// MSH|^~\&|||||||QRY^R02|1203|P|2.3.1<CR>
        /// QRD|20060731145557|R|I|Q895211|||||RES<CR>
        /// QRF|MON||||0&0^1^1^1^<CR>
        /// QRF|MON||||0&0^3^1^1^<CR>
        /// QRF|MON||||0&0^4^1^1^<CR>
        /// </summary>
        private void QueryDeviceParam()
        {
            try
            {
                //this.packed.Clear();
                ////查询一次, 所有参数， 生理报警， 技术报警

                ////string s = "MSH|^~\\&|||||||QRY^R02|1203|P|2.3.1\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|Q455352|||||RES\rQRF|MON||||" + GetIpTo32(client.RemoteEndPoint.ToString().Split(':')[0]) + "&1^1^10^1^0\r";
                ////string s = "MSH|^~\\&|||||||QRY^R02|1203|P|2.3.1\r" + "" +
                ////    "QRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|Q895211|||||RES\r" +
                ////    "QRF|MON||||" + GetIpTo32(client.RemoteEndPoint.ToString().Split(':')[0]) + "&1^1^10^1^0\r" +
                ////    "QRF|MON||||" + GetIpTo32(client.RemoteEndPoint.ToString().Split(':')[0]) + "&3^1^10^1^0\r" +
                ////    "QRF|MON||||" + GetIpTo32(client.RemoteEndPoint.ToString().Split(':')[0]) + "&4^1^10^1^0\r";

                ////仅获取床头监视器的九个参数。他们的ID是“ 101”，“ 102”，“ 103”，“ 104”，“ 151”，“ 160”，“ 170”，“ 171”和“ 172”
                //string s = "MSH|^~\\&|||||||QRY^R02|1203|P|2.3.1\r" + "" +
                //    "QRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|Q895211|||||RES\r" +
                //    "QRF|MON||||0&0^1^1^0^101&102&103&104\r" +
                //    "QRF|MON||||0&0^1^1^0^172\r" +
                //    "QRF|MON||||0&0^3^1^1^\r" +
                //    "QRF|MON||||0&0^4^1^1^\r";

                //this.packed.Add(this.Head);
                //this.packed.AddRange((IEnumerable<byte>)((IEnumerable<byte>)Encoding.GetEncoding("GBK").GetBytes(s)).ToList<byte>());
                //this.packed.AddRange((IEnumerable<byte>)this.Over);
                //MindrayIPM_TCPSocket.client.Send(this.packed.ToArray());

                //波形查询
                List<byte> pack = new List<byte>();

                //获取床头监视器的四个波形，ID为1151、1152、1101和1102
                string s = "MSH|^~\\&|||||||QRY^R02|1203|P|2.3.1\r" + "" +
                    "QRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|Q895211|||||RES\r" +
                    "QRF|MON||||0&0^2^1^0^1151&1152&1101&1102&1171\r" +
                    "QRF|MON||||0&0^1^1^0^101&151&160&161\r" +
                    "QRF|MON||||0&0^1^1^0^500&501&502\r";

                //ZMW迈瑞波形
                //string s = "MSH|^~\\&|||||||QRY^R02|157|P|2.3.1\r";

                pack.Add(MR_iPM.Head);
                pack.AddRange((IEnumerable<byte>)((IEnumerable<byte>)Encoding.GetEncoding("GBK").GetBytes(s)).ToList<byte>());
                pack.AddRange((IEnumerable<byte>)MR_iPM.Over);
                TCP.Send(pack.ToArray());
            }
            catch (Exception ex)
            {
                Log.d("发送数据包：" + ex.Message);
                Log.d("重新连接");
                Connect();
            }
        }
    }
}

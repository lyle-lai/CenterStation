using System;
using System.Collections.Generic;
using System.Text;
using GlobalClass;
using System.Net;
using System.Net.Sockets;
using ProtocolPack;
using System.Threading;
using System.Diagnostics;
//-------------------------------------------------------------------------------
//迈瑞监护仪通讯协议(CMS+)
//-------------------------------------------------------------------------------
namespace ProtocolPack
{
    //Class-------------------------------------------------------------------------------
    public class MR_CMS : PrtServerBase
    {
        private const int HEAD_LEN = 24;
        private static UdpClient Udp = null;
        private static Thread ThrUdp;//UDP接收线程
        private static IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);//Any IP any port from remote point
        public static List<MR_CMS> PrtList = new List<MR_CMS>();
        private static bool isRun = false;
        //public static MR_CMS Trans = new MR_CMS(0);
        //Static-------------------------------------------------------------------------------
        public static void Initialize()
        {
            isRun = true;
            Udp = new UdpClient(3501);
            ThrUdp = new Thread(UdpRun);
            ThrUdp.Start();
        }

        public static void Close()
        {
            try
            {
                isRun = false;
                if (ThrUdp != null && ThrUdp.ThreadState == System.Threading.ThreadState.Running)
                {
                    ThrUdp.Abort();
                }
                ThrUdp = null;

                if (Udp != null)
                {
                    Udp.Close();
                    Udp = null;
                }

            }
            catch (Exception  ex)
            {
                Log.d(ex.Message);
            }

        }
        //Static-------------------------------------------------------------------------------
        private static void UdpRun()
        {//接收数据
            while (isRun)
            {
                //接收数据
                if (Udp.Client.Available > 0)
                {
                    Byte[] RBuf = Udp.Receive(ref RemoteIpEndPoint);
                    OnUdpReceived(RemoteIpEndPoint, RBuf, RBuf.Length);
                    Log.d(" ---- Mindray UDP 收到数据," + RemoteIpEndPoint.Address.ToString() + "," + Udp.Available.ToString());
                }
                Thread.Sleep(100);
            }
        }
        //Static-------------------------------------------------------------------------------
        private static void OnUdpReceived(IPEndPoint EndPoint, Byte[] Buf, int Len)
        {
            String FromIP = EndPoint.Address.ToString();
            foreach (MR_CMS cms in PrtList)
            {
                if (GLB.Same(FromIP, cms.MonIP))
                {
                    return;
                }
            }
            //查找该IP是否已经建立TCP连接
            //int i = 0;
            //while (i < PrtList.Count && !GLB.Same(FromIP, PrtList[i].MonIP))
            //    i++;
            //if (i < PrtList.Count)
            //{
            //    return;//如果TCP连接已经建立，则退出
            //}
            //没找到对应监护仪，则初始化连接
            MR_CMS Cms = new MR_CMS(FromIP, 3500);
            //Trans.Connect(FromIP, 3500);
            //连接到迈瑞监护仪
            Cms.OnConnect(FromIP, EndPoint.Port);
            PrtList.Add(Cms);
        }

       
        //---------------------------------------------------------------------------
        //Static-------------------------------------------------------------------------------
        //Fun-------------------------------------------------------------------------------
        private TcpClient TC = new TcpClient();
        public String MonIP;
        public int Port;
        public Thread ThrRun;//接收线程
        //Fun-------------------------------------------------------------------------------
        public MR_CMS(String IP, int Port)
            //public MR_CMS(int Index)
            : base(0)
        {
            ThrRun = new Thread(Run);
            ThrRun.Start();

            MonIP = IP;
            IP4 = IP;
            //IP4 = IP.Split('.')[3];
        }

        
        //Fun-------------------------------------------------------------------------------
        public void OnConnect(String IP, int Port)
        {
            //if (TC.Connected) return;
            if (State == ConnectState.Connect) return;
            if (onConnected == null) return;
            State = ConnectState.Connect;
            this.MonIP = IP;
            IP4 = IP;
            //IP4 = IP.Split('.')[3];
            this.Port = Port;
            onConnected();
        }
        //Fun-------------------------------------------------------------------------------
        public void OnDisconnect()
        {
            State = ConnectState.OffLine;
            if (OnDisConnected != null)
                OnDisConnected(TC.Client);
        }
        //Fun-------------------------------------------------------------------------------       
        private void Run()
        {//接收数据
            while (isRun)
            {
                try
                {

                    if (!TC.Connected)
                    {
                        OnDisconnect();
                        TC.Connect(MonIP, 3500);
                        Log.d("TCP 连接:" + MonIP);
                    }
                    else
                    {
                        //接收数据
                        HeartBeat();
                        OnConnect(MonIP, 3500);
                        int RcvLen = TC.Client.Available;
                        Log.d("接收数据：ReceiveBufferSize=" + RcvLen);
                        byte[] buf = new byte[1024 * 100];
                        int BufLen = 0;
                        TC.Client.Receive(buf);
                        //TC.Client.Receive(buf, BufLen, RcvLen, SocketFlags.None);
                        OnReceived(buf, 0, RcvLen);
                        //Byte[] d = MR_Packager.GetRealTimeParaTransState();
                        //Byte[] d = MR_Packager.SetPatientInfo();
                        //Byte[] d = MR_Packager.GetPatientInfo();
                        //Byte[] d = MR_Packager.SetMeasureNIBPCmd();
                        //Int16 ck = MR_Packager.Checksum(d);
                        //TC.Client.Send(d);
                    }
                }
                catch (Exception)
                {
                    OnDisconnect();
                    Log.d("数据出错(迈瑞)");
                    if (TC != null && TC.Client != null)
                        TC.Client.Close();
                    TC = new TcpClient();
                }
                Thread.Sleep(1000);
            }
        }
        //----------------------------------------------------------------------------------------------
        private void OnReceived(Byte[] Buf, int StartIndex, int BufLen)
        {
            try
            {
                if (BufLen < 24)
                {
                    return;
                }
                //
                Byte C_Type = Buf[StartIndex + 1];
                Int16 C_ID = BigEndian.GetInt16(Buf, StartIndex + 2);
                Int32 Len = BigEndian.GetInt32(Buf, StartIndex + 4);
                Int32 P_ID = BigEndian.GetInt32(Buf, StartIndex + 8);
                Int32 U_ID = BigEndian.GetInt32(Buf, StartIndex + 12);
                Int16 ckSum = BigEndian.GetInt16(Buf, StartIndex + 16);
                Int16 Flag = BigEndian.GetInt16(Buf, StartIndex + 18);
                //
                Log.d("解包：StartIndex=" + StartIndex + ",包长度=" + Len + ",C_ID=" + C_ID);
                int sIdx = StartIndex + 24;
                int pLen = Len - 24;

                Byte[] bbb = new Byte[Len];
                Buffer.BlockCopy(Buf, StartIndex, bbb, 0, Len);
                Int16 ck = MR_Packager.Checksum(bbb);
                //if (ck != 0)
                //{
                //    int aaa = 0;
                //}
                if (C_ID == 503)
                {
                    RcvNibp_503(Buf, sIdx, pLen);
                }
                else if (C_ID == 104)
                {
                    //Log.d("C_ID == 203");
                }
                else if (C_ID == 204)
                {
                    //Log.d("实时参数=" + BigEndian.GetInt32(Buf, StartIndex + 4));
                    RcvPara(Buf, sIdx, pLen);
                }
                else if (C_ID == 157)
                {
                    //Log.d("实时波形数据"));
                    RcvWave(Buf, sIdx, pLen);
                }
                OnReceived(Buf, StartIndex + Len, BufLen - Len);
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }
            
        }
        //Fun-------------------------------------------------------------------------------
        private void RcvNibp_503(Byte[] Buf, int StartIndex, int Len)
        {
            try
            {
                if (Len < 7) return;
                Int16 F_ID = BigEndian.GetInt16(Buf, StartIndex);
                Byte F_Type = Buf[StartIndex + 2];
                Int32 F_Len = BigEndian.GetInt32(Buf, StartIndex + 3);
                if (Len < 7 + F_Len) return;

                //Log.d("RcvNibp_503 F_ID=" + F_ID);
                if (F_ID == 1121)
                {
                    int sIdx = StartIndex + 7;
                    Int16 Year = BigEndian.GetInt16(Buf, sIdx);
                    Byte Mon = Buf[sIdx + 2];
                    Byte Day = Buf[sIdx + 3];
                    Byte Hour = Buf[sIdx + 4];
                    Byte Min = Buf[sIdx + 5];
                    Byte Sec = Buf[sIdx + 6];

                    Int16 Sys = BigEndian.GetInt16(Buf, sIdx + 9);
                    Int16 Map = BigEndian.GetInt16(Buf, sIdx + 11);
                    Int16 Dia = BigEndian.GetInt16(Buf, sIdx + 13);
                    if (onGetParaGroup != null)
                        onGetParaGroup("NIBP.NIBP", new Int16[] { Sys, Dia, Map });
                    //PrtBase._onGetParamater("Lan1", 513, "NIBP.NIBP", new Int16[] { Sys, Dia, Map });
                    //Submit("NIBP.NIBP", new Double[] { Sys, Dia, Map });
                    //Log.d("NIBP：" + Sys + "/" + Dia + "(" + Map + ")");
                }
                //
                StartIndex += 7 + F_Len;
                Len -= 7 + F_Len;
                RcvNibp_503(Buf, StartIndex, Len);
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }

           
        }
        //Fun-------------------------------------------------------------------------------
        private void RcvPara(Byte[] Buf, int StartIndex, int Len)
        {
            try
            {
                Int16 F_ID = BigEndian.GetInt16(Buf, StartIndex);
                Byte F_Type = Buf[StartIndex + 2];
                Int32 F_Len = BigEndian.GetInt32(Buf, StartIndex + 3);

                Log.d("RcvPara " + F_ID + ":" + F_Type + ":" + F_Len);
                //if (F_ID != 586) return;
                StartIndex += 7;
                Len -= 7;

                if (F_ID == 589)
                {
                    while (Len >= 5)
                    {
                        short ParaType = BigEndian.GetInt16(Buf, StartIndex);
                        Byte Multiple = Buf[StartIndex + 2];           //表96，参数放大倍数
                        Byte DataType = Buf[StartIndex + 3];          //表8，参数数据类型
                        int Value = BigEndian.GetInt32(Buf, StartIndex + 4);
                        Log.d("ParaType=" + ParaType + ",Value=" + Value);
                        StartIndex += 5;
                        Len -= 5;
                    }
                    return;
                }
                //if (F_ID != 586) return;
                if (F_ID == 585)
                {
                    F_ID = BigEndian.GetInt16(Buf, StartIndex);
                    F_Type = Buf[StartIndex + 2];
                    F_Len = BigEndian.GetInt32(Buf, StartIndex + 3);
                    StartIndex += 8;
                    Len -= 8;
                }

                while (Len >= 5)
                {
                    //
                    Byte ParaType = Buf[StartIndex];          //表27，参数类型
                    Byte Multiple = Buf[StartIndex + 1];           //表96，参数放大倍数
                    Byte DataType = Buf[StartIndex + 2];          //表8，参数数据类型
                    Int16 Value = BigEndian.GetInt16(Buf, StartIndex + 3);
                    //
                    if (onGetParaGroup != null)
                    {
                        switch (ParaType)
                        {
                            case 1:
                                //Submit("ECG.HR", new Double[] { Value });
                                onGetParaGroup("ECG.HR", new Int16[] { Value });
                                //PrtBase._onGetParamater("Lan1", 257, "ECG.HR", new Int16[] { Value });
                                break;
                            case 17:
                                onGetParaGroup("RESP.RR", new Int16[] { Value });
                                //PrtBase._onGetParamater("Lan1", 1025, "RESP.RR", new Int16[] { Value });
                                //Submit("RESP.RR", new Double[] { Value });
                                break;
                            case 18:
                                onGetParaGroup("Spo2.Spo2", new Int16[] { Value });
                                //PrtBase._onGetParamater("Lan1", 769, "Spo2.Spo2", new Int16[] { Value });
                                //Submit("Spo2.Spo2", new Double[] { Value });
                                break;
                            case 19:
                            case 200:
                                onGetParaGroup("Spo2.PR", new Int16[] { Value });
                                //PrtBase._onGetParamater("Lan1", 770, "Spo2.PR", new Int16[] { Value });
                                //Submit("Spo2.PR", new Double[] { Value });
                                break;
                            /*
                        case 35:
                            Submit( "TEMP.TEMP", new String[] { Value.ToString() });
                            break;
                        case 36:
                            Submit( "TEMP.TEMP", new String[] { Value.ToString() });
                            break;*/
                        }
                    }
                    //Log.d("接收到参数：StartIndex=" + StartIndex + ",Len=" + Len + ",ParaType=" + ParaType + ",Value=" + Value);
                    //
                    StartIndex += 5;
                    Len -= 5;
                }
            }
            catch (Exception ex)
            {
               Log.d(ex.Message);
            }

        }
        //Fun-------------------------------------------------------------------------------
        private void RcvWave(Byte[] Buf, int StartIndex, int Len)
        {
            try
            {
                            if (onGetWaveGroup == null) return;

                float[] bufTemp = new float[Buf.Length];
                for (int i = 0; i < Buf.Length; i++)
                {
                    bufTemp[i] = Buf[i];
                }

            Int16 F_ID = BigEndian.GetInt16(Buf, StartIndex);
            Byte F_Type = Buf[StartIndex + 2];
            Int32 F_Len = BigEndian.GetInt32(Buf, StartIndex + 3);

            Byte W_Type = Buf[StartIndex + 7];
            UInt16 iSampleRate = BigEndian.GetUInt16(Buf, StartIndex + 8);
            Byte iDataType = Buf[StartIndex + 10];

            Log.d("RcvWave " + W_Type + ":" + iSampleRate + ":" + iDataType + ",Len=" + Len);
            //StartIndex += 7;
            //Len -= 7;

            if (W_Type == 1)
            {
                onGetWaveGroup("ECG.ECG", bufTemp, StartIndex + 11, Len - 11);
                Log.d("接收到波形数据：ECG I");
            }
            if (W_Type == 2)
            {
                onGetWaveGroup("ECG.ECG", bufTemp, StartIndex + 11, Len - 11);
                Log.d("接收到波形数据：ECG II");
            }
            if (W_Type == 25)
            {
                onGetWaveGroup("SPO2.Pleth", bufTemp, StartIndex + 11, Len - 11);
                //onGetWaveGroup("SPO2.Pleth", Buf, StartIndex + 10, 256);
                //Log.d("接收到波形数据：PLETH");
            }
            if (W_Type == 26)
            {
                //onGetWaveGroup("RESP.RESP", Buf, StartIndex + 10, 50);
                /*for (int i = 0; i < Len - 11; i++)
                {
                    int idx = StartIndex + 11 + i;
                    Buf[idx] = (Byte)(Buf[idx]);
                }*/
                onGetWaveGroup("RESP.RESP", bufTemp, StartIndex + 11, Len - 11);
                //Log.d("接收到波形数据：RESP");                
            }
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }

        }
        //Fun-------------------------------------------------------------------------------
        public void HeartBeat()
        {
            //base.HeartBeat();
            try
            {
                if (!TC.Connected) return;

                int Len = 24 + 7;
                Byte[] Buf = new Byte[Len];
                TC.Client.Send(Buf);

            }
            catch (Exception ex) { TC.Close(); Log.d(ex.Message);            }
        }
        /*public void HeartBeat()
        {//远程控制命令通用函数
            try
            {
                if (!TC.Connected) return;

                int Len = 24 + 7;
                Byte[] Buf = new Byte[Len];
                TC.Client.Send(Buf);

            }
            catch (Exception) { TC.Close(); }
        }*/
        //Fun-------------------------------------------------------------------------------
    }
    //Class-------------------------------------------------------------------------------
}

using System;
//using System.Linq;
using System.Collections.Generic;
using System.Text;
//----------------------------------------------------------------------------------------------
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using GlobalClass;
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class TPeer
    {
        //--------------------------------------
        //Socket对象的定义
        private Socket Sck = null;
        private String Addr;
        private int Port;
        //--------------------------------------
        //数据接收
        private Thread ThrRcv = null;//接收线程
        private Byte[] RcvBuf = new Byte[64 * 1024];//定义64K的接收缓冲
        private int RcvLen = 0;
        //--------------------------------------
        //事件定义
        public delegate void EVENT_DISCONNECT();
        public EVENT_DISCONNECT OnDisconnect = null;
        //--------------------------------------
        //事件定义-接收数据，检查合法性
        public delegate int EVENT_RECEIVED(Byte[] Buf, int Len);
        public EVENT_RECEIVED OnReceived = null;
        //--------------------------------------
        //统计变量
        public int SndCount = 0;
        public int RcvCount = 0;
        private DateTime tTime;
        //TimeSpan tSpan;
        //测试，序号应该是连续的
        private int Idx = -1;
        public int ErrCount = -1;
        //----------------------------------------------------------------------------------------------
        /*public TPeer(String RemoteAddress, int RemotePort)
        {//未
            Addr = RemoteAddress;
            Port = RemotePort;
            Init();
        }*/
        //----------------------------------------------------------------------------------------------
        public TPeer(Socket tcpSocket)
        {   //接收线程的初始化
            Sck = tcpSocket;
            Sck.ReceiveTimeout = 3000;
            RcvLen = 0;
            ThrRcv = new Thread(Receive);
            ThrRcv.Start();
        }
        //----------------------------------------------------------------------------------------------
        public void Disconnect()
        {//断开Socket连接
            if (Sck == null || !Sck.Connected) return;
            Sck.Shutdown(SocketShutdown.Both);
            Sck.Close();
            Sck = null;
        }
        //----------------------------------------------------------------------------------------------
        /*public void Stop()
        {
            if (ThrRcv != null)
            {
                ThrRcv.Abort();//终止线程
                ThrRcv = null;
            }

            Disconnect();
        }*/
        //----------------------------------------------------------------------------------------------
        private void Receive()
        {//接收数据
            int Len = 0;

            while (true)
            {
                //Thread.Sleep(100);
                Log.d("Receive..." + Sck.ReceiveBufferSize.ToString());
                if (Sck == null || !Sck.Connected)
                {
                    _OnDisconnect();
                    break;
                }

                if (Sck.ReceiveBufferSize <= 0) continue;

                try
                {
                    Log.d("Receive...TimeOut=" + Sck.ReceiveTimeout.ToString());
                    Len = Sck.Receive(RcvBuf, RcvLen, RcvBuf.Length - RcvLen, SocketFlags.None);
                    Log.d("Receive...2");
                    RcvLen += Len;

                    int UnPackLen = OnReceived(RcvBuf, RcvLen);
                    //数组拷贝
                    Buffer.BlockCopy(RcvBuf, UnPackLen, RcvBuf, 0, RcvLen - UnPackLen);
                    //Debug.WriteLine("RcvLen=" + RcvLen.ToString() + ", UnPackLen=" + UnPackLen.ToString());
                    RcvLen -= UnPackLen;
                }
                catch (Exception ex)
                {
                    Log.d(ex.Message);
                    _OnDisconnect();
                    break;
                }              
                RcvCount++;
            }
        }
        //----------------------------------------------------------------------------------------------
        public void Send(Byte[] Data, int Len)
        {//协议数据包发送
            if (Sck == null || !Sck.Connected) return;
            //添加包头
            Sck.Send(Data, Len, SocketFlags.None);
        }
        //----------------------------------------------------------------------------------------------
        public void Send(String Str)
        {//Socket字符串发送（测试）
            if (Sck == null || !Sck.Connected) return;

            Byte[] data = Encoding.Default.GetBytes(Str);
            tTime = DateTime.Now;
            SndCount++;
            Sck.Send(data);
        }
        //----------------------------------------------------------------------------------------------
        private void _OnDisconnect()
        {//断开连接，通知上级线程，异步执行释放操作，一般来说只用于服务器端，以释放连接资源。
            if (OnDisconnect == null) return;
            //OnDisconnect.BeginInvoke(this, null, null);
            OnDisconnect();
        }
        //----------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------
}

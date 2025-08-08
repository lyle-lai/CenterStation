using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using GlobalClass;
//----------------------------------------------------------------------------------------------
//DCS Protocol
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class PrtDCS_Client : PrtDCS_Base
    {
        public String ServerIP = "LocalHost";
        public int ServerPort = 0;
        public ConnectState State = ConnectState.OffLine;
        public enum ConnectState { OffLine, Connect, GetData };
        //
        protected UdpClient Udp = null;
        protected Thread ThrReceive;//接收线程
        protected IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);//Any IP any port from remote point
        protected DateTime LastTime = DateTime.Now;
        protected DateTime LastRecTime = DateTime.Now;
        protected System.Windows.Forms.Timer StateTimer = new System.Windows.Forms.Timer();
        //----------------------------------------------------------------------------------------------
        public PrtDCS_Client(int Port)
            : base()
        {
            Udp = new UdpClient(Port);  //客户端-端口号
            ThrReceive = new Thread(ReceiveRun);
            ThrReceive.Start();
            //
            StateTimer.Tick += StateTick;
            StateTimer.Interval = 2000;
            StateTimer.Enabled = true;
        }
        //----------------------------------------------------------------------------------------------
        private void ReceiveRun()
        {//接收数据
            while (true)
            {
                //接收数据
                if (Udp.Client.Available > 0)
                {
                    Log.d(DateTime.Now + " *** 客户端收到数据 " + Udp.Available.ToString());
                    Byte[] RBuf = Udp.Receive(ref RemoteIpEndPoint);
                    OnReceived(RBuf, RBuf.Length);
                }
                Thread.Sleep(100);
            }
        }
        //----------------------------------------------------------------------------------------------
        private void OnReceived(Byte[] Buf, int Len)
        {
            //Log.d("接收该数据");
            LastRecTime = DateTime.Now;
            PrtDCS Item = new PrtDCS(Buf);
            if (Item.ObjID == 0 && Item.Values[0] == 1)
            {
                State = ConnectState.Connect;
                ServerIP = RemoteIpEndPoint.Address.ToString();
                ServerPort = RemoteIpEndPoint.Port;

                Log.d("*** 服务器回复连接请求");
            }
        }
        //----------------------------------------------------------------------------------------------
        private void StateTick(object sender, EventArgs e)
        {
            switch (State)
            {
                case ConnectState.OffLine:
                    Send("255.255.255.255", PrtDCS_Base.SVR_PORT, 0, new Int16[]{0});     //发送服务器查询广播
                    break;
                case ConnectState.Connect:
                    Send(ServerIP, ServerPort, 100, new Int16[] { 100, 200, 300 });    //测试包
                    Log.d("*** 已连接");
                    break;
            }
        }
        //----------------------------------------------------------------------------------------------
        public void Send(String IP, int Port, Int32 ID, Int16[] ValueList)
        {
            if (State == ConnectState.OffLine && ID != 0) return;
            //
            Byte[] Buf = GetSendBuf(ID, ValueList);
            Udp.Send(Buf, Buf.Length, IP, Port);
        }
        //----------------------------------------------------------------------------------------------
        public void Send(Int32 ID, Int16[] ValueList)
        {
            if (State == ConnectState.OffLine && ID != 0) return;
            //
            Byte[] Buf = GetSendBuf(ID, ValueList);
            Udp.Send(Buf, Buf.Length, ServerIP, ServerPort);
        }
        //----------------------------------------------------------------------------------------------

    }
    //----------------------------------------------------------------------------------------------
}
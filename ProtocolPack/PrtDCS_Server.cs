using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
//using ViewPack;
using System.Threading;
using GlobalClass;
//----------------------------------------------------------------------------------------------
//DCS Protocol
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class PrtDCS_Server
    {
        //----------------------------------------------------------------------------------------------
        public const int ID_LMT = 250;
        public static PrtDCS_Server[] PrtList = new PrtDCS_Server[ID_LMT];
        //
        public static UdpClient Udp = null;
        public static Thread ThrUdp;//接收线程
        public static IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);//Any IP any port from remote point
        private static DateTime LastTime = DateTime.Now;
        //----------------------------------------------------------------------------------------------
        public static void Initialize()
        {
            try
            {
                if (Udp == null) Udp = new UdpClient(PrtDCS_Base.SVR_PORT);
                if (ThrUdp==null)ThrUdp = new Thread(UdpRun);
                if (ThrUdp != null && ThrUdp.ThreadState!= System.Threading.ThreadState.Running) ThrUdp.Start();
                for (int i = 0; i < ID_LMT; i++)
                    PrtList[i] = new PrtDCS_Server(i);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 关闭操作
        /// </summary>
        public static void Close()
        {
            if (Udp != null) Udp.Close();
            if (ThrUdp != null && ThrUdp.ThreadState == System.Threading.ThreadState.Running)
            {
                ThrUdp.Abort();
            }
            ThrUdp = null;

            if (PrtList != null) PrtList = null;
        }
        //----------------------------------------------------------------------------------------------
        private static void UdpRun()
        {//接收数据
            while (true)
            {
                //接收数据
                if (Udp != null && Udp.Client!=null&&Udp.Client.Available > 0)
                {
                    try
                    {
                        Byte[] RBuf = Udp.Receive(ref RemoteIpEndPoint);
                        int IP_ID = (int)((RemoteIpEndPoint.Address.Address & 0xFF000000) >> 24) - 0;
                        if (IP_ID >= PrtList.Length) continue;
                        //
                        PrtDCS Item = new PrtDCS(RBuf);
                        if (Item.ObjID == 0)//查询服务器的存在
                        {
                            Send(Udp, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port, 0, new Int16[] { 1 });     //回复服务器查询广播
                        }
                        else
                            PrtList[IP_ID].OnReceived(RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port, Item.ObjID, Item.Values);
                    }
                    catch (Exception e) {Log.d(e.Message);}
                }
                ////发送获取数据请求
                if (GLB.PassMs(LastTime) >= 5000)
                {
                    LastTime = DateTime.Now;
                    for (int i = 0; i < PrtList.Length; i++)
                        PrtList[i].GetDataTick();
                }
                //----检查连接状态
                for (int i = 0;PrtList!=null&&i < PrtList.Length; i++)
                {
                    if (PrtList[i] != null) PrtList[i].CheckConnect();
                } 
                //
                Thread.Sleep(100);
            }
        }
        //----------------------------------------------------------------------------------------------
        public static void Send(UdpClient Udp, String IP, int Port, Int32 ID, Int16[] ValueList)
        {
            Byte[] Buf = PrtDCS_Base.GetSendBuf(ID, ValueList);
            Udp.Send(Buf, Buf.Length, IP, Port);
        }
        //----------------------------------------------------------------------------------------------
        public static void Stop()
        {
        }
        //----------------------------------------------------------------------------------------------
        /*public static void OnConnected(Socket socket)
        {
            PrtList.Add(new PrtDCS(socket));            
        }*/
        //----------------------------------------------------------------------------------------------
        private int ViewID = -1;
        private enum ConnectState { OffLine, Connect, GetData };
        private ConnectState State = ConnectState.OffLine;
        private DateTime LastRecTime = DateTime.MinValue;
        public String RIP;
        public int RPort;
        //事件定义
        public delegate void OnDisConnected();
        public OnDisConnected onDisConnected = null;
        public delegate void OnConnected();
        public OnConnected onConnected = null;
        public delegate void OnGetParaGroup(uint ObjID, short[] Values);
        public OnGetParaGroup onGetParaGroup = null;
        //----------------------------------------------------------------------------------------------
        public PrtDCS_Server(int Index)
        {
            ViewID = Index;
            if (ViewID < 0 || ViewID >= ID_LMT) return;
            //VM = LayoutBedView.VM[ViewID];
            //IP = "192.168.66." + (ViewID + MIN_ID).ToString();
            //VM.DisConnected();
            //VM.Connected();
            //
            /*
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From PrtDCS Group By [GroupName]");
            while (dr.Read())
                G_List.Add(new PrtPhilipsGroup(dr, VM));*/
            /*
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From PrtDCS");
            while (dr.Read())
            {
                P_List.Add(new PrtPhilipsItem(dr, VM));
            }*/
        }
        //----------------------------------------------------------------------------------------------
        public void GetDataTick()
        {
            //if (State == ConnectState.Connect)
                //Send(Udp,ip
                //Udp.Send(FramePhilipsMP20.ExPollReqs, FramePhilipsMP20.ExPollReqs.Length, IP, 24105);
        }
        //----------------------------------------------------------------------------------------------
        private void OnReceived(String IP, int Port, uint ObjID, short[] Values)
        {
            //if(IP=="10.254.77.117")
                //Log.d("IP=" + IP + "，接收该数据");
            //状态维护
            LastRecTime = DateTime.Now;
            RIP = IP;
            RPort = Port;
            if (State == ConnectState.OffLine)
            {
                if (onConnected != null)
                    onConnected();
            }
            //数据处理
            if (onGetParaGroup != null)
                onGetParaGroup(ObjID, Values);
            //
            Send(Udp, IP, Port, 1, new Int16[] { 0 });//返回接收信息，以保持连接
        }
        //----------------------------------------------------------------------------------------------
        /*private void OnDisconnect()
        {
            VM.DisConnected();
            PrtList.Remove(this);
        }*/
        //----------------------------------------------------------------------------------------------
        private void CheckConnect()
        {   
            if (State == ConnectState.OffLine && GLB.PassSecond(LastRecTime) < 5*60 && onConnected != null)
            {
                State = ConnectState.Connect;
                onConnected();
                return;
            }

            if (State == ConnectState.Connect && GLB.PassSecond(LastRecTime) >= 5*60 && onDisConnected != null)
            {
                Log.d("终端断开连接，超时：" + GLB.PassSecond(LastRecTime) + "秒");
                State = ConnectState.OffLine;
                onDisConnected();
                return;
            }
        }
        //----------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------
}
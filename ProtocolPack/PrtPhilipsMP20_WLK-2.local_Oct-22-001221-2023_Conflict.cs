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
//----------------------------------------------------------------------------------------------
//PhilipsMP20 Protocol
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class PrtPhilipsMP20 : PrtServerBase
    {
        //----------------------------------------------------------------------------------------------
        private const int MIN_ID = 100;
        private static UdpClient Udp = null;
        private static List<PrtPhilipsMP20> ALL = new List<PrtPhilipsMP20>();
        private static Thread ThrUdp;//接收线程
        private static IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);//Any IP any port from remote point
        private static Byte[] invoke_id = new Byte[2];
        private static DateTime LastTime = DateTime.Now;
        //事件定义
        public delegate void OnGetParamater(int GroupID, String GroupName, short[] Values);
        //public OnGetParamater onGetPara = null;
        //----------------------------------------------------------------------------------------------
        public static void Initialize(OnGetParamater On_Get_Paramater)
        {
            Udp = new UdpClient(24005);
            ThrUdp = new Thread(UdpRun);
            ThrUdp.Start();
            //for (int i = 0; i < 4; i++)
            //    ALL.Add(new PrtPhilipsMP20(i, On_Get_Paramater, RemoteIpEndPoint));
            //----测试----
            /*
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Philips");
            while (dr.Read())
            {
                Byte[] Data = dr.GetBlob("Data");
                ALL[2].OnReceived(Data, Data.Length);
            }*/
            //----测试结束----
        }
        //----------------------------------------------------------------------------------------------
        private static void UdpRun()
        {//接收数据
            while (true)
            {
                //接收数据
                if (Udp.Client.Available > 0)
                {
                    //Log.d(DateTime.Now + " ---- 收到数据" + Udp.Available.ToString());
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
                    Item.OnReceived(RemoteIpEndPoint, RBuf, RBuf.Length);
                    //ALL[0].OnReceived(RemoteIpEndPoint, RBuf, RBuf.Length);
                }
                //发送获取数据请求
                if (GLB.PassMs(LastTime) >= 5000)
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
                if (GLB.Same(ALL[i].IP, RemoteIpEndPoint.Address.ToString())) return ALL[i];
            return new PrtPhilipsMP20(1, null, RemoteIpEndPoint);
        }
        //----------------------------------------------------------------------------------------------
        public static void Stop()
        {/*
            for (int i = 0; i < Lsn.Count; i++)
                Lsn[i].Stop();
            for (int i = 0; i < ALL.Count; i++)
                ALL[i].OnDisconnect();*/
        }
        //----------------------------------------------------------------------------------------------
        /*public static void OnConnected(Socket socket)
        {
            ALL.Add(new PrtPhilipsMP20(socket));            
        }*/
        //----------------------------------------------------------------------------------------------
        private List<PrtPhilipsGroup> G_List = new List<PrtPhilipsGroup>();
        //private ViewMonitor VM = null;
        private int ID = -1;
        private enum ConnectState { OffLine, Connect, GetData };
        private ConnectState State = ConnectState.OffLine;
        private String IP = "LocalHost";
        private DateTime LastRecTime = DateTime.Now;
        private IPEndPoint EndPoint = null;
        //----------------------------------------------------------------------------------------------
        public PrtPhilipsMP20(int Index, OnGetParamater On_Get_Paramater, IPEndPoint EndPoint)
            : base(Index)
        {
            ID = Index;
            ALL.Add(this);
            this.EndPoint = EndPoint;
            IP = EndPoint.Address.ToString();
            IP4 = IP;
            //IP4 = IP.Split('.')[3];
            //if (ID < 0 || ID >= LayoutBedView.VM.Length) return;
            //VM = LayoutBedView.VM[ID];
            //IP = "192.168.66." + (ID + MIN_ID).ToString();
            //VM.DisConnected();
            //VM.Connected();
            //
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From V_PrtPhilipsMP20 Group By [GroupName]");
            while (dr.Read())
                G_List.Add(new PrtPhilipsGroup(dr.GetInt("GroupID"), dr.GetStr("GroupName"), On_Get_Paramater));
            for (int i = 0; i < G_List.Count; i++)
                G_List[i].onGetPara = GetParamater;
            /*
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From PrtPhilipsMP20");
            while (dr.Read())
            {
                P_List.Add(new PrtPhilipsItem(dr, VM));
            }*/
        }
        //----------------------------------------------------------------------------------------------
        public void GetDataTick()
        {
            //if (State == ConnectState.GetData)
            //    Udp.Send(FramePhilipsMP20.ExPollReqs, FramePhilipsMP20.ExPollReqs.Length, IP, 24105);

            if (State == ConnectState.GetData)
                Udp.Send(FramePhilipsMP20.ExPollReqs, FramePhilipsMP20.ExPollReqs.Length, IP, 24105);
        }
        //----------------------------------------------------------------------------------------------
        private void OnReceived(IPEndPoint EndPoint, Byte[] Buf, int Len)
        {
            Log.d("ID=" + ID.ToString() + "，接收该数据");
            //
            this.EndPoint = EndPoint;
            IP = EndPoint.Address.ToString();

            if (State != ConnectState.Connect)
            {
                State = ConnectState.Connect;
                if (onConnected != null)
                    onConnected();
            }
            //
            LastRecTime = DateTime.Now;

            if (Buf[0] == 0x00 && Buf[1] == 0x00 && Buf[2] == 0x01 && Buf[3] == 0x00)
            {//CONNECT INDICATION EVENT 接收到连接广播包
                Log.d("ID=" + ID.ToString() + "，接收到连接广播包");
                Udp.Send(FramePhilipsMP20.AssocReq, FramePhilipsMP20.AssocReq.Length, IP, 24105);
                return;
            }
            //------------------------------
            if (Buf[0] == 0x0E)
            {//接收到连接回复信息，发送确认连接包
                State = ConnectState.Connect;
                Log.d("ID=" + ID.ToString() + "，接收到连接回复信息");
                Udp.Send(FramePhilipsMP20.MdsResult, FramePhilipsMP20.MdsResult.Length, IP, 24105);
                return;
            }
            //------------------------------
            if (Buf[0] == 0xE1 && Buf[1] == 0x00 && Buf[8] == 0x00 && Buf[9] == 0x01 && Buf[10] == 0x00 && Buf[11] == 0x01)
            {//MDS CREATE EVENT RESULT
                State = ConnectState.GetData;
                //VM.Connected();
                Log.d("ID=" + ID.ToString() + "，MDS CREATE EVENT RESULT");
                invoke_id[0] = Buf[8];
                invoke_id[1] = Buf[9];
                Log.d(" Inv ID=" + invoke_id[0].ToString() + "," + invoke_id[1].ToString());
             //   Udp.Send(FramePhilipsMP20.GetProList, FramePhilipsMP20.MdsResult.Length, IP, 24105);
                return;
            }
            //------------------------------
            if (Buf[0] == 0xE1 && Buf[1] == 0x00 && Buf[2] == 0x00 && Buf[3] == 0x02 && Buf[12] == 0x00 && Buf[13] == 0x07)
            {
                //EXTENDED POLL DATA RESULT
                Log.d("ID=" + ID.ToString() + "，接收到数据包, Len=" + Len.ToString());
                //onGetParaGroup(
                for (int i = 0; i < G_List.Count; i++)
                    G_List[i].UnPack(Buf, Len);
                return;
            }
        }
        //----------------------------------------------------------------------------------------------
        /*private void OnDisconnect()
        {
            VM.DisConnected();
            ALL.Remove(this);
        }*/
        //----------------------------------------------------------------------------------------------
        private void CheckConnect()
        {
            /*if(VM==null || !VM.IsConnected) return;
            if (GLB.PassMs(LastRecTime) >= 10*1000)
                VM.DisConnected();*/
        }
        //----------------------------------------------------------------------------------------------
        public void GetParamater(int GroupID, String GroupName, short[] Values)
        {
            if (onGetParaGroup == null) return;
            onGetParaGroup(GroupName, Values);
        }
        //----------------------------------------------------------------------------------------------

    }
    //----------------------------------------------------------------------------------------------
}
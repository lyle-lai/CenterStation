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
    public class PrtPhilips : PrtBase
    {
        //----------------------------------------------------------------------------------------------
        private const int MIN_ID = 100;     //起始IP
        private const int LMT_COUNT = 100;   //最多连接台数
        private static UdpClient Udp = null;
        private static List<PrtPhilips> PrtList = new List<PrtPhilips>();
        private static Thread ThrUdp;//接收线程
        private static IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);//Any IP any port from remote point
        private static Byte[] invoke_id = new Byte[2];
        private static DateTime LastTime = DateTime.Now;
        //----------------------------------------------------------------------------------------------
        public static void Initialize()
        {
            Udp = new UdpClient(24005);
            ThrUdp = new Thread(UdpRun);
            ThrUdp.Start();
            for (int i = 0; i < LMT_COUNT; i++)
                PrtList.Add(new PrtPhilips(i));
            //----测试----
            /*
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Philips");
            while (dr.Read())
            {
                Byte[] Data = dr.GetBlob("Data");
                PrtList[2].OnReceived(Data, Data.Length);
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
                    int ID = (int)(((RemoteIpEndPoint.Address.Address & 0xFF000000) >> 24) - MIN_ID);
                    if (ID >= PrtList.Count) continue;
                    PrtList[ID].OnReceived(RBuf, RBuf.Length);
                }
                //发送获取数据请求
                if (GLB.PassMs(LastTime) >= 5000)
                {
                    LastTime = DateTime.Now;
                    for (int i = 0; i < PrtList.Count; i++)
                        PrtList[i].GetDataTick();
                }
                //----检查连接状态
                //for (int i = 0; i < PrtList.Count; i++)
                    //PrtList[i].CheckConnect();
                //
                Thread.Sleep(100);
            }
        }
        //----------------------------------------------------------------------------------------------
        public static void Stop()
        {
        }
        //----------------------------------------------------------------------------------------------
        private List<PrtPhilipsGroup> G_List = new List<PrtPhilipsGroup>();
        //private ViewMonitor VM = null;
        private int ID = -1;
        private enum ConnectState { OffLine, Connect, GetData };
        private ConnectState State = ConnectState.OffLine;
        private String IP = "";
        private DateTime LastRecTime = DateTime.Now;
        //----------------------------------------------------------------------------------------------
        public PrtPhilips(int Index):base(Index)
        {
            ID = Index;
            IP = "192.168.66." + (ID + MIN_ID).ToString();
            /*if (ID < 0 || ID >= LayoutBedView.VM.Length) return;
            VM = LayoutBedView.VM[ID];
            //
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From PrtPhilipsMP20 Group By [GroupName]");
            while (dr.Read())
                G_List.Add(new PrtPhilipsGroup(dr, VM));*/
        }
        //----------------------------------------------------------------------------------------------
        public void GetDataTick()
        {
            if (State == ConnectState.GetData)
                Udp.Send(FramePhilipsMP20.ExPollReqs, FramePhilipsMP20.ExPollReqs.Length, IP, 24105);
        }
        //----------------------------------------------------------------------------------------------
        private void OnReceived(Byte[] Buf, int Len)
        {
            Log.d("ID=" + ID.ToString() + "，接收该数据");
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
                if (onConnected != null)
                    onConnected();
                //VM.Connected();
                Log.d("ID=" + ID.ToString() + "，MDS CREATE EVENT RESULT");
                invoke_id[0] = Buf[8];
                invoke_id[1] = Buf[9];
                Log.d(" Inv ID=" + invoke_id[0].ToString() + "," + invoke_id[1].ToString());

               // Udp.Send(FramePhilipsMP20.SingleReqs, FramePhilipsMP20.MdsResult.Length, IP, 24105);
                return;
            }
            //------------------------------
            if (Buf[0] == 0xE1 && Buf[1] == 0x00 && Buf[2] == 0x00 && Buf[3] == 0x02 && Buf[12] == 0x00 && Buf[13] == 0x07)
            {
                //EXTENDED POLL DATA RESULT
                Log.d("ID=" + ID.ToString() + "，接收到数据包, Len=" + Len.ToString());
                for (int i = 0; i < G_List.Count; i++)
                    G_List[i].UnPack(Buf, Len);
                return;
            }
        }
        //----------------------------------------------------------------------------------------------
        /*private void CheckConnect()
        {
            if(VM==null || !VM.IsConnected) return;
            if (GLB.PassMs(LastRecTime) >= 10*1000)
                VM.DisConnected();
        }*/
        //----------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------
}
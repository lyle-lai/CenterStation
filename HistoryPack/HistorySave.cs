using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViewPack;
using MGOA_Pack;
using GlobalClass;
using System.Threading;
using System.Runtime.InteropServices;
//-----------------------------------------------------------------------------
namespace HistoryPack
{
    //Class HistorySave-----------------------------------------------------------------------------
    public class HistorySave
    {
        private static Thread SaveThread = null;//new Thread(new ThreadStart(Run));
        private static void Run()
        {
            while (true)
            {
                Thread.Sleep(5000);
                SavePara();
                SaveWave();
            }
        }
        //-----------------------------------------------------------------------------
        public static void Initialize()
        {//初始化
            SaveThread = new Thread(new ThreadStart(Run));
            SaveThread.Start();
        }
        //-----------------------------------------------------------------------------
        private static void SavePara()
        {
            /*DBConnect Conn = DBConnect.DATA;
            DateTime dt = DateTime.Now;
            Conn.BeginTransaction();

            for (int iMonitor = 0; iMonitor < LayoutBedView.VM.Length; iMonitor++)
            {
                ViewMonitor Monitor = LayoutBedView.VM[iMonitor];
                if (Monitor == null || Monitor.Para_List == null) continue;

                for (int iPara = 0; iPara < Monitor.Para_List.Count; iPara++)
                {
                    ParaGroup PGrp = Monitor.Para_List[iPara];
                    if (!PGrp.NeedSave) continue;
                    PGrp.NeedSave = false;
                    //
                    Conn.AddPara("@ConnectID", iMonitor);
                    Conn.AddPara("@dTime", dt);
                    Conn.AddPara("@GroupName", PGrp.FullSN);
                    Conn.AddPara("@Value", PGrp.ValBuf);
                    Conn.ExecSQL("Insert Into Para (ConnectID, dTime, GroupName, Value) Values (@ConnectID, @dTime, @GroupName, @Value)");
                }
            }
            Conn.Commit();*/
        }
        //-----------------------------------------------------------------------------
        private static void SaveWave()
        {            //
            /*DBConnect Conn = DBConnect.DATA;
            DateTime dt = DateTime.Now;
            Conn.BeginTransaction();

            for (int iMonitor = 0; iMonitor < LayoutBedView.VM.Length; iMonitor++)
            {
                ViewMonitor Monitor = LayoutBedView.VM[iMonitor];
                if (Monitor==null || Monitor.Wave_List == null) continue;

                for (int iWave = 0; iWave < Monitor.Wave_List.Count; iWave++)
                {
                    WaveGroup WGrp = Monitor.Wave_List[iWave];
                    Conn.AddPara("@ConnectID", iMonitor);
                    Conn.AddPara("@dTime", dt);
                    Conn.AddPara("@GroupName", WGrp.FullSN);
                    Conn.AddPara("@Value", WGrp.HistoryOuter.GetAllBuffer());
                    //
                    Conn.ExecSQL("Insert Into Para (ConnectID, dTime, GroupName, Value) Values (@ConnectID, @dTime, @GroupName, @Value)");
                }
            }
            Conn.Commit();*/
        }
        //-----------------------------------------------------------------------------
        public static void Stop()
        {
            if (SaveThread != null)
                SaveThread.Abort();
        }
        //-----------------------------------------------------------------------------
    }
    //Class HistorySave-----------------------------------------------------------------------------
}

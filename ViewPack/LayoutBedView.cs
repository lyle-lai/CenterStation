using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GlobalClass;
using ProtocolPack;
using System.Threading;
using MGOA_Pack;
using ObjPack;
//---------------------------------------------------------------------------------------------------
namespace ViewPack
{
    //---------------------------------------------------------------------------------------------------
    public class LayoutBedView
    {
        public static ViewMonitor[] VM = null;
        private static Panel Parent = null;
        private static Thread ThrTimer = null;
        public delegate void OnSubmitPara(ParaGroup PGroup, String BedID, String InHosNum, String DeviceID, String BoxNum);
        public static OnSubmitPara SubmitPara = null;

        public delegate void OnFullScreea(ViewMonitor viewMonitor);
        public static event OnFullScreea fullScreen = null;

        public static  Dictionary<string, string> dicIPBed = new Dictionary<string, string>();
        //---------------------------------------------------------------------
        public static void Initialize(Panel Pan)
        {//构造函数
            Parent = Pan;
            Create(Config.Rows, Config.Cols);
            Layout(Config.Rows, Config.Cols);
            ThrTimer = new Thread(new ThreadStart(TimerRun));
            ThrTimer.Start();
        }
        //---------------------------------------------------------------------
        private static void TimerRun()
        {
            while (true)
            {
                RefreshDeviceInfo();
                Thread.Sleep(10000);
            }
        }
        //---------------------------------------------------------------------
        public static void SubmitData()
        {
            try
            {
                if (VM == null || SubmitPara == null) return;
                for (int i = 0; i < VM.Length; i++)
                {
                    for (int j = 0; j < VM[i].Para_List.Count; j++)
                    {
                        if (VM[i].IsVisible)
                            SubmitPara(VM[i].Para_List[j], VM[i].BedNum, VM[i].InHosNum, VM[i].DeviceID, VM[i].BedNum);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //---------------------------------------------------------------------
        public static void CloseAll()
        {
            if (VM == null) return;

            for(int i = 0; i < VM.Length;i++)
            {
                VM[i].ServerBase.Close();
            }
        }
        //---------------------------------------------------------------------
        public static void Create(int Rows, int Cols)
        {
            int num = 0;

            Dictionary<int, List<AlarmPara>> dicAlarmPara = new Dictionary<int, List<AlarmPara>>(); 
            DBReader drAlarmPara = new DBReader(DBConnect.SYS);
            drAlarmPara.Select("select * from AlarmPara");  // modify_by_limu_160606
            while (drAlarmPara.Read())
            {
                AlarmPara para = new AlarmPara();
                para.DeviceID = drAlarmPara.GetInt("DeviceID");
                para.DeviceSN = drAlarmPara.GetStr("DeviceSN");
                para.High = drAlarmPara.GetF("High", 0);
                para.Low = drAlarmPara.GetF("Low", 0);
                para.ID = drAlarmPara.GetInt("ID");
                para.isEnabled = drAlarmPara.GetInt("isEnabled");
                para.isRecord = drAlarmPara.GetInt("isRecord");
                para.Level = drAlarmPara.GetInt("Level");
                para.ParaName = drAlarmPara.GetStr("ParaName");
                if (!dicAlarmPara.ContainsKey(para.DeviceID))
                {
                    List<AlarmPara> list = new List<AlarmPara>();
                    list.Add(para);
                    dicAlarmPara.Add(para.DeviceID, list);
                }
                else
                {
                    dicAlarmPara[para.DeviceID].Add(para);
                }
            }

            Parent.Controls.Clear();
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Device where status = '1' Order By ID asc");  // modify_by_limu_160606
            Dictionary<int, Device> dicDev = new Dictionary<int, Device>(); 
            while (dr.Read())
            {
               Device dev = new Device();
               dev.ID = dr.GetInt("ID");
               dev.ARCHIVESID = dr.GetStr("ARCHIVESID");
               dev.BARCODE = dr.GetStr("BARCODE");
               dev.BEDNUM = dr.GetStr("BEDNUM");
               dev.DEVICEID = dr.GetStr("DEVICEID");

               dev.DEVICEIP = dr.GetStr("DEVICEIP");
               dev.DEVICETYPE = dr.GetStr("DEVICETYPE");
               dev.INHOSNUM = dr.GetStr("INHOSNUM");
               dev.PATIENTNAME = dr.GetStr("PATIENTNAME");
               dev.STATUS = dr.GetStr("STATUS");
               dicDev.Add(num, dev);
               num++;
            }
          

            VM = new ViewMonitor[num];
            for (int i = 0; i < num; i++)
            {
                if (dicDev.ContainsKey(i))
                {
                    if (dicAlarmPara.ContainsKey(dicDev[i].ID))
                    {
                        VM[i] = new ViewMonitor(i, dicAlarmPara[dicDev[i].ID]);
                    }
                    else if (dicAlarmPara.ContainsKey(0))
                    {
                        VM[i] = new ViewMonitor(i, dicAlarmPara[0]);
                    }
                    else
                    {
                        VM[i] = new ViewMonitor(i, null);
                    }
                    VM[i].Parent = Parent;
                    VM[i].Visible = true;
                    VM[i].Tag = dicDev[i];
                    //
                    Parent.Controls.Add(VM[i]);
                }


            }
            RefreshDeviceInfo();
            /*
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From BedDevMapping Order By BedID");
            while (dr.Read())
            {
                int ip = dr.GetInt("IP");
                int id = dr.GetInt("BedID");
                if (ip >= 0 && ip < PrtDCS_Server.PrtList.Length && id >= 0 && id < VM.Length)
                {
                    VM[id].SetIP(ip.ToString());
                    VM[id].SetBedSN(dr.GetStr("BedSN"));
                }
            }*/
        }

        //---------------------------------------------------------------------
        public static int RefreshDeviceInfo()
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Device where status = '1' Order By NUMSORT asc");  // modify_by_limu_160606
            int i = 0;
            while (dr.Read())
            {
                if (i >= VM.Length) break;
                if (VM[i] != null)
                {
                    if (VM[i].VTitle != null)
                    {
                        VM[i].VTitle.PatientName = dr.GetStr("PatientName");
                        VM[i].VTitle.Id = dr.GetStr("ID");
                        VM[i].VTitle.BarCode = dr.GetStr("BARCODE");
                        VM[i].SetIP(dr.GetStr("DeviceIP"));
                        VM[i].SetBedSN(dr.GetStr("BedNum"));
                    }
                    VM[i].DeviceType = dr.GetStr("DEVICETYPE");
                    VM[i].InHosNum = dr.GetStr("InHosNum");
                    VM[i].DeviceID = dr.GetStr("DeviceID");
                    VM[i].BoxNum = dr.GetStr("BedNum");
                    VM[i].BedNum = dr.GetStr("BedNum");
                    VM[i].CreateHistory(dr.GetInt("ID"));    // 开始历史记录

                    if (null == VM[i].ServerBase)
                    {
                        PrtServerBase ServerBase = PrtFactory.Build(VM[i].DeviceType, VM[i].ViewIP);
                        if (null != ServerBase)
                        {
                            VM[i].SetServerBase(ServerBase);
                        }
                    }
                    
                }
                i++;
            }

            // add_by_limu_160606
            while (i < VM.Length)
            {
                if (VM[i] != null)
                {
                    if (VM[i].VTitle != null)
                    {
                       // VM[i].VTitle.PatientName = string.Empty;
                    }
                    
                    VM[i].SetIP(string.Empty);
                    VM[i].SetBedSN(string.Empty);
                    VM[i].InHosNum = string.Empty;
                    VM[i].DeviceID = string.Empty;
                    VM[i].BoxNum = string.Empty;
                    VM[i].BedNum = string.Empty;
                    VM[i].DeviceType = string.Empty;
                    VM[i].CloseHistory();   // 停止历史记录
                    i++;
                }

            }
            return i;
        }
        //---------------------------------------------------------------------
        public static void Layout(int Rows, int Cols)
        {
            if (Parent == null) return;
            //按照行列布局
            int y = 0;
            int num = VM.Length % Cols == 0 ? VM.Length / Cols : (VM.Length / Cols)+1;

            for (int r = 0; r < num; r++)
            {
                int h = 0;
                h = Parent.ClientSize.Height / Rows;
                int x = 0;
                for (int c = 0; c < Cols; c++)
                {
                    int i = r * Cols + c;
                    if (i >= VM.Length) break;
                    int w = (Parent.ClientSize.Width - x) / (Cols - c);
                    VM[i].Width = w;
                    VM[i].Height = h;
                    VM[i].Top = y;
                    VM[i].Left = x;
                    VM[i].LayoutView();
                    x += w;
                }
                y += h;
            }
        }
        //-----------------------------------------------------------------------
        public static void CheckProtocolBind()
        {
            if (VM == null) return;
            /*Log.d("---- CheckProtocolBind ----");
            for (int iPrt = 0; iPrt < PrtServerBase.ALL.Count; iPrt++)
            {
                Log.d("IP4=" + PrtServerBase.ALL[iPrt].IP4);
            }*/
            /** -- 原始代码 --
            //匹配
            for (int iVm = 0; iVm < VM.Length; iVm++)
            {
                //Log.d("ViewIP=" + VM[iVm].ViewIP);
                for (int iPrt = 0; iPrt < PrtServerBase.ALL.Count; iPrt++)
                {
                    if (GLB.Same(VM[iVm].ViewIP, PrtServerBase.ALL[iPrt].IP4))
                    {
                        //Log.d("Same:" + VM[iVm].ViewIP);
                        VM[iVm].SetServerBase(PrtServerBase.ALL[iPrt]);
                    }
                }
            }*/

            bool status = false; // add_by_limu_160607
            for (int iVm = 0; iVm < VM.Length; iVm++)
            {
                for (int iPrt = 0; iPrt < PrtServerBase.ALL.Count; iPrt++)
                {
                    if (GLB.Same(VM[iVm].ViewIP, PrtServerBase.ALL[iPrt].IP4))
                    {
                        for (int i = iVm + 1; i < VM.Length; i++)
                        {
                            if (GLB.Same(VM[i].ViewIP, PrtServerBase.ALL[iPrt].IP4))
                            {
                                VM[i].DisConnected(VM[i].ServerBase.TCP);
                                break;
                            }
                        }
                        VM[iVm].SetServerBase(PrtServerBase.ALL[iPrt]);
                        //VM[iVm].Connected();
                        if (PrtServerBase.ALL[iPrt].State != PrtServerBase.ConnectState.OffLine)
                        {
                            VM[iVm].Connected();
                        }

                        status = true;
                    }
                }
                if (status == false)
                {
                    VM[iVm].ClearView();
                }
                status = false;
            }
            
        }

        public static void CheckSimpleProtocolBind(ViewMonitor Monitor)
        {
            bool status = false; // add_by_limu_160607
            for (int iPrt = 0; iPrt < PrtServerBase.ALL.Count; iPrt++)
            {
                if (GLB.Same(Monitor.ViewIP, PrtServerBase.ALL[iPrt].IP4))
                {
                    //if (GLB.Same(Monitor.ViewIP, PrtServerBase.ALL[iPrt].IP4))
                    //{
                    //    Monitor.DisConnected();
                    //    break;
                    //}
                    Monitor.SetServerBase(PrtServerBase.ALL[iPrt]);
                    Monitor.Connected();
                    status = true;
                    break;
                }
            }
            if (status == false)
            {
                Monitor.ClearView();
            }
        }

        //-----------------------------------------------------------------------
        public static void ParaTick()
        {
            if (VM == null) return;
            Alarm.Tick();
            for (int i = 0; i < VM.Length; i++)
                VM[i].ParaTick();
        }

        public static void ParaSimpleTick(ViewMonitor viewMonitor)
        {
            if (viewMonitor != null)
            {
                viewMonitor.ParaTick();
            }
        }
        //-----------------------------------------------------------------------
        public static void WaveTick()
        {
            if (VM == null) return;
            for (int i = 0; i < VM.Length; i++)
                VM[i].WaveTick();
        }
        //-----------------------------------------------------------------------
        public static void DemoTick()
        {
            if (VM == null) return;
            for (int i = 0; i < VM.Length; i++)
            {
                try
                {
                    //VM[i].IsConnected = true;
                    VM[i].DemoTick();
                }
                catch(Exception ex)
                {

                }
                
            }
        }
        //-----------------------------------------------------------------------
        public static void AlarmSetting(int ViewID, Boolean Syn, String Path, String SN, Double Low, Double High)
        {
            if (ViewID < 0)
            {
                for (int i = 0; i < VM.Length; i++)
                    VM[i].AlarmSetting(Syn, Path, SN, Low, High);
            }
            else
            {
                VM[ViewID].AlarmSetting(Syn, Path, SN, Low, High);
            }
        }
        //-----------------------------------------------------------------------
    }
    //---------------------------------------------------------------------------------------------------
}
//---------------------------------------------------------------------------------------------------

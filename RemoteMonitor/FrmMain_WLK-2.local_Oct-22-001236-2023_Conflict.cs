using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//-----------------------------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
using ViewPack;
using DemoPack;
using History_Pack;
using ProtocolPack;
using Settings;
using System.Runtime.InteropServices;
using Kenel32Pack;
using System.Threading;
using DotNetSpeech;
using ObjPack;
// using DotNetSpeech;
//using System.Threading.Tasks;
//-----------------------------------------------------------------------------------------
namespace RemoteMonitor
{
    public partial class FrmMain : Form
    {
        private FrmLayoutSetting frmLayoutSetting = new FrmLayoutSetting();
        private FrmHistory frmHistory = new FrmHistory();
        private FrmAbout frmAbout = new FrmAbout();
        private FrmLicence frmLicence = new FrmLicence();
        private FrmSysSetting frmSysSetting = new FrmSysSetting();
        private FrmBedManage frmBedManage = new FrmBedManage();
        
        //语音播报
        private ConcurrentQueue<string> alarmQueue = new ConcurrentQueue<string>();
        private Thread speechThread;
        private bool speechTreadStarted = true;
        private DotNetSpeech.SpeechVoiceSpeakFlags SSF = DotNetSpeech.SpeechVoiceSpeakFlags.SVSFlagsAsync;
        private readonly DotNetSpeech.SpVoice vo = new SpVoiceClass();

        //private PrtDCS_Server Server = new PrtDCS_Server(0);
        //-----------------------------------------------------------------------------------------
        public FrmMain()
        {
            try
            {
              
              InitializeComponent();

 			  GLB.Init();
              //SubmitToReport.Initialize();
              //SubmitToReport.SubmitData = LayoutBedView.SubmitData;
              //LayoutBedView.SubmitPara = SubmitToReport.Submit;
              UnitGroup.Initialize();
              SCR.Initialize();
              ViewBase.Initialize();
           //   PanMain.Dock = DockStyle.Fill;
              // PrtPhilipsMP20.Initialize(null);
              // MR_CMS.Initialize();

              ShowSystemInfo();
              Alarm.Initialize();//初始化报警

              tbAlarmOn.Text = Alarm.AlarmOn ? "报警音开" : "报警音关";
              tbAlarmOn.BackColor = Alarm.AlarmOn ? Color.LightBlue : Color.DarkGray;// ToolBar.BackColor;
              PrtDCS_Server.Initialize();  //可能导致第二次启动 启动不了
              Settings.SetForms.Initialize();

              lblstatus.Text = string.Empty;
              this.Load += FrmMain_Load;
              ViewTitle.closeAlarmEvent += CloseAlarm;
              ViewTitle.fullScreenEvent += LayoutBedView_fullScreen;
              ViewTitle.AlarmConfigEvent += AlarmConfig;
              ViewMonitor.addAlarmRecord += ViewMonitor_addAlarmRecord;

              ParaItem.onAlarm += ParaItem_onAlarm;
              
              // 告警播报线程
              speechThread = new Thread(new ThreadStart(() =>
              {
                  do
                  {
                      if (alarmQueue.TryDequeue(out var text))
                      {
                          vo.Speak(text, SSF);
                      }
                      Thread.Sleep(1000);
                  } while (speechTreadStarted);
              }));
              speechThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Control.CheckForIllegalCrossThreadCalls = false;
            //
            //UDog.Initialize();
            //
           

            //DemoPack.DemoWave.Demo16To8();
          
            //MGOA.Initialize();
            
            
            //ViewParaGroup.Initialize();
            //ViewWaveGroup.Initialize();
            //-----------------------
        
            //LayoutBedView.Initialize(PanMain);
            //Demo.Initialize();
            //------------------------
            //飞利浦监护仪服务，初始化

            //------------------------
            //cbAlarmOn.Checked = Alarm.AlarmOn;

        }
        string strTxt = string.Empty;
        /// <summary>
        /// 如果告警就在屏幕上显示
        /// </summary>
        /// <param name="ViewID"></param>
        /// <param name="key"></param>
        /// <param name="curVal"></param>
        /// <param name="para"></param>
        private void ParaItem_onAlarm(int ViewID, string key, double curVal, AlarmPara para)
        {
            string AlarmType = string.Empty;
            if (curVal > para.High)
            {
                AlarmType = "超上限告警";
            }
            else if (curVal < para.Low)
            {
                AlarmType = "超下限告警";
            }
           ViewMonitor vMonitor=LayoutBedView.VM[ViewID];
           if (vMonitor != null)
           {
               strTxt = string.Format("床号:{0},监护仪编号:{1},参数：{2},最高值：{3}，最低值：{4}，当前值：{5}", vMonitor.BedNum,vMonitor.VTitle.BedSN,para.ParaName, para.High, para.Low, curVal);
               MethodInvoker mi = new MethodInvoker(SetValue);
               this.Invoke(mi);
               // 如果关闭了告警，就不需要出发告警事件了
               if (!vMonitor.isCloseAlarm)
               {
                   vMonitor.ParaItem_onAlarm(ViewID, key, curVal, para, alarmQueue);
               }
               
           }
            
        }
        private void SetValue()
        {
            lblstatus.Text = strTxt;
        }
        /// <summary>
        /// 关闭告警
        /// </summary>
        /// <param name="ViewID"></param>
        /// <param name="isClose"></param>
        private void CloseAlarm(int ViewID, bool isClose)
        {
            try
            {
                foreach (ViewMonitor vm in LayoutBedView.VM)
                {
                    if (vm.ViewID == ViewID)
                    {
                        vm.isCloseAlarm = isClose;
                        if (isClose)
                        {
                            vm.VTitle.BackColor = Color.LightBlue;
                        }
                        else
                        {
                            // 南方医需求，关闭告警时不改变title的背景色
                            // vm.VTitle.BackColor = Color.Red;
                            vm.VTitle.BackColor = Color.LightBlue;
                        }
                      
                        break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void FrmMain_Load(object sender, EventArgs e)
        {
            //行数大于4，采用滚动条模式,函数小于4，采用全屏模式
            PanMain.Width = this.ClientSize.Width - 10;
            PanMain.Height = this.ClientSize.Height - ToolBar.ClientSize.Height - 20;
        }

        void ViewMonitor_addAlarmRecord(AlarmRecord CurAlarmRecord)
        {

            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql = string.Format(@"insert into AlarmRecord(DeviceID,DeviceSN,AlarmType,BedNum,PatientName,Level,Val,ParaName,High,Low,AlarmTime)
                                              values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                    CurAlarmRecord.ID, CurAlarmRecord.DeviceSN, CurAlarmRecord.AlarmType, CurAlarmRecord.BedNum,
                    CurAlarmRecord.PatientName, CurAlarmRecord.Level, CurAlarmRecord.Val, CurAlarmRecord.ParaName, CurAlarmRecord.High, CurAlarmRecord.Low, CurAlarmRecord.AlarmTime);
            if (DBConnect.SYS != null)
            {

             //    DBConnect.SYS.ExecSQL(strSql);
                System.Threading.Thread.Sleep(1000);
            }
        }
        //-----------------------------------------------------------------------------------------
        // add_by_limu
        protected override void WndProc(ref Message m)
        {
            try
            {
                // 解决{按住ALT键会导致界面重绘的问题}
                // ref http://stackoverflow.com/questions/8848203/alt-key-causes-form-to-redraw
                // Suppress the WM_UPDATEUISTATE message
                if (m.Msg == 0x128) return;
                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
                throw;
            }

        }
        //-----------------------------------------------------------------------------------------
        private void timerPara_Tick(object sender, EventArgs e)
        {
            try
            {
                LayoutBedView.ParaTick();
                LayoutBedView.CheckProtocolBind();
            }
            catch(Exception  ex)
            {
                Log.d(ex.Message);
            }
        }


        /// <summary>
        /// 全屏显示
        /// </summary>
        /// <param name="viewMonitor"></param>
        private void LayoutBedView_fullScreen(ViewMonitor viewMonitor)
        {
            try
            {
                FrmFullScreen frmfullScreea = new FrmFullScreen(viewMonitor);
                frmfullScreea.MaximizeBox = true;
                frmfullScreea.StartPosition = FormStartPosition.CenterScreen;
                if (frmfullScreea.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }

        }

        /// <summary>
        /// 告警配置
        /// </summary>
        /// <param name="viewMonitor"></param>
        private void AlarmConfig(List<AlarmPara> ListAlarm,int DeviceID,string DeviceSN)
        {
            try
            {
                FrmAlarmPara frmALarmPara = new FrmAlarmPara(ListAlarm, DeviceID, DeviceSN);
                frmALarmPara.Owner = this;
                frmALarmPara.StartPosition = FormStartPosition.CenterScreen;
                if (frmALarmPara.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }

        }
        //-----------------------------------------------------------------------------------------
        private void timerWave_Tick(object sender, EventArgs e)
        {
            try
            {
                if (UDog.IsValidity)
                {
                    WaveGroup.Tick();
                    LayoutBedView.WaveTick();
                }
                else
                {
                    timerWave.Enabled = false;
                    NoDog();
                }
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
                throw;
            }

        }
        //-----------------------------------------------------------------------------------------
        private void PanMain_Resize(object sender, EventArgs e)
        {
            try
            {
                if (Config.Rows == 0) Config.Rows = 1;
                if (Config.Cols == 0) Config.Cols = 1;
                LayoutBedView.Layout(Config.Rows, Config.Cols);
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
                throw;
            }
  
          //  LayoutBedView.Layout(1, 1);
        }
        //-----------------------------------------------------------------------------------------
        private void tbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //-----------------------------------------------------------------------------------------
        private void tbLayout_Click(object sender, EventArgs e)
        {
            frmLayoutSetting.ShowDialog();
        }
        //-----------------------------------------------------------------------------------------
        private void timerStart_Tick(object sender, EventArgs e)
        {
            try
            {
                timerStart.Enabled = false;
                LayoutBedView.Initialize(PanMain);

                timerPara.Enabled = true;
                timerDemo.Enabled = true;
                //
                timerWave.Enabled = true;

              //  PrtPhilipsMP20.Initialize();
              //  HistorySave.Initialize();
               // Demo.Initialize();

            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }
        }
        //-----------------------------------------------------------------------------------------
        private void Method3(Object state)
        {
            if (UDog.IsValidity)
            {
                WaveGroup.Tick();
                LayoutBedView.WaveTick();
            }
            else
            {
                timerWave.Enabled = false;
                NoDog();
            }
        }
        //-----------------------------------------------------------------------------------------
        private void timerDemo_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!GLB.Demo) return;
                LayoutBedView.DemoTick();
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }

        }
        //-----------------------------------------------------------------------------------------
        private void tbHistory_Click(object sender, EventArgs e)
        {
            try
            {
                frmBedManage.Left = this.Left;
                frmHistory.Top = ToolBar.Top - frmHistory.Height + SystemInformation.CaptionHeight;

                frmHistory.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
                throw;
            }

        }
        //-----------------------------------------------------------------------------------------
        private void tbAbout_Click(object sender, EventArgs e)
        {
            frmAbout.ShowDialog();
        }
        //-----------------------------------------------------------------------------------------
        private void tbSetting_Click(object sender, EventArgs e)
        {
            frmSysSetting.Left = this.Left > 0 ? this.Left : 0;
            frmBedManage.Left = this.Left;
            frmSysSetting.Top = ToolBar.Top - frmSysSetting.Height + SystemInformation.CaptionHeight;
            frmSysSetting.ShowDialog();
            ShowSystemInfo();
        }
        //-----------------------------------------------------------------------------------------
        private void tbBedManage_Click(object sender, EventArgs e)
        {
            frmBedManage.Left = this.Left;
            frmBedManage.Top = ToolBar.Top - frmBedManage.Height + SystemInformation.CaptionHeight;
            frmBedManage.ShowDialog();
            if (frmBedManage.IsChanged)
                LayoutBedView.Initialize(PanMain);
        }
        //-----------------------------------------------------------------------------------------
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                speechTreadStarted = false;
                PrtDCS_Server.Close();
                MR_CMS.Close();
                DBConnect.CloseConn();
                GC.Collect();
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }

            /*
            timerWave.Enabled = false;
            timerDemo.Enabled = false;
            timerPara.Enabled = false;
            timerStart.Enabled = false;
            //HistoryPack.HistorySave.Stop();
            for (int i = 0; i < LayoutBedView.VM.Length; i++)
                LayoutBedView.VM[i].Stop();*/
        }
        //-----------------------------------------------------------------------------------------
        private void tmDemo_Click(object sender, EventArgs e)
        {
            GLB.Demo = !GLB.Demo;
        }
        //-----------------------------------------------------------------------------------------
        private void ShowSystemInfo()
        {
            try
            {
                this.Text = Config.Get("Sys", "Hospital", "") + " " + Config.Get("Sys", "Department", "") + " 中央监护系统";
            }
            catch (Exception ex)
            {
                Log.d(ex.Message); 
                throw;
            }
    
        }
        //-----------------------------------------------------------------------------------------
        private void NoDog()
        {
            for (int i = 0; i < LayoutBedView.VM.Length; i++)
                LayoutBedView.VM[i].Visible = false;
            frmLicence.ShowDialog();

            System.Environment.Exit(0);
        }
        //-----------------------------------------------------------------------------------------
        private void tbAlarmOn_Click(object sender, EventArgs e)
        {
            Alarm.AlarmOn = !Alarm.AlarmOn;
            tbAlarmOn.Text = Alarm.AlarmOn ? "报警音开" : "报警音关";
            tbAlarmOn.BackColor = Alarm.AlarmOn ? Color.LightBlue : Color.DarkGray;// ToolBar.BackColor;
        }

        /// <summary>
        /// 监护仪配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenMonitor_Click(object sender, EventArgs e)
        {
            FrmMonitor frmMonitor = new FrmMonitor();
            frmMonitor.Owner = this;
            frmMonitor.StartPosition = FormStartPosition.CenterScreen;
            if (frmMonitor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        /// <summary>
        /// 监护仪配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonitorConfig_Click(object sender, EventArgs e)
        {
            try
            {
                FrmMonitorConfig frmMonitorConfig = new FrmMonitorConfig();
                frmMonitorConfig.StartPosition = FormStartPosition.CenterScreen;
                frmMonitorConfig.Owner = this;
                if (frmMonitorConfig.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LayoutBedView.Create(Config.Rows, Config.Cols);
                    LayoutBedView.Layout(Config.Rows, Config.Cols);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 告警配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlarmConfig_Click(object sender, EventArgs e)
        {
            try
            {
                FrmAlarmPara frmALarmPara = new FrmAlarmPara(null);
                frmALarmPara.Owner = this;
                frmALarmPara.StartPosition = FormStartPosition.CenterScreen;
                if (frmALarmPara.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LayoutBedView.Create(Config.Rows, Config.Cols);
                    LayoutBedView.Layout(Config.Rows, Config.Cols);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 告警参数查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                FrmAlarmQuery frmAlarmQuery = new FrmAlarmQuery();
                frmAlarmQuery.Owner = this;
                frmAlarmQuery.StartPosition = FormStartPosition.CenterScreen;
                if (frmAlarmQuery.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
            catch (Exception)
            {
            }
        }

        

        private void btnSystemSet_Click(object sender, EventArgs e)
        {
            FrmMonitor frmMonitor = new FrmMonitor();
            frmMonitor.Owner = this;
            frmMonitor.StartPosition = FormStartPosition.CenterScreen;
            if (frmMonitor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void btnDemo_Click(object sender, EventArgs e)
        {
            GLB.Demo = !GLB.Demo;
        }

        //-----------------------------------------------------------------------------------------
        // private void PanMain_Paint(object sender, PaintEventArgs e)
        // {
        //     throw new System.NotImplementedException();
        // }
    }
}

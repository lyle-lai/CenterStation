using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GlobalClass;
using Kenel32Pack;
using MGOA_Pack;
using ObjPack;
using ViewPack;

namespace RemoteMonitor
{
    public partial class FrmFullScreen : Form
    {

        private ViewMonitor mViewMonitor = null;
        private ViewMonitor mView;
        private List<AlarmPara> mAlarmPara = null;
        public FrmFullScreen(ViewMonitor view)
        {
            InitializeComponent();
            this.mView = view;
            this.mAlarmPara = view.mALarmPara;
            this.Load += FrmFullScreen_Load;
            // this.Width = (Screen.PrimaryScreen.Bounds.Width / 3)*2;
            // this.Height = (Screen.PrimaryScreen.Bounds.Height / 3)*2;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        /// <summary>
        /// 加载窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void FrmFullScreen_Load(object sender, EventArgs e)
        {
            try
            {
                mViewMonitor = new ViewMonitor(mView.ViewID, mAlarmPara, false);
               // mViewMonitor.VTitle = mView.VTitle;
                mViewMonitor.ViewID = mView.ViewID;
                mViewMonitor.VTitle.PatientName = mView.VTitle.PatientName;
                mViewMonitor.VTitle.BedSN = mView.VTitle.BedSN;
                mViewMonitor.VTitle.BedName = mView.VTitle.BedName;
                mViewMonitor.VTitle.Id = mView.VTitle.Id;
                // mViewMonitor.VTitle.ViewTitleFont = new Font("微软雅黑", 26, FontStyle.Bold);
                // mViewMonitor.VTitle.Height = 50;
                mViewMonitor.VTitle.ViewTitleFont = mView.VTitle.ViewTitleFont;
                mViewMonitor.VTitle.Height = mView.VTitle.Height;
                mViewMonitor.ViewIP = mView.ViewIP;
                mViewMonitor.Width = PanMain.ClientSize.Width;
                mViewMonitor.Height = PanMain.ClientSize.Height - mViewMonitor.VTitle.Height;
                mViewMonitor.Left = 5;
                mViewMonitor.LayoutView();
                //mViewMonitor.VTitle.Paint();
                PanMain.Controls.Add(mViewMonitor);
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }
        }

     

        private void timerStart_Tick(object sender, EventArgs e)
        {
            try
            {
                timerStart.Enabled = false;
              //  LayoutBedView.Initialize(PanMain);
                if (mViewMonitor != null)
                {
                    // mViewMonitor.Width = PanMain.Width;
                    // mViewMonitor.Height = PanMain.Height;
                    // mViewMonitor.Top = 0;
                    // mViewMonitor.Left = 0;
                    // mViewMonitor.Refresh();
                    mViewMonitor.Width = PanMain.ClientSize.Width;
                    mViewMonitor.Height = PanMain.ClientSize.Height - mViewMonitor.VTitle.Height;
                    mViewMonitor.Top = mViewMonitor.VTitle.Height;
                    mViewMonitor.Left = 0;
                    mViewMonitor.Refresh();
                }

                timerPara.Enabled = true;
                timerWave.Enabled = true;
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }
        }
        private void timerPara_Tick(object sender, EventArgs e)
        {
            try
            {
                if (mViewMonitor != null)
                {
                    mViewMonitor.ParaTick(false);
                   // LayoutBedView.ParaSimpleTick(mViewMonitor);
                    LayoutBedView.CheckSimpleProtocolBind(mViewMonitor);
                }
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }
        }

        private void timerWave_Tick(object sender, EventArgs e)
        {
            try
            {
                if (UDog.IsValidity)
                {
                    WaveGroup.Tick();
                    mViewMonitor.WaveTick();
                }
                else
                {
                    timerWave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }

        }

        private void FrmFullScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}

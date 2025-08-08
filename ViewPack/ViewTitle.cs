using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
//-----------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
using Settings;
using ProtocolPack;
using ObjPack;
//-----------------------------------------------------------------------
namespace ViewPack
{
    //-----------------------------------------------------------------------
    public class ViewTitle : ViewBase
    {
        private int ViewID = 0;
        public String BedName;    //床位号

        public delegate void OnFullScreea(ViewMonitor viewMonitor);
        public static event OnFullScreea fullScreenEvent = null;

        public delegate void OnAlarmConfig(List<AlarmPara> listAlarmPara,int DeviceID,string DeviceSN);
        public static event OnAlarmConfig AlarmConfigEvent = null;

        public delegate void OnCloseAlarm(int ViewID,bool isOpen);
        public static event OnCloseAlarm closeAlarmEvent = null;

        private string mViewIP = string.Empty;
        public String ViewIP
        {
            get { return mViewIP; }
            set {
                try
                {
                    if (mViewIP != value)
                    {
                        BedSN = GetBedNameInfo(value.Trim());
                    }
                }
                catch (Exception ex)
                {
                    Log.d(ex.Message);
                }
                mViewIP = value;
           // RefreshBedInfo();
         
            }
        }
        public String PatientName; //病人姓名
        public String Id;
        public String PatientSex;  //病人性别
        public String PatientAge;  //病人年龄
        public String BedSN = string.Empty;
        public String BarCode = string.Empty;
        //
        private String TitleText = "";  //标题显示文字
        private Button MenuBtn = null;
        private ContextMenuStrip Menu = new ContextMenuStrip();
        public ToolStripMenuItem[] MenuItem ;

      //  public Label lblBedNum= new Label();
        //------------------------------------------------------------------------------
        public ViewTitle(int ViewID,bool isShowScrenn)
        {
            this.ViewID = ViewID;
            if (isShowScrenn)
            {
                MenuItem = new ToolStripMenuItem[4];
            }
            else
            {
                MenuItem = new ToolStripMenuItem[3];
            }
            //lblBedNum.Parent = this;
            //lblBedNum.Visible = false;
            //lblBedNum.Width = 60;
            //lblBedNum.Top = 5;
            //lblBedNum.Left = 10;
            //lblBedNum.Show();

            //InitBaseSize();
            //创建弹出菜单按钮
            MenuBtn = new Button();
            MenuBtn.Parent = this;
            MenuBtn.Visible = true;
            MenuBtn.Dock = DockStyle.Right;
            MenuBtn.Width = 64;
            MenuBtn.Text = "菜单";
            MenuBtn.Click += OnButtonClick;
            //
            int i = 0;
            MenuItem[i] = new ToolStripMenuItem("信息设置", null, new EventHandler(MenuBed));
            MenuItem[i].Visible = true;  // modify_by_limu_160606
            i++;
            //MenuItem[i] = new ToolStripMenuItem("血压测量", null, new EventHandler(MenuNIBP));
            //MenuItem[i].Visible = false;

            MenuItem[i] = new ToolStripMenuItem("告警参数", null, new EventHandler(AlarmParaConfig));
            MenuItem[i].Visible = true;
            i++;

            MenuItem[i] = new ToolStripMenuItem("关闭告警", null, new EventHandler(CloseAlarm));
            if (isShowScrenn)
            {
                i++;
                MenuItem[i] = new ToolStripMenuItem("大屏显示", null, new EventHandler(MenuFullScreen));
            }

            //i++;
            //MenuItem[i] = new ToolStripMenuItem("选择病人", null, new EventHandler(MenuPatient));
            //將菜單项加入到菜单
            Menu.Items.AddRange(MenuItem);
            //5、調用快速功能表
            MenuBtn.ContextMenuStrip = Menu;
        }

        public void CancelFullScreen()
        {
            if (MenuItem.Length >= 3 && MenuItem[2] != null)
            {
                MenuItem[2].Visible = false;
                Menu.Refresh();
            }
        }
        //------------------------------------------------------------------------------
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            //LayoutView();
        }
        //------------------------------------------------------------------------------
        private void OnButtonClick(object sender, EventArgs e)
        {
            Menu.Show(MenuBtn, 0, MenuBtn.Height);
        }
        //------------------------------------------------------------------------------
        public override void Tick()
        {
            int a = 0;
        }
        //------------------------------------------------------------------------------
        public Boolean TitleIsChange()
        {
            String  Str = PatientName + " (" + BedName + ")" + ViewIP;
            bool bl=!GLB.Same(Str, TitleText);
            return bl;
        }
        //------------------------------------------------------------------------------

        //public  Font ViewTitleFont = GLB.SysFont;
        // public Font ViewTitleFont = new Font(GLB.SysFontName, 30, FontStyle.Regular);
        public Font ViewTitleFont = new Font(GLB.SysFontName, 10, FontStyle.Regular);
        public SolidBrush ViewTitleBrush =  new SolidBrush(Color.MediumBlue);
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            //g.DrawString((ViewID + 1).ToString() + "床 (IP:" + ViewIP + ")", GLB.SysFont, GLB.SysBrush, 0, 0);
            //g.DrawString(BedName + " (IP:" + ViewIP + ")", GLB.SysFont, GLB.SysBrush, 0, 4);
            
            // modify_by_limu_160606
            TitleText = BedName + ":" + PatientName + ":" + BedSN;
           // TitleText = PatientName+"";

            g.DrawString(TitleText, ViewTitleFont, ViewTitleBrush, 0, 4);
        }
        //------------------------------------------------------------------------------
        private void MenuBed(object sender, EventArgs e)
        {//
            Settings.SetForms.frmBed.Id = Id;
            Settings.SetForms.frmBed.txbBedNum.Text = BedName;
            Settings.SetForms.frmBed.txbPatient.Text = PatientName;
            Settings.SetForms.frmBed.txbCode.Text = BarCode;
            ShowFormOfMenu(Settings.SetForms.frmBed);
            this.PatientName = Settings.SetForms.frmBed.txbPatient.Text;
            this.BedName = Settings.SetForms.frmBed.txbBedNum.Text;
            this.Refresh();
        }
        //------------------------------------------------------------------------------
        private void MenuNIBP(object sender, EventArgs e)
        {//
            byte[] B = MR_Packager.SetMeasureNIBPCmd();
            //byte[] B = MR_Packager.SetNULL();            
        }

        private void MenuFullScreen(object sender, EventArgs e)
        {
            if (fullScreenEvent != null)
            {
                fullScreenEvent(this.Parent as ViewMonitor);
            }
        }

        /// <summary>
        /// 告警参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmParaConfig(object sender, EventArgs e)
        {
            if (AlarmConfigEvent != null)
            {
                ViewMonitor view = this.Parent as ViewMonitor;
               Device dev=view.Tag as Device;
               AlarmConfigEvent(view.mALarmPara, dev.ID, BedSN);
            }
        }

        /// <summary>
        /// 关闭告警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAlarm(object sender, EventArgs e)
        {
            if (closeAlarmEvent != null)
            {
                bool  isClose = false;
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                if (item != null)
                {
                    if (item.Text == "打开告警")
                    {
                        item.Text = "关闭告警";
                        isClose = false;
                    }
                    else
                    {
                        item.Text = "打开告警";
                        isClose = true;
                    }
                }
                closeAlarmEvent(ViewID, isClose);
            }
        }
        //------------------------------------------------------------------------------
        private void MenuPatient(object sender, EventArgs e)
        {//
        }
        //------------------------------------------------------------------------------
        private void ShowFormOfMenu(Form frm)
        {
            //SetForms.SetCommand(SendCommand);
            Rectangle Rect = RectangleToScreen(this.ClientRectangle);
            frm.Left = Rect.Left;
            frm.Top = Rect.Bottom;
            frm.StartPosition = FormStartPosition.CenterParent;
            if (frm.ShowDialog() == DialogResult.OK)
            { 
                if(frm is FrmBed)
                {
                  //  lblBedNum.Text = "(" + frmBed.strBedSN + ")";
                }
            
            };
        }
        //------------------------------------------------------------------------------
        private void RefreshBedInfo()
        {
            
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From BedDevMapping Where ID=" + Id);
            if (dr.Read())
            {
                BedName = dr.GetStr("BedSN");
                //PatientName = dr.GetStr("PatientName");
                //PatientType = Lang.Get("PatientType", dr.GetStr("PatientType"));
                //PatientSex = Lang.Get("PatientGender", dr.GetStr("Gender"));
                //PatientAge = GLB.BIRTH_OLD(dr.GetStr("Birthday"));
               // lblBedNum.Text ="(" + BedName + ")";
            }
           // this.Refresh();
            
        }

        private string GetBedNameInfo(string strIP)
        {
            string strBedName = string.Empty;
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From BedDevMapping Where IP='"+strIP+"'");
            
            if (dr.Read())
            {
                strBedName = dr.GetStr("BedSN");
            }
            return strBedName;
        }
        //------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------
}
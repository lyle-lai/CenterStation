namespace RemoteMonitor
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.timerPara = new System.Windows.Forms.Timer(this.components);
            this.timerWave = new System.Windows.Forms.Timer(this.components);
            this.PanMain = new System.Windows.Forms.Panel();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbLayout = new System.Windows.Forms.ToolStripButton();
            this.btnMonitorConfig = new System.Windows.Forms.ToolStripButton();
            this.tbBedManage = new System.Windows.Forms.ToolStripButton();
            this.DropDownAlarm = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnAlarmPara = new System.Windows.Forms.ToolStripMenuItem();
            this.btnQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSystemConfig = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnCascadeSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSystemSet = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDemo = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSystemConfig1 = new System.Windows.Forms.ToolStripButton();
            this.tbHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbAbout = new System.Windows.Forms.ToolStripButton();
            this.tbAlarmOn = new System.Windows.Forms.ToolStripButton();
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.timerDemo = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolBar.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerPara
            // 
            this.timerPara.Interval = 500;
            this.timerPara.Tick += new System.EventHandler(this.timerPara_Tick);
            // 
            // timerWave
            // 
            this.timerWave.Interval = 40;
            this.timerWave.Tick += new System.EventHandler(this.timerWave_Tick);
            // 
            // PanMain
            // 
            this.PanMain.AutoScroll = true;
            this.PanMain.BackColor = System.Drawing.Color.Gray;
            this.PanMain.Location = new System.Drawing.Point(0, 0);
            this.PanMain.Name = "PanMain";
            this.PanMain.Size = new System.Drawing.Size(895, 348);
            this.PanMain.TabIndex = 2;
            this.PanMain.Resize += new System.EventHandler(this.PanMain_Resize);
            // 
            // ToolBar
            // 
            this.ToolBar.AutoSize = false;
            this.ToolBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ToolBar.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tbLayout,
            this.btnMonitorConfig,
            this.tbBedManage,
            this.DropDownAlarm,
            this.btnSystemConfig,
            this.btnSystemConfig1,
            this.tbHistory,
            this.toolStripSeparator3,
            this.tbAbout,
            this.tbAlarmOn});
            this.ToolBar.Location = new System.Drawing.Point(0, 352);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(895, 30);
            this.ToolBar.TabIndex = 8;
            this.ToolBar.TabStop = true;
            this.ToolBar.Text = "辅助显隐";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            this.toolStripSeparator1.Visible = false;
            // 
            // tbLayout
            // 
            this.tbLayout.AutoSize = false;
            this.tbLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbLayout.Image = ((System.Drawing.Image)(resources.GetObject("tbLayout.Image")));
            this.tbLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbLayout.Name = "tbLayout";
            this.tbLayout.Size = new System.Drawing.Size(72, 61);
            this.tbLayout.Text = "布局设置";
            this.tbLayout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbLayout.Click += new System.EventHandler(this.tbLayout_Click);
            // 
            // btnMonitorConfig
            // 
            this.btnMonitorConfig.AutoSize = false;
            this.btnMonitorConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMonitorConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnMonitorConfig.Image")));
            this.btnMonitorConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMonitorConfig.Name = "btnMonitorConfig";
            this.btnMonitorConfig.Size = new System.Drawing.Size(72, 61);
            this.btnMonitorConfig.Text = "床位过滤";
            this.btnMonitorConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMonitorConfig.Click += new System.EventHandler(this.btnMonitorConfig_Click);
            // 
            // tbBedManage
            // 
            this.tbBedManage.AutoSize = false;
            this.tbBedManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbBedManage.Image = ((System.Drawing.Image)(resources.GetObject("tbBedManage.Image")));
            this.tbBedManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbBedManage.Name = "tbBedManage";
            this.tbBedManage.Size = new System.Drawing.Size(72, 61);
            this.tbBedManage.Text = "参数设置";
            this.tbBedManage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbBedManage.Click += new System.EventHandler(this.tbBedManage_Click);
            // 
            // DropDownAlarm
            // 
            this.DropDownAlarm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DropDownAlarm.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAlarmPara,
            this.btnQuery});
            this.DropDownAlarm.Image = ((System.Drawing.Image)(resources.GetObject("DropDownAlarm.Image")));
            this.DropDownAlarm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DropDownAlarm.Name = "DropDownAlarm";
            this.DropDownAlarm.Size = new System.Drawing.Size(87, 27);
            this.DropDownAlarm.Text = "告警配置";
            // 
            // btnAlarmPara
            // 
            this.btnAlarmPara.Name = "btnAlarmPara";
            this.btnAlarmPara.Size = new System.Drawing.Size(144, 26);
            this.btnAlarmPara.Text = "告警参数";
            this.btnAlarmPara.Click += new System.EventHandler(this.btnAlarmConfig_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(144, 26);
            this.btnQuery.Text = "告警查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnSystemConfig
            // 
            this.btnSystemConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSystemConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCascadeSetting,
            this.btnSystemSet,
            this.btnDemo});
            this.btnSystemConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnSystemConfig.Image")));
            this.btnSystemConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSystemConfig.Name = "btnSystemConfig";
            this.btnSystemConfig.Size = new System.Drawing.Size(87, 27);
            this.btnSystemConfig.Text = "系统配置";
            // 
            // btnCascadeSetting
            // 
            this.btnCascadeSetting.Name = "btnCascadeSetting";
            this.btnCascadeSetting.Size = new System.Drawing.Size(144, 26);
            this.btnCascadeSetting.Text = "级联设置";
            this.btnCascadeSetting.Click += new System.EventHandler(this.btnCascadeSetting_Click);
            // 
            // btnSystemSet
            // 
            this.btnSystemSet.Name = "btnSystemSet";
            this.btnSystemSet.Size = new System.Drawing.Size(144, 26);
            this.btnSystemSet.Text = "系统设置";
            this.btnSystemSet.Click += new System.EventHandler(this.btnSystemSet_Click);
            // 
            // btnDemo
            // 
            this.btnDemo.Name = "btnDemo";
            this.btnDemo.Size = new System.Drawing.Size(144, 26);
            this.btnDemo.Text = "演示模式";
            this.btnDemo.Click += new System.EventHandler(this.btnDemo_Click);
            // 
            // btnSystemConfig1
            // 
            this.btnSystemConfig1.AutoSize = false;
            this.btnSystemConfig1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSystemConfig1.Image = ((System.Drawing.Image)(resources.GetObject("btnSystemConfig1.Image")));
            this.btnSystemConfig1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSystemConfig1.Name = "btnSystemConfig1";
            this.btnSystemConfig1.Size = new System.Drawing.Size(72, 61);
            this.btnSystemConfig1.Text = "系统配置";
            this.btnSystemConfig1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSystemConfig1.Visible = false;
            this.btnSystemConfig1.Click += new System.EventHandler(this.MenMonitor_Click);
            // 
            // tbHistory
            // 
            this.tbHistory.AutoSize = false;
            this.tbHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbHistory.Image = ((System.Drawing.Image)(resources.GetObject("tbHistory.Image")));
            this.tbHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbHistory.Name = "tbHistory";
            this.tbHistory.Size = new System.Drawing.Size(72, 61);
            this.tbHistory.Text = "数据回顾";
            this.tbHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbHistory.Click += new System.EventHandler(this.tbHistory_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 30);
            // 
            // tbAbout
            // 
            this.tbAbout.AutoSize = false;
            this.tbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbAbout.Image = ((System.Drawing.Image)(resources.GetObject("tbAbout.Image")));
            this.tbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAbout.Name = "tbAbout";
            this.tbAbout.Size = new System.Drawing.Size(72, 61);
            this.tbAbout.Text = "关于";
            this.tbAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbAbout.Visible = false;
            this.tbAbout.Click += new System.EventHandler(this.tbAbout_Click);
            // 
            // tbAlarmOn
            // 
            this.tbAlarmOn.AutoSize = false;
            this.tbAlarmOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbAlarmOn.Image = ((System.Drawing.Image)(resources.GetObject("tbAlarmOn.Image")));
            this.tbAlarmOn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAlarmOn.Name = "tbAlarmOn";
            this.tbAlarmOn.Size = new System.Drawing.Size(72, 61);
            this.tbAlarmOn.Text = "报警音";
            this.tbAlarmOn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbAlarmOn.Visible = false;
            this.tbAlarmOn.Click += new System.EventHandler(this.tbAlarmOn_Click);
            // 
            // timerStart
            // 
            this.timerStart.Enabled = true;
            this.timerStart.Interval = 500;
            this.timerStart.Tick += new System.EventHandler(this.timerStart_Tick);
            // 
            // timerDemo
            // 
            this.timerDemo.Interval = 1000;
            this.timerDemo.Tick += new System.EventHandler(this.timerDemo_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblstatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 330);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(895, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblstatus
            // 
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(56, 17);
            this.lblstatus.Text = "告警状态";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 382);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ToolBar);
            this.Controls.Add(this.PanMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 420);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中央监护系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ToolStripMenuItem btnAlarmPara;
        private System.Windows.Forms.ToolStripMenuItem btnDemo;
        private System.Windows.Forms.ToolStripButton btnMonitorConfig;
        private System.Windows.Forms.ToolStripMenuItem btnQuery;
        private System.Windows.Forms.ToolStripDropDownButton btnSystemConfig;
        private System.Windows.Forms.ToolStripButton btnSystemConfig1;
        private System.Windows.Forms.ToolStripMenuItem btnSystemSet;
        private System.Windows.Forms.ToolStripDropDownButton DropDownAlarm;
        private System.Windows.Forms.ToolStripStatusLabel lblstatus;
        private System.Windows.Forms.Panel PanMain;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton tbAbout;
        private System.Windows.Forms.ToolStripButton tbAlarmOn;
        private System.Windows.Forms.ToolStripButton tbBedManage;
        private System.Windows.Forms.ToolStripButton tbHistory;
        private System.Windows.Forms.ToolStripButton tbLayout;
        private System.Windows.Forms.Timer timerDemo;
        private System.Windows.Forms.Timer timerPara;
        private System.Windows.Forms.Timer timerStart;
        private System.Windows.Forms.Timer timerWave;
        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

        #endregion

        private System.Windows.Forms.ToolStripMenuItem btnCascadeSetting;
    }
}


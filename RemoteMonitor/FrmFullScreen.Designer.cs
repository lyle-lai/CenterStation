using System;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteMonitor
{
    partial class FrmFullScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PanMain = new System.Windows.Forms.Panel();
            this.timerPara = new System.Windows.Forms.Timer(this.components);
            this.timerWave = new System.Windows.Forms.Timer(this.components);
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // PanMain
            // 
            this.PanMain.BackColor = System.Drawing.Color.Gray;
            this.PanMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanMain.Location = new System.Drawing.Point(0, 0);
            this.PanMain.Name = "PanMain";
            this.PanMain.Size = new System.Drawing.Size(484, 461);
            this.PanMain.TabIndex = 3;
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
            // timerStart
            // 
            this.timerStart.Enabled = true;
            this.timerStart.Interval = 500;
            this.timerStart.Tick += new System.EventHandler(this.timerStart_Tick);
            // 
            // FrmFullScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.PanMain);
            this.Name = "FrmFullScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "大屏显示";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmFullScreen_FormClosed);
            this.ResumeLayout(false);

        }
        private void FrmFullScreen_Resize(object sender, EventArgs e)
        {
            this.PanMain.Size = this.ClientSize;
            // 更新 mViewMonitor 的大小和位置以适应窗体的变化
            if (mViewMonitor != null)
            {
                // 设置外层大小
                mViewMonitor.Width = this.ClientSize.Width;
                mViewMonitor.Height = this.ClientSize.Height - mViewMonitor.VTitle.Height;
                // mViewMonitor.Top = mViewMonitor.VTitle.Height;
                mViewMonitor.Left = 0;
                
                // 计算百分比
                // mViewMonitor.SetSize(widthPercentage,heightPercentage);
                mViewMonitor.LayoutView();
                mViewMonitor.Refresh();
            }
        }

        private System.Windows.Forms.Panel PanMain;
        private System.Windows.Forms.Timer timerPara;
        private System.Windows.Forms.Timer timerStart;
        private System.Windows.Forms.Timer timerWave;

        #endregion
    }
}
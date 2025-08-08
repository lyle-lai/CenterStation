namespace Settings
{
    partial class FrmAlarmSetting
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.PanBack = new System.Windows.Forms.Panel();
            this.PanWork = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.PanBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanBack
            // 
            this.PanBack.AutoScroll = true;
            this.PanBack.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PanBack.Controls.Add(this.PanWork);
            this.PanBack.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanBack.Location = new System.Drawing.Point(0, 0);
            this.PanBack.Name = "PanBack";
            this.PanBack.Size = new System.Drawing.Size(320, 213);
            this.PanBack.TabIndex = 1;
            // 
            // PanWork
            // 
            this.PanWork.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.PanWork.Location = new System.Drawing.Point(51, 0);
            this.PanWork.Name = "PanWork";
            this.PanWork.Size = new System.Drawing.Size(200, 338);
            this.PanWork.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 218);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 48);
            this.button1.TabIndex = 2;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FrmAlarmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 268);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.PanBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAlarmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "报警限设置";
            this.Activated += new System.EventHandler(this.FrmAlarmSetting_Activated);
            this.PanBack.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanBack;
        private System.Windows.Forms.Panel PanWork;
        private System.Windows.Forms.Button button1;



    }
}


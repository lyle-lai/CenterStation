namespace RemoteMonitor
{
    partial class FrmMonitorConfig
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.plContent = new System.Windows.Forms.Panel();
            this.chkCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(118, 8);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 28);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.Location = new System.Drawing.Point(17, 9);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(5);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(97, 28);
            this.btnConfig.TabIndex = 17;
            this.btnConfig.Text = "确定";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // plContent
            // 
            this.plContent.Location = new System.Drawing.Point(7, 45);
            this.plContent.Margin = new System.Windows.Forms.Padding(4);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(673, 408);
            this.plContent.TabIndex = 19;
            // 
            // chkCheck
            // 
            this.chkCheck.AutoSize = true;
            this.chkCheck.Location = new System.Drawing.Point(258, 16);
            this.chkCheck.Name = "chkCheck";
            this.chkCheck.Size = new System.Drawing.Size(99, 20);
            this.chkCheck.TabIndex = 20;
            this.chkCheck.Text = "全选/全否";
            this.chkCheck.UseVisualStyleBackColor = true;
            this.chkCheck.CheckedChanged += new System.EventHandler(this.chkCheck_CheckedChanged);
            // 
            // FrmMonitorConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 455);
            this.Controls.Add(this.chkCheck);
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfig);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmMonitorConfig";
            this.Text = "显示配置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.CheckBox chkCheck;
    }
}
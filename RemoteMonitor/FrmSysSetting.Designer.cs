namespace RemoteMonitor
{
    partial class FrmSysSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbHistory = new System.Windows.Forms.TextBox();
            this.tbDept = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "医院名称";
            // 
            // tbHistory
            // 
            this.tbHistory.Location = new System.Drawing.Point(98, 19);
            this.tbHistory.Name = "tbHistory";
            this.tbHistory.Size = new System.Drawing.Size(223, 21);
            this.tbHistory.TabIndex = 1;
            // 
            // tbDept
            // 
            this.tbDept.Location = new System.Drawing.Point(98, 62);
            this.tbDept.Name = "tbDept";
            this.tbDept.Size = new System.Drawing.Size(223, 21);
            this.tbDept.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "科室名称";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(132, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 48);
            this.button1.TabIndex = 4;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmSysSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 193);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbDept);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbHistory);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSysSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "系统设置";
            this.Activated += new System.EventHandler(this.FrmSysSetting_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHistory;
        private System.Windows.Forms.TextBox tbDept;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}
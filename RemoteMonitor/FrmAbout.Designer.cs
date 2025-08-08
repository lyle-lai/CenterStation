namespace RemoteMonitor
{
    partial class FrmAbout
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
            this.label2 = new System.Windows.Forms.Label();
            this.LabHistory = new System.Windows.Forms.Label();
            this.LabDept = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(74, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "中央监护系统";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "VER 2.0";
            // 
            // LabHistory
            // 
            this.LabHistory.Location = new System.Drawing.Point(60, 39);
            this.LabHistory.Name = "LabHistory";
            this.LabHistory.Size = new System.Drawing.Size(200, 12);
            this.LabHistory.TabIndex = 2;
            this.LabHistory.Text = "LabHistory";
            this.LabHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabDept
            // 
            this.LabDept.Location = new System.Drawing.Point(60, 54);
            this.LabDept.Name = "LabDept";
            this.LabDept.Size = new System.Drawing.Size(200, 12);
            this.LabDept.TabIndex = 3;
            this.LabDept.Text = "LabDept";
            this.LabDept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 235);
            this.Controls.Add(this.LabDept);
            this.Controls.Add(this.LabHistory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于";
            this.Activated += new System.EventHandler(this.FrmAbout_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LabHistory;
        private System.Windows.Forms.Label LabDept;
    }
}
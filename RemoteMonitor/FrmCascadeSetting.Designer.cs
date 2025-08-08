using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteMonitor
{
    partial class FrmCascadeSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.btSave = new System.Windows.Forms.Button();
            this.tbCascadeAddr = new System.Windows.Forms.TextBox();
            this.tbCascadePort = new System.Windows.Forms.TextBox();
            this.cbCascadeMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(30, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "级联IP";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(30, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "级联端口";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btSave
            // 
            this.btSave.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btSave.Location = new System.Drawing.Point(148, 152);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(80, 26);
            this.btSave.TabIndex = 2;
            this.btSave.Text = "保存";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // tbCascadeAddr
            // 
            this.tbCascadeAddr.Location = new System.Drawing.Point(106, 37);
            this.tbCascadeAddr.Name = "tbCascadeAddr";
            this.tbCascadeAddr.Size = new System.Drawing.Size(238, 21);
            this.tbCascadeAddr.TabIndex = 3;
            // 
            // tbCascadePort
            // 
            this.tbCascadePort.Location = new System.Drawing.Point(106, 77);
            this.tbCascadePort.Name = "tbCascadePort";
            this.tbCascadePort.Size = new System.Drawing.Size(238, 21);
            this.tbCascadePort.TabIndex = 4;
            // 
            // cbCascadeMode
            // 
            this.cbCascadeMode.Location = new System.Drawing.Point(106, 122);
            this.cbCascadeMode.Name = "cbCascadeMode";
            this.cbCascadeMode.Size = new System.Drawing.Size(238, 24);
            this.cbCascadeMode.TabIndex = 5;
            this.cbCascadeMode.Text = "启用级联（重启生效）";
            this.cbCascadeMode.UseVisualStyleBackColor = true;
            // 
            // FrmCascadeSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 190);
            this.Controls.Add(this.cbCascadeMode);
            this.Controls.Add(this.tbCascadePort);
            this.Controls.Add(this.tbCascadeAddr);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FrmCascadeSetting";
            this.Text = "级联设置";
            this.Activated += new System.EventHandler(this.FrmCascadeSetting_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.CheckBox cbCascadeMode;

        private System.Windows.Forms.Button btSave;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Label label1;

        #endregion

        private TextBox tbCascadeAddr;
        private TextBox tbCascadePort;
    }
}
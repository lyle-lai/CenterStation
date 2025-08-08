namespace RemoteMonitor
{
    partial class FrmMonitorEdit
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDeviceType = new System.Windows.Forms.ComboBox();
            this.txbBedSN = new System.Windows.Forms.TextBox();
            this.txbDeviceId = new System.Windows.Forms.TextBox();
            this.txbBarcode = new System.Windows.Forms.TextBox();
            this.txbIP = new System.Windows.Forms.TextBox();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txbSort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(73, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = " 编  号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(67, 76);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "设备名称:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(67, 115);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "设备类型:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(67, 152);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "条 形 码:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(83, 192);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "IP地址:";
            // 
            // cmbDeviceType
            // 
            this.cmbDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeviceType.FormattingEnabled = true;
            this.cmbDeviceType.Items.AddRange(new object[] { "Mindray-ePM", "Mindray-iPM10", "Mindray-PM9000", "Mindray-IPM9800", "Philips-IntelliVue", "GE-Dash" });
            this.cmbDeviceType.Location = new System.Drawing.Point(183, 113);
            this.cmbDeviceType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbDeviceType.Name = "cmbDeviceType";
            this.cmbDeviceType.Size = new System.Drawing.Size(244, 24);
            this.cmbDeviceType.TabIndex = 3;
            // 
            // txbBedSN
            // 
            this.txbBedSN.Location = new System.Drawing.Point(183, 32);
            this.txbBedSN.Margin = new System.Windows.Forms.Padding(4);
            this.txbBedSN.Name = "txbBedSN";
            this.txbBedSN.Size = new System.Drawing.Size(244, 26);
            this.txbBedSN.TabIndex = 1;
            // 
            // txbDeviceId
            // 
            this.txbDeviceId.Location = new System.Drawing.Point(183, 71);
            this.txbDeviceId.Margin = new System.Windows.Forms.Padding(4);
            this.txbDeviceId.Name = "txbDeviceId";
            this.txbDeviceId.Size = new System.Drawing.Size(244, 26);
            this.txbDeviceId.TabIndex = 2;
            // 
            // txbBarcode
            // 
            this.txbBarcode.Location = new System.Drawing.Point(183, 149);
            this.txbBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.txbBarcode.Name = "txbBarcode";
            this.txbBarcode.Size = new System.Drawing.Size(244, 26);
            this.txbBarcode.TabIndex = 4;
            // 
            // txbIP
            // 
            this.txbIP.Location = new System.Drawing.Point(183, 185);
            this.txbIP.Margin = new System.Windows.Forms.Padding(4);
            this.txbIP.Name = "txbIP";
            this.txbIP.Size = new System.Drawing.Size(244, 26);
            this.txbIP.TabIndex = 5;
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Checked = true;
            this.chkStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStatus.Location = new System.Drawing.Point(183, 255);
            this.chkStatus.Margin = new System.Windows.Forms.Padding(4);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(59, 20);
            this.chkStatus.TabIndex = 14;
            this.chkStatus.Text = "启用";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // btnConfig
            // 
            this.btnConfig.Location = new System.Drawing.Point(292, 277);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(100, 31);
            this.btnConfig.TabIndex = 15;
            this.btnConfig.Text = "确定";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(400, 277);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 31);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(73, 229);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 16);
            this.label6.TabIndex = 18;
            this.label6.Text = " 排  序：";
            // 
            // txbSort
            // 
            this.txbSort.Location = new System.Drawing.Point(183, 219);
            this.txbSort.Margin = new System.Windows.Forms.Padding(4);
            this.txbSort.Name = "txbSort";
            this.txbSort.Size = new System.Drawing.Size(244, 26);
            this.txbSort.TabIndex = 19;
            // 
            // FrmMonitorEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 320);
            this.Controls.Add(this.txbSort);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.txbIP);
            this.Controls.Add(this.txbBarcode);
            this.Controls.Add(this.txbDeviceId);
            this.Controls.Add(this.txbBedSN);
            this.Controls.Add(this.cmbDeviceType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmMonitorEdit";
            this.Text = "监护仪维护";
            this.Load += new System.EventHandler(this.FrmMonitorEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDeviceType;
        private System.Windows.Forms.TextBox txbBedSN;
        private System.Windows.Forms.TextBox txbDeviceId;
        private System.Windows.Forms.TextBox txbBarcode;
        private System.Windows.Forms.TextBox txbIP;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbSort;
    }
}
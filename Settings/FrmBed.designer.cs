namespace Settings
{
    partial class FrmBed
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
            this.LabID = new System.Windows.Forms.Label();
            this.txbBedNum = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txbPatient = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txbCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbIndptNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LabID
            // 
            this.LabID.Location = new System.Drawing.Point(14, 131);
            this.LabID.Name = "LabID";
            this.LabID.Size = new System.Drawing.Size(80, 20);
            this.LabID.TabIndex = 0;
            this.LabID.Text = "床位号";
            this.LabID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txbBedNum
            // 
            this.txbBedNum.Location = new System.Drawing.Point(111, 131);
            this.txbBedNum.Name = "txbBedNum";
            this.txbBedNum.Size = new System.Drawing.Size(160, 21);
            this.txbBedNum.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(111, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 48);
            this.button1.TabIndex = 11;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txbPatient
            // 
            this.txbPatient.Location = new System.Drawing.Point(111, 104);
            this.txbPatient.Name = "txbPatient";
            this.txbPatient.Size = new System.Drawing.Size(160, 21);
            this.txbPatient.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "病人姓名";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txbID
            // 
            this.txbID.Location = new System.Drawing.Point(111, 19);
            this.txbID.Name = "txbID";
            this.txbID.Size = new System.Drawing.Size(160, 21);
            this.txbID.TabIndex = 1;
            this.txbID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbID_KeyDown);
            this.txbID.Leave += new System.EventHandler(this.txbID_Leave);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "病人ID";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txbCode
            // 
            this.txbCode.Location = new System.Drawing.Point(111, 75);
            this.txbCode.Name = "txbCode";
            this.txbCode.Size = new System.Drawing.Size(160, 21);
            this.txbCode.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "条形码:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txbIndptNo
            // 
            this.txbIndptNo.Location = new System.Drawing.Point(110, 48);
            this.txbIndptNo.Name = "txbIndptNo";
            this.txbIndptNo.Size = new System.Drawing.Size(160, 21);
            this.txbIndptNo.TabIndex = 2;
            this.txbIndptNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbIndptNo_KeyDown);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(15, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "住 院 号:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmBed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 211);
            this.Controls.Add(this.txbIndptNo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txbID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txbCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txbPatient);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txbBedNum);
            this.Controls.Add(this.LabID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "床位号设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabID;
        public System.Windows.Forms.TextBox txbBedNum;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox txbPatient;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txbID;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txbCode;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txbIndptNo;
        private System.Windows.Forms.Label label4;



    }
}


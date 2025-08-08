namespace RemoteMonitor
{
    partial class FrmAlarmQuery
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txbPatientname = new System.Windows.Forms.TextBox();
            this.txbBedNum = new System.Windows.Forms.TextBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtBegin = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgAlarm = new System.Windows.Forms.DataGridView();
            this.ColBedNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColHigh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Collow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAlarmType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAlarmTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.txbSN = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAlarm)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txbSN);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txbPatientname);
            this.panel1.Controls.Add(this.txbBedNum);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.dtEnd);
            this.panel1.Controls.Add(this.dtBegin);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(843, 47);
            this.panel1.TabIndex = 10;
            // 
            // txbPatientname
            // 
            this.txbPatientname.Location = new System.Drawing.Point(509, 14);
            this.txbPatientname.Name = "txbPatientname";
            this.txbPatientname.Size = new System.Drawing.Size(95, 21);
            this.txbPatientname.TabIndex = 17;
            // 
            // txbBedNum
            // 
            this.txbBedNum.Location = new System.Drawing.Point(379, 14);
            this.txbBedNum.Name = "txbBedNum";
            this.txbBedNum.Size = new System.Drawing.Size(83, 21);
            this.txbBedNum.TabIndex = 16;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(756, 11);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 15;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dtEnd
            // 
            this.dtEnd.Location = new System.Drawing.Point(227, 15);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(103, 21);
            this.dtEnd.TabIndex = 14;
            // 
            // dtBegin
            // 
            this.dtBegin.Location = new System.Drawing.Point(86, 15);
            this.dtBegin.Name = "dtBegin";
            this.dtBegin.Size = new System.Drawing.Size(112, 21);
            this.dtBegin.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(468, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "姓名:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(342, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "床号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "到";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "报警时间";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgAlarm);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(843, 447);
            this.panel2.TabIndex = 11;
            // 
            // dgAlarm
            // 
            this.dgAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAlarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColBedNum,
            this.ColPatientName,
            this.ColSN,
            this.ParaName,
            this.ColVal,
            this.ColHigh,
            this.Collow,
            this.ColAlarmType,
            this.ColAlarmTime});
            this.dgAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAlarm.Location = new System.Drawing.Point(0, 0);
            this.dgAlarm.Name = "dgAlarm";
            this.dgAlarm.RowTemplate.Height = 23;
            this.dgAlarm.Size = new System.Drawing.Size(843, 447);
            this.dgAlarm.TabIndex = 10;
            // 
            // ColBedNum
            // 
            this.ColBedNum.DataPropertyName = "BedNum";
            this.ColBedNum.HeaderText = "床号";
            this.ColBedNum.Name = "ColBedNum";
            this.ColBedNum.ReadOnly = true;
            this.ColBedNum.Width = 80;
            // 
            // ColPatientName
            // 
            this.ColPatientName.DataPropertyName = "PatientName";
            this.ColPatientName.HeaderText = "病人姓名";
            this.ColPatientName.Name = "ColPatientName";
            this.ColPatientName.ReadOnly = true;
            this.ColPatientName.Width = 80;
            // 
            // ColSN
            // 
            this.ColSN.DataPropertyName = "DeviceSN";
            this.ColSN.HeaderText = "编号";
            this.ColSN.Name = "ColSN";
            this.ColSN.Width = 80;
            // 
            // ParaName
            // 
            this.ParaName.DataPropertyName = "paraName";
            this.ParaName.HeaderText = "参数名";
            this.ParaName.Name = "ParaName";
            this.ParaName.Width = 80;
            // 
            // ColVal
            // 
            this.ColVal.DataPropertyName = "Val";
            this.ColVal.HeaderText = "告警值";
            this.ColVal.Name = "ColVal";
            this.ColVal.Width = 80;
            // 
            // ColHigh
            // 
            this.ColHigh.DataPropertyName = "High";
            this.ColHigh.HeaderText = "最高值";
            this.ColHigh.Name = "ColHigh";
            this.ColHigh.Width = 80;
            // 
            // Collow
            // 
            this.Collow.DataPropertyName = "Low";
            this.Collow.HeaderText = "最低值";
            this.Collow.Name = "Collow";
            this.Collow.Width = 80;
            // 
            // ColAlarmType
            // 
            this.ColAlarmType.DataPropertyName = "AlarmType";
            this.ColAlarmType.HeaderText = "告警类型";
            this.ColAlarmType.Name = "ColAlarmType";
            this.ColAlarmType.Width = 80;
            // 
            // ColAlarmTime
            // 
            this.ColAlarmTime.DataPropertyName = "AlarmTime";
            this.ColAlarmTime.HeaderText = "告警时间";
            this.ColAlarmTime.Name = "ColAlarmTime";
            this.ColAlarmTime.Width = 160;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(620, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "编号:";
            // 
            // txbSN
            // 
            this.txbSN.Location = new System.Drawing.Point(655, 15);
            this.txbSN.Name = "txbSN";
            this.txbSN.Size = new System.Drawing.Size(83, 21);
            this.txbSN.TabIndex = 19;
            // 
            // FrmAlarmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 494);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmAlarmQuery";
            this.Text = "告警查询";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAlarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txbPatientname;
        private System.Windows.Forms.TextBox txbBedNum;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.DateTimePicker dtBegin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgAlarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBedNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColHigh;
        private System.Windows.Forms.DataGridViewTextBoxColumn Collow;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAlarmType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAlarmTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbSN;

    }
}
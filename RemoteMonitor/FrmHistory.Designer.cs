namespace RemoteMonitor
{
    partial class FrmHistory
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
            this.TabCon = new System.Windows.Forms.TabControl();
            this.tabTrendTable = new System.Windows.Forms.TabPage();
            this.ucTrendTable1 = new HistoryPack.ucTrendTable();
            this.tabTrendChart = new System.Windows.Forms.TabPage();
            this.ucTrendChart1 = new HistoryPack.ucTrendChart();
            this.tabWave = new System.Windows.Forms.TabPage();
            this.ucHistoryWave1 = new HistoryPack.ucHistoryWave();
            //this.tabAlarm = new System.Windows.Forms.TabPage();
            //this.ucHistoryAlarm1 = new HistoryPack.ucHistoryAlarm();
            this.TabCon.SuspendLayout();
            this.tabTrendTable.SuspendLayout();
            this.tabTrendChart.SuspendLayout();
            this.tabWave.SuspendLayout();
            //this.tabAlarm.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabCon
            // 
            this.TabCon.Controls.Add(this.tabTrendTable);
            this.TabCon.Controls.Add(this.tabTrendChart);
            //this.TabCon.Controls.Add(this.tabAlarm);
            this.TabCon.Controls.Add(this.tabWave);
            this.TabCon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabCon.ItemSize = new System.Drawing.Size(80, 24);
            this.TabCon.Location = new System.Drawing.Point(0, 0);
            this.TabCon.Multiline = true;
            this.TabCon.Name = "TabCon";
            this.TabCon.SelectedIndex = 0;
            this.TabCon.Size = new System.Drawing.Size(780, 372);
            this.TabCon.TabIndex = 0;
            // 
            // tabTrendTable
            // 
            this.tabTrendTable.Controls.Add(this.ucTrendTable1);
            this.tabTrendTable.Location = new System.Drawing.Point(4, 28);
            this.tabTrendTable.Name = "tabTrendTable";
            this.tabTrendTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabTrendTable.Size = new System.Drawing.Size(772, 340);
            this.tabTrendTable.TabIndex = 0;
            this.tabTrendTable.Text = "趋势表";
            this.tabTrendTable.UseVisualStyleBackColor = true;
            // 
            // ucTrendTable1
            // 
            this.ucTrendTable1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ucTrendTable1.Location = new System.Drawing.Point(119, 59);
            this.ucTrendTable1.Name = "ucTrendTable1";
            this.ucTrendTable1.Size = new System.Drawing.Size(150, 150);
            this.ucTrendTable1.TabIndex = 0;
            // 
            // tabTrendChart
            // 
            this.tabTrendChart.Controls.Add(this.ucTrendChart1);
            this.tabTrendChart.Location = new System.Drawing.Point(4, 28);
            this.tabTrendChart.Name = "tabTrendChart";
            this.tabTrendChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabTrendChart.Size = new System.Drawing.Size(772, 340);
            this.tabTrendChart.TabIndex = 1;
            this.tabTrendChart.Text = "趋势图";
            this.tabTrendChart.UseVisualStyleBackColor = true;
            // 
            // ucTrendChart1
            // 
            this.ucTrendChart1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ucTrendChart1.Location = new System.Drawing.Point(164, 79);
            this.ucTrendChart1.Name = "ucTrendChart1";
            this.ucTrendChart1.Size = new System.Drawing.Size(150, 150);
            this.ucTrendChart1.TabIndex = 0;
            ////
            //// tabAlarm
            ////
            //this.tabAlarm.Controls.Add(this.ucHistoryAlarm1);
            //this.tabAlarm.Location = new System.Drawing.Point(4, 28);
            //this.tabAlarm.Name = "tabAlarm";
            //this.tabAlarm.Padding = new System.Windows.Forms.Padding(3);
            //this.tabAlarm.Size = new System.Drawing.Size(772, 340);
            //this.tabAlarm.TabIndex = 2;
            //this.tabAlarm.Text = "告警回顾";
            //this.tabAlarm.UseVisualStyleBackColor = true;
            ////
            //// ucHistoryAlarm1
            ////
            //this.ucHistoryAlarm1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            //this.ucHistoryAlarm1.Location = new System.Drawing.Point(119, 59);
            //this.ucHistoryAlarm1.Name = "ucHistoryAlarm1";
            //this.ucHistoryAlarm1.Size = new System.Drawing.Size(150, 150);
            //this.ucHistoryAlarm1.TabIndex = 0;
            // 
            // tabWave
            // 
            this.tabWave.Controls.Add(this.ucHistoryWave1);
            this.tabWave.Location = new System.Drawing.Point(4, 28);
            this.tabWave.Name = "tabWave";
            this.tabWave.Size = new System.Drawing.Size(772, 340);
            this.tabWave.TabIndex = 3;
            this.tabWave.Text = "波形回顾";
            this.tabWave.UseVisualStyleBackColor = true;
            // 
            // ucHistoryWave1
            // 
            this.ucHistoryWave1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ucHistoryWave1.Location = new System.Drawing.Point(150, 82);
            this.ucHistoryWave1.Name = "ucHistoryWave1";
            this.ucHistoryWave1.Size = new System.Drawing.Size(150, 150);
            this.ucHistoryWave1.TabIndex = 0;
            // 
            // FrmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 372);
            this.Controls.Add(this.TabCon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHistory";
            this.Text = "历史回顾";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmHistory_FormClosing);
            this.TabCon.ResumeLayout(false);
            this.tabTrendTable.ResumeLayout(false);
            this.tabTrendChart.ResumeLayout(false);
            //this.tabAlarm.ResumeLayout(false);
            this.tabWave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabCon;
        private System.Windows.Forms.TabPage tabTrendTable;
        private System.Windows.Forms.TabPage tabTrendChart;
        private System.Windows.Forms.TabPage tabWave;
        private System.Windows.Forms.TabPage tabAlarm;
        private HistoryPack.ucTrendTable ucTrendTable1;
        private HistoryPack.ucTrendChart ucTrendChart1;
        private HistoryPack.ucHistoryWave ucHistoryWave1;
        private HistoryPack.ucHistoryAlarm ucHistoryAlarm1;
    }
}
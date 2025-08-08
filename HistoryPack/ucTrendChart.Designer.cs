namespace HistoryPack
{
    partial class ucTrendChart
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbBed = new System.Windows.Forms.ComboBox();
            this.cbParaGroupSelector = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.PanChart = new System.Windows.Forms.Panel();
            this.PanCooY = new System.Windows.Forms.Panel();
            this.PanTime = new System.Windows.Forms.Panel();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.cbDay = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.PanTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDay);
            this.panel1.Controls.Add(this.cbBed);
            this.panel1.Controls.Add(this.cbParaGroupSelector);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(319, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 274);
            this.panel1.TabIndex = 3;
            // 
            // cbBed
            // 
            this.cbBed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBed.FormattingEnabled = true;
            this.cbBed.Items.AddRange(new object[] {
            "1 床",
            "2 床",
            "3 床",
            "4 床",
            "5 床",
            "6 床",
            "7 床",
            "8 床",
            "9 床",
            "10 床",
            "11 床",
            "12 床",
            "13 床",
            "14 床",
            "15 床",
            "16 床"});
            this.cbBed.Location = new System.Drawing.Point(29, 23);
            this.cbBed.Name = "cbBed";
            this.cbBed.Size = new System.Drawing.Size(121, 20);
            this.cbBed.TabIndex = 4;
            this.cbBed.SelectedIndexChanged += new System.EventHandler(this.cbBed_SelectedIndexChanged);
            // 
            // cbParaGroupSelector
            // 
            this.cbParaGroupSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParaGroupSelector.FormattingEnabled = true;
            this.cbParaGroupSelector.Location = new System.Drawing.Point(29, 60);
            this.cbParaGroupSelector.Name = "cbParaGroupSelector";
            this.cbParaGroupSelector.Size = new System.Drawing.Size(121, 20);
            this.cbParaGroupSelector.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(52, 152);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 48);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // PanChart
            // 
            this.PanChart.BackColor = System.Drawing.Color.Black;
            this.PanChart.Location = new System.Drawing.Point(79, 60);
            this.PanChart.Name = "PanChart";
            this.PanChart.Size = new System.Drawing.Size(200, 100);
            this.PanChart.TabIndex = 4;
            this.PanChart.Paint += new System.Windows.Forms.PaintEventHandler(this.PanChart_Paint);
            // 
            // PanCooY
            // 
            this.PanCooY.BackColor = System.Drawing.Color.Black;
            this.PanCooY.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanCooY.Location = new System.Drawing.Point(0, 0);
            this.PanCooY.Name = "PanCooY";
            this.PanCooY.Size = new System.Drawing.Size(48, 229);
            this.PanCooY.TabIndex = 5;
            // 
            // PanTime
            // 
            this.PanTime.Controls.Add(this.trackBar1);
            this.PanTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanTime.Location = new System.Drawing.Point(0, 229);
            this.PanTime.Name = "PanTime";
            this.PanTime.Size = new System.Drawing.Size(319, 45);
            this.PanTime.TabIndex = 6;
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar1.Location = new System.Drawing.Point(0, 0);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(319, 45);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // cbDay
            // 
            this.cbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDay.FormattingEnabled = true;
            this.cbDay.Location = new System.Drawing.Point(29, 101);
            this.cbDay.Name = "cbDay";
            this.cbDay.Size = new System.Drawing.Size(121, 20);
            this.cbDay.TabIndex = 6;
            // 
            // ucTrendChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.PanChart);
            this.Controls.Add(this.PanCooY);
            this.Controls.Add(this.PanTime);
            this.Controls.Add(this.panel1);
            this.Name = "ucTrendChart";
            this.Size = new System.Drawing.Size(505, 274);
            this.Load += new System.EventHandler(this.ucTrendChart_Load);
            this.panel1.ResumeLayout(false);
            this.PanTime.ResumeLayout(false);
            this.PanTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbBed;
        private System.Windows.Forms.ComboBox cbParaGroupSelector;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel PanChart;
        private System.Windows.Forms.Panel PanCooY;
        private System.Windows.Forms.Panel PanTime;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.ComboBox cbDay;
    }
}

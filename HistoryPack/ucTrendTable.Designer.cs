namespace HistoryPack
{
    partial class ucTrendTable
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbBed = new System.Windows.Forms.ComboBox();
            this.cbParaGroupSelector = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbDay = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(387, 97);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "时间";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "参数值";
            this.columnHeader2.Width = 120;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDay);
            this.panel1.Controls.Add(this.cbBed);
            this.panel1.Controls.Add(this.cbParaGroupSelector);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(387, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 240);
            this.panel1.TabIndex = 2;
            // 
            // cbBed
            // 
            this.cbBed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBed.FormattingEnabled = true;
            // modify_by_limu_160614，床位号应该动态加载
            //this.cbBed.Items.AddRange(new object[] {
            //"1 床",
            //"2 床",
            //"3 床",
            //"4 床",
            //"5 床",
            //"6 床",
            //"7 床",
            //"8 床",
            //"9 床",
            //"10 床",
            //"11 床",
            //"12 床",
            //"13 床",
            //"14 床",
            //"15 床",
            //"16 床"});
            this.cbBed.Location = new System.Drawing.Point(29, 29);
            this.cbBed.Name = "cbBed";
            this.cbBed.Size = new System.Drawing.Size(121, 20);
            this.cbBed.TabIndex = 4;
            this.cbBed.SelectedIndexChanged += new System.EventHandler(this.cbBed_SelectedIndexChanged);
            // 
            // cbParaGroupSelector
            // 
            this.cbParaGroupSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParaGroupSelector.FormattingEnabled = true;
            this.cbParaGroupSelector.Location = new System.Drawing.Point(29, 67);
            this.cbParaGroupSelector.Name = "cbParaGroupSelector";
            this.cbParaGroupSelector.Size = new System.Drawing.Size(121, 20);
            this.cbParaGroupSelector.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(48, 156);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 48);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbDay
            // 
            this.cbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDay.FormattingEnabled = true;
            this.cbDay.Location = new System.Drawing.Point(29, 108);
            this.cbDay.Name = "cbDay";
            this.cbDay.Size = new System.Drawing.Size(121, 20);
            this.cbDay.TabIndex = 5;
            // 
            // ucTrendTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Name = "ucTrendTable";
            this.Size = new System.Drawing.Size(573, 240);
            this.Load += new System.EventHandler(this.ucTrendTable_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbParaGroupSelector;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbBed;
        private System.Windows.Forms.ComboBox cbDay;
    }
}

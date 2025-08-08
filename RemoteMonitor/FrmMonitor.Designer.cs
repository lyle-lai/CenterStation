namespace RemoteMonitor
{
    partial class FrmMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMonitor));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnDel = new System.Windows.Forms.ToolStripButton();
            this.dgMontior = new System.Windows.Forms.DataGridView();
            this.ColBed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDeviceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDeviceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDeviceIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStatusName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNumSort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMontior)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnUpdate,
            this.btnDel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(741, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(36, 22);
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(36, 22);
            this.btnUpdate.Text = "修改";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDel
            // 
            this.btnDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(36, 22);
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // dgMontior
            // 
            this.dgMontior.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMontior.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColBed,
            this.ColDeviceID,
            this.ColDeviceType,
            this.ColDeviceIP,
            this.ColBarcode,
            this.ColStatusName,
            this.ColNumSort});
            this.dgMontior.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMontior.Location = new System.Drawing.Point(0, 25);
            this.dgMontior.MultiSelect = false;
            this.dgMontior.Name = "dgMontior";
            this.dgMontior.RowTemplate.Height = 23;
            this.dgMontior.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMontior.Size = new System.Drawing.Size(741, 400);
            this.dgMontior.TabIndex = 1;
            // 
            // ColBed
            // 
            this.ColBed.DataPropertyName = "BEDSN";
            this.ColBed.HeaderText = "编号";
            this.ColBed.Name = "ColBed";
            this.ColBed.ReadOnly = true;
            // 
            // ColDeviceID
            // 
            this.ColDeviceID.DataPropertyName = "DEVICEID";
            this.ColDeviceID.HeaderText = "设备编号";
            this.ColDeviceID.Name = "ColDeviceID";
            this.ColDeviceID.ReadOnly = true;
            // 
            // ColDeviceType
            // 
            this.ColDeviceType.DataPropertyName = "DEVICETYPE";
            this.ColDeviceType.HeaderText = "设备类型";
            this.ColDeviceType.Name = "ColDeviceType";
            this.ColDeviceType.ReadOnly = true;
            // 
            // ColDeviceIP
            // 
            this.ColDeviceIP.DataPropertyName = "DEVICEIP";
            this.ColDeviceIP.HeaderText = "设备IP";
            this.ColDeviceIP.Name = "ColDeviceIP";
            this.ColDeviceIP.ReadOnly = true;
            // 
            // ColBarcode
            // 
            this.ColBarcode.DataPropertyName = "BARCODE";
            this.ColBarcode.HeaderText = "条形码";
            this.ColBarcode.Name = "ColBarcode";
            this.ColBarcode.ReadOnly = true;
            // 
            // ColStatusName
            // 
            this.ColStatusName.DataPropertyName = "STATUSNAME";
            this.ColStatusName.HeaderText = "状态";
            this.ColStatusName.Name = "ColStatusName";
            this.ColStatusName.ReadOnly = true;
            // 
            // ColNumSort
            // 
            this.ColNumSort.DataPropertyName = "NUMSORT";
            this.ColNumSort.HeaderText = "排序";
            this.ColNumSort.Name = "ColNumSort";
            // 
            // FrmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 425);
            this.Controls.Add(this.dgMontior);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmMonitor";
            this.Text = "监护仪维护";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMontior)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripButton btnDel;
        private System.Windows.Forms.DataGridView dgMontior;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBed;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDeviceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDeviceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDeviceIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStatusName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNumSort;
    }
}
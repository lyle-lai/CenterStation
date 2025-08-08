namespace RemoteMonitor
{
    partial class FrmBedManage
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
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("节点4");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("节点5");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("节点0", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode14,
            treeNode15});
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("节点1");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("节点2");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("节点4");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("节点5");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("节点0", new System.Windows.Forms.TreeNode[] {
            treeNode19,
            treeNode20,
            treeNode21});
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("节点1");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("节点2");
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 224);
            this.panel1.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeView2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 224);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择显示参数";
            // 
            // treeView2
            // 
            this.treeView2.CheckBoxes = true;
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView2.Location = new System.Drawing.Point(3, 17);
            this.treeView2.Name = "treeView2";
            treeNode13.Name = "节点3";
            treeNode13.Text = "节点3";
            treeNode14.Name = "节点4";
            treeNode14.Text = "节点4";
            treeNode15.Name = "节点5";
            treeNode15.Text = "节点5";
            treeNode16.Checked = true;
            treeNode16.Name = "节点0";
            treeNode16.Text = "节点0";
            treeNode17.Name = "节点1";
            treeNode17.Text = "节点1";
            treeNode18.Name = "节点2";
            treeNode18.Text = "节点2";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode16,
            treeNode17,
            treeNode18});
            this.treeView2.Size = new System.Drawing.Size(194, 204);
            this.treeView2.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(215, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 224);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择显示波形";
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(3, 17);
            this.treeView1.Name = "treeView1";
            treeNode19.Name = "节点3";
            treeNode19.Text = "节点3";
            treeNode20.Name = "节点4";
            treeNode20.Text = "节点4";
            treeNode21.Name = "节点5";
            treeNode21.Text = "节点5";
            treeNode22.Checked = true;
            treeNode22.Name = "节点0";
            treeNode22.Text = "节点0";
            treeNode23.Name = "节点1";
            treeNode23.Text = "节点1";
            treeNode24.Name = "节点2";
            treeNode24.Text = "节点2";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode22,
            treeNode23,
            treeNode24});
            this.treeView1.Size = new System.Drawing.Size(194, 204);
            this.treeView1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 48);
            this.button1.TabIndex = 4;
            this.button1.Text = "应用于所有窗口";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(241, 230);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 48);
            this.button2.TabIndex = 5;
            this.button2.Text = "应用于当前窗口";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FrmBedManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 294);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBedManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "视图管理";
            this.Activated += new System.EventHandler(this.FrmBedManage_Activated);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;

    }
}
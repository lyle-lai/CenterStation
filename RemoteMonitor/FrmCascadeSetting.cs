// Copyright (c) YiCare Corporation. All rights reserved.
// 文件：FrmCascadeSetting.cs
// 作者：SUSHI
// 日期：11/21/2023
// 说明：

using System;
using System.Windows.Forms;
using GlobalClass;

namespace RemoteMonitor
{
    public partial class FrmCascadeSetting : Form
    {
        public FrmCascadeSetting()
        {
            InitializeComponent();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            Config.Set("SYS", "CascadeAddr", tbCascadeAddr.Text);
            Config.Set("SYS", "CascadePort", tbCascadePort.Text);
            Config.Set("SYS", "CascadeMode", cbCascadeMode.Checked.ToString());
            this.Close();
        }

        private void FrmCascadeSetting_Activated(object sender, EventArgs e)
        {
            tbCascadeAddr.Text = Config.Get("SYS", "CascadeAddr", "10.10.252.108");
            tbCascadePort.Text = Config.Get("SYS", "CascadePort", "10021");
            cbCascadeMode.Checked = Convert.ToBoolean(Config.Get("SYS", "CascadeMode", "True"));
        }
    }
}
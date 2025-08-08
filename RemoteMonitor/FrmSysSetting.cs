using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalClass;
//----------------------------------------------------------------------------
namespace RemoteMonitor
{
    //----------------------------------------------------------------------------
    public partial class FrmSysSetting : Form
    {
        //----------------------------------------------------------------------------
        public FrmSysSetting()
        {
            InitializeComponent();
        }
        //----------------------------------------------------------------------------
        private void FrmSysSetting_Activated(object sender, EventArgs e)
        {
            tbHistory.Text = Config.Get("SYS", "Hospital", "");
            tbDept.Text = Config.Get("SYS", "Department", "");
        }
        //----------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            Config.Set("SYS", "Hospital", tbHistory.Text);
            Config.Set("SYS", "Department", tbDept.Text);
            this.Close();
        }
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------
}

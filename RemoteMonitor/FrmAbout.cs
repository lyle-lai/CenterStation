using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalClass;

namespace RemoteMonitor
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void FrmAbout_Activated(object sender, EventArgs e)
        {
            LabHistory.Text = Config.Get("Sys", "Hospital", "");
            LabDept.Text = Config.Get("Sys", "Department", "");
        }
    }
}

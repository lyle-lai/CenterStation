using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GlobalClass;
using ViewPack;
//---------------------------------------------------------------------------------------------------

namespace RemoteMonitor
{
    //---------------------------------------------------------------------------------------------------
    public partial class FrmLayoutSetting : Form
    {
        //---------------------------------------------------------------------------------------------------
        public FrmLayoutSetting()
        {
            InitializeComponent();
        }
        //---------------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            Config.Set("Sys", "Rows", listView1.FocusedItem.SubItems[1].Text);
            Config.Set("Sys", "Cols", listView1.FocusedItem.SubItems[2].Text);

            
            //
            int Rows = GLB.ToInt(listView1.FocusedItem.SubItems[1].Text, 3);
            int Cols = GLB.ToInt(listView1.FocusedItem.SubItems[2].Text, 2);
            Config.Rows = Rows;
            Config.Cols = Cols;
            //
            LayoutBedView.Create(Rows, Cols);
            LayoutBedView.Layout(Rows, Cols);
            //
            this.Close();
        }
        //---------------------------------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //---------------------------------------------------------------------------------------------------
    }
    //---------------------------------------------------------------------------------------------------
}

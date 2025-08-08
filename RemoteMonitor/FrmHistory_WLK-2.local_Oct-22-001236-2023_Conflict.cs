using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using HistoryPack;
//------------------------------------------------------------------------------------
namespace RemoteMonitor
{
    //------------------------------------------------------------------------------------
    public partial class FrmHistory : Form
    {
        //------------------------------------------------------------------------------------
        public FrmHistory()
        {
            InitializeComponent();
            tabWave.Parent = null;  //暂不显示波形回顾

            this.StartPosition = FormStartPosition.Manual;
            //
            ucTrendTable1.Dock = DockStyle.Fill;
            ucTrendChart1.Dock = DockStyle.Fill;
            ucHistoryWave1.Dock = DockStyle.Fill;
            //
            ucTrendTable1.StartTick();
        }
        //------------------------------------------------------------------------------------
        private void FrmHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            //IsOpen = false;
            //ucTrendTable1.Clear();
        }
        //------------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------------
}

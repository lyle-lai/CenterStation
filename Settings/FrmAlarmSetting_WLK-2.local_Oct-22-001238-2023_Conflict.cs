using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GlobalClass;
//-----------------------------------------------------------------------------
namespace Settings
{
    //-----------------------------------------------------------------------------
    public partial class FrmAlarmSetting : Form
    {
        public int ID = 0;
        //-----------------------------------------------------------------------------
        public FrmAlarmSetting()
        {
            InitializeComponent();
            PanWork.Top = 0;
            PanWork.Left = 0;
        }
        //-----------------------------------------------------------------------------
        private void FrmAlarmSetting_Activated(object sender, EventArgs e)
        {
            PanWork.Width = PanBack.ClientSize.Width;
            /*
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From AlarmLimit Where DevID=" + ID.ToString());
            while (dr.Read())
            {
            }*/
        }
        //-----------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------------
}

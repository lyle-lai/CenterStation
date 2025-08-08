using GlobalClass;
using ObjPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RemoteMonitor
{
    public partial class FrmAlarmQuery : Form
    {
        public FrmAlarmQuery()
        {
            InitializeComponent();
            this.Load += FrmAlarmQuery_Load;
        }

        private  void FrmAlarmQuery_Load(object sender, EventArgs e)
        {
            try
            {
                dtBegin.Value = DateTime.Now.AddDays(-1);
                dgAlarm.AutoGenerateColumns = false;
                dgAlarm.ReadOnly = true;
                Query();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtEnd.Value < dtBegin.Value)
                {
                    MessageBox.Show("开始时间必须小于结束时间");
                    return;
                }
                Query();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        private void Query()
        {
            string strSql = "select * from AlarmRecord where 1=1";

            if (dtBegin.Value != null)
            {
                strSql += string.Format(" and  datetime(AlarmTime)>=datetime('{0}')", dtBegin.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (dtEnd.Value != null)
            {
                strSql += string.Format(" and  datetime(AlarmTime)<=datetime('{0}')", dtEnd.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            if (!string.IsNullOrEmpty(txbBedNum.Text))
            {
                strSql += string.Format(" and  BedNum like'%{0}%'", txbBedNum.Text.Trim());
            }

            if (!string.IsNullOrEmpty(txbPatientname.Text))
            {
                strSql += string.Format(" and  PatientName like'%{0}%'", txbPatientname.Text);
            }

            if (!string.IsNullOrEmpty(txbSN.Text))
            {
                strSql += string.Format(" and  DeviceSN like'%{0}%'", txbSN.Text);
            }

            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select(strSql);
            List<AlarmRecord> list = new List<AlarmRecord>();
            while (dr.Read())
            {
                AlarmRecord alarmRecord = new AlarmRecord();
                alarmRecord.ID = dr.GetInt("ID");
                alarmRecord.DeviceID = dr.GetInt("DeviceID");
                alarmRecord.DeviceSN = dr.GetStr("DeviceSN");
                alarmRecord.AlarmType = dr.GetStr("AlarmType");
                alarmRecord.BedNum = dr.GetStr("BedNum");
                alarmRecord.PatientName = dr.GetStr("PatientName");
                alarmRecord.Level = dr.GetStr("Level");
                alarmRecord.Val = dr.GetF("Val", 1);
                alarmRecord.ParaName = dr.GetStr("ParaName");
                alarmRecord.High = dr.GetF("High", 1);
                alarmRecord.Low = dr.GetF("Low", 1);
                alarmRecord.AlarmTime = DateTime.Parse(dr.GetStr("AlarmTime"));
                list.Add(alarmRecord);
            }

            dgAlarm.DataSource = list;
            dgAlarm.Refresh();
        }
    }
}

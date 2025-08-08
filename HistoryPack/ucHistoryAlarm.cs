using GlobalClass;
using MGOA_Pack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HistoryPack
{
    public partial class ucHistoryAlarm : UserControl
    {
        private List<BedDev> bedDevList = new List<BedDev>();
        private List<String> FullSN_List = new List<string>();
        private List<DateTime> Date_List = new List<DateTime>();
        private DateTime _dtBegin = DateTime.Now;

        public ucHistoryAlarm()
        {
            InitializeComponent();
            listView1.Dock = DockStyle.Fill;
        }

        private void ucAlarmTable_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCbBedData();
                if (this.cbBed.Items.Count > 0)
                    cbBed.SelectedIndex = 0;

                cbParaGroupSelector.Items.Clear();
                //DBReader dr = new DBReader(DBConnect.SYS);
                //dr.Select("Select * From ParaGroup Where Enable=1");
                //while (dr.Read())
                //{
                //    String FullSN = dr.GetStr("Path") + "." + dr.GetStr("SN");
                //    FullSN_List.Add(FullSN);
                //    cbParaGroupSelector.Items.Add(Lang.Get("MGOA", FullSN));
                //}
                //cbParaGroupSelector.SelectedIndex = 0;
            }
            catch
            {

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            DateTime begin = DateTime.Now;

            String FullSN = FullSN_List[cbParaGroupSelector.SelectedIndex];
            ParaGroup pGrp = ParaGroup.Find(FullSN);
            if (pGrp == null || cbDay.SelectedIndex < 0) return;

            DateTime day = Convert.ToDateTime(cbDay.SelectedItem.ToString());
            int devId = Convert.ToInt32(this.cbBed.SelectedValue.ToString());

            string queryData = $"select [dTime], [Value] from Alarms where UPPER([FullSN]) = UPPER('{FullSN}') AND [dTime] > '{day.ToString("yyyy-MM-dd HH:mm:ss")}' AND [dTime] < '{day.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")}'";
            DBReader pr = new DBReader(DBConnect.HisConn[devId]);
            pr.Select(queryData);
            while (pr.Read())
            {
                //Console.WriteLine(pr.GetStr(FullSN));
                string strVal = pr.GetStr("Value");
                ListViewItem lvi = new ListViewItem();
                lvi.Text = pr.GetStr("dTime");
                lvi.SubItems.Add(strVal);
                listView1.Items.Add(lvi);
            }
        }

        private void cbBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitParaSelector();
            InitDaySelector();
        }

        private void LoadCbBedData()
        {
            DBReader reader = new DBReader(DBConnect.SYS);
            reader.Select("select dev.ID, bdm.BedSN from BedDevMapping bdm left join Device dev on dev.DEVICEIP = bdm.IP WHERE dev.ID is NOT NULL and dev.STATUS = '1'");
            bedDevList.Clear();
            while (reader.Read())
            {
                bedDevList.Add(new BedDev() { BedSN = reader.GetStr("BedSN"), DevID = reader.GetInt("ID") });
            }
            cbBed.DisplayMember = "BedSN";
            cbBed.ValueMember = "DevID";
            cbBed.DataSource = bedDevList;

            if (cbBed.Items.Count > 0)
            {
                cbBed.SelectedIndex = 0;
            }
        }

        private void InitDaySelector()
        {
            Log.d("== Load_1 cost " + DateTime.Now.Subtract(_dtBegin).TotalMilliseconds + " ms");  // 23443.5414 ms
            // modify_by_limu_160615
            if (this.cbBed.Items.Count == 0)
                return;
            // 先清空日期下拉选项
            cbDay.Items.Clear();
            Date_List.Clear();
            // 
            // int boxnum = Convert.ToInt32(this.cbBed.SelectedValue.ToString().TrimStart('0'));
            int devId = int.Parse(this.cbBed.SelectedValue.ToString());
            string queryDay = "select min(dtime) as MinTime, max(dtime) as MaxTime from Alarms";
            DBReader pr = new DBReader(DBConnect.HisConn[devId]);
            pr.Select(queryDay);
            if (pr.Read())
            {
                DateTime minTime, maxTime;
                DateTime.TryParse(pr.GetStr("MinTime"), out minTime);
                DateTime.TryParse(pr.GetStr("MaxTime"), out maxTime);
                if (minTime.Year > 1 && maxTime.Year > 1)
                {
                    DateTime dt = minTime.Date;
                    while (dt > DateTime.MinValue && dt < maxTime)
                    {
                        Date_List.Add(dt);
                        cbDay.Items.Add(dt.ToLongDateString());
                        dt = dt.AddDays(1);
                    }
                    cbDay.SelectedIndex = cbDay.Items.Count - 1;
                }
            }

            Log.d("== Load_2 cost " + DateTime.Now.Subtract(_dtBegin).TotalMilliseconds + " ms");  // 23443.5414 ms
        }

        private void InitParaSelector()
        {
            //int devId = int.Parse(this.cbBed.SelectedValue.ToString());
            //string sql = $"SELECT * FROM AlarmPara WHERE DeviceID={devId}";
            //DBReader pr = new DBReader(DBConnect.SYS);
            //pr.Select(sql);
            //while (pr.Read())
            //{
            //    string FullSN= pr.GetStr("ParaName");
            //    FullSN_List.Add(FullSN);
            //    cbParaGroupSelector.Items.Add(Lang.Get("Para", FullSN));
            //}
            //cbParaGroupSelector.SelectedIndex = 0;
        }
    }
}

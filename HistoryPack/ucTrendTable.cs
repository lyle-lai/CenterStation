using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//----------------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
using System.IO;
using ObjPack;
using System.Linq;
//----------------------------------------------------------------------------
namespace HistoryPack
{
    //----------------------------------------------------------------------------
    public partial class ucTrendTable : UserControl
    {
        private List<BedDev> bedDevList = new List<BedDev>();
        private List<String> FullSN_List = new List<string>();
        private List<DateTime> Date_List = new List<DateTime>();
        private DateTime _dtBegin = DateTime.Now;

        //----------------------------------------------------------------------------
        public ucTrendTable()
        {
            InitializeComponent();
            listView1.Dock = DockStyle.Fill;
        }
        public void StartTick()
        {
            _dtBegin = DateTime.Now;
        }
        //----------------------------------------------------------------------------
        public void Clear()
        {
            listView1.Items.Clear();
            cbParaGroupSelector.SelectedIndex = 0;
            InitDaySelector();
        }
        //----------------------------------------------------------------------------
        public void LoadPara()
        {
            listView1.Items.Clear();

            DateTime begin = DateTime.Now;

            String FullSN = FullSN_List[cbParaGroupSelector.SelectedIndex];
            ParaGroup pGrp = ParaGroup.Find(FullSN);
            if (pGrp == null || cbDay.SelectedIndex < 0) return;

            DateTime day  = Convert.ToDateTime(cbDay.SelectedItem.ToString());
            int devId = Convert.ToInt32(this.cbBed.SelectedValue.ToString());

            string queryData = $"select [dTime], [Value] from Para where UPPER([FullSN]) = UPPER('{FullSN}') AND [dTime] > '{day.ToString("yyyy-MM-dd HH:mm:ss")}' AND [dTime] < '{day.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")}'";
            DBReader pr = new DBReader(DBConnect.HisConn[devId]);
            pr.Select(queryData);
            while(pr.Read())
            {
                //Console.WriteLine(pr.GetStr(FullSN));
                byte[] valBuf = pr.GetBlob("Value");
                Int16[] val = new Int16[pGrp.Childs.Count];
                string strVal = "/";
                if (valBuf != null)
                {
                    Buffer.BlockCopy(valBuf, 0, val, 0, valBuf.Length);
                    strVal = string.Join("/", val.Select(x => x.ToString()).ToArray());
                }

                ListViewItem lvi = new ListViewItem();
                lvi.Text = pr.GetStr("dTime");
                lvi.SubItems.Add(strVal);
                listView1.Items.Add(lvi);
            }


            //int boxnum = Convert.ToInt32(this.cbBed.SelectedValue.ToString().TrimStart('0'));
            ////string dayS = cbDay.SelectedItem.ToString();
            //DateTime day = Convert.ToDateTime(cbDay.SelectedItem.ToString());

            //OracleHelper dbHelper = new OracleHelper();
            ////string queryData = string.Format(@"Select meastime, val1, val2, val3 From icu_realtimedata Where paragroup = '{0}' and boxnum = '{1}' and meastime >= to_date('{2}','yyyy-mm-dd hh24:mi:ss') And meastime<=to_date('{3}','yyyy-mm-dd hh24:mi:ss') Order By meastime ASC", FullSN, boxnum, day.ToString("yyyy-MM-dd HH:mm:ss"), day.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));
            //string queryData = string.Format(@"select meastime, case when (val2 is not null and val3 is null) then (val1 || '/' || val2) when (val2 is not null and val3 is not null) then (val1 || '/' || val2 || '(' || val3 || ')') else val1 end as value from ( Select meastime, val1, val2, val3 From icu_realtimedata Where upper(paragroup) = '{0}' and boxnum = '{1}' and meastime >= to_date('{2}','yyyy-mm-dd hh24:mi:ss') And meastime<=to_date('{3}','yyyy-mm-dd hh24:mi:ss') Order By meastime ASC )", FullSN.ToUpper(), boxnum, day.ToString("yyyy-MM-dd HH:mm:ss"), day.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));
            //DataTable dt = dbHelper.Query(queryData);

            //Log.d("== Query cost " + DateTime.Now.Subtract(begin).TotalMilliseconds + " ms");  // 1324.8782 ms

            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        //
            //        ListViewItem lvi = new ListViewItem();
            //        listView1.Items.Add(lvi);
            //        lvi.Text = dt.Rows[i]["meastime"].ToString();
            //        lvi.SubItems.Add(dt.Rows[i]["Value"].ToString());
            //    }
            //}
            //Log.d("== draw cost " + DateTime.Now.Subtract(begin).TotalMilliseconds + " ms");  // 1416.9426 ms

            /** 
            //DBReader dr = new DBReader(DBConnect.DATA);
            //dr.Select("Select * From Para Where ConnectID=" + cbBed.SelectedIndex.ToString() + " And GroupName='" + FullSN + "'");
            DBReader dr = new DBReader(DBConnect.HisConn[cbBed.SelectedIndex]);
            //dr.Select("Select * From [" + FullSN + "] Order By dTime DESC Limit 3600");
            DateTime st = Date_List[cbDay.SelectedIndex];
            DateTime et = st.AddDays(1);
            dr.Select("Select dTime, [" + FullSN + "] as Value From Para Where dTime>='" + GLB.DBTime(st) + "' And dTime<='" + GLB.DBTime(et)
                + "' And Value Is NOT Null Order By dTime ASC");
            //
            while (dr.Read())
            {
                Byte[] VData = dr.GetBlob("Value");
                if (VData == null) continue;
                //
                ListViewItem lvi = new ListViewItem();
                listView1.Items.Add(lvi);
                lvi.Text = dr.GetStr("dTime");
                //
                String Str = "";
                for (int i = 0; i < pGrp.PItems.Length; i++)
                {
                    String Val = pGrp.PItems[i].GetValue(BitConverter.ToInt16(VData, i * 2));
                    switch (i)
                    {
                        case 0: Str = Str + Val; break;
                        case 1: Str = Str + "/" + Val; break;
                        case 2: Str = Str + "(" + Val + ")"; break;
                        default: break;
                    }
                }
                lvi.SubItems.Add(Str);
            }
             **/
        }
        //----------------------------------------------------------------------------
        private void ucTrendTable_Load(object sender, EventArgs e)
        {
            // add_by_limu_160615
            try 
            {
                LoadCbBedData();
                if (this.cbBed.Items.Count > 0)
                    cbBed.SelectedIndex = 0;

                cbParaGroupSelector.Items.Clear();
                DBReader dr = new DBReader(DBConnect.SYS);
                dr.Select("Select * From ParaGroup Where Enable=1");
                while (dr.Read())
                {
                    String FullSN = dr.GetStr("Path") + "." + dr.GetStr("SN");
                    FullSN_List.Add(FullSN);
                    cbParaGroupSelector.Items.Add(Lang.Get("MGOA", FullSN));
                }
                cbParaGroupSelector.SelectedIndex = 0;
            }
            catch
            {

            }

        }
        //----------------------------------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadPara();
        }
        //----------------------------------------------------------------------------
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
            string queryDay = "select min(dtime) as MinTime, max(dtime) as MaxTime from Para";
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
            //string queryDay = @"select min(meastime) as MinTime, max(meastime) as MaxTime from icu_realtimedata where boxnum = '" + boxnum + "'";
            //OracleHelper dbHelper = new OracleHelper();
            //DataTable data = dbHelper.Query(queryDay);
            //if (data.Rows.Count > 0)
            //{
            //    DateTime minTime, maxTime;
            //    DateTime.TryParse(data.Rows[0]["MinTime"].ToString(), out minTime);
            //    DateTime.TryParse(data.Rows[0]["MaxTime"].ToString(), out maxTime);
            //    if (minTime.Year > 1 && maxTime.Year > 1)
            //    {
            //        DateTime dt = minTime.Date;
            //        while (dt > DateTime.MinValue && dt <= maxTime)
            //        {
            //            Date_List.Add(dt);
            //            cbDay.Items.Add(dt.ToLongDateString());
            //            dt = dt.AddDays(1);
            //        }
            //        cbDay.SelectedIndex = cbDay.Items.Count - 1;
            //    }
            //}
            Log.d("== Load_2 cost " + DateTime.Now.Subtract(_dtBegin).TotalMilliseconds + " ms");  // 23443.5414 ms

            //DBReader dr = new DBReader(DBConnect.HisConn[cbBed.SelectedIndex]);
            //dr.Select("SELECT min(dTime) as MinTime, max(dTime) as MaxTime FROM Para Where dTime>'" + GLB.DBTime(GLB.MinTime) + "'");
            //dr.Read();
            //DateTime MinTime = dr.GetDateTime("MinTime");
            //DateTime MaxTime = dr.GetDateTime("MaxTime");
            //
            //cbDay.Items.Clear();
            //Date_List.Clear();
            //DateTime dt = MinTime.Date;
            //while (dt > DateTime.MinValue && dt <= MaxTime)
            //{
            //    Date_List.Add(dt);
            //    cbDay.Items.Add(dt.ToLongDateString());
            //    dt = dt.AddDays(1);
            //}
            //cbDay.SelectedIndex = cbDay.Items.Count - 1;
        }
        //----------------------------------------------------------------------------
        private void cbBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDaySelector();
        }
        //----------------------------------------------------------------------------
        // add_by_limu_160614
        private void LoadCbBedData()
        {
            // 动态加载床位
            ////OracleConnection OConn = new OracleConnection();
            //OracleHelper dbHeler = new OracleHelper();
            //DataTable dt = dbHeler.Query("select distinct boxnum, boxname, barcode from icu_box where flag = 'Y' order by boxnum asc");
            ////for (int i = 0; i < dt.Rows.Count; i++)
            ////{
            ////    this.cbBed.Items.Add(dt.Rows[i]["boxname"]);
            ////}
            //cbBed.DisplayMember = "boxname";
            //cbBed.ValueMember = "boxnum";
            //cbBed.DataSource = dt;
            //if (this.cbBed.Items.Count > 0)
            //    this.cbBed.SelectedIndex = 0;

            //if (cbBed.Items.Count > 17)
            //{
            //    cbBed.SelectedValue = 17;
            //}
            DBReader reader = new DBReader(DBConnect.SYS);
            reader.Select("select dev.ID, bdm.BedSN from BedDevMapping bdm left join Device dev on dev.DEVICEIP = bdm.IP WHERE dev.ID is NOT NULL and dev.STATUS = '1'");
            bedDevList.Clear();
            while(reader.Read())
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

            //this.cbBed.Items.AddRange(new object[] {
            //"1 床",
            //"2 床",
            //"3 床",
            //"4 床",
            //"5 床",
            //"6 床",
            //"7 床",
            //"8 床",
            //"9 床",
            //"10 床",
            //"11 床",
            //"12 床",
            //"13 床",
            //"14 床",
            //"15 床",
            //"16 床"});
        }
    }
    //----------------------------------------------------------------------------
    internal class BedDev
    {
        public string BedSN { get; set; }
        public int DevID { get; set; }
    }
}

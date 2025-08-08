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
//----------------------------------------------------------------------------
namespace HistoryPack
{
    //----------------------------------------------------------------------------
    public partial class ucTrendChart : UserControl
    {
        private List<String> FullSN_List = new List<string>();
        private PointF[] PrePointList = null;
        private String FullSN;
        private ParaGroup pGrp = null;
        private Pen p;
        private SolidBrush brush;
        //-------------------------------
        private int PPS = 5;//每点秒数
        private int PageSec = 1;//每页秒数
        private int PageCount = 0;//页数
        private DateTime MinTime;
        private DateTime MaxTime;
        //-------------------------------
        private List<DateTime> Date_List = new List<DateTime>();
        //----------------------------------------------------------------------------
        private List<BedDev> bedDevList = new List<BedDev>();
        //----------------------------------------------------------------------------
        public ucTrendChart()
        {
            InitializeComponent();
            PanChart.Dock = DockStyle.Fill;
        }
        //----------------------------------------------------------------------------
        private void ucTrendChart_Load(object sender, EventArgs e)
        {
            LoadCbBedData();
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
            InitDaySelector();
        }
        //----------------------------------------------------------------------------
        private void DrawChart()
        {
            Graphics DC = PanChart.CreateGraphics();
            DC.Clear(PanChart.BackColor);
            //
            if (pGrp == null || MinTime == DateTime.MinValue) return;
            //
            Rectangle Rect = new Rectangle(0, PanCooY.Font.Height / 2, PanChart.Width, PanChart.Height - PanCooY.Font.Height);
            //
            PrePointList = new PointF[pGrp.PItems.Length];
            for (int i = 0; i < PrePointList.Length; i++)
                PrePointList[i] = new Point(-1, 0);
            //
            DateTime st = MinTime.AddSeconds(trackBar1.Value);
            DateTime et = st.AddSeconds(PageSec);
            //
            int devId = int.Parse(cbBed.SelectedValue.ToString());
            DBReader dr = new DBReader(DBConnect.HisConn[devId]);
            /*dr.Select("Select * From [" + FullSN + "] Where "
                            + " dTime >='" + GLB.DBTime(st) + "' And dTime<='" + GLB.DBTime(et) + "'"
                            + " Order By dTime ASC");*/
            dr.Select("Select dTime, Value From Para Where Value Is NOT Null And UPPER([FullSN]) = UPPER('" + FullSN + "') AND "
                            + " dTime >='" + GLB.DBTime(st) + "' And dTime<='" + GLB.DBTime(et) + "'"
                            + " Order By dTime ASC");
            //
            DateTime sTime = DateTime.MinValue;
            while (dr.Read())
            {
                if (sTime == DateTime.MinValue)
                    sTime = dr.GetDateTime("dTime");
                //
                Byte[] VData = dr.GetBlob("Value");
                if (VData == null) continue;
                //
                DateTime dTime = dr.GetDateTime("dTime");
                TimeSpan Span = dTime - sTime;
                float xDraw = (float)Span.TotalSeconds / PPS;  //5秒1个点
                //
                for (int i = 0; i < pGrp.PItems.Length; i++)
                {
                    Int16 Val = BitConverter.ToInt16(VData, i * 2);
                    //float yDraw = (float)((pGrp.PItems[i].Max - Val) * PanChart.Height) / (pGrp.PItems[i].Max - pGrp.PItems[i].Min);
                    float yDraw = Rect.Top + pGrp.PItems[i].GetDrawY(Val, Rect.Height);
                    if (GLB.Same("NIBP.NIBP", FullSN))
                        DC.FillRectangle(brush, new RectangleF(xDraw - 1, yDraw - 1, 5, 5));
                    else
                    {
                        if (PrePointList[i].X >= 0)
                        {
                            PointF P1 = new PointF(PrePointList[i].X, yDraw);
                            PointF P2 = new PointF(xDraw, yDraw);
                            DC.DrawLine(p, PrePointList[i], P1);
                            DC.DrawLine(p, P1, P2);
                        }
                    }
                    //
                    PrePointList[i].X = xDraw;
                    PrePointList[i].Y = yDraw;
                }
            }
            //
            DC.DrawString(GLB.ShortDT(st.ToString()), this.Font, brush, 0, 0);
            DC.DrawString(GLB.ShortDT(et.ToString()), this.Font, brush, PanChart.Width-80, 0);
        }
        //----------------------------------------------------------------------------
        private void DrawCooY()
        {//画纵坐标轴
            Graphics DC = PanCooY.CreateGraphics();
            DC.Clear(PanCooY.BackColor);
            //
            if (pGrp == null) return;
            //初始化轴位置
            float xRight = PanCooY.Width - p.Width;
            float yTop = PanCooY.Font.Height / 2;
            float yBot = PanCooY.Height - PanCooY.Font.Height / 2;
            //
            DC.DrawLine(p, xRight, yTop, xRight, yBot);
            //
            float dy = (yBot - yTop) / 10;
            float dv = (pGrp.Max - pGrp.Min) / 10;
            for (int i = 0; i <= 10; i++)
                DrawCooLine(DC, (int)(pGrp.Min + dv * i), xRight, yBot - dy * i);
        }
        //----------------------------------------------------------------------------
        private void DrawCooLine(Graphics DC, int Val, float xRight, float y)
        {
            DC.DrawString(pGrp.GetValueString(Val), PanCooY.Font, brush, 0, y - PanCooY.Font.Height / 2);
            DC.DrawLine(p, xRight - 5, y, xRight, y);
        }
        //----------------------------------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DrawAll();
        }
        //----------------------------------------------------------------------------
        private void PanChart_Paint(object sender, PaintEventArgs e)
        {
            DrawAll();
        }
        //----------------------------------------------------------------------------
        private void DrawAll()
        {
            FullSN = FullSN_List[cbParaGroupSelector.SelectedIndex];
            pGrp = ParaGroup.Find(FullSN);
            if (pGrp == null || cbDay.SelectedIndex < 0) return;
            //
            p = new Pen(pGrp.dColor);
            brush = new SolidBrush(pGrp.dColor);
            //
            PageSec = PanChart.Width * PPS;
            //
            int devId = int.Parse(cbBed.SelectedValue.ToString());
            DBReader dr = new DBReader(DBConnect.HisConn[devId]);
            //dr.Select("SELECT min(dTime) as MinTime, max(dTime) as MaxTime FROM [" + FullSN + "]");
            DateTime st = Date_List[cbDay.SelectedIndex];
            DateTime et = st.AddDays(1);
            dr.Select("SELECT min(dTime) as MinTime, max(dTime) as MaxTime FROM Para Where dTime>='" 
                + GLB.DBTime(st) + "' And dTime<='" + GLB.DBTime(et)+ "'");
            dr.Read();
            MinTime = dr.GetDateTime("MinTime");
            MaxTime = dr.GetDateTime("MaxTime");
            trackBar1.Minimum = 0;
            trackBar1.Maximum = (int)(MaxTime - MinTime).TotalSeconds - PageSec;
            //
            PageCount = trackBar1.Maximum / PageSec;
            trackBar1.LargeChange = PageSec;
            trackBar1.Value = trackBar1.Maximum;
            //
            DrawChart();
            DrawCooY();
            //
        }
        //----------------------------------------------------------------------------
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            DrawChart();
            DrawCooY();
        }
        //----------------------------------------------------------------------------
        private void InitDaySelector()
        {
            int devId = int.Parse(cbBed.SelectedValue.ToString());
            DBReader dr = new DBReader(DBConnect.HisConn[devId]);
            dr.Select("SELECT min(dTime) as MinTime, max(dTime) as MaxTime FROM Para Where dTime>'" + GLB.DBTime(GLB.MinTime) + "'");
            dr.Read();
            DateTime MinTime = dr.GetDateTime("MinTime");
            DateTime MaxTime = dr.GetDateTime("MaxTime");

            cbDay.Items.Clear();
            Date_List.Clear();
            DateTime dt = MinTime.Date;
            while (dt > DateTime.MinValue && dt <= MaxTime)
            {
                Date_List.Add(dt);
                cbDay.Items.Add(dt.ToLongDateString());
                dt = dt.AddDays(1);
            }
            cbDay.SelectedIndex = cbDay.Items.Count - 1;
        }
        //----------------------------------------------------------------------------
        private void cbBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDaySelector();
        }
        //----------------------------------------------------------------------------
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
    }
    //----------------------------------------------------------------------------
}

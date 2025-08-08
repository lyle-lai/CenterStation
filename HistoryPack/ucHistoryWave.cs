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
using ViewPack;
using System.Text.RegularExpressions;
using System.Linq;
//----------------------------------------------------------------------------
namespace HistoryPack
{
    public partial class ucHistoryWave : UserControl
    {
        private List<BedDev> bedDevList = new List<BedDev>();
        //------------------------------------------------------------------------------
        public List<MGOA> MList = new List<MGOA>();
        public List<WaveGroup> Wave_List = new List<WaveGroup>();
        public List<ViewWaveGroup> vwList = null;
        private int ViewID = 0;
        private ViewList ViewW = null;
        //----------------------------------------------------------------------------
        private ViewWaveGroup vwGrp = null;
        private WaveGroup wGrp = null;
        private Pen p;
        private SolidBrush brush;
        //-------------------------------
        private List<DateTime> Date_List = new List<DateTime>();
        //-------------------------------
        private const int PageSec = 6;//每页秒数
        private int PageCount = 0;//页数
        private DateTime MinTime;
        private DateTime MaxTime;
        //
        //定义作画线程及委托        
        public delegate void DrawInvoke();
        private DrawInvoke drawInvoke = null;
        private BackgroundWorker bw = new BackgroundWorker();
        //
        public delegate void InitInvoke();
        private InitInvoke initInvoke = null;
        private BackgroundWorker bwInit = new BackgroundWorker();
        //----------------------------------------------------------------------------
        public ucHistoryWave()
        {
            InitializeComponent();
            ViewW = new ViewList();
            ViewW.BackColor = Color.Black;
            ViewW.Parent = this;
            ViewW.Visible = true;
            ViewW.Dock = DockStyle.Fill;
            PanWave.Dock = DockStyle.Fill;
            //
            bw.DoWork += bw_DoWork;
            drawInvoke = new DrawInvoke(DrawWave);
            //
            bwInit.DoWork += bwInit_DoWork;
            initInvoke = new InitInvoke(InitMGOA);
        }
        //----------------------------------------------------------------------------
        private void ucHistoryWave_Load(object sender, EventArgs e)
        {
            //---------------
            LoadCbBedData();
            cbBed.SelectedIndex = 0;
            if(MList.Count<=0)
            {
                InitMGOA();
                cbGroupSelector.Items.Clear();
                for(int i=0; i<Wave_List.Count; i++)
                    cbGroupSelector.Items.Add(Lang.Get("MGOA", Wave_List[i].FullSN));
            }
            //
            cbGroupSelector.SelectedIndex = 0;
            InitTimeSelector();
        }
        //----------------------------------------------------------------------------
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginInvoke(drawInvoke);
            //DrawWave();
        }
        //----------------------------------------------------------------------------
        private void bwInit_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginInvoke(initInvoke);
        }
        //----------------------------------------------------------------------------
        public void Init(int Index)
        {
            ViewID = Index;
            //InitMGOA();
            //vwList = ViewW.LoadWaveList(ViewID, MList);
        }
        //----------------------------------------------------------------------------
        public void InitMGOA()
        {
            //Log.d("InitMGOA 0 " + DateTime.Now.Millisecond);
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Module Where Enable=1 And ID>=10 Order By ID");
            if (dr.Count <= 0) return;
            while (dr.Read())
                MList.Add(new MGOA(null, dr));
            BindGroups();
            //vwList = ViewW.LoadWaveList(ViewID, MList);
            vwList = ViewW.LoadWaveList(-1, MList);
        }
        //------------------------------------------------------------------------------
        public void BindGroups()
        {
            //绑定参数组,波形组
            for (int mi = 0; mi < MList.Count; mi++)
            {
                MGOA M = MList[mi];
                for (int gi = 0; gi < M.Childs.Count; gi++)
                {
                    MGOA G = M.Childs[gi];
                    //绑定波形组
                    if (!(G is WaveGroup)) continue;
                    Wave_List.Add((WaveGroup)G);
                }
            }
        }
        //------------------------------------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
            /*FullSN = FullSN_List[cbParaGroupSelector.SelectedIndex];
            wGrp = new WaveGroup( WaveGroup.Find(FullSN);
            p = new Pen(wGrp.dColor);
            brush = new SolidBrush(wGrp.dColor);
            */
        }
        //----------------------------------------------------------------------------
        private void Search()
        {
            wGrp = Wave_List[cbGroupSelector.SelectedIndex];
            if (wGrp == null || cbTime.SelectedIndex < 0) return;
            //
            p = new Pen(wGrp.dColor);
            brush = new SolidBrush(wGrp.dColor);
            //
            int devId = int.Parse(cbBed.SelectedValue.ToString());
            DBReader dr = new DBReader(DBConnect.HisConn[devId]);
            DateTime st = Date_List[cbTime.SelectedIndex];
            DateTime et = st.AddHours(1);
            //dr.Select("SELECT min(sTime) as MinTime, max(eTime) as MaxTime FROM Wave Where sTime>='"
            //    + GLB.DBTime(st) + "' And sTime<='" + GLB.DBTime(et) + "'");
            string sql = $"SELECT min(dTime) as MinTime, max(dTime) as MaxTime FROM Wave WHERE UPPER(FullSN) = UPPER('{wGrp.FullSN}') AND dTime >= '{GLB.DBTime(st)}' AND dTime <= '{GLB.DBTime(et)}'";
            dr.Select(sql);
            dr.Read();
            MinTime = dr.GetDateTime("MinTime");
            MaxTime = dr.GetDateTime("MaxTime");
            //
            trackBar1.ValueChanged -= trackBar1_ValueChanged;
            trackBar1.Minimum = 0;
            trackBar1.Maximum = (int)(MaxTime - MinTime).TotalSeconds - PageSec;
            //
            PageCount = trackBar1.Maximum / PageSec;
            trackBar1.LargeChange = PageSec;
            trackBar1.Value = trackBar1.Maximum;
            trackBar1.ValueChanged += trackBar1_ValueChanged;
            //
            //DrawWave();
            if (!bw.IsBusy)
                bw.RunWorkerAsync();
        }
        //----------------------------------------------------------------------------
        private void DrawWave()
        {
            Graphics DC = PanTitle.CreateGraphics();
            DC.Clear(PanTitle.BackColor);
            //
            if (wGrp == null || MinTime == DateTime.MinValue)
            {
                DC.DrawString("该波形此时间段内无数据", this.Font, brush, 0, 0);
                if (vwGrp != null) vwGrp.Visible = false;
                return;
            }
            //
            DateTime st = MinTime.AddSeconds(trackBar1.Value);
            DateTime et = st.AddSeconds(PageSec);
            //
            int devId = int.Parse(cbBed.SelectedValue.ToString());
            DBReader dr = new DBReader(DBConnect.HisConn[devId]);
            string sql = $"SELECT dTime, Value FROM Wave WHERE Value IS NOT NULL AND UPPER([FullSN]) = UPPER('{wGrp.FullSN}') AND dTime >= '{GLB.DBTime(st)}' AND dTime <= '{GLB.DBTime(et)}' ORDER BY dTime ASC";
            //dr.Select("Select dTime, Value From Wave Where Value Is NOT Null And UPPER([FullSN]) = UPPER('" + wGrp.FullSN + "') AND "
            //                + " dTime >='" + GLB.DBTime(st) + "' And dTime<='" + GLB.DBTime(et) + "'"
            //                + " Order By dTime ASC");
            dr.Select(sql);
            //
            vwGrp = vwList[cbGroupSelector.SelectedIndex];
            for (int i = 0; i < vwList.Count; i++)
                    vwList[i].Visible = false;
            vwGrp.Parent = PanWave;
            vwGrp.Visible = true;
            vwGrp.Dock = DockStyle.Fill;
            //
            vwGrp.StartDirect(0.75F);
            while (dr.Read())
            {
                Byte[] VData = dr.GetBlob("Value");
                if (VData == null) continue;

                float[] data = new float[VData.Length / sizeof(float)];
                Buffer.BlockCopy(VData, 0, data, 0, VData.Length);
                float aveDX = (float)(100 * 1000 * vwGrp.waveGroup.Count / (data.Length * 1000));
                //vwGrp.aveDX = aveDX;
                //vwGrp.aveDX = (float)data.Length / 1000;

                //DateTime sTime = dr.GetDateTime("sTime");
                //DateTime eTime = dr.GetDateTime("eTime");
                //TimeSpan Span = eTime-sTime;
                //if (VData == null) continue;
                ////int Len = VData.Length;
                ////float Freq = VData.Length/ (TimeSpan) (eTime-sTime).toto
                ////
                //float[] data = new float[VData.Length / sizeof(float)];
                //Buffer.BlockCopy(VData, 0, data, 0, VData.Length);
                //// 计算需要绘制的数据区域
                //int dfSec = (int)Math.Round((eTime - sTime).TotalSeconds); // 这条记录有多少秒的数据
                //int dsPer = data.Length / dfSec;    // 每秒多少个数
                //int skipSec = (int)Math.Round((st - sTime).TotalSeconds);   // 忽略前面多长时间的数据
                //if (skipSec < 0) { skipSec = 0; }
                //int skipCount = (skipSec * dsPer) > data.Length ? data.Length : (skipSec * dsPer);
                //int takeCount = (dsPer * PageSec) + skipCount > data.Length ? (data.Length - skipCount) : (dsPer * PageSec);

                //float[] pure = data.Skip(skipCount).Take(takeCount).ToArray();
                ////float aveDX = (float)(100 * Span.TotalMilliseconds * vwGrp.waveGroup.Count / (pure.Length * 1000));
                //float aveDX = (float)(100 * 6000 * vwGrp.waveGroup.Count / (dsPer * 6 * 1000));
                ////float aveDX = (float)(100 * Span.TotalMilliseconds * 1 / (data.Length * 1000));
                //vwGrp.aveDX = aveDX;

                for (int i = 0; i < data.Length; )
                {
                    i += vwGrp.SetDataDirect(data, i);
                    if (vwGrp.DirX > PanWave.Width) break;
                }
                if (vwGrp.DirX > PanWave.Width) break;
            }
            vwGrp.DrawDirect();
            //
            DC.DrawString(st.ToString(), this.Font, brush, 0, 0);
        }
        //----------------------------------------------------------------------------
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            //DrawWave();
            if (!bw.IsBusy)
                bw.RunWorkerAsync();
        }
        //----------------------------------------------------------------------------
/*        private void InitTimeSelector()
        {
            Log.d("InitTimeSelector 0 " + DateTime.Now.Millisecond);
            DBReader dr = new DBReader(DBConnect.HisConn[cbBed.SelectedIndex]);
            dr.Select("SELECT min(sTime) as MinTime, max(sTime) as MaxTime FROM Wave Where sTime>'" + GLB.DBTime(GLB.MinTime) + "'");
            Log.d("InitTimeSelector 1 " + DateTime.Now.Millisecond);
            dr.Read();
            Log.d("InitTimeSelector 1-1 " + DateTime.Now.Millisecond);
            DateTime MinTime = dr.GetDateTime("MinTime");
            DateTime MaxTime = dr.GetDateTime("MaxTime");

            cbTime.Items.Clear();
            Date_List.Clear();
            DateTime dt = MinTime.Date.AddHours(MinTime.Hour);
            Log.d("InitTimeSelector 1-2 " + DateTime.Now.Millisecond);
            while (dt > DateTime.MinValue && dt <= MaxTime)
            {
                Date_List.Add(dt);
                cbTime.Items.Add(GLB.DHour(dt));
                
                //Date_List[0] = dt;
                //cbTime.Items[0] = GLB.DHour(dt);

                dt = dt.AddHours(1);
            }
            Log.d("InitTimeSelector 1-3 " + DateTime.Now.Millisecond);
            cbTime.SelectedIndex = cbTime.Items.Count - 1;
            Log.d("InitTimeSelector 2 " + DateTime.Now.Millisecond);
        }*/
        //----------------------------------------------------------------------------
        private void InitTimeSelector()
        {
            DateTime dt = DateTime.Now.Date;
            dt = dt.AddHours(DateTime.Now.Hour);
            dt = dt.AddHours(-23);
            //
            if (cbTime.Items.Count <= 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    Date_List.Add(dt);
                    cbTime.Items.Add(GLB.DHour(dt));
                    dt = dt.AddHours(1);
                }
            }
            else
            {
                for (int i = 0; i < 24; i++)
                {
                    Date_List[i] = dt;
                    cbTime.Items[i] = GLB.DHour(dt);
                    dt = dt.AddHours(1);
                }
            }
            cbTime.SelectedIndex = cbTime.Items.Count - 1;
        }
        //----------------------------------------------------------------------------
        private void cbBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitTimeSelector();
            //if (!bwInit.IsBusy)
                //bwInit.RunWorkerAsync();
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

        //----------------------------------------------------------------------------
    }
}

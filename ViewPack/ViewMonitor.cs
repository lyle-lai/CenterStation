using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Drawing;
using System.Net.Sockets;
//-----------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
using System.Windows.Forms;
using DemoPack;
using History_Pack;
using ProtocolPack;
using ObjPack;

// using DotNetSpeech;
// using System.Data.SQLite;
// using System.Data;
//-----------------------------------------------------------------------
namespace ViewPack
{
    //-----------------------------------------------------------------------
    public class ViewMonitor : ViewBase
    {
        //------------------------------------------------------------------------------
        public List<MGOA> MList = new List<MGOA>();
        public List<ParaGroup> Para_List = new List<ParaGroup>();

        public List<WaveGroup> Wave_List = new List<WaveGroup>();

        //------------------------------------------------------------------------------
        public ViewList ViewP1 = new ViewList();
        public ViewList ViewP2 = new ViewList();

        public ViewList ViewW = new ViewList();

        //
        public ViewTitle VTitle = null;
        public ViewBase VMain = new ViewBase();
        public List<ViewWaveGroup> vwList = null;
        public int ViewID = 0;

        private string mViewIP;

        public String ViewIP
        {
            get { return mViewIP; }
            set { mViewIP = value; }
        }

        // 设备类型
        public String DeviceType { get; set; }

        //演示数据
        private List<DemoPara> DemoP = new List<DemoPara>();

        private List<DemoWave> DemoW = new List<DemoWave>();

        //历史数据保存
        private History history = null;

        //
        public Boolean IsVisible = false;

        public Boolean IsConnected = false;

        //数据发送
        //public delegate void SEND_COMMAND(String FullSN, int Value);
        public GLB.SEND_COMMAND SendCommand = null;

        //
        private delegate void BeginInvokeDelegate();

        //-------
        public PrtServerBase ServerBase = null;

        //
        public String InHosNum { get; set; }

        private string mDeviceID = string.Empty;
        public String DeviceID { get; set; }
        public String BoxNum { get; set; }

        private string strBedNum = string.Empty;
        public String BedNum { get; set; }

        public List<AlarmPara> mALarmPara = new List<AlarmPara>();

        private System.Timers.Timer timer = new System.Timers.Timer(500); //实例化Timer类，设置间隔时间为100毫秒；

        public delegate void AddAlarmRecord(AlarmRecord alarmRecord);

        public static event AddAlarmRecord addAlarmRecord = null;

        private Graphics DC = null;

        //------------------------------------------------------------------------------
        public ViewMonitor(int Index, List<AlarmPara> list, bool isShowScreen = true)
        {
            mALarmPara = list;
            ViewID = Index;
            this.Padding = new Padding(1, 1, 1, 1);
            InitMGOA();
            //
            VTitle = new ViewTitle(ViewID, isShowScreen);
            VTitle.Parent = this;
            VTitle.Visible = true;
            VTitle.Height = 24;
            // VTitle.Height = 58;
            VTitle.BackColor = Color.LightBlue;
            VTitle.Dock = DockStyle.Top;
            //VTitle.Paint += Title_Paint;
            //
            VMain.Parent = this;
            VMain.Visible = true;
            VMain.BackColor = Color.Black;
            //
            ViewP1.Parent = VMain;
            ViewP1.Visible = IsVisible;
            ViewP1.LoadParaList(ViewID, 1, MList);
            ViewP1.BackColor = Color.Black;
            //-----------------------
            ViewP2.Parent = VMain;
            ViewP2.Visible = IsVisible;
            ViewP2.LoadParaList(ViewID, 2, MList);
            ViewP2.BackColor = Color.Black;
            //-----------------------
            ViewW.BackColor = Color.Black;
            ViewW.Parent = VMain;
            ViewW.Visible = IsVisible;
            vwList = ViewW.LoadWaveList(ViewID, MList);

            //
            //history = new History(ViewID, Para_List, Wave_List);

            //2020-01-16 lim
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(AlarmNotify);//到达时间的时候执行事件；
            //timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

            // ParaItem.onAlarm += ParaItem_onAlarm;
            //  ViewTitle.closeAlarmEvent += CloseAlarm;
            this.DC = this.CreateGraphics();
        }

        public void SetSize(double widthPercentage,double heightPercentage)
        {
            // VMain.Width = (int) (VMain.Width * widthPercentage);
            // VMain.Height = (int) (VMain.Height * heightPercentage);
            //
            // ViewP1.Width = (int) (ViewP1.Width * widthPercentage);
            // ViewP1.Height = (int) (ViewP1.Height * heightPercentage);
            //
            // ViewP2.Width = (int) (ViewP2.Width * widthPercentage);
            // ViewP2.Height = (int) (ViewP2.Height * heightPercentage);
            //
            // ViewW.Width = (int) (ViewW.Width * widthPercentage);
            // ViewW.Height = (int) (ViewW.Height * heightPercentage);

            VMain.Dock = DockStyle.Fill;
            // VMain.Width = Parent.Size.Height / 5;
            // VMain.Width = Parent.Size.Width / 5;
            // ViewW.Dock = DockStyle.Fill;
            // ViewP1.Dock = DockStyle.Fill;
            // ViewP2.Dock = DockStyle.Fill;

            // VMain.BackColor = Color.Black;
            // ViewP1.BackColor = Color.Black;
            // ViewP2.BackColor = Color.Black;
            // //-----------------------
            // ViewW.BackColor = Color.Black;
        }

        bool isBlack = false;

        /// <summary>
        /// 定时更新背景颜色
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void AlarmNotify(object source, System.Timers.ElapsedEventArgs e)
        {
            if (isCloseAlarm)
            {
                VTitle.BackColor = Color.LightBlue;
                System.Threading.Thread.Sleep(1000);
                return;
            }
            else
            {
                VTitle.BackColor = Color.Red;
            }

            if (isBlack)
            {
                isBlack = false;
                ViewP1.BackColor = Color.Transparent;
                ViewP2.BackColor = Color.Transparent;
            }
            else
            {
                isBlack = true;
                ViewP1.BackColor = Color.Black;
                ViewP2.BackColor = Color.Black;
            }
        }

        public bool isCloseAlarm = false; //是否为手动关闭告警，如果是 则有新告警过来将不告警，除非重启
        private AlarmRecord CurAlarmRecord = new AlarmRecord();

        /// <summary>
        /// 告警
        /// </summary>
        /// <param name="key"></param>
        /// <param name="curVal"></param>
        /// <param name="para"></param>
        public void ParaItem_onAlarm(int ViewID, string key, double curVal, AlarmPara para,
            ConcurrentQueue<string> queue)
        {
            try
            {
                if (this.ViewID == ViewID && para != null)
                {
                    this.timer.Enabled = true; //定时器启动

                    string AlarmType = string.Empty;
                    if (curVal > para.High)
                    {
                        AlarmType = "超上限告警";
                    }
                    else if (curVal < para.Low)
                    {
                        AlarmType = "超下限告警";
                    }

                    if (CurAlarmRecord != null && this.VTitle != null && para != null)
                    {
                        CurAlarmRecord.DeviceID = int.Parse(this.VTitle.Id);
                        CurAlarmRecord.DeviceSN = this.VTitle.BedSN;
                        CurAlarmRecord.AlarmType = AlarmType;
                        CurAlarmRecord.BedNum = BedNum;
                        CurAlarmRecord.PatientName = this.VTitle.PatientName;
                        CurAlarmRecord.Level = para.Level == 0 ? "高" : para.Level == 1 ? "中" : "低";
                        CurAlarmRecord.Val = curVal;
                        CurAlarmRecord.ParaName = para.ParaName;
                        CurAlarmRecord.High = para.High;
                        CurAlarmRecord.Low = para.Low;
                        CurAlarmRecord.AlarmTime = DateTime.Now;

                        // string strSpeenVal = string.Format("床号：{0}，病人姓名：{1}：监护仪：{2}，{7}当前值：{3}，最高值为{4}，最低值{5}，{6}",
                        //          CurAlarmRecord.BedNum, CurAlarmRecord.PatientName, CurAlarmRecord.DeviceSN, CurAlarmRecord.Val, CurAlarmRecord.High,
                        //          CurAlarmRecord.Low, CurAlarmRecord.AlarmType, CurAlarmRecord.ParaName);
                        string strSpeenVal = CurAlarmRecord.BedNum + "号床";
                        if (!string.IsNullOrEmpty(CurAlarmRecord.PatientName))
                        {
                            strSpeenVal += "病人：" + CurAlarmRecord.PatientName + ",";
                        }

                        strSpeenVal += CurAlarmRecord.ParaName + "当前值：" + CurAlarmRecord.Val;
                        if (AlarmType.Equals("超下限告警"))
                        {
                            strSpeenVal += "低于" + CurAlarmRecord.Low + "告警";
                        }
                        else
                        {
                            strSpeenVal += "高于" + CurAlarmRecord.High + "告警";
                        }
                        // 记录历史记录
                        // 告警字符串入队列
                        if (para.isEnabled == 0)
                        {
                            queue.Enqueue(strSpeenVal); 
                        }
                        // vo.Speak(strSpeenVal, SSF);//告警播报
                        // if (addAlarmRecord != null && para.isRecord == 0)//记录是否保存
                        // {
                        //     addAlarmRecord(CurAlarmRecord);
                        // }
                        if (addAlarmRecord != null)
                        {
                            addAlarmRecord(CurAlarmRecord);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.d("ParaItem_onAlarm:" + e);
            }
        }

        //------------------------------------------------------------------------------
        // add_by_limu_160607
        public void ClearView()
        {
            //VMain.c
            //this.DisConnected();

            //ViewP1.Visible = false;
            //ViewP2.Visible = false;
            //ViewW.Visible  = false;
        }

        //------------------------------------------------------------------------------
        public void InitMGOA()
        {
            ////更改DemoWave中art的data值
            //byte[] buff = new byte[308] {
            //    90,77,87,124,204,208,132,147,130,128,128,200,128,128,210,128,210,128,209,128,128,208,128,208,128,208,128,128,208,128,208,128,208,128,207,128,128,207,128,207,128,207,128,128,206,128,206,128,206,128,206,128,128,206,128,205,128,205,128,128,205,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,204,128,204,128,204,128,128,204,128,204,128,204,128,128,204,128,206,128,208,128,212,128,128,217,128,222,128,227,128,128,231,128,234,128,237,128,240,128,128,242,128,243,128,245,128,128,246,128,247,128,248,128,248,128,128,249,128,249,128,249,128,128,248,128,248,128,247,128,246,128,128,244,128,243,128,241,128,128,239,128,238,128,236,128,235,128,128,234,128,233,128,233,128,128,233,128,233,128,234,128,235,128,128,235,128,236,128,236,128,128,235,128,235,128,234,128,233,128,128,232,128,231,128,229,128,128,227,128,225,128,224,128,222,128,128,220,128,219,128,217,128,128,216,128,215,128,214,128,213,128,128,212,128,212,128,211,128,128,211,128,210,128,210,128,128,128
            //};
            //DBConnect.SYS.BeginTransaction();
            //DBConnect.SYS.AddPara("Data", buff);
            //DBConnect.SYS.ExecSQL("update DemoWave set Data=@Data where ID=4");
            //DBConnect.SYS.Commit();

            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Module Where Enable=1 And ID>=10 Order By ID");
            if (dr.Count <= 0) return;
            while (dr.Read())
                MList.Add(new MGOA(null, dr, ViewID, mALarmPara));
            BindGroups();
            InitDemo();
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
                    //绑定参数组
                    if (G is ParaGroup)
                    {
                        Para_List.Add((ParaGroup) G);
                        continue;
                    }

                    //绑定波形组
                    if (G is WaveGroup)
                    {
                        Wave_List.Add((WaveGroup) G);
                        continue;
                    }
                }
            }
        }

        //------------------------------------------------------------------------------
        public void InitDemo()
        {
            //参数组列表
            for (int i = 0; i < Para_List.Count; i++)
                DemoP.Add(new DemoPara(Para_List[i]));
            //波形组列表
            for (int i = 0; i < Wave_List.Count; i++)
                DemoW.Add(new DemoWave(Wave_List[i]));
        }

        //------------------------------------------------------------------------------
        public void LayoutView()
        {
            VMain.Left = 1;
            VMain.Width = this.Width - 2;
            VMain.Top = VTitle.Bottom;
            VMain.Height = this.Height - VMain.Top - 2;

            int W = VMain.Width - 10;
            int X = W;
            //--------
            ViewP1.Top = 0;
            ViewP1.Height = VMain.Height;
            ViewP1.Width = W * 40 / 100;
            X -= ViewP1.Width;
            ViewP1.Left = X; // VMain.Width - ViewP1.Width;
            //--------
            ViewP2.Top = 0;
            ViewP2.Height = VMain.Height;
            ViewP2.Width = W * 20 / 100;
            X -= ViewP2.Width;
            ViewP2.Left = X; //VMain.Width - ViewP1.Width;
            //--------
            ViewW.Top = 0;
            ViewW.Height = VMain.Height;
            ViewW.Width = VMain.Width - ViewP1.Width - ViewP2.Width; //5
            ViewW.Left = 0;
            //
            ViewP1.LayoutView();
            ViewP2.LayoutView();
            ViewW.LayoutView();
            //
        }

        //------------------------------------------------------------------------------
        public void SetCommand(GLB.SEND_COMMAND SendCommand)
        {
            this.SendCommand = SendCommand;
            //VTitle.SetCommand(SendCommand);
        }

        //------------------------------------------------------------------------------
        public void ParaTick(bool isRefresh = true)
        {
            //if (!IsConnected) return;
            //更新标题显示
            if (VTitle.TitleIsChange() && isRefresh)
            {
                VTitle.Refresh();
            }

            //刷新参数显示
            for (int i = 0; i < Para_List.Count; i++)
            {
                if (Alarm.isAlarm) break;
                if (Para_List[i] == null || Para_List[i].PItems == null) continue;
                for (int j = 0; j < Para_List[i].PItems.Length; j++)
                {
                    if (Para_List[i].PItems[j].alaMode != Alarm.Mode.Normal)
                        Alarm.isAlarm = true;
                }
            }

            ViewP1.Tick();
            ViewP2.Tick();
        }

        //------------------------------------------------------------------------------
        public void WaveTick()
        {
            if (ServerBase != null && ((DateTime.Now - ServerBase.LastDataTicks).Seconds > 60))
            {
                Log.d($"{VTitle.BedSN}=>接收数据超时，清除历史数据");
                for (int i = 0; i < vwList.Count; i++)
                {
                    vwList[i].Reset();
                    vwList[i].Clean();
                }
                ClearParaGroup();
                ServerBase.LastDataTicks = DateTime.Now; // 这里主要是为了抑制上面的日志
            }
            //if (!ViewW.Visible) return;
            //准备缓冲数据
            PushData();
            //各波形作画(如果可见)
            Draw();
            //this.BeginInvoke(new BeginInvokeDelegate(BeginInvokeMethod));
        }

        //------------------------------------------------------------------------------
        /*private void BeginInvokeMethod()
        {
            Draw();
        }*/
        //------------------------------------------------------------------------------
        public void DemoTick()
        {
            for (int i = 0; i < DemoP.Count; i++)
                DemoP[i].Tick();
            for (int i = 0; i < DemoW.Count; i++)
                DemoW[i].Tick();
        }

        //------------------------------------------------------------------------------
        public void PushData()
        {
            //if (this.Top < 300 || this.Left < 300) return;
            for (int i = 0; i < vwList.Count; i++)
            {
                ViewWaveGroup v = vwList[i];
                //if (GLB.Same(v.waveGroup.FullSN, "ECG.ECG")) continue;
                //if (GLB.Same(v.waveGroup.FullSN, "RESP.RESP")) continue;
                //if (GLB.Same(v.waveGroup.FullSN, "Spo2.Pleth")) continue;
                v.PushData();
                //Log.d("PushData : " + v.waveGroup.FullSN + ", vwList.Count=" + vwList.Count);
            }
        }

        //------------------------------------------------------------------------------
        public Boolean CheckVisible()
        {
            Boolean visible = GLB.Demo || IsConnected;
            //Log.d("CheckVisible 1:" + ViewIP + ":" + ViewID.ToString() + ",IsConnected=" + IsConnected.ToString());
            if (IsVisible == visible)
                return IsVisible;
            //Log.d("CheckVisible 2:" + ViewIP);
            //初始化显示
            if (visible)
            {
                for (int i = 0; i < vwList.Count; i++)
                    vwList[i].Reset();
            }

            //Log.d("CheckVisible 3:" + ViewIP);
            IsVisible = visible;
            ViewP1.Visible = IsVisible;
            ViewP2.Visible = IsVisible;
            ViewW.Visible = IsVisible;
            //Log.d("CheckVisible 4:" + ViewIP);
            return IsVisible;
        }

        //------------------------------------------------------------------------------
        public void Draw()
        {
            if (!CheckVisible()) return;
            //
            for (int i = 0; i < vwList.Count; i++)
            {
                ViewWaveGroup v = vwList[i];
                v.Draw();
            }
        }

        //------------------------------------------------------------------------------
        /*private void Title_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString((ViewID + 1).ToString() + ":床 ", GLB.SysFont, GLB.SysBrush, 0, 0);
        }*/
        //-----------------------------------------------------------------------------------------
        public void CreateHistory(int devId)
        {
            history = new History(devId, Para_List, Wave_List);
        }
        //-----------------------------------------------------------------------------------------
        public void CloseHistory()
        {
            history = null;
        }

        //-----------------------------------------------------------------------------------------
        public void Connected()
        {
            //Log.d("终端" + ViewID.ToString() + " 连接成功...");
            IsConnected = true;
        }

        //-----------------------------------------------------------------------------------------
        public void DisConnected(Socket source)
        {
            // Log.d("终端" + ViewID.ToString() + " 断开连接...");
            IsConnected = false;
            ClearParaGroup();
        }

        //-----------------------------------------------------------------------------------------
        public void SetParaGroup(String SN, short[] Values)
        {
            //Log.d("SetParaGroup:" + ObjID.ToString() + "=" + Values[0].ToString());
            for (int i = 0; i < Para_List.Count; i++)
            {
                if (!GLB.Same(Para_List[i].SN, SN)) continue;
                //if (!GLB.Same(Para_List[i].FullSN, "NIBP.NIBP")) continue;
                Para_List[i].SetValue(Values);
                history.SavePara(Para_List[i].FullSN, Para_List[i].ValBuf);
            }
        }

        //-----------------------------------------------------------------------------------------
        public void ClearParaGroup()
        {
            foreach(var p in Para_List)
            {
                p.SetValue(new short[] {short.MinValue, short.MinValue, short.MinValue, short.MinValue, short.MinValue, short.MinValue });
            }
        }


        //-----------------------------------------------------------------------------------------
        public void SetWaveGroup(String SN, float[] Data, int sPos, int Len)
        {
            //Log.d("1 SetWaveGroup " + SN);
            
            for (int i = 0; i < Wave_List.Count; i++)
            {
                //Log.d("2 SetWaveGroup " + SN + ", FullSN=" + Wave_List[i].FullSN);
                if (!GLB.Same(Wave_List[i].FullSN, SN)) continue;
                Wave_List[i].SetData_ACQ(Data, sPos, Len);
                history.SaveWave(Wave_List[i].FullSN, Data);
                break;
            }
        }

        //-----------------------------------------------------------------------------------------
        public void AlarmSetting(Boolean Syn, String Path, String SN, Double Low, Double High)
        {
            for (int i = 0; i < Para_List.Count; i++)
            {
                if (!GLB.Same(Para_List[i].FullSN, Path)) continue;
                ParaItem PItem = (ParaItem) Para_List[i].FindChild(SN);
                if (PItem == null) return;
                //设置运行期报警值
                PItem.AlarmSetting(Low, High);
                //发送设置到床边机

                if (Syn && SendCommand != null)
                {
                    SendCommand(PItem.FullSN + ".Low", PItem.FloatValueToInt(Low));
                    SendCommand(PItem.FullSN + ".High", PItem.FloatValueToInt(High));
                }

                return;
            }
        }

        //-----------------------------------------------------------------------------------------
        public void SetIP(String IP)
        {
            ViewIP = IP;
            VTitle.ViewIP = IP;
        }

        //-----------------------------------------------------------------------------------------
        public void SetBedSN(String BedName)
        {
            VTitle.BedName = BedName;
        }

        //-----------------------------------------------------------------------------------------
        public void SetServerBase(PrtServerBase ServerBase)
        {
            if (this.ServerBase == ServerBase) return;
            this.ServerBase = ServerBase;
            ServerBase.onConnected += this.Connected;
            ServerBase.OnDisConnected += this.DisConnected;
            ServerBase.onGetParaGroup += this.SetParaGroup;
            ServerBase.onGetWaveGroup += this.SetWaveGroup;
        }
        //-----------------------------------------------------------------------------------------
        public void UnsetServerBase()
        {
            if (this.ServerBase != null)
            {
                this.ServerBase.onConnected -= this.Connected;
                this.ServerBase.OnDisConnected -= this.DisConnected;
                this.ServerBase.onGetParaGroup -= this.SetParaGroup;
                this.ServerBase.onGetWaveGroup -= this.SetWaveGroup;
            }
        }
    }
    //-----------------------------------------------------------------------
}
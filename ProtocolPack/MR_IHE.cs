// Copyright (c) YiCare Corporation. All rights reserved.
// 文件：MR_IHE.cs
// 作者：SUSHI
// 日期：10/21/2023
// 说明：

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GlobalClass;
using MGOA_Pack;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace ProtocolPack
{
    /// <summary>
    /// IHE协议支持波形枚举
    /// </summary>
    internal enum KindOfWave
    {
        MDC_CONC_AWAY_CO2 = 0,
        MDC_CONC_AWAY_02,
        MDC_CONC_AWAY_N2O,
        MDC_CONC_AWAY_AGENT,
        MDC_CONC_AWAY_DESFL,
        MDC_CONC_AWAY_ENFL,
        MDC_CONC_AWAY_HALOTH,
        MDC_CONC_AWAY_SEVOFL,
        MDC_CONC_AWAY_ISOFL,
        MDC_PRESS_AWAY,
        MDC_FLOW_AWAY,
        MDC_VOL_AWAY,
        MDC_ECG_ELEC_POTL_I,
        MDC_ECG_ELEC_POTL_II,
        MDC_ECG_ELEC_POTL_III,
        MDC_ECG_ELEC_POTL_AVL,
        MDC_ECG_ELEC_POTL_AVR,
        MDC_ECG_ELEC_POTL_AVF,
        MDC_ECG_ELEC_POTL_V,
        MDC_ECG_ELEC_POTL_V1,
        MDC_ECG_ELEC_POTL_V2,
        MDC_ECG_ELEC_POTL_V3,
        MDC_ECG_ELEC_POTL_V4,
        MDC_ECG_ELEC_POTL_V5,
        MDC_ECG_ELEC_POTL_V6,
        MDC_IMPED_TTHOR,
        MDC_PRESS_BLD,
        MDC_PRESS_BLD_ART,
        MDC_PRESS_BLD_ART_UMB,
        MDC_PRESS_BLD_VENT_LEFT,
        MDC_PRESS_BLD_ART_PULM,
        MDC_PRESS_BLD_VEN_CENT,
        MDC_PRESS_INTRA_CRAN,
        MDC_PRESS_BLD_ATR_LEFT,
        MDC_PRESS_BLD_ATR_RIGHT,
        MDC_PRESS_BLD_AORT,
        MDC_PRESS_BLD_ART_BRACHIAL,
        MDC_PRESS_BLD_ART_FEMORAL,
        MDC_PRESS_BLD_VEN_UMB,
        MDC_EEG_ELEC_POTL_CRTX,
        MDC_PULS_OXIM_PLETH,
        MDC_SPO2_SIGNAL_QUALITY_INDEX,
        MNDRY_EEG_ELEC_POTL_BIS_EYE,
    }

    /// <summary>
    /// 波形数据结构
    /// </summary>
    internal class IHE_WAVE_DATA
    {
        public KindOfWave ChannelId;
        public ushort SimplateRate;
        public float[] DataBuffer;
    }

    public class MR_IHE :　PrtServerBase
    {
        private Dictionary<string, string> KeyGroupNames;

        /// <summary>
        /// 初始化度量值
        /// </summary>
        private void InitMeasureKeyGroupNames()
        {
            // KeyGroupName = new List<KeyValuePair<string, string>>();
            KeyGroupNames = new Dictionary<string, string>();

            //KeyGroupName.Add("149546^MDC_PULS_RATE_NON_INV", "脉率");
            KeyGroupNames.Add("147842^MDC_ECG_HEART_RATE^MDC", "HR"); //心电心率
            KeyGroupNames.Add("148066^MDC_ECG_V_P_C_RATE^MDC", "PVCs"); //PVC
            KeyGroupNames.Add("131841^MDC_ECG_AMPL_ST_I^MDC", "ST-I"); //PVC
            KeyGroupNames.Add("131842^MDC_ECG_AMPL_ST_II^MDC", "ST-II"); //PVC
            KeyGroupNames.Add("131901^MDC_ECG_AMPL_ST_III^MDC", "ST-III"); //PVC
            KeyGroupNames.Add("131902^MDC_ECG_AMPL_ST_AVR^MDC", "ST-aVR"); //PVC
            KeyGroupNames.Add("151562^MDC_RESP_RATE^MDC", "RR"); //呼吸频率
            KeyGroupNames.Add("150456^MDC_PULS_OXIM_SAT_O2^MDC", "SpO2"); //血氧
            KeyGroupNames.Add("149530^MDC_PULS_OXIM_PULS_RATE^MDC", "PR"); //脉搏
            KeyGroupNames.Add("161^PI", "PI"); //血流灌注指数、灌流指标
            // KeyGroupName.Add("150021^MDC_PRESS_BLD_NONINV_SYS^MDC", "Sys"); //无创高血压（收缩压）SBP
            // KeyGroupName.Add("150022^MDC_PRESS_BLD_NONINV_DIA^MDC", "Dia"); //无创低血压（舒张压）
            // KeyGroupName.Add("150023^MDC_PRESS_BLD_NONINV_MEAN^MDC", "Mean"); //无创平均压
            // KeyGroupName.Add("149546^MDC_PULS_RATE_NON_INV^MDC", "NIBP_PR"); //无创血压 脉率
            KeyGroupNames.Add("150301^MDC_PRESS_CUFF_SYS^MDC", "Sys"); // 无创血压（收缩压）
            KeyGroupNames.Add("150302^MDC_PRESS_CUFF_DIA^MDC", "Dia"); // 无创血压（舒张压）
            KeyGroupNames.Add("150303^MDC_PRESS_CUFF_MEAN^MDC", "Mean"); // 无创血压（平均压）
            KeyGroupNames.Add("149546^MDC_PULS_RATE_NON_INV^MDC", "NIBP_PR"); //无创血压 脉率
            KeyGroupNames.Add("150344^MDC_TEMP^MDC", "T1"); //第一通道温度T1
            // KeyGroupName.Add("201^T2", "T2"); //第一通道温度T2
            // KeyGroupName.Add("202^TD", "TD"); //两个通道的温度差TD
            KeyGroupNames.Add("151928^MDC_VENT_AWAY_CO2_ET^MDC", "ETCO2"); //EtCO2
            KeyGroupNames.Add("151936^MDC_VENT_AWAY_CO2_INSP^MDC", "FIO2"); //FiO2
            //KeyGroupName.Add("500^Sys", "动脉收缩压");//动脉高血压（收缩压）
            KeyGroupNames.Add("150037^MDC_PRESS_BLD_ART_ABP_SYS^MDC", "ART_Sys"); //动脉高血压（收缩压）
            //KeyGroupName.Add("501^Mean", "动脉平均压");//无创平均压
            KeyGroupNames.Add("150039^MDC_PRESS_BLD_ART_ABP_MEAN^MDC", "ART_Mean"); //无创平均压
            //KeyGroupName.Add("502^Dia", "动脉舒张压");//动脉低血压（舒张压）
            KeyGroupNames.Add("150038^MDC_PRESS_BLD_ART_ABP_DIA^MDC", "ART_Dia"); //动脉低血压（舒张压）
            KeyGroupNames.Add("150087^MDC_PRESS_BLD_VEN_CENT_MEAN^MDC", "IBP_Mean"); //中心静脉压
        }

        /// <summary>
        /// 无创血压的参数，由于当前的ViewParaGroup对象无法对单个血压参数进行绘图设置
        /// 所以这里定义一个静态的无创血压参数数组，数组的大小固定为3，第一个元素表示
        /// 高压，第二参数表示低压，第三个参数表示平均压
        /// </summary>
        internal short[] NIBP_Para = new short[3] { 0, 0, 0 };
        internal short[] ART_Para = new short[3] { 0, 0, 0 };

        /// <summary>
        /// 监护仪设备地址
        /// </summary>
        public string DeviceIP { get; private set; }
        /// <summary>
        /// 监护仪设备端口
        /// </summary>
        public int DevicePort { get; private set; }
        /// <summary>
        /// 是否级联模式
        /// </summary>
        public bool CascadeMode { get; private set; }
        /// <summary>
        /// 通信SOCKET对象
        /// </summary>
        public override Socket TCP { get; protected set; } = null;

        /// <summary>
        /// 接收缓存区，默认10K大小
        /// </summary>
        private CircularBuffer ReceivedBuffer = new CircularBuffer(10 * 1024);
        private const int FrameSize = 5 * 1024;
        private byte[] FrameBuffer = new byte[FrameSize];

        private string CascadeAddr;
        private int CascadePort;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="cascadeMode">级联模式</param>
        /// <param name="cascadeAddr"></param>
        /// <param name="cascadePort"></param>
        public MR_IHE(string ip, int port, bool cascadeMode = true, string cascadeAddr = "10.10.252.108", int cascadePort = 10023)　: base(0)
        {
            IP4 = ip;
            DeviceIP = ip;
            DevicePort = port;
            CascadeMode = cascadeMode;
            CascadeAddr = cascadeAddr;
            CascadePort = cascadePort;
            InitMeasureKeyGroupNames();
            Connect();
        }

        /// <summary>
        /// 连接监护仪
        /// </summary>
        public void Connect()
        {
            State = ConnectState.OffLine;
            Thread.Sleep(60);
            string remoteIp = CascadeMode ? CascadeAddr : DeviceIP;
            int remotePort = CascadeMode ? CascadePort : DevicePort;
            if (CascadeMode)
            {
                Log.d($"MR_IHE连接级联平台：{remoteIp}:{remotePort}");
            }
            else
            {
                Log.d($"MR_IHE连接监护仪：{remoteIp}:{remotePort}");
            }

            IPAddress address = IPAddress.Parse(remoteIp);
            if (TCP != null)
            {
                TCP.Close();
            }
            TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 设置接收超时时间
            TCP.ReceiveTimeout = 10000;
            TCP.LingerState = new LingerOption(true, 0);
            TCP.BeginConnect(address, remotePort, new AsyncCallback(this.Connected), null);
        }

        /// <summary>
        /// 连接建立完成
        /// </summary>
        /// <param name="ar"></param>
        public void Connected(IAsyncResult ar)
        {
            try
            {
                TCP.EndConnect(ar);
                if (this.onConnected != null) this.onConnected();
                if (CascadeMode)
                {
                    Log.d($"连接上级平台{TCP.RemoteEndPoint.ToString()}成功");
                }
                else
                {
                    Log.d($"连接监护仪{this.DeviceIP}成功");
                }
                State = ConnectState.Connect;

                // 连接建立完毕，创建数据处理线程，开始处理数据
                StateObject state = new StateObject();
                state.Worker = this;
                TCP.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceivedCallback), state);
            }
            catch (SocketException sex)
            {
                Log.d(string.Format("套接字错误：code: {0}, message: {1}", sex.ErrorCode, sex.Message));
                State = ConnectState.OffLine;
                Connect();
            }
            catch (Exception ex)
            {
                Log.d("连接失败：" + ex.Message);
                Log.d("重新连接");
                State = ConnectState.OffLine;
                Connect();
            }
        }

        /// <summary>
        /// 数据接收到的处理
        /// </summary>
        /// <param name="ar"></param>
        public static void ReceivedCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            MR_IHE worker = state.Worker as MR_IHE;

            try
            {
                if (worker == null || worker.TCP == null) return;

                int bytesReceived = worker.TCP.EndReceive(ar);
                if (bytesReceived > 0)
                {
                    worker.Process(state.Buffer, bytesReceived);
                    worker.TCP.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceivedCallback), state);

                }
                else
                {
                    Log.d(string.Format("IHE远端关闭了数据连接"));
                    worker.State = ConnectState.OffLine;
                    worker.Connect();
                }
            }
            catch(ObjectDisposedException)
            {
                Log.d(string.Format("连接已关闭"));
                worker.State = ConnectState.OffLine;
                worker.Connect();
            }
            catch(SocketException sex)
            {
                Log.d(string.Format("套接字错误：code: {0}, message: {1}", sex.ErrorCode, sex.Message));
                worker.State = ConnectState.OffLine;
                worker.Connect();
            }
            catch(Exception ex)
            {
                Log.d(string.Format("未知错误：{0}", ex.Message));
                Log.d(string.Format("错误堆栈：{0}", ex.StackTrace));
                Log.d(string.Format("当前报文明文：{0}", Encoding.GetEncoding("GB2312").GetString(state.Buffer)));
                worker.State = ConnectState.OffLine;
                worker.Connect();
            }
        }

        /// <summary>
        /// 处理协议数据包
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private void Process(byte[] buffer, int count)
        {
            byte BP = (byte)0x0B;
            byte EP = (byte)0x1C;
            byte EF = (byte)0x0D;

            int iStart = 0;
            int iEnd = 0;

            //将接受到的数据放入缓存区
            this.ReceivedBuffer.WriteBuffer(buffer, 0, count);
            //Array.Copy(buffer, 0, this.ReceivedBuffer, 0, count);
            //count = this.ReceivedBuffer.ReadBuffer(this.FrameBuffer, 0, MR_IHE.FrameSize);
            // 提取数据包
            for(int i = 0; i < this.ReceivedBuffer.DataCount; i++)
            {
                if (this.ReceivedBuffer[i] == BP)
                {
                    iStart = i + 1;
                    continue;
                }

                if (this.ReceivedBuffer[i] == EF)
                {
                    if (i == 0) continue;
                    if (this.ReceivedBuffer[i -1] == EP)
                    {
                        iEnd = i;
                        int iSize = (iEnd - 2) - iStart;
                        //byte[] package = new byte[iSize];
                        var package = new byte[iEnd];
                        this.ReceivedBuffer.ReadBuffer(package, 0, iEnd);
                        //Array.Copy(this.FrameBuffer, iStart, package, 0, iSizek);
                        string message = Encoding.GetEncoding("GB2312").GetString(package, iStart, iSize);
                        string[] segments = message.Split('\r');    // 分割HL7的段
                        if (segments.Length <= 0)
                        {
                            Log.d($"消息内容[{message}] => 无法解析成HL7消息");
                            continue;
                        }
                        string[] msh = segments[0].Trim().Split('|');
                        if (!msh[0].Trim().ToUpper().Equals("MSH"))
                        {
                            // 数据包异常则丢弃
                            Log.d($"[{segments[0]}] => 不是合法的MSH节");
                            continue;
                        }
                        if (!"ORU^R01^ORU_R01".Equals(msh[8].Trim()))
                        {
                            // 如果不是监护消息，跳出循环
                            Log.d($"[{segments[0]}] => 不是合法的监护数据包");
                            continue;
                        }
                        if (!this.DeviceIP.Trim().Equals(msh[3].Trim())) continue;  // 与设备IP不匹配的数据包直接丢弃
                        Process(segments);
                    }

                }
            }

            // 清除处理过的数据
            this.ReceivedBuffer.Clear(iEnd);

        }

        private void Process(string[] segments)
        {
            foreach(var segment in segments)
            {
                var fields = segment.Trim().Split('|');
                if ("OBR".Equals(fields[0]) && "CONTINUOUS WAVEFORM".Equals(fields[4]))
                {
                    // 处理波形数据
                    //Log.d($"=>处理[{DeviceIP}]的波形数据");
                    IEnumerable<IHE_WAVE_DATA> waves = ParseWaveData(segments);
                    DispatchWaveEvent(waves);
                    
                }
                if ("OBR".Equals(fields[0]) && "182777000^monitoring of patient^SCT".Equals(fields[4]))
                {
                    // 监护参数处理
                    //Log.d($"=>处理[{DeviceIP}]的监护数据");
                    DispatchMeasureData(segments);
                }
            }
            LastDataTicks = DateTime.Now;
        }

        private static IEnumerable<IHE_WAVE_DATA> ParseWaveData(IReadOnlyList<string> fields)
        {
            for(int i = 0; i < fields.Count; i++)
            {
                var item = fields[i].Trim().Split('|');
                if ("OBX".Equals(item[0]) && "NA".Equals(item[2]))
                {
                    var waveId = item[3].Split('^')[1];
                    var wave = new IHE_WAVE_DATA();
                    if (ushort.TryParse(fields[i + 1].Trim().Split('|')[5], out var rate))
                    {
                        wave.SimplateRate = rate;
                    }

                    wave.DataBuffer = item[5].Split('^').Select(float.Parse).ToArray();
                    wave.ChannelId = (KindOfWave)Enum.Parse(typeof(KindOfWave), waveId);
                    yield return wave;
                }
            }
        }

        private void DispatchWaveEvent(IEnumerable<IHE_WAVE_DATA> waves)
        {
            foreach (var wave in waves)
            {
                //if (wave.ChannelId == KindOfWave.MDC_ECG_ELEC_POTL_II || wave.ChannelId == KindOfWave.MDC_ECG_ELEC_POTL_I)
                if (wave.ChannelId == KindOfWave.MDC_ECG_ELEC_POTL_I)
                {
                    //Log.d($"====>{DeviceIP}的心电波，数据长度{wave.DataBuffer.Length}");
                    DispatchECG(wave);
                }

                if (wave.ChannelId == KindOfWave.MDC_PULS_OXIM_PLETH)
                {
                    //Log.d($"====>{DeviceIP}的脉搏波，数据长度{wave.DataBuffer.Length}");
                    DispatchPleth(wave);
                }
            }
        }

        private void DispatchECG(IHE_WAVE_DATA wave)
        {
            // 值域映射，绘图控件的值域为[0,255]，将波形数据映射到这个值域空间
            //var test = wave.DataBuffer.Select(x => x - 2046).ToList();
            float max = wave.DataBuffer.Max();
            float min = wave.DataBuffer.Min();
            float dep = Math.Max(Math.Abs(max), Math.Abs(min));

            Func<float, float> Fixed = (x) =>
            {
                if (max == min) return 0;
                float z = (x / dep) * 127;
                if (z < 0)
                {
                    z += 255;
                }
                return z;
            };

            //float[] normalized = wave.DataBuffer.Select(x =>  (x / dep)).ToArray();
            //float[] normalized = wave.DataBuffer.Select(Fixed).ToArray();
            float[] normalized = wave.DataBuffer.Select(Fixed).ToArray();
            //Log.d($"====>心电数据归一后长度：{normalized.Length}");
            onGetWaveGroup("ECG.ECG", normalized, 0, normalized.Length);

        }
    
        private void DispatchPleth(IHE_WAVE_DATA wave)
        {
            float max = wave.DataBuffer.Max();
            float min = wave.DataBuffer.Min();
            float dep = Math.Max(Math.Abs(max), Math.Abs(min));

            Func<float, float> Fixed = (x) =>
            {
                if (max == min) return 0;
                float z = (x / dep) * 127;
                if (z < 0)
                {
                    z += 255;
                }
                return z;
            };

            float[] normalized = wave.DataBuffer.Select(Fixed).ToArray();
            //Log.d($"====>脉搏数据归一后长度：{normalized.Length}");
            onGetWaveGroup("SPO2.Pleth", normalized, 0, normalized.Length);
        }
        
        private void DispatchMeasureData(string[] segments)
        {
            for(int i = 0; i < segments.Length; i++)
            {
                try
                {
                    string[] item = segments[i].Trim().Split('|');
                    if ("OBX".Equals(item[0]) && "NM".Equals(item[2]))
                    {
                        short result = (short)Convert.ToDecimal(item[5]);
                        if (KeyGroupNames.TryGetValue(item[3], out var ob))
                        {
                            if (ob.ToUpper().Equals("SYS"))
                            {
                                this.NIBP_Para[0] = result;
                                onGetParaGroup("NIBP", this.NIBP_Para);
                            }
                            else if (ob.ToUpper().Equals("DIA"))
                            {
                                this.NIBP_Para[1] = result;
                                onGetParaGroup("NIBP", this.NIBP_Para);
                            }
                            else if (ob.ToUpper().Equals("MEAN"))
                            {
                                this.NIBP_Para[2] = result;
                                onGetParaGroup("NIBP", this.NIBP_Para);
                            }
                            else if ((ob.ToUpper().Equals("ART_SYS")))
                            {
                                this.ART_Para[0] = result;
                                onGetParaGroup("ABP", this.ART_Para);
                            }
                            else if ((ob.ToUpper().Equals("ART_MEAN")))
                            {
                                this.ART_Para[2] = result;
                                onGetParaGroup("ABP", this.ART_Para);
                            }
                            else if ((ob.ToUpper().Equals("ART_DIA")))
                            {
                                this.ART_Para[1] = result;
                                onGetParaGroup("ABP", this.ART_Para);
                            }
                            else
                            {
                                onGetParaGroup(ob, new short[1] { result });
                            }
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
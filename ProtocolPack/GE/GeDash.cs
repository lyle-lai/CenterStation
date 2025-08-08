using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GlobalClass;

namespace ProtocolPack.GE
{
    public class GeDash:PrtServerBase
    {
        private static UdpClient Udp = null;
        private static Thread ThrUdp; //接收线程
        private static List<GeDash> ALL = new List<GeDash>();
        private readonly byte[] _generateVitalSignRequest;

        private static IPEndPoint
            remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 2000); //Any IP any port from remote point
        
        private Dictionary<string, string> KeyGroupNames;
        internal short[] NIBP_Para = new short[3] { 0, 0, 0 };
        internal short[] ART_Para = new short[3] { 0, 0, 0 };
        
        private const int SensorEntryLength = 70;

        private const int SensorIdentifierOffset = 1;

        private const int FirstValueOffset = 6;
        private const int ValueLength = 2;

        private void InitMeasureKeyGroupNames()
        {
            KeyGroupNames = new Dictionary<string, string>();
            KeyGroupNames.Add("Temperature","T1");
            KeyGroupNames.Add("HeartRate","HR");
            KeyGroupNames.Add("SpO2", "SpO2"); //血氧
            KeyGroupNames.Add("SHeartRate", "PR"); //脉搏
            KeyGroupNames.Add("SystolicBloodPressure", "Sys"); // 无创血压（收缩压）
            KeyGroupNames.Add("DiastolicBloodPressure", "Dia"); // 无创血压（舒张压）
            KeyGroupNames.Add("MeanArterialPressure", "Mean"); 
            KeyGroupNames.Add("ArtSystolicBloodPressure", "ART_Sys"); //动脉高血压（收缩压） 
            KeyGroupNames.Add("ArtMeanArterialPressure", "ART_Mean"); //无创平均压
            KeyGroupNames.Add("ArtDiastolicBloodPressure", "ART_Dia");
        }
        public GeDash(string addr) : base(0)
        {
            VitalSignRequestData requestData = new VitalSignRequestData();
            _generateVitalSignRequest = requestData.GenerateVitalSignRequest();
            
            ALL.Add(this);
            remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(addr), 3073);
            IP4 = addr;
            
            Initialize();
            InitMeasureKeyGroupNames();
        }
        
        public void Initialize()
        {
            if (Udp == null)
            {
                Udp = new UdpClient(3073);
            }
            ThrUdp = new Thread(UdpRun);
            LastDataTicks = DateTime.Now;
            ThrUdp.Start();
        }
        
        private void UdpRun()
        {
            //接收数据
            while (true)
            {
                //接收数据
                if (Udp.Client.Available > 0)
                {
                    Log.d(DateTime.Now + " ---- 收到数据" + Udp.Available);
                    Byte[] RBuf = Udp.Receive(ref remoteIpEndPoint);
                    
                    GeDash Item = GetItemByEndPoint(remoteIpEndPoint);
                    Item.OnReceived(RBuf);
                    
                }
                
                //发送获取数据请求
                if (GLB.PassMs(LastDataTicks) >= 10000)
                {
                    for (int i = 0; i < ALL.Count; i++)
                        ALL[i].GetDataTick();
                }

                //----检查连接状态
                for (int i = 0; i < ALL.Count; i++)
                    ALL[i].CheckConnect();
                //
                Thread.Sleep(100);
            }
        }
        
        private void CheckConnect()
        {
            if (LastDataTicks == DateTime.MinValue || GLB.PassMs(LastDataTicks) >= 60 * 1000)
            {
                State = ConnectState.OffLine;
                if (OnDisConnected != null)
                {
                    OnDisConnected(null);
                }
                
            }
            else
            {
                State = ConnectState.Connect;
                onConnected();
            }
        }

        private void OnReceived(Byte[] buffer)
        {
            Log.d("IP=" + IP4 + "，接收到数据");
            LastDataTicks = DateTime.Now;
            var entryPointIndices =
                GetEntryStartIndices(buffer
                    .Length);
            foreach (var entryPointIndex in entryPointIndices)
            {
                var sectionBytes = buffer.Skip(entryPointIndex).Take(SensorEntryLength).ToArray();
                var sensorCodeByte = sectionBytes[SensorIdentifierOffset + 2];
                try
                {
                    ParseSensorSection(sectionBytes, sensorCodeByte);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Log.d($"sectionBytes:{sectionBytes};sensorCodeByte{sensorCodeByte}");
                }
            }
        }
        
        private void ParseSensorSection(byte[] sectionBytes, byte sensorCodeByte)
        {
            var sensorType = Informations.MapVitalSignSenorCodeToSensorType(sensorCodeByte);
            var vitalSigns = Informations.VitalSignTypesForSensor(sensorType);
            var valueCount = vitalSigns.Count;
            for (int valueIdx = 0; valueIdx < valueCount; valueIdx++)
            {
                var vitalSignType = vitalSigns[valueIdx];
                var valueBytes = sectionBytes.Skip(FirstValueOffset + ValueLength * valueIdx).Take(ValueLength);
                if (valueBytes.First() == 0x80)
                    continue;
                var value = ToShortValue(BitConverter.ToUInt16(valueBytes.Reverse().ToArray(), 0));
                if (value < 0)
                    continue;
                var paramName = vitalSignType.Equals(VitalSignType.UnKnow) ? sensorCodeByte + "" : vitalSignType.ToString();
                
                if (KeyGroupNames.TryGetValue(paramName, out var ob))
                {
                    if (ob.ToUpper().Equals("SYS"))
                    {
                        NIBP_Para[0] = value;
                        onGetParaGroup("NIBP", this.NIBP_Para);
                    }
                    else if (ob.ToUpper().Equals("DIA"))
                    {
                        NIBP_Para[1] = value;
                        onGetParaGroup("NIBP", this.NIBP_Para);
                    }
                    else if (ob.ToUpper().Equals("MEAN"))
                    {
                        NIBP_Para[2] = value;
                        onGetParaGroup("NIBP", this.NIBP_Para);
                    }
                    else if ((ob.ToUpper().Equals("ART_SYS")))
                    {
                        ART_Para[0] = value;
                        onGetParaGroup("ABP", this.ART_Para);
                    }
                    else if ((ob.ToUpper().Equals("ART_MEAN")))
                    {
                        ART_Para[2] = value;
                        onGetParaGroup("ABP", this.ART_Para);
                    }
                    else if ((ob.ToUpper().Equals("ART_DIA")))
                    {
                        ART_Para[1] = value;
                        onGetParaGroup("ABP", ART_Para);
                    }
                    else
                    {
                        onGetParaGroup(ob, new [] { value });
                    }
                }
                //未知参数时,使用字节表示参数,后续由现场确认后添加转换配置
                // yield return new ObservationData(
                //     vitalSignType.Equals(VitalSignType.UnKnow) ? sensorCodeByte + "" : vitalSignType.ToString(),
                //     value.ToString(), null, null, DateTime.Now);
            }
        }

        private short ToShortValue(ushort ushortValue)
        {
            if (ushortValue > ushort.MaxValue / 2)
                return (short)(ushortValue - ushort.MaxValue);
            return (short)ushortValue;
        }
        
        private static IEnumerable<int> GetEntryStartIndices(int bufferLength)
        {
            var idx = 124;
            while (idx + SensorEntryLength < bufferLength)
            {
                yield return idx;
                idx += SensorEntryLength;
            }
        }
        
        public void GetDataTick()
        {
            Udp.Send(_generateVitalSignRequest,
                    _generateVitalSignRequest.Length, IP4, 2000);
        }
        private GeDash GetItemByEndPoint(IPEndPoint RemoteIpEndPoint)
        {
            for (int i = 0; i < ALL.Count; i++)
                if (GLB.Same(ALL[i].IP4, RemoteIpEndPoint.Address.ToString()))
                    return ALL[i];
            return new GeDash(RemoteIpEndPoint.Address.MapToIPv4().ToString());
        }
    }
}
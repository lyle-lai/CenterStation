using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

using GlobalClass;
using ProtocolPack.GE;

namespace ProtocolPack
{
    /// <summary>
    /// 协议工厂
    /// </summary>
    public static class PrtFactory
    {
        internal static string CascadeAddr;
        internal static int CascadePort;
        internal static bool CascadeMode;
        internal static Dictionary<string, PrtServerBase> PrtServers = new Dictionary<string, PrtServerBase>();

        static PrtFactory()
        {
            CascadeMode = bool.Parse(Config.Get("SYS", "CascadeMode", "True"));
            CascadeAddr = Config.Get("SYS", "CascadeAddr", "10.10.252.108");
            CascadePort = int.Parse(Config.Get("SYS", "CascadePort", "10023"));
        }

        public static PrtServerBase Build(string deviceType, string ipAddr)
        {
            if ("mindray-ipm10".ToUpper().Equals(deviceType.ToUpper()))
            {
                Log.d($"Mindray_iPM10: {ipAddr}");
                if (PrtServers.TryGetValue(ipAddr, out PrtServerBase srv))
                {
                    return srv;
                }
                PrtServerBase newSrv = new MR_iPM(ipAddr, 4601);
                PrtServers.Add(ipAddr, newSrv);
                return newSrv;

            }

            if ("mindray-epm".ToUpper().Equals(deviceType.ToUpper()))
            {
                Log.d($"Mindray_ePM: {ipAddr}");
                if (PrtServers.TryGetValue(ipAddr, out PrtServerBase srv)) { return srv; }
                PrtServerBase newSrv = new MR_IHE(ipAddr, 3500, CascadeMode, CascadeAddr, CascadePort);
                PrtServers.Add(ipAddr, newSrv);
                return newSrv;
            }
            
            if ("Philips-IntelliVue".ToUpper().Equals(deviceType.ToUpper()))
            {
                Log.d($"PhilipUdp: {ipAddr}");
                if (PrtServers.TryGetValue(ipAddr, out PrtServerBase srv)) { return srv; }
                // PrtServerBase newSrv = new PrtPhilipsMP20( ipAddr);
                PrtServerBase newSrv = new PrtPhilipsMP20( ipAddr,CascadeMode,CascadeAddr, CascadePort);
                PrtServers.Add(ipAddr, newSrv);
                return newSrv;
            }
            
            if ("GE-Dash".ToUpper().Equals(deviceType.ToUpper()))
            {
                Log.d($"GE Udp: {ipAddr}");
                if (PrtServers.TryGetValue(ipAddr, out PrtServerBase srv)) { return srv; }
                PrtServerBase newSrv = new GeDash( ipAddr);
                PrtServers.Add(ipAddr, newSrv);
                return newSrv;
            }
            return null;
        }
    }
}

using System;

namespace RemoteMonitor
{
    public class LicenseInfo
    {
        public string Customer { get; set; }
        public string CPU { get; set; }
        public string Board { get; set; }
        public string Disk { get; set; }
        public DateTime Expiry { get; set; }
        public string Signature { get; set; }
    }
}
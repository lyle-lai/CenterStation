using System.Management;
using System.Text;

namespace RemoteMonitor
{
    public class HardwareHelper
    {
        public static string GetCpuId()
        {
            try
            {
                using (var mc = new ManagementClass("Win32_Processor"))
                {
                    foreach (ManagementObject mo in mc.GetInstances())
                    {
                        var id = mo["ProcessorId"]?.ToString();
                        if (!string.IsNullOrEmpty(id)) return id.Trim();
                    }
                }
            }
            catch
            {
            }

            return "";
        }

        public static string GetBoardSerial()
        {
            try
            {
                using (var mos = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                {
                    foreach (ManagementObject mo in mos.Get())
                    {
                        var s = mo["SerialNumber"]?.ToString();
                        if (!string.IsNullOrEmpty(s)) return s.Trim();
                    }
                }
            }
            catch
            {
            }

            return "";
        }

        public static string GetDiskSerial()
        {
            try
            {
                using (var mos = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_PhysicalMedia"))
                {
                    foreach (ManagementObject mo in mos.Get())
                    {
                        var s = mo["SerialNumber"]?.ToString();
                        if (!string.IsNullOrEmpty(s)) return s.Trim();
                    }
                }
            }
            catch
            {
            }

            try
            {
                using (var mos = new ManagementObjectSearcher(
                           "SELECT VolumeSerialNumber FROM Win32_LogicalDisk WHERE DeviceID='C:'"))
                {
                    foreach (ManagementObject mo in mos.Get())
                    {
                        var s = mo["VolumeSerialNumber"]?.ToString();
                        if (!string.IsNullOrEmpty(s)) return s.Trim();
                    }
                }
            }
            catch
            {
            }

            return "";
        }

        // 辅助：规范化字符串比较（去空格，ToUpper）
        public static string Norm(string s) => (s ?? "").Trim().ToUpperInvariant();
    }
}
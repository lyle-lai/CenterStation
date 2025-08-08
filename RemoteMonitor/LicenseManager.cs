using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace RemoteMonitor
{
    public class LicenseManager
    {
        private const string PublicKeyXml = @"<RSAKeyValue><Modulus>s6tuJM2VMNdYwK0HFu7g+4mqQOavotA/2/DTAPhaD9x+FvHwPiHGWSVY0qjQP2cTGNpUE4flj1+9Wg18JQRx7pzIA7GXAlz9eVr4njmtedBlrRjAtNJIun6+KOuk0uQbuyNsoSjXZTSUjMOdUEmdz+j3+vcsSY86Lfa3FQk0bKDLVMSKhb+hBrtdQrlmGW4vAJEt2y4jA7QS+seJ8CBHcO3tVGxV+6w1iPaUn6uX+/MKkSEnxhTcUZmz6YtBmnyo3VSk2E3ttsAs8SobteWNKGbogAXBXMfiSZW/8+nfFFhbNGrqkgz/HpHsBM9FgyfuOdQKBi6fZ6n07HtH0k1jgQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        // 与生成器一致的 AES key/iv（Base64->byte[]），生产请替换并混淆
        private static readonly byte[]
            AesKey = Convert.FromBase64String("w7Jm1F1q9Wqv7zVYQ3xN8s9h2qJ5kT6vZr9sV1x2a3Y=");

        private static readonly byte[] AesIv = Convert.FromBase64String("z1h2G3f4J5k6L7m8N9o0PQ==");

        /// <summary>
        /// 验证 license，返回 true/false，并通过 out 参数返回失败原因（便于排查）
        /// </summary>
        public static bool CheckLicense(out string failReason)
        {
            failReason = null;
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "license.dat");
                if (!File.Exists(path))
                {
                    failReason = "license file not found";
                    return false;
                }

                string base64 = File.ReadAllText(path, Encoding.UTF8);
                byte[] cipher = Convert.FromBase64String(base64);
                byte[] plain = AesDecrypt(cipher, AesKey, AesIv);
                string json = Encoding.UTF8.GetString(plain);

                var license = JsonConvert.DeserializeObject<LicenseInfo>(json);
                if (license == null)
                {
                    failReason = "invalid license format";
                    return false;
                }

                // 到期检查
                if (license.Expiry != DateTime.MaxValue && license.Expiry.Date < DateTime.Now.Date)
                {
                    failReason = "expired";
                    return false;
                }

                // 验签：rawData = Customer|CPU|Board|Disk|yyyy-MM-dd（或9999-12-31）
                string expStr = (license.Expiry == DateTime.MaxValue)
                    ? "9999-12-31"
                    : license.Expiry.ToString("yyyy-MM-dd");
                string rawData = $"{license.Customer}|{license.CPU}|{license.Board}|{license.Disk}|{expStr}";
                byte[] rawBytes = Encoding.UTF8.GetBytes(rawData);
                byte[] sig = Convert.FromBase64String(license.Signature);
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(PublicKeyXml);
                    bool ok = rsa.VerifyData(rawBytes, CryptoConfig.MapNameToOID("SHA256"), sig);
                    if (!ok)
                    {
                        failReason = "signature invalid";
                        return false;
                    }
                }

                // 获取本机三项
                string cpuLocal = HardwareHelper.Norm(HardwareHelper.GetCpuId());
                string boardLocal = HardwareHelper.Norm(HardwareHelper.GetBoardSerial());
                string diskLocal = HardwareHelper.Norm(HardwareHelper.GetDiskSerial());

                string cpuLic = HardwareHelper.Norm(license.CPU);
                string boardLic = HardwareHelper.Norm(license.Board);
                string diskLic = HardwareHelper.Norm(license.Disk);

                // 计算匹配数量（只统计 license 中非空的项目）
                int requiredNonEmpty = 0;
                int matchCount = 0;

                if (!string.IsNullOrEmpty(cpuLic))
                {
                    requiredNonEmpty++;
                    if (!string.IsNullOrEmpty(cpuLocal) && cpuLocal == cpuLic) matchCount++;
                }

                if (!string.IsNullOrEmpty(boardLic))
                {
                    requiredNonEmpty++;
                    if (!string.IsNullOrEmpty(boardLocal) && boardLocal == boardLic) matchCount++;
                }

                if (!string.IsNullOrEmpty(diskLic))
                {
                    requiredNonEmpty++;
                    if (!string.IsNullOrEmpty(diskLocal) && diskLocal == diskLic) matchCount++;
                }

                // 容错策略：
                // - 如果 license 中非空项 >= 2：只要 matchCount >= 2 即通过
                // - 如果 license 中非空项 < 2：要求所有非空项都必须匹配（即 matchCount == requiredNonEmpty）
                if (requiredNonEmpty >= 2)
                {
                    if (matchCount >= 2) return true;
                    failReason = $"hardware mismatch (matched {matchCount} of {requiredNonEmpty}, need >=2)";
                    return false;
                }
                else
                {
                    // 0 or 1 non-empty field
                    if (matchCount == requiredNonEmpty) return true; // 如果为0则通过（慎用）
                    failReason = $"hardware mismatch (matched {matchCount} of {requiredNonEmpty})";
                    return false;
                }
            }
            catch (Exception ex)
            {
                failReason = "exception: " + ex.Message;
                return false;
            }
        }

        private static byte[] AesDecrypt(byte[] cipher, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipher, 0, cipher.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
}
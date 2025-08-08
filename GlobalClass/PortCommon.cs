using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalClass
{
    public static class PortCommon
    {
        /// <summary>
        /// 字节数组转ASCII字符
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Ascii2Str(byte[] data)
        {
            return Encoding.ASCII.GetString(data);
        }
    }
}

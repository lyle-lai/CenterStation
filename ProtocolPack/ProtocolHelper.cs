using System;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;

namespace ProtocolPack
{
    public static class ProtocolHelper
    {
        public static byte[] Structure2Byte(object structure)
        {
            int size = Marshal.SizeOf(structure);
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                var bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// 字节数组转结构体（按小端模式）
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="obj"></param>
        /// <param name="startOffset"></param>
        public static void ByteArrayToStructure(byte[] byteArray, ref object obj, int startOffset)
        {
            int len = Marshal.SizeOf(obj);
            IntPtr ptr = Marshal.AllocHGlobal(len);
            // 从结构体指针构造结构体
            obj = Marshal.PtrToStructure(ptr, obj.GetType());
            try
            {
                Marshal.Copy(byteArray, startOffset, ptr, Math.Min(len, byteArray.Length));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            obj = Marshal.PtrToStructure(ptr, obj.GetType());
            Marshal.FreeHGlobal(ptr);
        }

        public static T Bytes2Structure<T>(byte[] bytes) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size < bytes.Length ? size : bytes.Length);
                return Marshal.PtrToStructure<T>(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// 将一条十六进制字符串转换为ASCII
        /// </summary>
        /// <param name="hexstring">一条十六进制字符串</param>
        /// <returns>返回一条ASCII码</returns>
        public static string HexStringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }


            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }

        /**/
        /// <summary>
        /// 16进制字符串转换为二进制数组
        /// </summary>
        /// <param name="hexstring">字符串每个字节之间都应该有空格，大多数的串口通讯资料上面的16进制都是字节之间都是用空格来分割的。</param>
        /// <returns>返回一个二进制字符串</returns>
        public static byte[] HexStringToBinary(string hexstring)
        {
            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }

            return buff;
        }


        /// <summary>
        /// 从十进制转换到十六进制
        /// </summary>
        /// <param name="ten"></param>
        /// <returns></returns>
        public static string Ten2Hex(string ten)
        {
            ulong tenValue = Convert.ToUInt64(ten);
            ulong divValue, resValue;
            string hex = "";
            do
            {
                //divValue = (ulong)Math.Floor(tenValue / 16);

                divValue = (ulong)Math.Floor((decimal)(tenValue / 16));

                resValue = tenValue % 16;
                hex = tenValue2Char(resValue) + hex;
                tenValue = divValue;
            } while (tenValue >= 16);

            if (tenValue != 0)
                hex = tenValue2Char(tenValue) + hex;
            return hex;
        }

        public static string tenValue2Char(ulong ten)
        {
            switch (ten)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return ten.ToString();
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                default:
                    return "";
            }
        }
    }
}

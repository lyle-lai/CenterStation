using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalClass
{
    public static class StructConver
    {
        /// <summary>
        /// 结构体转字节数组（按小端模式）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] StructureToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        /// <summary>
        /// 结构体转字节数组（按大端模式）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] StructureToByteArrayEndian(object obj)
        {
            object thisBoxed = obj; // copy 将struct装箱
            Type test = thisBoxed.GetType();

            int offset = 0;
            byte[] data = new byte[Marshal.SizeOf(thisBoxed)];

            object fieldValue;
            TypeCode typeCode;
            byte[] temp;
            // 枚举结构体的每个成功，并Reverse
            foreach (var field in test.GetFields())
            {
                fieldValue = field.GetValue(thisBoxed);
                typeCode = Type.GetTypeCode(fieldValue.GetType());

                switch (typeCode)
                {
                    // float
                    case TypeCode.Single:
                        {
                            temp = BitConverter.GetBytes((Single)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Single));
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            temp = BitConverter.GetBytes((Int16)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Int16));
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            temp = BitConverter.GetBytes((UInt16)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(UInt16));
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            temp = BitConverter.GetBytes((Int32)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Int32));
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            temp = BitConverter.GetBytes((UInt32)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(UInt32));
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            temp = BitConverter.GetBytes((Int64)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Int64));
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            temp = BitConverter.GetBytes((UInt64)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(UInt64));
                            break;
                        }
                    case TypeCode.Double:
                        {
                            temp = BitConverter.GetBytes((Double)fieldValue);
                            Array.Reverse(temp);
                            Array.Copy(temp, 0, data, offset, sizeof(Double));
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            data[offset] = (Byte)fieldValue;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                };
                if (typeCode == TypeCode.Object)
                {
                    int length = ((byte[])fieldValue).Length;
                    Array.Copy((byte[])fieldValue, 0, data, offset, length);
                    offset += length;
                }
                else
                {
                    offset += Marshal.SizeOf(fieldValue);
                }
            }
            return data;
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

        /// <summary>
        /// 字节数组转结构体（按大端模式）
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="obj"></param>
        /// <param name="startOffset"></param>
        public static void ByteArrayToStructureEndian(byte[] byteArray, ref object obj, int startOffset)
        {
            int len = Marshal.SizeOf(obj);
            IntPtr ptr = Marshal.AllocHGlobal(len);
            byte[] tempArray = (byte[])byteArray.Clone();
            // 从结构体指针构造结构体
            obj = Marshal.PtrToStructure(ptr, obj.GetType());
            // 做大端转换
            object thisBoxed = obj;
            Type test = thisBoxed.GetType();
            int reverseStartOffset = startOffset;
            // 枚举结构体每个成员，并反转
            foreach (var field in test.GetFields())
            {
                object fieldValue = field.GetValue(thisBoxed);
                TypeCode typeCode = Type.GetTypeCode(fieldValue.GetType());
                if (typeCode != TypeCode.Object)
                {
                    Array.Reverse(tempArray, reverseStartOffset, Marshal.SizeOf(fieldValue));
                    reverseStartOffset += Marshal.SizeOf(fieldValue);
                }
                else
                {
                    reverseStartOffset += ((byte[])fieldValue).Length;
                }
            }
            try
            {
                // 将字节数组复制到结构指针
                Marshal.Copy(tempArray, startOffset, ptr, Math.Min(len, byteArray.Length));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            obj = Marshal.PtrToStructure(ptr, obj.GetType());
            Marshal.FreeHGlobal(ptr);
        }
    }
}

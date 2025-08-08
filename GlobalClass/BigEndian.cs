using System;
using System.Collections.Generic;
using System.Text;
//-----------------------------------------------------------------------
namespace GlobalClass
{
    //-----------------------------------------------------------------------
    public class BigEndian
    {
        //-----------------------------------------------------------------------
        public static Int16 GetInt16(Byte[] Buf, int StartPos)
        {
            return (Int16)(GetInt(Buf, StartPos, 2));
        }
        //-----------------------------------------------------------------------
        public static Int32 GetInt32(Byte[] Buf, int StartPos)
        {
            return GetInt(Buf, StartPos, 4);
        }
        //-----------------------------------------------------------------------
        public static Int32 GetInt(Byte[] Buf, int StartPos, int Len)
        {
            Int32 Val = 0;
            for (int i = 0; i < Len; i++)
            {
                Val <<= 8;
                Val += Buf[StartPos + i];
            }
            return Val;
        }


        //-----------------------------------------------------------------------
        public static UInt16 GetUInt16(Byte[] Buf, int StartPos)
        {
            return (UInt16)(GetUInt(Buf, StartPos, 2));
        }
        //-----------------------------------------------------------------------
        public static UInt32 GetUInt32(Byte[] Buf, int StartPos)
        {
            return GetUInt(Buf, StartPos, 4);
        }
        //-----------------------------------------------------------------------
        public static UInt32 GetUInt(Byte[] Buf, int StartPos, int Len)
        {
            UInt32 Val = 0;
            for (int i = 0; i < Len; i++)
            {
                Val <<= 8;
                Val += Buf[StartPos + i];
            }
            return Val;
        }


        //-----------------------------------------------------------------------
        public static void SetInt(ref Byte[] Buf, int StartPos, Int16 Val)
        {
            byte[] dat = BitConverter.GetBytes(Val);  // 小端在前 34-12（0x1234）
            for (int i = 0; i < dat.Length; i++)      // dat.Length = 2（Int16占2个字节）
            {
                // 数组起始下标为0，大端在前
                Buf[StartPos++] = dat[dat.Length - 1 - i];
                //Buf[StartPos++] = dat[1];
                //Buf[StartPos++] = dat[0];
            }
        }
        //-----------------------------------------------------------------------
        public static void SetInt(ref Byte[] Buf, int StartPos, Int32 Val)
        {
            byte[] dat = BitConverter.GetBytes(Val);  // 小端在前 78-56-34-12（0x12345678）
            for (int i = 0; i < dat.Length; i++)      // dat.Length = 4（Int16占4个字节）
            {
                // 数组起始下标为0，大端在前
                Buf[StartPos++] = dat[dat.Length - 1 - i];
                //Buf[StartPos++] = dat[3];
                //Buf[StartPos++] = dat[2];
                //Buf[StartPos++] = dat[1];
                //Buf[StartPos++] = dat[0];
            }
        }


    }
    //-----------------------------------------------------------------------
}

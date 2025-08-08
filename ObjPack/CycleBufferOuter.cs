using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Drawing;
using System.Runtime.InteropServices;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class CycleBufferOuter
    {
        public CycleBuffer CycBuf;      //数据缓冲
        public int OutPos;              //出指针
        public DateTime OutTime = DateTime.MinValue;       //最后数据取出时间
        //------------------------------------------------------------------------------
        public CycleBufferOuter(CycleBuffer CycBuffer)
        {
            CycBuf = CycBuffer;
            OutPos = 0;
        }
        //------------------------------------------------------------------------------
        public void GetData(float[] Data)
        {
            int Len = Math.Min(CycBuf.Count, CycBuf.Freq);
            for (int i = 0; i < Len; i++)
                Data[i] = CycBuf.Buf[OutPos + i];

            OutPos += Len;
            if (OutPos >= CycBuf.Buf.Length) OutPos = 0;
        }
        //------------------------------------------------------------------------------
        public int PassMs()
        {
            if (OutTime == DateTime.MinValue) return -1;
            return GLB.PassMs(OutTime);
        }
        //------------------------------------------------------------------------------
        public int GetBufferLen()
        {
            int BufLen = CycBuf.InPos - OutPos;
            if (BufLen < 0) BufLen += CycBuf.Buf.Length;
            if (BufLen > CycBuf.HalfLen)
            {
                OutPos = CycBuf.InPos;
                OutTime = CycBuf.InTime;
                return 0;
            }
            return BufLen;
        }
        //------------------------------------------------------------------------------
        public float[] GetAllBuffer()
        {//一次取出所有缓冲数据
            int Len = GetBufferLen();
            if (Len <= 0) return null;
            //
            float[] Buf = new float[Len];
            if (OutPos < CycBuf.InPos)
            {
                //IntPtr dPtr = Marshal.UnsafeAddrOfPinnedArrayElement(CycBuf.Buf, OutPos);
                //Marshal.Copy(dPtr, Buf, 0, Buf.Length);
                Array.Copy(CycBuf.Buf, OutPos, Buf, 0, Buf.Length);
            }
            else
            {
                //填入第一段数据
                int Len1 = CycBuf.Buf.Length - OutPos;
                if (Len1 > Buf.Length) return Buf;
                //IntPtr dPtr = Marshal.UnsafeAddrOfPinnedArrayElement(CycBuf.Buf, OutPos);
                //Marshal.Copy(dPtr, Buf, 0, Len1);
                Array.Copy(CycBuf.Buf, OutPos, Buf, 0, Len1);
                //填入第二段数据
                OutPos = 0;
                int Len2 = CycBuf.InPos;
                if (Len1 + Len2 > Buf.Length) return Buf;
                //dPtr = Marshal.UnsafeAddrOfPinnedArrayElement(CycBuf.Buf, OutPos);
                //Marshal.Copy(dPtr, Buf, Len1, Len2);
                Array.Copy(CycBuf.Buf, 0, Buf, Len1, Len2);
            }
            OutPos = CycBuf.InPos;
            return Buf;
        }
        //------------------------------------------------------------------------------
        public void Reset()
        {
            OutPos = 0;
            OutTime = DateTime.MinValue;
            if (CycBuf != null)
                CycBuf.Reset();
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------

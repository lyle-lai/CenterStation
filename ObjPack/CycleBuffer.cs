using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Drawing;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class CycleBuffer 
    {
        public int Count;           //点数据个数长度
        public float[] Buf;         //实时数据缓冲
        public int InPos = 0;           //入指针
        public DateTime InTime = DateTime.MinValue;     //最后数据填入时间
        public int HalfLen;
        public int Freq; // 采样波特率
        //------------------------------------------------------------------------------
        public CycleBuffer(int Count, int Len, int Freq)
        {
            this.Count = Count;
            Buf = new float[Len];
            HalfLen = Len / 2;
            InPos = 0;
            this.Freq = Freq;
        }
        //------------------------------------------------------------------------------
        public void SetValue(float[] Data)
        {
            //for (int i = 0; i < Count; i++)
                //Buf[InPos + i] = Data[i];

            InTime = DateTime.Now;
            if (InPos + Count >= Buf.Length)
                InPos = 0;
            Array.Copy(Data, 0, Buf, InPos, Count);
            InPos += Count;
            if (InPos >= Buf.Length) InPos = 0;
        }
        //------------------------------------------------------------------------------
        public int SetData(float[] Data, int sPos, int Len)
        {
            InTime = DateTime.Now;
            //for (int i = 0; i < Count; i++)
                //Buf[InPos + i] = Data[sPos + i];
            if (InPos + Len >= Buf.Length)
                InPos = 0;
            Array.Copy(Data, sPos, Buf, InPos, Data.Length - sPos < Count ? (Data.Length - sPos) : Count);

            InPos += Count;
            //Log.d("CycleBuffer SetData Buf.Length=" + Buf.Length + ",InPos=" + InPos);
            if (InPos >= Buf.Length) InPos = 0;
            //
            return Count;
        }
        //------------------------------------------------------------------------------
        public void Reset()
        {
            InPos = 0;
            InTime = DateTime.MinValue;
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------

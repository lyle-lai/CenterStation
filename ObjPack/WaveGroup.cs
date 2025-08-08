using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Drawing;
using ObjPack;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class WaveGroup : MGOA
    {
        public static List<WaveGroup> Items = new List<WaveGroup>();
        public static void Tick()
        {
            for(int i=0; i<Items.Count; i++)
                Items[i].ACQ_TO_BUF();
        }
        //------------------------------------------------------------------------------
        //public String FullSN;
        public Color dColor;
        public Boolean isECG7 = false; //7心电
        public Boolean isRESP = false; // 呼吸
        public Boolean isART = false; // 有创
        //--------------------
        public CycleBuffer CycBuf = null;           //数据使用缓冲
        public CycleBufferOuter HistoryOuter = null;//存储历史数据指针
        //--------------------
        public CycleBuffer AcqBuf = null;           //数据采集缓冲
        public CycleBufferOuter AcqOuter = null;    //数据采集出指针
        private Double AcqPPMS = 0;
        private float[] AcqData = null;
        public int Count;
        //------------------------------------------------------------------------------
        public WaveGroup(MGOA Parent, DBReader dReader,List<AlarmPara> list)
            : base(Parent, dReader, -1, list)
        {
            Items.Add(this);
            //FullSN = dReader.GetStr("Path") + "." + dReader.GetStr("SN");
            dColor = dReader.GetColor("Color");
            int Freq = dReader.GetInt("Freq");

            //时间：2020年02月09日 修改人：lim
            //临时屏蔽
            //if (GLB.Same("ECG.ECG", FullSN))
            //    Freq = 500;

            isECG7 = GLB.Same(FullSN, "ECG.ECG");
            isRESP = GLB.Same(FullSN, "RESP.RESP");
            isART = GLB.Same(FullSN, "IBP.ART");
            //
            Count = isECG7 ? 3 : Childs.Count;
            CycBuf = new CycleBuffer(Count, Count * 120 * Freq, Freq);//数据缓冲
            HistoryOuter = new CycleBufferOuter(CycBuf);
            //
            //Log.d("New WaveGroup FullSN=" + FullSN + ",Count=" + Count + ",Freq=" + Freq);
            AcqBuf = new CycleBuffer(Count, Count * 120 * Freq, Freq);
            AcqOuter = new CycleBufferOuter(AcqBuf);
            AcqData = new float[Count];
        }
        //------------------------------------------------------------------------------
        public void SetValue(Byte[] Data)
        {
        }
        //------------------------------------------------------------------------------
        public int SetData(float[] Data, int sPos, int Len)
        {
            CycBuf.SetData(Data, sPos, Len);
            return CycBuf.Count;
        }
        //------------------------------------------------------------------------------
        public void FixECG(float[] dBuf)
        {
            if (!isECG7) return;

            //I II V 的值 
            //时间：2020年02月09日 修改人：李铭
            //int I = GLB.ByteToInt(dBuf[0]);
            //int II = GLB.ByteToInt(dBuf[0]);
            //int V = GLB.ByteToInt(dBuf[0]);

            float I = dBuf[0];
            float II = dBuf[1];
            float V = dBuf[2];
            //int II = GLB.ByteToInt(dBuf[1]);
            //int V = GLB.ByteToInt(dBuf[2]);
            dBuf[3] = (Byte)(II - I);
            dBuf[4] = (Byte)(I + II / 2);
            dBuf[5] = (Byte)(II / 2 - I);
            dBuf[6] = (Byte)(I / 2 - II);


        }
        //------------------------------------------------------------------------------
        private Byte ToByte(int Val)
        {
            return (Byte)((Val < 0) ? (256 - Val) : Val);
        }
        //------------------------------------------------------------------------------
        public void SetData_ACQ(float[] Data, int sPos, int Len)
        {
            //Log.d("1 SetData_ACQ FullSN=" + FullSN + ",SN=" + SN + ",Len=" + Len + ",BufferLen=" + AcqOuter.GetBufferLen());            
            int ePos = sPos + Len;
            while (sPos < ePos)
            {
                //if (GLB.Same("Spo2.Pleth", FullSN))
                    //Log.d("2 SetData_ACQ FullSN=" + FullSN + ",sPos=" + sPos);
                sPos += AcqBuf.SetData(Data, sPos, Len);
            }
            /*if (GLB.Same("Spo2.Pleth", FullSN))
                AcqPPMS = 0.025;
                //AcqPPMS = (Double)AcqOuter.GetBufferLen() / (256 * Count);
            else*/
                AcqPPMS = (Double)AcqOuter.GetBufferLen() / (3000 * Count);

            //if (GLB.Same("Spo2.Pleth", FullSN))
            /*Log.d(FullSN + " BufLen=" + AcqOuter.GetBufferLen().ToString()
                + ",AcqPPMS=" + AcqPPMS.ToString()
                + ",InPos=" + AcqOuter.CycBuf.InPos.ToString() 
                +",OutPos=" + AcqOuter.OutPos.ToString());
            */
            //AcqBuf.InTime = DateTime.Now;
        }
        //------------------------------------------------------------------------------
        private void ACQ_TO_BUF()
        {
            //Log.d("1 ACQ_TO_BUF FullSN=" + FullSN);

            if (AcqBuf.InTime == DateTime.MinValue) return; //未填入数据
            //Log.d("2 ACQ_TO_BUF FullSN=" + FullSN);
            int PassMs = AcqOuter.PassMs();
            if (PassMs <= 0)
            {
                AcqOuter.OutTime = DateTime.Now;
                return;
            }
            //
            int PCount = (int)(PassMs * AcqPPMS);//数据点个数
            if (PCount < 1) return;//是否足够一个点
            //Log.d("3 ACQ_TO_BUF FullSN=" + FullSN);
            for (int i = 0; i < PCount; i++)
            {
                //if (GLB.Same("RESP.RESP", FullSN))
                  //  Log.d("BufLen="+AcqOuter.GetBufferLen().ToString());
                if (AcqOuter.GetBufferLen() <= 0) break;
                AcqOuter.GetData(AcqData);
                CycBuf.SetValue(AcqData);
            }
            //Log.d("4 ACQ_TO_BUF FullSN=" + FullSN);

            AcqOuter.OutTime = AcqOuter.OutTime.AddMilliseconds(PCount / AcqPPMS);
        }
        //------------------------------------------------------------------------------
        public void Reset()
        {
            CycBuf.Reset();
            AcqOuter.Reset();
            HistoryOuter.Reset();
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------

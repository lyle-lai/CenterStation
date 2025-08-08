using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
//-----------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
using System.Diagnostics;
//-----------------------------------------------------------------------
namespace ViewPack
{
    //-----------------------------------------------------------------------
    public class ViewWaveGroup : ViewList
    {
        private static float AveLevel_1 = 0.95F;
        private static float AveLevel_2 = 1 - AveLevel_1;
        //------------------------------------------------------------------------------
        private static long STime = DateTime.Now.Ticks / 10000;
        private static float PixPerMm = SCR.xPix / (1000 * SCR.xMm);
        public static float GetNowX(float Speed)
        {
            return Speed * PixPerMm * (DateTime.Now.Ticks / 10000 - STime);
        }
        //------------------------------------------------------------------------------
        public static ViewWaveGroup[] Items = null;
        /*public static void Initialize()
        {
            Items = new ViewWaveGroup[WaveGroup.Items.Count];
            for (int i = 0; i < WaveGroup.Items.Count; i++)
            {
                ViewWaveGroup v = new ViewWaveGroup(WaveGroup.Items[i]);
                Items[i] = v;
                v.Visible = false;
            }
            PixPerMm = SCR.xPix / (1000 * SCR.xMm);
        }*/
        //------------------------------------------------------------------------------
        public static ViewWaveGroup Find(String GroupName)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (GLB.Same(Items[i].waveGroup.FullSN, GroupName)) return Items[i];
            }
            return null;
        }
        //------------------------------------------------------------------------------
        public WaveGroup waveGroup = null;
        private List<ViewWaveItem> vwItems = new List<ViewWaveItem>();
        protected SolidBrush dBrush = null;

        //private int outPtr = 0;     //出指针
        private CycleBufferOuter BufOuter;//

        private float[] getBuf;
        private float scanX = -1;
        public float aveDX = 2.0F;
        public float DirX = 0;
        //------------------------------------------------------------------------------
        public ViewWaveGroup(WaveGroup waveGroup, int ViewID)
        {
            this.waveGroup = waveGroup;
            if (waveGroup == null) return;
            //
            dBrush = new SolidBrush(waveGroup.dColor);
            getBuf = new float[waveGroup.Childs.Count];
            BufOuter = new CycleBufferOuter(waveGroup.CycBuf);
            //
            LoadWaveItems(ViewID);
            LayoutView();
        }
        //------------------------------------------------------------------------------
        private void LoadWaveItems(int ViewID)
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From ViewList Where ViewID=" + ViewID.ToString() + " And Visible=1 And GroupName='" + waveGroup.FullSN + "' And Type='WaveItem'");
            while (dr.Read())
            {
                WaveItem wItem = (WaveItem)waveGroup.FindChild(dr.GetStr("ItemName"));
                ViewWaveItem v = new ViewWaveItem(wItem, dBrush);
                vwItems.Add(v);
                v.Parent = this;
                v.Visible = true;
                v.Weight = dr.GetF("Weight", 1);
                Childs.Add(v);
                Weight += v.Weight;
            }
        }
        //------------------------------------------------------------------------------
        public void PushData()
        {
            float NowX = 0.0f;

            if (this.waveGroup.isRESP)
            {
                // 迈瑞监护仪上，呼吸波形的绘制大约比心电和脉搏的慢4倍
                NowX = GetNowX(6);
            }
            else
            {
                NowX = GetNowX(24);
            }

            //int BufLen = waveGroup.InPtr - outPtr;
            //if (BufLen < 0) BufLen += waveGroup.RT_Buf.Length;
            //BufLen = BufLen / 5;
            int BufLen = BufOuter.GetBufferLen();

            //if (GLB.Same(waveGroup.FullSN, "Spo2.Pleth"))
            //Log.d("1 *** " + waveGroup.FullSN + ",BufLen=" + BufLen.ToString());

            if (BufLen <= 5) return;

            //if (GLB.Same(waveGroup.FullSN, "Spo2.Pleth"))
            //Log.d("2 *** " + waveGroup.FullSN + ",BufLen=" + BufLen.ToString());
            //
            if (scanX < 0) scanX = NowX;
            if (NowX < scanX) return;
            //
            float dx = (NowX - scanX) / BufLen;
            aveDX = aveDX * AveLevel_1 + dx * AveLevel_2;
            /*
            if (GLB.Same(waveGroup.FullSN, "Spo2.Pleth"))
            Log.d("3 *** " + waveGroup.FullSN +
                ",BufLen=" + BufLen.ToString() +
                ",dx=" + dx.ToString()+
                ",aveDX=" + aveDX.ToString());*/
            //
            BufLen = BufLen / 5;

            //if (this.waveGroup.isECG7) {

            //    Debug.WriteLine("isECG7未做归一化处理，画波数据包");
            for (int iBuf = 0; iBuf < BufLen; iBuf++)
            {
                BufOuter.GetData(getBuf);
                waveGroup.FixECG(getBuf);
                //outPtr = waveGroup.GetRealTimeData(getBuf, outPtr);
                //if (GLB.Same(waveGroup.FullSN, "RESP.RESP"))
                //Log.d(waveGroup.FullSN + ":x=" + scanX + ", Val=" + getBuf[0].ToString());
                for (int i = 0; i < vwItems.Count; i++)
                {
                    vwItems[i].SetData(scanX, getBuf[i]);
                    //Debug.WriteLine($"scanX:{scanX},getBuf[i]:{getBuf[i]}");
                    //if (this.waveGroup.isART)
                    //{
                    //    Debug.WriteLine("一个art数据包,len:" + getBuf.Length);
                    //    foreach (var item in getBuf)
                    //    {
                    //        Debug.Write(item + ",");
                    //    }
                    //}
                    //if (GLB.Same(waveGroup.FullSN, "Spo2.Pleth"))
                    //Log.d(waveGroup.FullSN + ":x=" + scanX + ", Val=" + getBuf[i].ToString());
                    //if (getBuf[i] <= 10 && waveGroup.isECG7)
                    //BufOuter.GetData(getBuf);                       
                }

                scanX += aveDX;
                if (scanX >= NowX) break;
                //}
            }
        }
        //------------------------------------------------------------------------------
        public void Draw()
        {
            //Log.d("vwItems = " + vwItems.Count);
            if (vwItems == null || vwItems.Count <= 0) return;
            for (int i = 0; i < vwItems.Count; i++)
                vwItems[i].Draw();
        }
        //------------------------------------------------------------------------------
        public void StartDirect(float DX)
        {
            DirX = 0;
            aveDX = DX;
            for (int i = 0; i < vwItems.Count; i++)
                vwItems[i].StartDirect();
        }
        //------------------------------------------------------------------------------
        public int SetDataDirect(float[] Data, int Pos)
        {
            if (vwItems == null || vwItems.Count <= 0)
                return waveGroup.Count;
            //
            if (DirX >= 2048) return 0;

            for (int i = 0; i < waveGroup.Count; i++)
            {
                if (Pos + i >= Data.Length) break;
                getBuf[i] = Data[Pos + i];
            }
                
            waveGroup.FixECG(getBuf);
            //
            for (int i = 0; i < vwItems.Count; i++)
                vwItems[i].SetDataDirect(DirX, getBuf[i]);
            DirX += aveDX;
            return waveGroup.Count;
        }
        //------------------------------------------------------------------------------
        public void DrawDirect()
        {
            if (vwItems == null || vwItems.Count <= 0) return;
            for (int i = 0; i < vwItems.Count; i++)
            {
                vwItems[i].DrawDirect();
            }
        }
        //------------------------------------------------------------------------------
        public void Reset()
        {
            waveGroup.Reset();
            BufOuter.Reset();
            scanX = -1;
            aveDX = 2.0F;
            DirX = 0;
        }
        //------------------------------------------------------------------------------
        public void Clean()
        {
            foreach(ViewWaveItem item in vwItems)
            {
                item.Clean();
            }
        }
    }
    //-----------------------------------------------------------------------
}
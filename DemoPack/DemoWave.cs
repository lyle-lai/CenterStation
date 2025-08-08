using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
//------------------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
//------------------------------------------------------------------------------
namespace DemoPack
{
    //------------------------------------------------------------------------------
    public class DemoWave
    {
        //------------------------------------------------------------------------------
        public static void Demo16To8()
        {
            DBReader dReader = new DBReader(DBConnect.SYS);
            Byte[] B16;
            Byte[] B8 = null;
            //ECG.ECG
            dReader.Select("Select * From DemoWave Where GroupName='ECG.ECG'");
            if (dReader.Read())
            {
                B16 = (Byte[])dReader.GetBlob("Data");
                B8 = new Byte[B16.Length / 2];
                for (int i = 0; i < B8.Length; i++)
                {
                    short v16 = BitConverter.ToInt16(B16, i * 2);
                    //v16 = (short)(v16 * 128 / 2048);
                    //B8[i] = (Byte)((v16 > 0) ? v16 : 256 - v16);
                    v16 = (short)((-2048 + v16) * 255 / 4096);
                    B8[i] = (Byte)(v16 - 128);
                    //B8[i] = (Byte)((255 * (v16+2048) / 4096));
                }
                //DBConnect.SYS.AddPara("@BData", B8);
                //DBConnect.SYS.ExecSQL("Update DemoWave Set BData=@BData Where GroupName='ECG.ECG'");
            }
            //RESP.RESP
            dReader.Select("Select * From DemoWave Where GroupName='RESP.RESP'");
            if (dReader.Read())
            {
                B16 = dReader.GetBlob("Data");
                B8 = new Byte[B16.Length / 2];
                for (int i = 0; i < B8.Length; i++)
                {
                    short v16 = BitConverter.ToInt16(B16, i * 2);
                    //v16 = (short)((0 + v16) * 255 / 100);
                    B8[i] = (Byte)(v16 - 128);
                }
                //DBConnect.SYS.AddPara("@BData", B8);
                //DBConnect.SYS.ExecSQL("Update DemoWave Set BData=@BData Where GroupName='RESP.RESP'");
            }
            //Spo2.Pleth
            dReader.Select("Select * From DemoWave Where GroupName='Spo2.Pleth'");
            if (dReader.Read())
            {
                B16 = dReader.GetBlob("Data");
                B8 = new Byte[B16.Length / 2];
                for (int i = 0; i < B8.Length; i++)
                {
                    short v16 = BitConverter.ToInt16(B16, i * 2);
                    //B8[i] = (Byte)(((v16-128) * 255)/100);
                    v16 = (short)(v16 * 255 / 100);
                    B8[i] = (Byte)(v16 - 128);
                }
                //DBConnect.SYS.AddPara("@BData", B8);
                //DBConnect.SYS.ExecSQL("Update DemoWave Set BData=@BData Where GroupName='Spo2.Pleth'");
            }
        }
        //------------------------------------------------------------------------------
        private WaveGroup waveGroup = null;
        //private DBReader dr = new DBReader(DBConnect.SYS);
        private Byte[] Data = null;
        private int sPos = 0;
        private int Freq = 50;
        //private DateTime LastTime = DateTime.Now;
        //private double msRate = 0;		//演示数据的采样频率(点/毫秒)

        //------------------------------------------------------------------------------
        public DemoWave(WaveGroup waveGroup)
        {
            Demo16To8();
            this.waveGroup = waveGroup;
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From DemoWave Where GroupName='" + waveGroup.FullSN + "' Order By ID");
            if (!dr.Read()) return;
            Data = dr.GetBlob("BData");
            Freq = dr.GetInt("Freq") * waveGroup.Count;

            //if (GLB.Same(waveGroup.FullSN, "Spo2.Pleth"))
            //{
            //    for (int i = 0; i < Data.Length; i++)
            //        Data[i] = (Byte)(Data[i] - 128);
            //}
            //if (GLB.Same(waveGroup.FullSN, "RESP.RESP"))
            //{
            //    for (int i = 0; i < Data.Length; i++)
            //        Data[i] = (Byte)(Data[i] - 128);
            //}

            //msRate = dr.GetF("Rate", 0) /1000;
        }
        //------------------------------------------------------------------------------
        public void Tick()
        {
            float[] temp = new float[Data.Length];
            for (int i = 0; i < Data.Length; i++)
            {
                temp[i] = Data[i];
            }
            waveGroup.SetData_ACQ(temp, sPos, Freq);
            sPos += Freq;
            if (sPos >= Data.Length - 1)
                sPos = 0;
            //waveGroup.SetData_ACQ(Data, 0, Data.Length);

            /*
            double Count = GLB.PassMs(LastTime) * msRate;//数据点个数
            if (Count < 1) return;	//是否足够一个点
            LastTime = LastTime.AddMilliseconds(Count / msRate);
            //
            for (int i = 0; i < Count; i++)
            {
                dPos += waveGroup.SetData(Data, dPos) * 2;
                if (dPos >= Data.Length) dPos = 0;
            }*/
        }
        //------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------
}
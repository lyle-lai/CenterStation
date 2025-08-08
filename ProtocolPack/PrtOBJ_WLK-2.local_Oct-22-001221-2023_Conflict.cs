using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//------------------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
//using ViewPack;
//------------------------------------------------------------------------------
namespace ProtocolPack
{
    //Begin class ProtOBJ------------------------------------------------------------------------------
    class PrtOBJ
    {
        private List<TFlag> Flags = new List<TFlag>();
        private int sPos = 0;
        private int Count = 0;
        private int dLen = 1;
        private String Mode;
        private String FullSN;
        private ParaGroup PGrp = null;
        private WaveGroup WGrp = null;
        //------------------------------------------------------------------------------
        public PrtOBJ(DBReader dr, ViewMonitor VM)
        {
            Flags = TFlag.CreateFlag(dr.GetStr("Flag"));
            sPos = dr.GetInt("sPos");
            Count = dr.GetInt("Count");
            dLen = dr.GetInt("dLen");
            Mode = dr.GetStr("Mode");
            FullSN = dr.GetStr("FullSN");
            //绑定对应的子窗体对象
            MGOA g = MGOA.Find(VM.MList, FullSN);
            if (g is ParaGroup)
                PGrp = (ParaGroup)g;
            else if (g is WaveGroup)
                WGrp = (WaveGroup)g;
        }
        //------------------------------------------------------------------------------
        /*private void Bind()
        {//绑定对应的子窗体对象
            PGrp = ParaGroup.Find(FullSN);
            //WGrp = WaveGroup.Find(FullSN);
        }*/
        //------------------------------------------------------------------------------
        public Boolean Check(Byte[] Buf, int Len)
        {
            if (sPos >= Len) return false;
            for (int i = 0; i < Flags.Count; i++)
                if (!Flags[i].Check(Buf, Len)) return false;
            //
            return true;
        }
        //------------------------------------------------------------------------------
        public void SetValue(Byte[] Buf, int BufPos)
        {   //读取数据，将其推入波形，或参数缓冲区
            //Log.d("****** SetValue " + FullSN);
            WaveSetValue(Buf, BufPos);
            ParaSetValue(Buf, BufPos);
        }
        //------------------------------------------------------------------------------
        private void WaveSetValue(Byte[] Buf, int BufPos)
        {
            if (WGrp == null) return;
            /*
            short[] Values = new short[WGrp.Count];
            for (int i = 0; i < Values.Length; i++)
            {
                if (GLB.IsEmpty(Mode) && dLen == 1)
                {
                    Values[i] = Buf[BufPos + i];
                }
            }
            //WGrp.SetValue(Values);
             */

            //int Len1 = Len - BufPos;
            int DataLen = Count * WGrp.Count;
            Byte[] Data = new Byte[DataLen];
            if (GLB.IsEmpty(Mode))
            {
                for (int i = 0; i < Count; i++)
                {
                    for (int j = 0; j < WGrp.Count; j++)
                    {
                        int iDes = i * WGrp.Count + j;
                        int iSrc = BufPos + sPos + iDes;
                        //Data[iDes] = Buf[iSrc];
                        Data[iDes] = (Byte)(Buf[iSrc] - 128);
                    }
                }
                //WGrp.SetData_ACQ(Buf, BufPos + sPos, DataLen);
            }
            else if (GLB.Same(Mode, "CHN"))
            {
                for (int i = 0; i < Count; i++)
                {
                    for (int j = 0; j < WGrp.Count; j++)
                    {
                        int iSrc = BufPos + sPos + j * Count + i;
                        int iDes = i * WGrp.Count + j;
                        //Data[iDes] = Buf[iSrc];
                        Data[iDes] = (Byte)(Buf[iSrc] - 128);
                    }
                }
            }
            WGrp.SetData_ACQ(Data, 0, DataLen);
            /*
            Byte[] Data = new Byte[DataLen];
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < WGrp.Count; j++)
                {
                    if (GLB.IsEmpty(Mode))
                        Data[i] = Buf[BufPos + i * dLen];
                    if (GLB.Same(Mode, "CHN"))
                        Data[i] = Buf[BufPos + i * dLen];
                }
            }
            WGrp.SetData_ACQ(Data, 0, DataLen);*/

            /*
            int DataLen = (dLen == 1) ? Len * 2 : Len;
            Byte[] Data = new Byte[DataLen];

            if (GLB.IsEmpty(Mode) && dLen == 1)
            {
                for (int i = 0; i < Len; i++)
                    Data[i * 2] = Buf[BufPos + i];
            }*/
            /*
            if(GLB.Same(Mode,"CHN") && dLen==1)
            {
                for (int i = 0; i < Len; i++)
                    Data[i * 2] = Buf[BufPos + i];
            }*/

            //WGrp.SetData_ACQ(Data, 0, DataLen);
        }
        //------------------------------------------------------------------------------
        private void ParaSetValue(Byte[] Buf, int BufPos)
        {
            if (PGrp == null) return;
            short[] Values = new short[PGrp.Childs.Count];
            //short[] Values = new short[1];
            for (int i = 0; i < Values.Length; i++)
            {
                int Pos = BufPos + sPos;
                //--------
                if (GLB.IsEmpty(Mode) && dLen == 2)
                {
                    Values[i] = BitConverter.ToInt16(Buf, Pos + 2 * i);
                    //Log.d("** Val" + i.ToString() + "=" + Values[i].ToString());
                    continue;
                }
                //--------
                if (GLB.Same(Mode, "F*10") && dLen == 4)
                {
                    Values[i] = (Int16)(BitConverter.ToSingle(Buf, Pos + 4 * i) * 10);
                    //Log.d("** Val" + i.ToString() + "=" + Values[i].ToString());
                    continue;
                }
                //--------

            }
            PGrp.SetValue(Values);
        }
        //------------------------------------------------------------------------------
    }
    //End class ProtOBJ------------------------------------------------------------------------------
    //Begin class TFlag------------------------------------------------------------------------------
    class TFlag
    {
        //------------------------------------------------------------------------------
        public static List<TFlag> CreateFlag(String Str)
        {
            List<TFlag> Flags = new List<TFlag>();
            String[] sList = Str.Split(',');
            for (int i = 0; i < sList.Length; i++)
                Flags.Add(new TFlag(sList[i]));

            return Flags;
        }
        //------------------------------------------------------------------------------
        private int Pos;
        private Byte Value;
        //------------------------------------------------------------------------------
        public TFlag(String Str)
        {
            String[] sList = Str.Split('=');
            Pos = GLB.ToInt(sList[0], 0);
            Value = (Byte)GLB.ToInt(sList[1], 0);
        }
        //------------------------------------------------------------------------------
        public Boolean Check(Byte[] Buf, int Len)
        {
            if (Pos >= Len) return false;
            return (Buf[Pos] == Value);
        }
        //------------------------------------------------------------------------------
    }
    //End class TFlag------------------------------------------------------------------------------
}

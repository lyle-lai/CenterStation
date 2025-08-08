using System;
using System.Collections.Generic;
using System.Text;
//------------------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
//using ViewPack;
//------------------------------------------------------------------------------
namespace ProtocolPack
{
    //Begin class PrtPhilipsItem------------------------------------------------------------------------------
    class PrtPhilipsItem
    {
        private Byte[] Flag;
        private int dLen = 1;
        private String Mode;
        private String GroupName;
        private ParaGroup PGrp = null;
        private int OID = 0;
        private String OName;
        private int Value;
        //------------------------------------------------------------------------------
        public PrtPhilipsItem(String GroupName, int OID)
        {
            this.GroupName = GroupName;
            this.OID = OID;

            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From PrtPhilipsMP20 Where GroupName='" + GroupName + "' And OID=" + OID.ToString());
            if (dr.Read())
            {
                Flag = dr.GetBlob("Flag");
                dLen = dr.GetInt("dLen");
                Mode = dr.GetStr("Mode");
                OName = dr.GetStr("OName");
            }
        }
        //------------------------------------------------------------------------------
        public PrtPhilipsItem(DBReader dr)
        {
            GroupName = dr.GetStr("GroupName");
            OID = dr.GetInt("OID");
            Flag = dr.GetBlob("Flag");
            dLen = dr.GetInt("dLen");
            Mode = dr.GetStr("Mode");
            OName = dr.GetStr("OName");
        }
        //------------------------------------------------------------------------------
        /*public PrtPhilipsItem(DBReader dr, ViewMonitor VM)
        {
            Flag = dr.GetBlob("Flag");
            dLen = dr.GetInt("dLen");
            Mode = dr.GetStr("Mode");
            GroupName = dr.GetStr("GroupName");
            OName = dr.GetStr("OName");
            OID = dr.GetInt("OID");
            //绑定对应的子窗体对象
        }*/
        //------------------------------------------------------------------------------
        public Boolean UnPack(Byte[] Buf, int Len, short[] V)
        {
            int ePos = Len - Flag.Length;
            if (ePos <= 0) return false;
            //
            for (int i = 0; i < ePos; i++)
            {
                Boolean isSame = true;
                for (int j = 0; j < Flag.Length; j++)
                {
                    if (j == 2 || j == 3) continue;//忽略2字节的检查
                    if (Flag[j] != Buf[i + j])
                    {
                        isSame = false;
                        break;
                    }
                }
                if (!isSame) continue;

                GetValue(Buf, i + Flag.Length, V);
                return true;
            }
            //
            return false;
        }
        //------------------------------------------------------------------------------
        private void GetValue(Byte[] Buf, int Pos, short[] V)
        {
            //Flag符合，取出数值
            //int Pos = i + Flag.Length;
            Byte[] VBuf = new Byte[4];
            for (int vi = 0; vi < VBuf.Length; vi++)
                VBuf[vi] = Buf[Pos + VBuf.Length - vi - 1];

            Value = BitConverter.ToInt32(VBuf, 0);
            if (GLB.Same(Mode, "F*0.1"))
            {
                Value = Value & 0x00FFFFFF;
                if (Value != 0x7FFFFF)
                    Value = Value / 10;
            }
            //
            if (OID == 0)
            {
                for (int i = 0; i < V.Length; i++)
                    V[i] = short.MaxValue;
            }
            //
            V[OID] = (short)Value;
            Log.d("Check " + GroupName + "." + OName + "=" + Value.ToString());
        }
        //------------------------------------------------------------------------------
    }
    //End class PrtPhilipsItem------------------------------------------------------------------------------
}

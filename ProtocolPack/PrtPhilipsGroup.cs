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
    //Begin class PrtPhilipsGroup------------------------------------------------------------------------------
    class PrtPhilipsGroup
    {
        private int GroupID;
        private String GroupName;
        private short[] Values;
        //private ParaGroup PGrp = null;
        private List<PrtPhilipsItem> Items = new List<PrtPhilipsItem>();
        //事件定义
        public PrtPhilipsMP20.OnGetParamater onGetPara = null;
        //------------------------------------------------------------------------------
        public PrtPhilipsGroup(int GroupID, String GroupName, PrtPhilipsMP20.OnGetParamater On_Get_Paramater)
        {
            this.GroupID = GroupID;
            this.GroupName = GroupName;
            onGetPara = On_Get_Paramater;
            //绑定对应的子窗体对象
            //MGOA g = MGOA.Find(VM.MList, GroupName);
            //if (g is ParaGroup)
                //PGrp = (ParaGroup)g;
            //
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From PrtPhilipsMP20 Where GroupName='" + GroupName + "' Order By OID");
            Values = new short[dr.Count];
            while (dr.Read())
                Items.Add(new PrtPhilipsItem(dr));
        }
        //------------------------------------------------------------------------------
        public void UnPack(Byte[] Buf, int Len)
        {
            //Log.d("UnPack GroupName=" + GroupName);
            Boolean IsCheck = false;
            for (int i = 0; i < Items.Count; i++)
                IsCheck = IsCheck | Items[i].UnPack(Buf, Len, Values);
            //转发到中央监护系统
            if (IsCheck && onGetPara!=null)
            {
                onGetPara(GroupID, GroupName, Values);
            }
        }
        //------------------------------------------------------------------------------
    }
    //End class PrtPhilipsGroup------------------------------------------------------------------------------
}

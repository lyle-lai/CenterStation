using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//------------------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
//------------------------------------------------------------------------------
namespace DemoPack
{
    //------------------------------------------------------------------------------
    public class DemoPara
    {
        //------------------------------------------------------------------------------
        private ParaGroup paraGroup = null;
        private DBReader dr = new DBReader(DBConnect.SYS);
        private Int16[] Data = null;
        private DateTime LastTime = DateTime.Now;
        private int Inte = -1;
        //------------------------------------------------------------------------------
        public DemoPara(ParaGroup paraGroup)
        {
            this.paraGroup = paraGroup;
            dr.Select("Select * From DemoPara Where GroupName='" + paraGroup.FullSN + "' Order By ID");
            Data = new Int16[paraGroup.Childs.Count];
        }
        //------------------------------------------------------------------------------
        public void Tick()
        {
            if (dr.Count <= 0) return;

            if (GLB.PassMs(LastTime) < Inte) return;//时间间隔未到
            if (!dr.Read()) dr.Goto(0);
            //
            Inte = dr.GetInt("Inte") * 1000;
            LastTime = DateTime.Now;
            String Str = dr.GetStr("Value");
            String[] sList = Str.Split(',');
            //
            for (int i = 0; i < paraGroup.Childs.Count; i++)
            {
                if (i >= sList.Length) break;
                Data[i] = GLB.ToShort(sList[i], 0);
            }
            paraGroup.SetValue(Data);
        }
        //------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------
}
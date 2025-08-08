using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Diagnostics;
using ObjPack;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class UnitGroup : MGOA
    {
        //------------------------------------------------------------------------------
        private static List<MGOA> Items = new List<MGOA>();
        public static void Initialize()
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Module Where SN='Unit'");
            while (dr.Read())
            {
                Items.Add(new MGOA(null, dr, -1));
            }
        }
        //------------------------------------------------------------------------------
        public static UnitGroup Find(String UnitName)
        {
            return (UnitGroup)MGOA.Find(Items, "Unit." + UnitName);
        }
        //------------------------------------------------------------------------------
        public UnitGroup(MGOA Parent, DBReader dReader, List<AlarmPara> list)
            : base(Parent,dReader,-1,list)
        {
            //CreateChild("UnitItem");
        }
        //------------------------------------------------------------------------------
        public String GetUnitName()
        {
            if (Childs == null || Childs[0] == null) return "";
            return Childs[0].GetName();
           // return Childs[0].GetENName();
        }
        //------------------------------------------------------------------------------
        public String GetDigitStr()
        {
            if (Childs == null || Childs[0] == null) return "";
            return ((UnitItem)Childs[0]).DStr;
        }
    }
}
//--------------------------------------------------------------------------------------

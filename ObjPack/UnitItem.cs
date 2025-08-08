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
    public class UnitItem : MGOA
    {
        public Double a = 1;
        public Double b = 0;
        public int Digit = 0;
        public String InvStr;
        public String DStr;
        //------------------------------------------------------------------------------
        public UnitItem(MGOA Parent, DBReader dReader, List<AlarmPara> list)
            : base(Parent,dReader,-1,list)
        {
            a = dReader.GetF("a", 1);
            b = dReader.GetF("b", 0);
            Digit = dReader.GetInt("Digit");
            DStr = "f" + Digit.ToString();
            InvStr = dReader.GetStr("Inv");
        }
        //------------------------------------------------------------------------------
        public String GetString(Double Value)
        {
            return (Value * a + b).ToString(DStr);
        }
        //------------------------------------------------------------------------------
        public Double ToDouble(Double Value)
        {
            return Value * a + b;
        }
        //------------------------------------------------------------------------------
        public Int16 ToInt(Double FloatValue)
        {
            return (short)GLB.ToInt((FloatValue - b) / a);
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------

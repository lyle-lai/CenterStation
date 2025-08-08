using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Diagnostics;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class AttFloat: Att
    {
        public double Value;
        //------------------------------------------------------------------------------
        public AttFloat(MGOA Parent, DBReader dReader, UInt32 ID_Att, String SN_Att)
            : base(Parent, dReader, ID_Att, SN_Att)
        {
            Value = dReader.GetF(SN, 0);
            //Debug.WriteLine("Value=" + Value);
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------

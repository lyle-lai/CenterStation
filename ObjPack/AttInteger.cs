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
    public class AttInteger : Att
    {
        public int Value;
        //------------------------------------------------------------------------------
        public AttInteger(MGOA Parent, DBReader dReader, UInt32 ID_Att, String SN_Att)
            : base(Parent, dReader, ID_Att, SN_Att)
        {
            Value = dReader.GetInt(SN);
            //Debug.WriteLine("Value=" + Value);
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------

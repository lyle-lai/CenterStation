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
    public class Att : MGOA
    {
        //------------------------------------------------------------------------------
        public Att(MGOA Parent, DBReader dReader, UInt32 ID_Att, String SN_Att)
            : base(Parent,dReader,ID_Att, SN_Att)
        {
            //dReader.
            //CreateAtt("Max");
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Drawing;
using ObjPack;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class WaveItem : MGOA
    {
        public Color dColor;
        public int Min;
        public int Max;
        //------------------------------------------------------------------------------
        public WaveItem(MGOA Parent, DBReader dReader,List<AlarmPara> list)
            : base(Parent, dReader, -1, list)
        {
            //Min = dReader.GetInt("Min");
            //Max = dReader.GetInt("Max");
            Min = -128;
            Max = 127;
        }
        //------------------------------------------------------------------------------
        public void SetData(short Data)
        {

        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------

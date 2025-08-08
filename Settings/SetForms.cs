using System;
using System.Collections.Generic;
using System.Text;
using GlobalClass;
//-----------------------------------------------------------------------------
namespace Settings
{
    //-----------------------------------------------------------------------------
    public class SetForms
    {
        public static FrmAlarmSetting frmAlarm = null;
        public static FrmBed frmBed = null;
        //-----------------------------------------------------------------------------
        public static void Initialize()
        {
            frmAlarm = new FrmAlarmSetting();
            frmBed = new FrmBed();
        }
        //-----------------------------------------------------------------------------
        public static void SetCommand(GLB.SEND_COMMAND SendCommand)
        {
        }
        //------------------------------------------------------------------------------

    }
    //-----------------------------------------------------------------------------
}

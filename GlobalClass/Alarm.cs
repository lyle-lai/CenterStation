using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
//-----------------------------------------------------------------------
namespace GlobalClass
{
    //-----------------------------------------------------------------------
    public class Alarm
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool Beep(int frequency, int duration);

        public enum Mode { Normal, ToLow, ToHigh };	//报警模式
        public static Boolean isTwink = false;
        private static DateTime LastTime = DateTime.Now;
        public static Boolean isAlarm = false;
        public static Boolean AlarmOn = false;
        //-----------------------------------------------------------------------
        private static int AlarmColorLever = 50;	//报警闪烁颜色的亮度(%)
        public static Color getAlarmColor(Color rawColor)
        {
            return Color.FromArgb(
                    rawColor.R * AlarmColorLever / 100,
                    rawColor.G * AlarmColorLever / 100,
                    rawColor.B * AlarmColorLever / 100);
        }
        //-----------------------------------------------------------------------
        public static void Tick()
        {
            if (GLB.PassMs(LastTime) < 1000) return;
            //
            //if(isAlarm)
                //Beep(1000, 500);
            //
            isAlarm = false;
            LastTime = DateTime.Now;
            isTwink = !isTwink;
        }
        //-----------------------------------------------------------------------
        private static Thread AlarmThread = null;//new Thread(new ThreadStart(Run));
        private static void Run()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (!(isAlarm && AlarmOn)) continue;
                Beep(1000, 500);
                isAlarm = false;
            }
        }
        //-----------------------------------------------------------------------------
        public static void Initialize()
        {//初始化
            AlarmThread = new Thread(new ThreadStart(Run));
            AlarmThread.Start();
        }
        //-----------------------------------------------------------------------------

    }
    //-----------------------------------------------------------------------
}

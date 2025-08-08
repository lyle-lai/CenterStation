using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace GlobalClass
{
    public class Log
    {
        public enum LOG_MODE { None, Debug, Console };	//LOG的方式
        private static LOG_MODE Mode = LOG_MODE.None;        
        // -------------------------------------------------------------------------------
        public static void Init(LOG_MODE LogMode)
        {
            Mode = LogMode;

            //初始化控制台输入
            if (LogMode == LOG_MODE.Console)
            {
                StreamWriter sw = File.CreateText(GLB.EXE_DIR + "Log.Txt");
                sw.AutoFlush = true;
                Console.SetOut(sw);
                Console.WriteLine("**** " + DateTime.Now + " ****");
            }
        }
        // -------------------------------------------------------------------------------
        public static void d(String str)
        {
            switch (Mode)
            {
                case LOG_MODE.Console:
                    Console.WriteLine(DateTime.Now + " : " + str);
                    break;
                case LOG_MODE.Debug:
                    Debug.WriteLine(str);
                    break;
            }
        }
        // -------------------------------------------------------------------------------
        public static void d(String targ, String str)
        {
            d(str);
        }
        // -------------------------------------------------------------------------------
    }
}

using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
//-----------------------------------------------------------------------
//using System.Net.NetworkInformation;
using Microsoft.Win32;
//------------------------------------------------------------------------
namespace GlobalClass
{
    public class GLB
    {
        //public static GLB GLB = null;
        //-----------------------------------------------------------------------
        //常量定义
        public static Boolean SECR = false; //保密状态写程序，5月后可取消此项
        public const float FloatMin = 1e-10F;      //最小浮点数
        public static DateTime MinTime = DateTime.Now.AddYears(-10);
        //-----------------------------------------------------------------------
        //默认值定义
        public static String EXE_DIR;
        public static String CFG_DIR;
        public static String DAT_DIR;
        public static String IMG_DIR;
        public static Color BackColor = Color.White;
        public static Color ForeColor = Color.Black;
        //-----------------------------------------------------------------------
        public static int BaseWidth = 240;
        public static int BaseHeight = 320;
        public static String SysFontName = "微软雅黑";
        public static Font SysFont = new Font(SysFontName, 9, FontStyle.Regular);
        public static SolidBrush SysBrush = new SolidBrush(ForeColor);
        public static SolidBrush ForeBrush = new SolidBrush(ForeColor);
        public static SolidBrush BackBrush = new SolidBrush(BackColor);
        //-----------------------------------------------------------------------
        //私有成员
        private static Double xScal = 1;//x方向的缩放比例
        private static Double yScal = 1;//y方向的缩放比例
        private const int MPD = 24 * 60;     //每日分钟数
        private static String SDT_Format = null;
        //-----------------------------------------------------------------------
        //public static DBReader drData = null;
        public static DBReader drSys = null;
        //-----------------------------------------------------------------------
        public static IntPtr DC;
        public static Boolean Demo = false;
        //-----------------------------------------------------------------------
        public enum DIR { Lower, Upper };
        public enum Direction { Upper, Down };	//数据传输方向
        //-----------------------------------------------------------------------
        public delegate void SEND_COMMAND(String FullSN, int Value);
        //-----------------------------------------------------------------------
        //函数定义
        public static void Init()
        {//构造函数
            //初始化程序所在的文件夹
            EXE_DIR = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            if (Same(EXE_DIR.Substring(0, 4), "File"))
                EXE_DIR = EXE_DIR.Substring(8);
            EXE_DIR = Path.GetDirectoryName(EXE_DIR) + "\\";

            DirectoryInfo diExe = new DirectoryInfo(EXE_DIR);
            CFG_DIR = EXE_DIR + "Config\\";
            DAT_DIR = EXE_DIR + "Data\\";// diExe.Parent.FullName + "\\PublicFiles\\";
            IMG_DIR = EXE_DIR + "Image\\";
            
            if (!Directory.Exists(GLB.DAT_DIR))
            {
                Directory.CreateDirectory(DAT_DIR);
            }

            // Log.Init(Log.LOG_MODE.Console);    //LOG写到文件
            Log.Init(Log.LOG_MODE.Debug);    //LOG写到debug
            //Log.Init(true);     //LOG写到Debug
            //初始化配置文件
            /*Config Cfg = new Config();
            //设置显示颜色
            ForeColor = Config.GetColor("[Color][Fore]");
            BackColor = Config.GetColor("[Color][Back]");
             */ 
            SysBrush = new SolidBrush(ForeColor);
            ForeBrush = new SolidBrush(ForeColor);
            BackBrush = new SolidBrush(BackColor);

            //初始化
            //DBConnect.DATA = new DBConnect(EXE_DIR + "Data.DB;");
            DBConnect.SYS = new DBConnect(CFG_DIR + "RW_SYS.DB;");
            DBConnect.Initialize();
            //drData = new DBReader(DBConnect.DATA);
            drSys = new DBReader(DBConnect.SYS);
            //
            //初始化配置文件
            Config.Initialize();
            Lang.Initialize();
            //
            Init_SDT_Format();
        }
        //-----------------------------------------------------------------------
        private static void Init_SDT_Format()
        {
            SDT_Format = "M月d日";
            return;

            SDT_Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern.ToString();//取出当前系统中的短日期格式
            SDT_Format = SDT_Format.Replace("y", "");//去掉年份(小写)
            SDT_Format = SDT_Format.Replace("Y", "");//去掉年份(大写)
            //去掉两端的分隔符
            String Sep = System.Globalization.DateTimeFormatInfo.CurrentInfo.DateSeparator.ToString();
            if (SDT_Format.Substring(0, 1) == Sep)//去掉前端的分隔符
                SDT_Format = SDT_Format.Substring(1, SDT_Format.Length - 1);
            if (SDT_Format.Substring(SDT_Format.Length - 1, 1) == Sep)//去掉末端的分隔符
                SDT_Format = SDT_Format.Substring(0, SDT_Format.Length - 1);
        }
        //-----------------------------------------------------------------------
        public static int ToInt(Object Val, int DefVal)
        {//字符串转整型，自动识别10、16进制，及出错处理。
            if (Val == null) return (DefVal);

            String Str = Val.ToString().Trim();
            int ReInt = DefVal;
            if (IsEmpty(Str)) return (ReInt);
            //十六进制的转换
            if (Str.IndexOf("0x") >= 0)
            {
                try { ReInt = Convert.ToInt32(Str, 16); }
                catch (Exception ex) {Log.d(ex.Message) ;}
                return (ReInt);
            }
            //十进制的转换
            try { ReInt = Convert.ToInt32(Str, 10); }
            catch (Exception ex) { Log.d(ex.Message); }
            return (ReInt);
        }
        //-------------------------------------------------------------------------------------------------
        public static short ToShort(Object Val, short DefVal)
        {//字符串转短整型，自动识别10、16进制，及出错处理。
            if (Val == null) return (DefVal);

            String Str = Val.ToString().Trim();
            short ReInt = DefVal;
            if (IsEmpty(Str)) return (ReInt);
            //十六进制的转换
            if (Str.IndexOf("0x") >= 0)
            {
                try { ReInt = Convert.ToInt16(Str, 16); }
                catch (Exception ex) {Log.d(ex.Message) ;}
                return (ReInt);
            }
            //十进制的转换
            try { ReInt = Convert.ToInt16(Str, 10); }
            catch (Exception ex) { Log.d(ex.Message); }
            return (ReInt);
        }
        //-------------------------------------------------------------------------------------------------
        public static Double ToDouble(String Val, float Default)
        {
            Double f = Default;
            if (IsEmpty(Val)) return (f);
            try
            {
                f = Convert.ToDouble(Val.Trim());
            }
            catch (Exception ex) { Log.d(ex.Message); }
            return (f);
        }
        //-------------------------------------------------------------------------------------------------
        public static float ToFloat(String Val, float Default)
        {
            float f = Default;
            if (IsEmpty(Val)) return (f);
            try
            {
                f = (float)Convert.ToDouble(Val.Trim());
            }
            catch (Exception ex) { Log.d(ex.Message); }
            return (f);
        }
        //-------------------------------------------------------------------------------------------------
        public static int PassMs(DateTime dt)
        {
            TimeSpan ts = DateTime.Now - dt;
            return (Convert.ToInt32(ts.TotalMilliseconds));
        }
        //-------------------------------------------------------------------------------------------------
        public static double PassSecond(DateTime dt)
        {
            TimeSpan ts = DateTime.Now - dt;
            return ts.TotalSeconds;
            //return (Convert.ToInt32(ts.TotalSeconds));
        }
        //-------------------------------------------------------------------------------------------------
        public static void SetZoom(Size S)
        {  //主窗口的基础大小
            xScal = Convert.ToDouble(S.Width) / Convert.ToDouble(BaseWidth);
            yScal = Convert.ToDouble(S.Height) / Convert.ToDouble(BaseHeight);
        }
        //-------------------------------------------------------------------------------------------------
        public static int xZoom(object xVal)
        {
            Double Val = Convert.ToDouble(xVal);
            return (ToInt(xScal * Val));
        }
        //-------------------------------------------------------------------------------------------------
        public static int yZoom(object yVal)
        {
            Double Val = Convert.ToDouble(yVal);
            return (ToInt(yScal * Val));
        }
        //-------------------------------------------------------------------------------------------------
        /*public static Font GetFont(float FontSize)
        {
            //float FSize =(float)ToFloat(FontSize, SysFont.Size);
            if (FontSize <= 0) return (SysFont);
            return (new Font(SysFontName, FontSize, FontStyle.Regular));
        }*/
        //-------------------------------------------------------------------------------------------------
        public static String ShortDay(String vDateTime)
        {
            if (IsEmpty(vDateTime)) return null;
            DateTime dt = Convert.ToDateTime(vDateTime);
            return dt.ToString(SDT_Format);
        }
        //------------------------------------------------------%-------------------------------------------
        public static String ShortDT(String vDateTime)
        {
            //String Str = ShortDay(vDateTime);
            if (IsEmpty(vDateTime)) return null;
            DateTime dt = Convert.ToDateTime(vDateTime);
            return dt.ToString(SDT_Format) + " " + dt.ToString("HH:mm");
        }
        //-------------------------------------------------------------------------------------------------
        public static String DHour(DateTime dTime)
        {
            return dTime.ToString(SDT_Format) + " " + dTime.ToString("HH 时");
        }
        //-------------------------------------------------------------------------------------------------
        public static String DBTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        //-------------------------------------------------------------------------------------------------
        public static String BIRTH_OLD(String Birthday)
        {//根据日期计算年龄，不足1岁的显示月，不足1月的显示天
            DateTime BirTime;
            try
            {
                BirTime = Convert.ToDateTime(Birthday);
            }
            catch (Exception ex) { Log.d(ex.Message); return ""; }

            DateTime cTime = DateTime.Now;
            int Year = cTime.Year - BirTime.Year;
            if ((cTime.Month < BirTime.Month) || (cTime.Month == BirTime.Month && cTime.Day < BirTime.Day))
                Year--;

            //1岁及以上返回
            if (Year > 0)
                return (Year.ToString() + "岁");

            //0岁返回月数
            int Month = cTime.Month - BirTime.Month;
            if (cTime.Year > BirTime.Year)//去年生
                Month += 12;

            if (cTime.Day < BirTime.Day)
                Month--;
            if (Month > 3)//3个月以上返回月数
                return (Month.ToString() + "个月");

            //3个月以下，返回天数
            TimeSpan tSpan = cTime - BirTime;
            if (tSpan.Days > 0)
                return (tSpan.Days.ToString() + "天");

            return ("");
        }
        //-------------------------------------------------------------------------------------------------
        public static int ToInt(Double Val)
        {//浮点数转为整数，防止出现 5.9999998 被认为是 5 这样的情况
            return (Convert.ToInt32(Val + 1E-3));
        }
        //-------------------------------------------------------------------------------------------------
        public static Bitmap LoadImg(String ImgName)
        {   //读入显示的图片
            if (IsEmpty(ImgName)) return (null);
            String fName = IMG_DIR + ImgName;
            if (!File.Exists(fName)) return (null);
            return (new Bitmap(fName));
        }
        //-----------------------------------------------------------------------
        public static void Dispose(IDisposable Disp)
        {
            if (Disp == null) return;
            Disp.Dispose();
            Disp = null;
        }
        //-----------------------------------------------------------------------
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        public static Boolean IsEmpty(String Str)
        {
            if (Str == null) return true;
            if (Str.Trim() == "") return true;
            return false;
        }
        //-----------------------------------------------------------------------
        /// <summary>
        /// 忽略大小写，判断两字符串是否相等
        /// </summary>
        public static Boolean Same(String Str1, String Str2)
        {
            if (Str1 == null && Str2 == null) return true;
            if (Str1 == null) return false;
            if (Str2 == null) return false;
            Str1 = Str1.Trim().ToUpper();
            Str2 = Str2.Trim().ToUpper();
            return (Str1 == Str2);
        }
        //-----------------------------------------------------------------------
        public static String DT2Date(String Str)
        {
            if (IsEmpty(Str)) return null;
            DateTime dt = Convert.ToDateTime(Str);
            return dt.ToLongDateString();
        }
        //-----------------------------------------------------------------------
        public static Color DayColor(DateTime dt)
        {
            int wDay = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));
            int dWeek = (wDay) % 7;
            return (dWeek > 0 && dWeek < 6) ? ForeColor : Color.Maroon;
        }
        //-----------------------------------------------------------------------
        public static String WeekText(int WeekIndex)
        {
            switch (WeekIndex % 7)
            {
                case 0: return "日";
                case 1: return "一";
                case 2: return "二";
                case 3: return "三";
                case 4: return "四";
                case 5: return "五";
                case 6: return "六";
            }
            return null;
        }
        //------------------------------------------------------------------------
        /*public static String GetRegist(String Name)
        {
            RegistryKey Machine = Registry.LocalMachine;
            RegistryKey Software = Machine.OpenSubKey("SOFTWARE", true);
            RegistryKey Key = Software.OpenSubKey("Foxseer", true);
            if (Key == null) return (null);
            return Key.GetValue(Name).ToString();
        }
        //------------------------------------------------------------------------
        public static void SetRegist(String Name, String Value)
        {
            RegistryKey Machine = Registry.LocalMachine;
            RegistryKey Software = Machine.OpenSubKey("SOFTWARE", true);
            RegistryKey Key = Software.CreateSubKey("Foxseer");
            Key.SetValue(Name, Value);
        }
        //------------------------------------------------------------------------    
        public static Boolean MacMatch(String Mac)
        {
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in nis)
            {
                PhysicalAddress pa = ni.GetPhysicalAddress();
                if (Same(Mac, pa.ToString()))
                    return true;
            }
            return false;
        }*/
        //------------------------------------------------------------------------    
        public static String getRString(String RName)
        {
            //int resId = Res.getIdentifier(RName, "string", Owner.getPackageName());
            //return resId <= 0 ? "" : Res.getText(resId).toString();
            return "getRString 未定义";
        }
        //------------------------------------------------------------------------------
        public static int ByteToInt(Byte Val)
        {
            return Val <= 127 ? Val : (Val - 256);
        }
        //------------------------------------------------------------------------------

    }
}
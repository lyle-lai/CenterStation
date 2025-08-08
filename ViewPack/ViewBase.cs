using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalClass;
using System.Drawing;
//-----------------------------------------------------------------------
namespace ViewPack
{
//-----------------------------------------------------------------------
    public class ViewBase : Panel
    {
        public double Weight = 0;
        //------------------------------------------------------------------------------
        public ViewBase()
            : base()
        {
            //InitBaseSize();
        }
        //------------------------------------------------------------------------------
        //字体大小基准
        public static StringFormat FormatLeft = new StringFormat();
        public static StringFormat FormatRight = new StringFormat();
        public static StringFormat FormatCenter = new StringFormat();
        private static SizeF BaseSize;
        private static Image Img = new Bitmap(200, 100);
        private static Graphics DC = Graphics.FromImage(Img);
        public static void Initialize()
        {
            Font stringFont = new Font("Arial", 100);
            BaseSize = DC.MeasureString("88.8", stringFont);
            //居中对齐
            FormatCenter.Alignment = StringAlignment.Center;
            FormatCenter.LineAlignment = StringAlignment.Center;
            //居左对齐
            FormatLeft.Alignment = StringAlignment.Near;
            FormatLeft.LineAlignment = StringAlignment.Center;
            //居右对齐
            FormatRight.Alignment = StringAlignment.Far;
            FormatRight.LineAlignment = StringAlignment.Center;
        }
        //------------------------------------------------------------------------------
        public static int GetFontSize(int w, int h, String Text)
        {
            Font stringFont = new Font("Arial", 100);
            SizeF size = DC.MeasureString(Text, stringFont);

            Double xZoom = (Double)w / size.Width;
            Double yZoom = (Double)h / size.Height;
            return (int)(100 * Math.Min(xZoom, yZoom));
        }
        //------------------------------------------------------------------------------
        public static int GetFontSize(int w, int h)
        {
            Double xZoom = (Double)w / BaseSize.Width;
            Double yZoom = (Double)h / BaseSize.Height;
            return (int)(100 * Math.Min(xZoom, yZoom));
        }
        //------------------------------------------------------------------------------
        public virtual void Tick()
        {
        }
        //------------------------------------------------------------------------------
    }

//-----------------------------------------------------------------------
}

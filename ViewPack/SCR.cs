using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//------------------------------------------------------------------------------
using System.Windows.Forms;
using GlobalClass;
//------------------------------------------------------------------------------
namespace ViewPack
{
    //-------------------------------------------------------------------------------
    //显示屏的属性，及缩放
    //-------------------------------------------------------------------------------
    public class SCR
    {
        //-------------------------------------------------------------------------------
        //从配置文件获取的设置值，暂定义为常量
        private static int xPixRaw = 1024;	//原始定义的宽度(Pix)
        private static int yPixRaw = 600;	//原始定义的高度(Pix)
        //屏幕的实际尺寸(Pix)
        public static int xPix;	//屏幕的尺寸(Pix)
        public static int yPix;	//屏幕的尺寸(Pix)
        //屏幕的实际尺寸(mm)
        public static float xMm;	//屏幕的尺寸(mm)
        public static float yMm;	//屏幕的尺寸(mm)
        //缩放系数
        private static float xZoom = 1.0F;	//x方向缩放
        private static float yZoom = 1.0F;	//y方向缩放
        //英寸毫米换算
        private static float MPI = 25;	//每英寸25毫米
        //根据屏幕点间距 dp和px互转的比例 
        //private static float scale = 1.0F;
        //背光设置
        //private static WindowManager.LayoutParams LayPara = null;
        //private static Window Win = null;
        //-------------------------------------------------------------------------------
        public static void Initialize()
        {
            //背光设置
            //LayPara = MainActivity.getWindow().getAttributes();
            //Win = MainActivity.getWindow();
            //Win.addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);//保持屏幕背光

            //根据屏幕点间距 dp和px互转的比例 
            //scale = GLB.Res.getDisplayMetrics().density;
            //屏幕的宽度、高度(Pix)
            //Display display = MainActivity.getWindowManager().getDefaultDisplay();
            //xPix = 800;
            xPix = Screen.PrimaryScreen.Bounds.Width;
            yPix = Screen.PrimaryScreen.Bounds.Height;
            xZoom = (float)xPix / xPixRaw;
            yZoom = (float)yPix / yPixRaw;

            //Log.d("Fox","---------- Initialize Display xPix=" + String.valueOf(xPix) +
            //	",yPix=" + String.valueOf(yPix));
            //用户可配置屏幕参数
            float scrInch = 21;		//屏幕尺寸(对角线)
            float xRate = 16;		//屏幕的宽高比(16:9、4:3等)
            float yRate = 9;
            float dRate = (float)Math.Sqrt(xRate * xRate + yRate * yRate);
            //根据参数计算
            xMm = scrInch * MPI * xRate / dRate;
            yMm = scrInch * MPI * yRate / dRate;
            //
        }
        //-------------------------------------------------------------------------------
        public static int ZoomX(int Val)
        {
            return (int)(xZoom * Val + 1E-10);
        }
        //-----------------------------------------------------------------------
        public static int ZoomY(int Val)
        {
            return (int)(yZoom * Val + 1E-10);
        }
        //-----------------------------------------------------------------------
        //根据手机的分辨率从 dp 的单位 转成为 px(像素) 
        /*public static int dip2px(float dp)
        {
            return (int)(dp * scale + 0.5f);
        }
        //------------------------------------------------------------------------------
        public static float dip2pxF(float dp)
        {
            return (dp * scale);
        }
        //------------------------------------------------------------------------------
        //根据手机的分辨率从 px(像素) 的单位 转成为 dp 
        public static int px2dip(float pxValue)
        {
            return (int)(pxValue / scale + 0.5f);
        }*/
        //------------------------------------------------------------------------------
        //设置亮度
        /*public static void setBrightness(int brightness)
        {
            LayPara.screenBrightness = Float.valueOf(brightness) * (1f / 255f);
            Win.setAttributes(LayPara);
        }*/
        //------------------------------------------------------------------------------
        /*public static int get_px(AttributeSet attrs, String attName, int Default)
        {
            String str = attrs.getAttributeValue(null, attName);
            if (GLB.IsEmpty(str)) return Default;
            str = str.toLowerCase();
            //px为单位
            int idx = str.indexOf("px");
            if (idx >= 0)
            {
                try
                {
                    return Integer.valueOf(str.substring(0, idx));
                }
                catch (Exception e) { ;}
                return Default;
            }
            //dp、dip为单位(没单位也默认为dp),转换为px值返回
            idx = str.indexOf("dp");
            if (idx < 0)
                idx = str.indexOf("dip");
            if (idx < 0)
                idx = str.length();

            try
            {
                str = str.substring(0, idx);
                return dip2px(Float.valueOf(str.substring(0, idx)));
            }
            catch (Exception e) { ;}

            return Default;

            //int Value = attrs.getAttributeIntValue(null, attName, Default);
            //return dip2px(Value);
        }*/
        //------------------------------------------------------------------------------
    }
}

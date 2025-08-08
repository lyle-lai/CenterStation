using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//------------------------------------------------------------------------------
using GlobalClass;
//------------------------------------------------------------------------------
namespace ViewPack
{
    public class ScanLine
    {
        //------------------------------------------------------------------------------
        private static long STime = DateTime.Now.Ticks / 10000;
        private static float a = SCR.xPix / (1000 * SCR.xMm);
        public static float NowX(float Speed)
        {
            return Speed * a *(DateTime.Now.Ticks / 10000 - STime);
        }
        //------------------------------------------------------------------------------
        //实例化
        //-------------------------------------------------------------------------------
        private static Timer t = new Timer();           //扫描线作画时钟定义
        //public List<ScanLineItem> Lines = new List<ScanLineItem>();	//不同走速的扫描线(列表)
        //private Boolean Started = false;
        //事件定义(开始扫描)
        //public delegate void OnStart();
        //public OnStart Start = null;
        //推入数据点
        public delegate void OnPushData();
        public OnPushData PushData = null;
        //作画
        public delegate void OnDarw();
        public OnDarw Draw = null;
        //-------------------------------------------------------------------------------
        public ScanLine(float Speed)
        {
            t.Tick += timerTick;
            t.Interval = 15;
            t.Enabled = true;
            //
            /*Lines.Add(new ScanLineItem(6.25F));
            Lines.Add(new ScanLineItem(12.5F));
            Lines.Add(new ScanLineItem(25));
            Lines.Add(new ScanLineItem(50));*/
        }
        //-------------------------------------------------------------------------------
        //扫描线作画时钟定义
        private void timerTick(object sender, EventArgs e)
        {
            //移动扫描线
            //for (int i = 0; i < Lines.Count; i++)
               // Lines[i].LineTimer();

            //准备缓冲数据
            PushData();
            //各波形作画(如果可见)
            Draw();
        }
        //------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------
    //扫描线
    //------------------------------------------------------------------------------
    public class ScanLineItem
    {
        private static long STime = DateTime.Now.Ticks / 10000;
        private static long NowMs()
        {
            return DateTime.Now.Ticks / 10000 - STime;
        }
        //-----------------------------------------
        public float ePos = 0;		//本次作画的x结束
        public float Speed;			//走速
        public float PPMS;			//每毫秒点数
        //-------------------------------------------------------------------------------
        public ScanLineItem(float Speed)
        {
            this.Speed = Speed;
            PPMS = (Speed * SCR.xPix) / (1000 * SCR.xMm);	//每毫秒的点数
        }
        //-------------------------------------------------------------------------------
        public void LineTimer()
        {
            ePos = PPMS * NowMs();
        }
        //-------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------

}
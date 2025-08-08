using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
//-----------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
//-----------------------------------------------------------------------
namespace ViewPack
{
    //-----------------------------------------------------------------------
    public class ViewWaveItem : ViewBase
    {
        //------------------------------------------------------------------------------
        public WaveItem waveItem = null;
        private Pen dPen = null;
        protected SolidBrush dBrush = null;
        private SolidBrush brushClear = new SolidBrush(Color.Black);
        //背景
        private Bitmap BackBmp = null;
        private Graphics BackDC = null;
        //缓冲
        private Bitmap BufBmp = null;
        private Graphics BufDC = null;
        //
        public PointF[] DrawPoints = new PointF[100];//作画点
        private int LenDP = 0;
        private Graphics DC = null;
        private float dY = 0;
        //
        private PointF DirPoint = new PointF(0, 0);
        //------------------------------------------------------------------------------
        public ViewWaveItem(WaveItem waveItem, SolidBrush dBrush)
        {
            this.waveItem = waveItem;
            this.dBrush = dBrush;
            Weight = 1;
            dPen = new Pen(dBrush, 2.0F);
        }
        //------------------------------------------------------------------------------
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            InitSize();
        }
        //------------------------------------------------------------------------------
        private void InitSize()
        {
            DC = this.CreateGraphics();
            //DC.SmoothingMode = SmoothingMode.HighQuality;
            //
            BackBmp = new Bitmap(this.Width, this.Height);
            BackDC = Graphics.FromImage(BackBmp);
            //BackDC.FillRectangle(brushClear, this.Bounds);
           // BackDC.DrawString(waveItem.GetName(), GLB.SysFont, dBrush, 0, 0);
            BackDC.DrawString(waveItem.GetName(), GLB.SysFont, dBrush, 0, 0);
            //BackDC.DrawRectangle(new Pen(Color.Red, 1), this.ClientRectangle);
            //
            BufBmp = new Bitmap(this.Width, this.Height);
            BufDC = Graphics.FromImage(BufBmp);
            BufDC.SmoothingMode = SmoothingMode.HighQuality;
            //
            DC.DrawImage(BackBmp, 0, 0);
            //重新布局
            dY = (float)(this.Height - dPen.Width) / 255;
            //dY = (float)(this.Height - dPen.Width) / 2;
        }
        //------------------------------------------------------------------------------
        public void SetData(float scanX, float Y)
        {
            //Log.d("SetData " + waveItem.SN);
            //int y = Y - 128;
            float y = Y <= 127.0f ? Y : (Y - 256.0f);
            float ny = Math.Abs(Y) * ((this.Height - dPen.Width) / 2);
            /*int y = Y;
            if (Y > 127)
            {
                y = y - 256;
            }*/

            if (LenDP >= DrawPoints.Length)
            {
                LenDP = 0;
                return;
            }
            DrawPoints[LenDP].X = scanX % this.Width;
            //DrawPoints[LenDP].Y = y * dY;
            DrawPoints[LenDP].Y = (128 - y) * dY;
            //DrawPoints[LenDP].Y = (Y < 0) ? dY + ny : dY - ny;
            //DrawPoints[LenDP].Y = y * dY;
            //DrawPoints[LenDP].Y = (255 - Y) * dY;
            LenDP++;
        }
        //-------------------------------------------------------------------------------
        private Boolean IsVisible()
        {
            return (this.Visible && this.Width > 10);
        }
        //-------------------------------------------------------------------------------
        public void Draw()
        {
            if (!IsVisible()) return;
            //if (!(this.Visible && Parent.Visible && Parent.Parent.Visible)) return;	//如果不可见，则不作画
            if (LenDP <= 1) return;

            try
            {
                //清屏
                RectangleF RectLeft = new RectangleF(0, 0, 10, this.Height);
                RectangleF RectClear = new RectangleF(DrawPoints[0].X + 1, 0, DrawPoints[LenDP - 1].X - DrawPoints[0].X + 5, this.Height);
                //DC.FillRectangle(brushClear, Rect);
                //DC.DrawImage(BackBmp, Rect, Rect, GraphicsUnit.Pixel);
                BufDC.FillRectangle(brushClear, RectClear);
                BufDC.DrawImage(BackBmp, RectClear, RectClear, GraphicsUnit.Pixel);


                float sx = DrawPoints[0].X;
                float ex = 0;
                RectangleF RectDraw = new RectangleF(sx, 0, 0, this.Height);
                //作画
                for (int i = 0; i < LenDP - 1; i++)
                {
                    ex = DrawPoints[i + 1].X;

                    //if (GLB.Same(waveItem.SN, "RESP"))
                    //    Log.d("Draw " + waveItem.SN + " x=" + DrawPoints[i].X + ",y=" + DrawPoints[i].Y);
                    if (DrawPoints[i].X < DrawPoints[i + 1].X)
                        BufDC.DrawLine(dPen, DrawPoints[i], DrawPoints[i + 1]);
                    else
                    {
                        //RectDraw.Width = ex - sx;
                        BufDC.FillRectangle(brushClear, Bounds);
                        BufDC.DrawImage(BackBmp, Bounds, Bounds, GraphicsUnit.Pixel);
                        DC.DrawImage(BufBmp, RectLeft, RectLeft, GraphicsUnit.Pixel);
                        //BufDC.FillRectangle(brushClear, RectClear);
                        //BufDC.DrawImage(BackBmp, R, R, GraphicsUnit.Pixel);
                        sx = 0;
                    }
                }

                RectDraw.Width = ex - sx + 5;
                //RectangleF RectDraw = new RectangleF(sx, 0, DrawPoints[i].X - sx, this.Height);
                DC.DrawImage(BufBmp, RectDraw, RectDraw, GraphicsUnit.Pixel);

                //整理未点
                DrawPoints[0].X = DrawPoints[LenDP - 1].X;
                DrawPoints[0].Y = DrawPoints[LenDP - 1].Y;
                LenDP = 1;
            }
            catch (Exception e) {Log.d(e.Message) ;}
        }
        //------------------------------------------------------------------------------
        public void Clean()
        {
            DC.Clear(Color.Black);
            //DC.DrawImage(BackBmp, Bounds, Bounds, GraphicsUnit.Pixel);
        }
        //------------------------------------------------------------------------------
        public void SetDataDirect(float DirX, float Val)
        {
            //float Y = (128 - Val) * dY;
            float y = Val <= 127.0f ? Val : (Val - 256.0f);
            float Y = (128 - y) * dY;
            BufDC.DrawLine(dPen, DirPoint.X, DirPoint.Y, DirX, Y);
            DirPoint.X = DirX;
            DirPoint.Y = Y;
        }
        //------------------------------------------------------------------------------
        public void DrawDirect()
        {
            Rectangle Rect = this.ClientRectangle;// new Rectangle(0, 0, this.Width, this.Height);

            DC = this.CreateGraphics();
            DC.DrawImage(BufBmp, Rect, Rect, GraphicsUnit.Pixel);
        }
        //------------------------------------------------------------------------------
        public void StartDirect()
        {
            DirPoint.X = 0;
            DirPoint.Y = 0;
            BufDC.FillRectangle(brushClear, this.ClientRectangle);
            BufDC.DrawImage(BackBmp, this.ClientRectangle, this.ClientRectangle, GraphicsUnit.Pixel);
        }
        //------------------------------------------------------------------------------

    }
    //-----------------------------------------------------------------------
}
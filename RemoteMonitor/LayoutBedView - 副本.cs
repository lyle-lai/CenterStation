using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GlobalClass;
using ViewPack;
//---------------------------------------------------------------------------------------------------
namespace RemoteMonitor
{
    //---------------------------------------------------------------------------------------------------
    class LayoutBedView
    {
        private static ViewMonitor[] VM = null;
        private static Panel Parent = null;
        //---------------------------------------------------------------------
        public static void Initialize(Panel Pan)
        {//构造函数
            Parent = Pan;
            Create(Config.Rows, Config.Cols);
            Layout(Config.Rows, Config.Cols);
        }
        //---------------------------------------------------------------------
        public static void Create(int Rows, int Cols)
        {
            Parent.Controls.Clear();
            //初始化各窗口对象
            int Count = Rows * Cols;
            VM = new ViewMonitor[Count];
            for (int i = 0; i < Count; i++)
            {
                VM[i] = new ViewMonitor(i);
                VM[i].Parent = Parent;
                VM[i].Visible = true;
                Parent.Controls.Add(VM[i]);
            }
        }
        //---------------------------------------------------------------------
        public static void Layout(int Rows, int Cols)
        {
            if (Parent == null) return;
            //按照行列布局
            int y = 0;
            for (int r = 0; r < Rows; r++)
            {
                int h = (Parent.ClientSize.Height - y) / (Rows - r);
                int x = 0;
                for (int c = 0; c < Cols; c++)
                {
                    int i = r * Cols + c;
                    int w = (Parent.ClientSize.Width - x) / (Cols - c);
                    VM[i].Width = w;
                    VM[i].Height = h;
                    VM[i].Top = y;
                    VM[i].Left = x;
                    VM[i].LayoutView();
                    x += w;
                }
                y += h;
            }
        }
        //-----------------------------------------------------------------------
        public static void ParaTick()
        {
            if (VM == null) return;
            for (int i = 0; i < VM.Length; i++)
                VM[i].ParaTick();
        }
        //-----------------------------------------------------------------------
        public static void WaveTick()
        {
            if (VM == null) return;
            for (int i = 0; i < VM.Length; i++)
                VM[i].WaveTick();
        }
        //-----------------------------------------------------------------------
        public static void DemoTick()
        {
            if (VM == null) return;
            for (int i = 0; i < VM.Length; i++)
                VM[i].DemoTick();
        }
        //-----------------------------------------------------------------------
    }
    //---------------------------------------------------------------------------------------------------
}
//---------------------------------------------------------------------------------------------------

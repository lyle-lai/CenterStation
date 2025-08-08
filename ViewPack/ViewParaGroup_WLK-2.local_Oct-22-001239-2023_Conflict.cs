using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
//-----------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
using System.Runtime.InteropServices;
//-----------------------------------------------------------------------
namespace ViewPack
{
    //-----------------------------------------------------------------------
    public class ViewParaGroup : ViewBase
    {
        //------------------------------------------------------------------------------
        public static ViewParaGroup[] Items = null;
        /*public static void Initialize()
        {
            Items = new ViewParaGroup[ParaGroup.Items.Count];
            for (int i = 0; i < ParaGroup.Items.Count; i++)
            {
                ViewParaGroup v = new ViewParaGroup(ParaGroup.Items[i]);
                Items[i] = v;
                v.Visible = false;
            }
        }*/
        //------------------------------------------------------------------------------
        public static ViewParaGroup Find(String GroupName)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (GLB.Same(Items[i].ParaGrp.FullSN, GroupName)) return Items[i];
            }
            return null;
        }
        //------------------------------------------------------------------------------
        private ParaGroup ParaGrp = null;
        private ParaItem[] ParaList = null;
        // 
        private Rectangle RectTitle;
        private Rectangle RectLow;
        private Rectangle RectHigh;
        private Rectangle RectVal;
        //private Rectangle[] RectValue;
        //
        private int BorderHeight = 13;
        private int BorderWidth = 40;
        //protected Font CFont = new Font("Arial", 20);
        //       
        protected SolidBrush norBrush = null;
        protected SolidBrush alaBrush = null;
        //
        private Bitmap Bmp = null;
        private Graphics BmpDC = null;
        private Graphics DC = null;
        //
        private List<ViewParaItem> vpiList = new List<ViewParaItem>();
        //------------------------------------------------------------------------------
        public ViewParaGroup(ParaGroup ParameterGroup, String LayoutXml)
        {
            InitSize();
            ParaGrp = ParameterGroup;
            if (ParaGrp == null) return;
            //
            norBrush = new SolidBrush(ParaGrp.dColor);
            // 根据南方医院的需求，告警用户红色显示值
            // alaBrush = new SolidBrush(Alarm.getAlarmColor(ParaGrp.dColor));
            alaBrush = new SolidBrush(Color.Red);
            ParaList = new ParaItem[ParaGrp.Childs.Count];
            //
            for (int i = 0; i < ParaGrp.Childs.Count; i++)
                ParaList[i] = (ParaItem)ParaGrp.Childs[i];
            //参数值显示区布局
            XmlDoc Doc = new XmlDoc(LayoutXml);
            Xml_Node xNode = Doc.getRoot();
            xNode = xNode.FirstChild();

            while (xNode != null)
            {
                String Text = xNode.getStr("Text");
                ViewParaItem vpi = null;
                if (Text.Length > 2 && GLB.Same(Text.Substring(0, 2), "@P"))
                {
                    int i = GLB.ToInt(Text.Substring(2), 0);
                    vpi = new ViewParaItem(xNode, ParaList[i]);
                    //vpiList.Add(new ViewParaItem(xNode, ParaList[i]));
                }
                else
                    vpi = new ViewParaItem(xNode, Text);
                    //vpiList.Add(new ViewParaItem(xNode, Text));
                vpi.alaBrush = alaBrush;
                vpi.norBrush = norBrush;
                vpiList.Add(vpi);
                xNode = xNode.Next();
            }
            InitSize();
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
            Bmp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            BmpDC = Graphics.FromImage(Bmp);
            BmpDC.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            DC = this.CreateGraphics();

            //重新布局
            RectTitle = new Rectangle(0, 0, this.Width, BorderHeight);
            RectHigh = new Rectangle(0, BorderHeight, BorderWidth, BorderWidth);
            RectLow = new Rectangle(0, 2 * BorderHeight, BorderWidth, BorderWidth);
            RectVal = new Rectangle(0, BorderHeight-5, this.Width, this.Height - BorderHeight+10);
            //
            for (int i = 0; i < vpiList.Count; i++)
                vpiList[i].SetRecgt(RectVal);
            //CFont = new Font("Arial", GetFontSize(RectVal.Width, RectVal.Height));
        }
        //------------------------------------------------------------------------------
        public override void Tick()
        {
            Pen p = new Pen(Color.Red);
            if (ParaGrp == null) return;
            //ParaItem Para = (ParaItem)ParaGrp.Childs[0];
            //Log.d(ParaGrp.FullSN + ":" + Para.Value.Value);
            //
            BmpDC.Clear(Color.Black);
            //BmpDC.DrawRectangle(new Pen(Color.Red, 1), this.ClientRectangle);
            //
           // BmpDC.DrawString(ParaGrp.GetName(), GLB.SysFont, norBrush, RectTitle, FormatLeft);
            BmpDC.DrawString(ParaGrp.GetENName(), GLB.SysFont, norBrush, RectTitle, FormatLeft);
            
            //显示报警限
            //BmpDC.DrawString(ParaList[0].GetHigh(), GLB.SysFont, norBrush, RectHigh);
            //BmpDC.DrawString(ParaList[0].GetLow(), GLB.SysFont, norBrush, RectLow);
            //
            string alaStr = string.Empty;
            for (int i = 0; i < vpiList.Count; i++)
            {
                if (vpiList[i].GetParaItem() != null)
                {
                    if (vpiList[i].GetParaItem().alaMode == Alarm.Mode.ToLow)
                    {
                        alaStr += vpiList[i].GetParaItem().GetName() + "过低 ";
                    }
                    else if (vpiList[i].GetParaItem().alaMode == Alarm.Mode.ToHigh)
                    {
                        alaStr += vpiList[i].GetParaItem().GetName() + "过高 ";
                    }
                }
                vpiList[i].Draw(BmpDC);
            }
            
            // 告警信息
            if (!string.IsNullOrEmpty(alaStr))
            {
                BmpDC.DrawString(alaStr + " " + ParaGrp.GetUnitName(), GLB.SysFont, new SolidBrush(Color.Red), RectTitle, FormatRight);
            }
            else
            {
                BmpDC.DrawString(ParaGrp.GetUnitName(), GLB.SysFont, norBrush, RectTitle, FormatRight);
            }
            
            //DrawPara(ParaList[0]);
            //
            DC.DrawImage(Bmp, 0, 0);
        }
        //------------------------------------------------------------------------------
        /*private void DrawPara(ParaItem Para)
        {
            //BmpDC.DrawString(
            if (Alarm.isTwink && Para.alaMode != Alarm.Mode.Normal)
                BmpDC.DrawString(GetParaListString(), CFont, alaBrush, RectVal, FormatCenter);
                //BmpDC.DrawString(Para.GetValue(), CFont, alaBrush, RectVal, FormatCenter);
            else
                BmpDC.DrawString(GetParaListString(), CFont, norBrush, RectVal, FormatCenter);
            //BmpDC.DrawString(Para.GetValue(), CFont, norBrush, RectVal, FormatCenter);
        }
        //------------------------------------------------------------------------------
        private String GetParaListString()
        {
            switch (ParaList.Length)
            {
                case 1: return ParaList[0].GetValue();
                case 2: return ParaList[0].GetValue() + "  " + ParaList[1].GetValue();
                case 3: return ParaList[0].GetValue() + "/" + ParaList[1].GetValue() + "(" + ParaList[2].GetValue() + ")";
                default: return "";
            }
        }*/
        //------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------
}
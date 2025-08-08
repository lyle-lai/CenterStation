using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//------------------------------------------------------------------------------
using System.Drawing;
using GlobalClass;
using MGOA_Pack;
//--------------------------------------
namespace ViewPack
{
    //------------------------------------------------------------------------------
    public class ViewParaItem
    {
        private Rectangle Rect;
        private String Text = null;
        private ParaItem paraItem = null;
        private Xml_Node xNode;
        private Font font = new Font("Arial", 20);
        public SolidBrush norBrush = null;
        public SolidBrush alaBrush = null;
        private StringFormat Align = ViewBase.FormatCenter;
        //------------------------------------------------------------------------------
        public ViewParaItem(Xml_Node xNode, String Text)
        {
            this.xNode = xNode;
            this.Text = Text;
            Align = GetAlign(xNode.getStr("Align"));
        }
        //------------------------------------------------------------------------------
        public ViewParaItem(Xml_Node xNode, ParaItem paraItem)
        {
            this.xNode = xNode;
            this.paraItem = paraItem;
            Align = GetAlign(xNode.getStr("Align"));
        }
        //------------------------------------------------------------------------------
        public StringFormat GetAlign(String AlignStr)
        {
            if (GLB.Same(AlignStr, "Left")) return ViewBase.FormatLeft;
            if (GLB.Same(AlignStr, "Right")) return ViewBase.FormatRight;
            return ViewBase.FormatCenter;
        }
        //------------------------------------------------------------------------------
        public void SetRecgt(Rectangle ParentRect)
        {
            Rect.X = ParentRect.Left+ xNode.getInt("Left", 0) * ParentRect.Width / 100;
            Rect.Y = ParentRect.Top + xNode.getInt("Top", 0) * ParentRect.Height / 100;
            Rect.Width = xNode.getInt("Width", 0) * ParentRect.Width / 100;
            Rect.Height = xNode.getInt("Height", 0) * ParentRect.Height / 100;

            if (Rect.Height == 0) Rect.Height = 100;
            if (paraItem == null && !GLB.IsEmpty(Text))
            {
                int fontSize = ViewBase.GetFontSize(Rect.Width, Rect.Height, Text);
                font = new Font("Arial", fontSize);
            }
            else
            {
                int fontSize = ViewBase.GetFontSize(Rect.Width, Rect.Height, "88.8");
                font = new Font("Arial", fontSize);
            }
        }
        //------------------------------------------------------------------------------
        public void Draw(Graphics DC)
        {
            if (paraItem == null)
                DC.DrawString(Text, font, norBrush, Rect, ViewBase.FormatCenter);
            else
            {
                if (Alarm.isTwink && paraItem.alaMode != Alarm.Mode.Normal)
                {
                    DC.DrawString(paraItem.GetValue(), font, alaBrush, Rect, Align);
                }
                else
                {
                    DC.DrawString(paraItem.GetValue(), font, norBrush, Rect, Align);
                }
            }
            //DC.DrawRectangle(new Pen(norBrush), Rect);
        }

        public ParaItem GetParaItem()
        {
            return this.paraItem;
        }
        //------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------
}
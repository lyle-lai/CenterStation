using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace GlobalClass
{
    public class Xml_Node
    {
        private XmlNode Node = null;
        //-----------------------------------------------------------------------
        public Xml_Node(XmlNode Node)
        {
            this.Node = Node;
        }
        //-----------------------------------------------------------------------
        //****** 返回节点的子、或同级节点 ******
        public Xml_Node FirstChild()
        {//返回第一个子节点
            if (Node == null) return null;
            XmlNode n = Node.FirstChild;
            //非有效节点(如注释)，则跳转到同级下一个节点，直至有效节点
            while (n != null && n.NodeType != XmlNodeType.Element)
                n = n.NextSibling;
            return (n == null) ? null : new Xml_Node(n);
        }
        //-----------------------------------------
        public Xml_Node Next()
        {//返回同级的下一个节点
            if (Node == null) return null;
            XmlNode n = Node.NextSibling;
            //非有效节点(如注释)，则继续跳转，直至有效节点
            while (n != null && n.NodeType != XmlNodeType.Element)
                n = n.NextSibling;
            return (n == null) ? null : new Xml_Node(n);
        }
        //-----------------------------------------------------------------------
        public String getName()
        {
            if (Node == null) return "";
            return Node.Name;
        }
        //-----------------------------------------------------------------------
        //****** 返回属性的字符串类型 ******
        public String getStr(String AttName)
        {
            return getStr(AttName, "");
        }
        //------------------------------------------
        public String getStr(String AttName, String Default)
        {
            if (Node == null) return Default;
            if (Node.Attributes[AttName] == null) return (Default);
            return Node.Attributes[AttName].Value;
        }
        //------------------------------------------
        //****** 返回属性的整数类型 ******
        public int getInt(String AttName, int Default)
        {
            if (Node == null) return Default;
            if (Node.Attributes[AttName] == null) return (Default);
            return (GLB.ToInt(Node.Attributes[AttName].Value, Default));
        }
        //------------------------------------------
        //****** 返回属性的短整数类型 ******
        public short getShort(String AttName, short Default)
        {
            if (Node == null) return Default;
            if (Node.Attributes[AttName] == null) return (Default);
            return (GLB.ToShort(Node.Attributes[AttName].Value, Default));
        }
        //------------------------------------------
        //****** 返回属性的浮点数类型 ******
        public float getFloat(String AttName, float Default)
        {
            if (Node.Attributes[AttName] == null) return (0);
            return (float)(Convert.ToDouble(Node.Attributes[AttName].Value));
        }
        //------------------------------------------
        //****** 返回属性的boolean类型 ******
        public Boolean getBool(String AttName, Boolean Default)
        {
            if (Node == null) return (Default);
            if (Node.Attributes[AttName] == null) return (Default);
            return (Convert.ToBoolean(Node.Attributes[AttName].Value));
        }
        //-------------------------------------------------------------------------------
        /*public Rect getRect()
        {//返回显示区域的四至
            Rect rect = new Rect();
            rect.left = Screen.ZoomX(getInt("Left", 0));
            rect.top = Screen.ZoomY(getInt("Top", 0));
            rect.right = rect.left + Screen.ZoomX(getInt("Width", 0));
            rect.bottom = rect.top + Screen.ZoomY(getInt("Height", 0));
            //Log.d("Fox", getStr("SN") + "=" + String.valueOf(rect.left)+":"+String.valueOf(rect.top)+":"+
            //String.valueOf(rect.right)+":"+String.valueOf(rect.bottom));
            return rect;
        }*/
        //-------------------------------------------------------------------------------
        public DockStyle getDock()
        {
            return getDock("Dock");
        }
        //-------------------------------------------------------------------------------
        public DockStyle getDock(String AttName)
        {//返回节点的对齐方式
            String dStr = getStr(AttName, "None");
            if (GLB.Same(dStr, "None")) return DockStyle.None;
            if (GLB.Same(dStr, "Left")) return DockStyle.Left;
            if (GLB.Same(dStr, "Top")) return DockStyle.Top;
            if (GLB.Same(dStr, "Right")) return DockStyle.Right;
            if (GLB.Same(dStr, "Bottom")) return DockStyle.Bottom;
            if (GLB.Same(dStr, "Fill")) return DockStyle.Fill;
            return DockStyle.None;
        }
        //-------------------------------------------------------------------------------
        /*public int getOrientation()
        {//获取排列方向
            String Ori = getStr("Ori", "Ver").toLowerCase();
            return Ori.equals("hor") ? LinearLayout.HORIZONTAL : LinearLayout.VERTICAL;
        }
        //-------------------------------------------------------------------------------
        */
    }
    //-----------------------------------------------------------------------
}

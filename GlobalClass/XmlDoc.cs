using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace GlobalClass
{
    public class XmlDoc
    {
        public XmlDocument Xml = new XmlDocument();
        //private String FileName = "";
        private static DBReader dr = new DBReader(DBConnect.SYS);
        //---------------------------------------------------------------------------------------------------
        public XmlDoc(String XmlOrFileName)
        {//构造函数
            Xml.LoadXml(XmlOrFileName);
            /*
            if (GLB.IsEmpty(Config.XmlPath))
                Xml.LoadXml(XmlOrFileName);
            else            //从文件读入
                Xml.Load(GLB.EXE_DIR + Config.XmlPath + "\\" + XmlOrFileName);*/
        }
        //-------------------------------------------------------------------------------------------------
        /*public static XmlDocument FromFile(String XmlFileName)
        {
            return Xml.Load(GLB.EXE_DIR +"XML\\"+ XmlFileName);
        }*/
        //-------------------------------------------------------------------------------------------------
        /*private XmlNodeReader GetReader(String NodePath)
        {
            NodePath = FormatPath(NodePath);

            XmlNode Node = Xml.SelectSingleNode(NodePath);
            if (Node == null)
            {
                MessageBox.Show("null(" + FileName + "):" + NodePath);
                return (null);
            }
            XmlNodeReader Reader = new XmlNodeReader(Node);
            if (Reader == null || Reader.EOF || Reader.IsEmptyElement)
            {
                MessageBox.Show("null(" + FileName + "):" + NodePath);
                return (null);
            }

            Reader.Read();
            return (Reader);
        }*/
        //-------------------------------------------------------------------------------------------------
        public Xml_Node getRoot()
        {
            return new Xml_Node(FirstValidNode());
        }
        //-------------------------------------------------------------------------------------------------
        public XmlNode FirstValidNode()
        {//查找第一个有效的节点，可以是Area、Module、Repeat
            XmlNode Node = Xml.FirstChild;
            while (Node != null)
            {
                if (IsValid(Node))
                    return Node;
                Node = Node.NextSibling;
            }
            return null;
        }
        //-------------------------------------------------------------------------------------------------
        private Boolean IsValid(XmlNode Node)
        {
            if (GLB.Same(Node.Name, "#Comment") || GLB.Same(Node.Name, "xml")) return false;
            //已知项
            /*
            if (GLB.Same(Node.Name, "Area") ||
                GLB.Same(Node.Name, "Repeat") ||
                GLB.Same(Node.Name, "WorkArea"))
                return true;
            //检查Module
            return !FindXml(Node.Name + ".Xml");*/
            return true;
            /*
            String ModuName = Node.Name + ".Xml";
            if (!File.Exists(GLB.EXE_DIR + ModuName)) return true;
            return false;
             */
        }
        //-------------------------------------------------------------------------------------------------
        public static Boolean FindXml(String XmlName)
        {
            if (GLB.IsEmpty(Config.XmlPath))
            {//从数据库生成
                dr.First();
                while (dr.Read())
                {
                    if (GLB.Same(XmlName, dr.GetStr("Name")))
                        return true;
                }
                return false;
            }
            else//从文件生成
                return File.Exists(GLB.EXE_DIR + Config.XmlPath + "\\" + XmlName);
        }
        //-------------------------------------------------------------------------------------------------
        public XmlNode GetNode(String NodePath)
        {
            NodePath = FormatPath(NodePath);
            return (Xml.SelectSingleNode(NodePath));
        }
        //-------------------------------------------------------------------------------------------------
        public XmlNodeList GetList(String NodePath)
        {
            NodePath = FormatPath(NodePath);
            return (Xml.SelectNodes(NodePath));
        }
        //-------------------------------------------------------------------------------------------------
        /*public String GetAtt(String NodePath, String AttName)
        {
            return (GetReader(NodePath).GetAttribute(AttName));
        }*/
        //-------------------------------------------------------------------------------------------------
        /*public int GetInt(String NodePath)
        {
            String Str = GetStr(NodePath);
            if (Str.Substring(0, 2).ToLower() == "0x")
                return (Convert.ToInt32(Str, 16));
            else
                return (Convert.ToInt32(Str, 10));
            //return (GetReader(NodePath).ReadElementContentAsInt());
        }*/
        //-------------------------------------------------------------------------------------------------
        /*public String GetStr(String NodePath)
        {
            return (GetReader(NodePath).ReadElementContentAsString());
        }*/
        //-------------------------------------------------------------------------------------------------
        /*public Color GetColor(String NodePath)
        {
            Color clr = Color.FromArgb(GetInt(NodePath));
            return (Color.FromArgb(clr.R, clr.G, clr.B));
        }*/
        //-------------------------------------------------------------------------------------------------
        public static Color GetAttColor(XmlNode Node, String Att, Color Default)
        {
            if (Node.Attributes[Att] == null) return (Default);

            int cInt = GLB.ToInt(Node.Attributes[Att].Value, -1);
            if (cInt < 0) return Default;

            Color clr = Color.FromArgb(cInt);
            return (Color.FromArgb(clr.R, clr.G, clr.B));
        }
        //-------------------------------------------------------------------------------------------------
        public static int GetAttInt(XmlNode Node, String Att, int Default)
        {
            if (Node == null) return Default;
            if (Node.Attributes[Att] == null) return (Default);
            return (GLB.ToInt(Node.Attributes[Att].Value, Default));
        }
        //-------------------------------------------------------------------------------------------------
        public static int xZoomAtt(XmlNode Node, String Att, int Default)
        {
            return (GLB.xZoom(GetAttInt(Node, Att, Default)));
        }
        //-------------------------------------------------------------------------------------------------
        public static int yZoomAtt(XmlNode Node, String Att, int Default)
        {
            return (GLB.yZoom(GetAttInt(Node, Att, Default)));
        }
        //-------------------------------------------------------------------------------------------------
        public static bool GetAttBool(XmlNode Node, String Att, bool Default)
        {
            if (Node == null) return (Default);
            if (Node.Attributes[Att] == null) return (Default);
            return (Convert.ToBoolean(Node.Attributes[Att].Value));
        }
        //-------------------------------------------------------------------------------------------------
        public static Double GetAttFloat(XmlNode Node, String Att)
        {
            if (Node.Attributes[Att] == null) return (0);
            //            if (Node.Attributes[Att] == null)
            //                MessageBox.Show("null Att (" + Node.Name + "):" + Att);
            return (Convert.ToDouble(Node.Attributes[Att].Value));
        }
        //-------------------------------------------------------------------------------------------------
        public static DockStyle GetDock(XmlNode Node, String Att)
        {
            String dStr = GetAttStr(Node, Att, "None");
            if (GLB.Same(dStr, "None")) return DockStyle.None;
            if (GLB.Same(dStr, "Left")) return DockStyle.Left;
            if (GLB.Same(dStr, "Top")) return DockStyle.Top;
            if (GLB.Same(dStr, "Right")) return DockStyle.Right;
            if (GLB.Same(dStr, "Bottom")) return DockStyle.Bottom;
            if (GLB.Same(dStr, "Fill")) return DockStyle.Fill;
            return DockStyle.None;
        }
        //-------------------------------------------------------------------------------------------------
        public static int xAlign(XmlNode Node, String Att)
        {
            String dStr = GetAttStr(Node, Att, "Center");
            if (GLB.Same(dStr, "Left")) return 0;
            if (GLB.Same(dStr, "Center")) return 1;
            if (GLB.Same(dStr, "Right")) return 2;
            return 1;
        }
        //-------------------------------------------------------------------------------------------------
        public static int yAlign(XmlNode Node, String Att)
        {
            String dStr = GetAttStr(Node, Att, "Center");
            if (GLB.Same(dStr, "Top")) return 0;
            if (GLB.Same(dStr, "Center")) return 1;
            if (GLB.Same(dStr, "Bottom")) return 2;
            return 1;
        }
        //-------------------------------------------------------------------------------------------------
        /*public static FillMode GetFillMode(XmlNode Node, String Att)
        {
            String dStr = GetAttStr(Node, Att, "Scale");
            if (GLB.Same(dStr, "None")) return FillMode.None;
            if (GLB.Same(dStr, "Scale")) return FillMode.Scale;
            if (GLB.Same(dStr, "Fill")) return FillMode.Fill;
            return FillMode.Scale;

        }
        //-------------------------------------------------------------------------------------------------
        public static TextStyle GetStyle(XmlNode Node, String Att)
        {
            String dStr = GetAttStr(Node, Att, "None");
            if (GLB.Same(dStr, "None")) return TextStyle.None;
            if (GLB.Same(dStr, "Engrave")) return TextStyle.Engrave;
            return TextStyle.None;
        }
        //-------------------------------------------------------------------------------------------------
        public static DataType GetDataType(XmlNode Node, String Att)
        {
            String dStr = GetAttStr(Node, Att, "Variable");
            if (GLB.Same(dStr, "Field")) return DataType.Field;
            if (GLB.Same(dStr, "Function")) return DataType.Function;
            if (GLB.Same(dStr, "Variable")) return DataType.Variable;
            return DataType.Variable;
        }
        //---------------------------------------------------------------------------
        public static Font GetFont(XmlNode Node, String Att)
        {
            String dStr = GetAttStr(Node, Att, "Normal");
            if (GLB.Same(dStr, "Normal")) return FontFromSize((int)FontSize.Normal);
            if (GLB.Same(dStr, "Big")) return FontFromSize((int)FontSize.Big);
            if (GLB.Same(dStr, "Small")) return FontFromSize((int)FontSize.Small);
            if (GLB.Same(dStr, "Large")) return FontFromSize((int)FontSize.Large);
            if (GLB.Same(dStr, "Tiny")) return FontFromSize((int)FontSize.Tiny);

            return FontFromSize(GLB.ToInt(dStr, 0));
            //return GLB.SysFont;
        }*/
        //---------------------------------------------------------------------------
        public static Font FontFromSize(int TextFontSize)
        {//根据用户定义字号，取得字体
            float fSize = GLB.SysFont.Size * (4 + TextFontSize) / 4;
            /*
            if (TextFontSize == 2)
            {
                Font f = new Font(GLB.SysFontName, fSize, FontStyle.Regular);
                int h = f.Height;
            }
            */
            if (fSize <= 0) return (GLB.SysFont);
            return (new Font(GLB.SysFontName, fSize, FontStyle.Regular));
        }
        //-------------------------------------------------------------------------------------------------
        public int ChildCount(String NodePath)
        {
            XmlNode Node = GetNode(NodePath);
            if (Node == null) return (0);
            return (Node.ChildNodes.Count);
        }
        //-------------------------------------------------------------------------------------------------
        public static String GetAttStr(XmlNode Node, String Att, String Default)
        {
            if (Node == null || Node.Attributes == null || Node.Attributes[Att] == null) return (Default);
            return (Node.Attributes[Att].Value);
        }
        //-------------------------------------------------------------------------------------------------
        public String Get(String NodeName)
        {
            XmlNode xn = Xml.SelectSingleNode("//System/Color[@ID='aaa']");
            XmlNodeReader Reader = new XmlNodeReader(xn);
            Reader.Read();
            return (Reader.ReadElementContentAsString());
            //return (Reader.GetAttribute("Com"));
        }
        //-------------------------------------------------------------------------------------------------
        private String FormatPath(String NodePath)
        {
            NodePath = NodePath.ToLower();
            NodePath = NodePath.Replace("[", "/node()[translate(local-name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='");
            NodePath = NodePath.Replace("]", "']");
            NodePath = "//root" + NodePath;
            return (NodePath);
        }
        //-------------------------------------------------------------------------------------------------
        public static XmlNode SelectByID(XmlNode Node, String ID)
        {
            ID = ID.ToLower();
            //return Node.SelectSingleNode("//*[./@id='" + ID + "']");
            return Node.SelectSingleNode("//*[translate(./@id, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='" + ID + "']");
        }
        //-------------------------------------------------------------------------------------------------
        public static XmlNode SelectByName(XmlNode Node, String Name)
        {
            Name = Name.ToLower();
            return Node.SelectSingleNode("//*[translate(./@Name, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='" + Name + "']");
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
    }

}

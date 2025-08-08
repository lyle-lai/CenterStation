using System;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
//---------------------------------------------------------------------------------------------------
using System.Data;
using System.Data.SQLite;
//---------------------------------------------------------------------------------------------------
namespace GlobalClass
{
    //多国语言版本，XML文件的读取和给界面赋值管理
    public class Config
    {
        //----------------------------------------------------
        private static SQLiteCommand Cmd = new SQLiteCommand();
        private static DBReader dr = new DBReader(DBConnect.SYS);
        //----------------------------------------------------
        public static String XmlPath = null;
        public static String ImgPath = null;
        public static int Rows = 1;
        public static int Cols = 1;
        //---------------------------------------------------------------------------------------------------
        public static void Initialize()
        {//构造函数
            Cmd.Connection = DBConnect.SYS.Conn;
            XmlPath = Get("Sys", "XmlPath", "Xml");
            Rows = GLB.ToInt(Get("Sys", "Rows", "3"), 3);
            Cols = GLB.ToInt(Get("Sys", "Cols", "2"), 2);
        }
        //-------------------------------------------------------------------------------------------------
        public static String Get(String Section, String Key, String Default)
        {
            dr.Select("Select * From Config Where Section='" + Section + "' And Key='" + Key + "'");
            if (dr.Read())
                return dr.GetStr("Value");
            else
            {
                Insert(Section, Key, Default);
                return Default;
            }
        }
        //-------------------------------------------------------------------------------------------------
        public static void Set(String Section, String Key, String Value)
        {
            dr.Select("Select * From Config Where Section='" + Section + "' And Key='" + Key + "'");
            if (dr.Read())
                Update(Section, Key, Value);
            else
                Insert(Section, Key, Value);
        }
        //-------------------------------------------------------------------------------------------------
        private static void Insert(String Section, String Key, String Value)
        {
            Cmd.CommandText = "Insert Into Config (Section, Key, Value) Values (@Section, @Key, @Value)";
            //
            Cmd.Parameters.Add("Section", DbType.String);
            Cmd.Parameters["Section"].Value = Section;
            //
            Cmd.Parameters.Add("Key", DbType.String);
            Cmd.Parameters["Key"].Value = Key;
            //
            Cmd.Parameters.Add("Value", DbType.String);
            Cmd.Parameters["Value"].Value = Value;
            //
            Cmd.ExecuteNonQuery();
        }
        //-------------------------------------------------------------------------------------------------
        private static void Update(String Section, String Key, String Value)
        {
            Cmd.CommandText = "Update Config Set Value=@Value Where Section='" + Section + "' And Key='" + Key + "'";
            //
            Cmd.Parameters.Add("Value", DbType.String);
            Cmd.Parameters["Value"].Value = Value;
            Cmd.ExecuteNonQuery();
        }
        //-------------------------------------------------------------------------------------------------
    }
}
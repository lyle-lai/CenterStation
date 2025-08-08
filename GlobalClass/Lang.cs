using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Data.SQLite;
// -------------------------------------------------------------------------------
namespace GlobalClass
{
    public class Lang
    {
        // -------------------------------------------------------------------------------
        private static DBReader dReader = new DBReader(DBConnect.SYS);
        // -------------------------------------------------------------------------------
        public static void Initialize()
        {
            dReader.Select("Select * From Lang Order By Section,Key");
            dReader.SetPrimaryKey(new String[] { "Section", "Key" });
        }
        // -------------------------------------------------------------------------------
        public static String Get(String Section, String Key)
        {
            if(!dReader.Find(new String[] { Section, Key})) return null;
            return dReader.GetStr("CN_1");
        }
        // -------------------------------------------------------------------------------
    }
}

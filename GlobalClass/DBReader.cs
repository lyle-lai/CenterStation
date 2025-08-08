using System;
using System.Text;
using System.IO;
//-----------------------------------------------------------------------
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
//-----------------------------------------------------------------------
namespace GlobalClass
{
    //-----------------------------------------------------------------------
    public class DBReader
    {
        //-----------------------------------------------------------------------
        private SQLiteConnection Conn = null;
        private DataSet dSet = new DataSet();
        private DataTable dTab = null;
        private DataRow dRow = null;
        private int Idx = -1;
        public int Index { get { return Idx; } }

        private static int SelCount = 0;
        //-----------------------------------------------------------------------
        /// <summary>
        /// 获取当前查询的记录行数 
        /// </summary>
        public int Count
        {
            get
            {
                if (dTab == null) return 0;
                if (dTab.Rows == null) return 0;
                return dTab.Rows.Count;
            }
        }
        //-----------------------------------------------------------------------
        /// <summary>
        /// 数据库连接的构造
        /// </summary>
        public DBReader(DBConnect DBCon)
        {
            Conn = DBCon.Conn;
        }
        //-----------------------------------------------------------------------
        /// <summary>
        /// 移动记录指针到头
        /// </summary>
        public void First()
        {
            Idx = -1;
        }
        //-----------------------------------------------------------------------
        public void Select(String SelectText)
        {
            //清理上一次的查询结果
            if (dSet != null) dSet.Clear();
            if (dTab != null) dTab.Clear();
            //SQL查询，结果放到DataSet中
            dSet = null;
            try
            {
                SelCount++;
                //Console.WriteLine("Select(" + SelCount + "):" + SelectText);
                SQLiteDataAdapter Da = new SQLiteDataAdapter(SelectText, Conn);
                dSet = new DataSet();
                Da.Fill(dSet);
                Da.Dispose();
                dTab = dSet.Tables[0];
            }
            catch (Exception ex) {Log.d(ex.Message);}
            Idx = -1;
            /*
            DataSet ds = new DataSet();
            ds.ReadXml(sr, XmlReadMode.IgnoreSchema);
             */
        }
        //-----------------------------------------------------------------------
        public bool Read()
        {
            Idx++;
            if (dSet == null || dSet.Tables.Count <= 0 || dTab == null || dTab.Rows.Count <= 0 || Idx >= dTab.Rows.Count)
            {
                dRow = null;
                return false;
            }
            dRow = dTab.Rows[Idx];
            return true;
        }
        //-----------------------------------------------------------------------
        public void SetVal(String FieldName, Object Val)
        {
            if (dRow == null) return;
            if (dRow.Table.Columns[FieldName] == null) return;
            dRow[FieldName] = Val;
        }
        //-----------------------------------------------------------------------
        public Object GetVal(String FieldName)
        {
            return dRow[FieldName];
        }
        //-----------------------------------------------------------------------
        public DateTime GetDateTime(String FieldName)
        {
            if (dRow == null) return DateTime.MinValue;
            if (dRow.Table.Columns[FieldName] == null) return DateTime.MinValue;
            if (dRow[FieldName] == DBNull.Value) return DateTime.MinValue;

            return Convert.ToDateTime(dRow[FieldName]);
        }
        //-----------------------------------------------------------------------
        public String GetStr(String FieldName)
        {
            if (dRow == null) return null;
            if (dRow.Table.Columns[FieldName] == null) return null;

            return (dRow[FieldName].ToString());
        }
        //-----------------------------------------------------------------------
        /*public AlphaImage GetImg(String FieldName)
        {
            return AlphaImage.FromBuffer((byte[])dRow["Img"]);
        }*/
        //-----------------------------------------------------------------------
        public Bitmap GetBitImg(String FieldName)
        {
            //Bitmap _image = new Bitmap(();
            Stream ss = new MemoryStream((byte[])dRow["Img"]);
            return new Bitmap(ss);
        }
        //-----------------------------------------------------------------------
        public String GetUpper(String FieldName)
        {
            String Str = GetStr(FieldName);
            if (GLB.IsEmpty(Str)) return Str;
            return Str.ToUpper();
        }
        //-----------------------------------------------------------------------
        public int GetInt(String FieldName)
        {
            return GLB.ToInt(GetStr(FieldName), 0);
            //return Convert.ToInt32(GetStr(FieldName));
        }
        //-----------------------------------------------------------------------
        public Color GetColor(String FieldName)
        {
            return Color.FromArgb(unchecked((int)0xFF000000 | GetInt(FieldName)));
        }
        //-----------------------------------------------------------------------
        public Boolean GetBoolean(String FieldName)
        {
            return GLB.ToInt(GetStr(FieldName), 0) != 0;
        }
        //-----------------------------------------------------------------------
        public Double GetF(String FieldName, float DefVal)
        {
            return GLB.ToFloat(GetStr(FieldName), DefVal);
        }
        //-----------------------------------------------------------------------
        public void Goto(int RowIndex)
        {
            Idx = RowIndex;
            if (Idx >= 0)
                dRow = dTab.Rows[Idx];
        }
        //-----------------------------------------------------------------------
        public Boolean Seek(String FieldName, String Val)
        {
           
            First();
            while (Read())
            {
                if (GLB.Same(GetStr(FieldName), Val))
                    return true;
            }
            return false;
        }
        //-----------------------------------------------------------------------
        public Boolean Eof()
        {
            return (Count <= 0 || Index >= Count);
        }
        //------------------------------------------------------------------------------
        public Boolean hasField(String FieldName)
        {
            return (dRow != null && dRow.Table.Columns[FieldName] != null);
        }
        //------------------------------------------------------------------------------
        public byte[] GetBlob(String FieldName)
        {
            if (dRow == null) return null;
            if (dRow.Table.Columns[FieldName] == null) return null;

            try
            {
                return (byte[])dRow[FieldName];
            }
            catch (Exception ex) {
                Log.d(ex.Message);
                return null; }
        }
        //------------------------------------------------------------------------------
        public void SetPrimaryKey(String[] sList)
        {
            DataColumn[] keys = new DataColumn[2];
            for (int i = 0; i < sList.Length; i++)
                keys[i] = dTab.Columns[sList[i]];
            dTab.PrimaryKey = keys;

            /*
            dTab.PrimaryKey = new DataColumn[sList.Length];
            for (int i = 0; i < sList.Length; i++)
                dTab.PrimaryKey[i] = new DataColumn(sList[i]);// dTab.Columns[sList[i]];
            //dTab.PrimaryKey = new DataColumn[] { dTab.Columns["Section"], dTab.Columns["Key"] };
             */
        }
        //------------------------------------------------------------------------------
        public bool Find(String[] sList)
        {
            dRow = dTab.Rows.Find(sList);
            return dRow != null;
        }
        //------------------------------------------------------------------------------
    }//End Of class CDB_Reader
    //-----------------------------------------------------------------------
}

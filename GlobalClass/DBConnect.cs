using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
//-----------------------------------------------------------------------
using System.Data;
using System.Data.SQLite;
//-----------------------------------------------------------------------
namespace GlobalClass
{
    public class DBConnect
    {
        //public static DBConnect DATA;
        public static DBConnect SYS;
        //-----------------------------------------------------------------------
        //public static List<DBConnect> HisConn = new List<DBConnect>();
        public static Dictionary<int, DBConnect> HisConn = new Dictionary<int, DBConnect>();
        public static void Initialize()
        {
            DBReader dr = new DBReader(SYS);
            dr.Select("SELECT * FROM Device where status = '1'");
            while(dr.Read())
            {
                int devId = dr.GetInt("ID");
                string fn = GLB.DAT_DIR + "Data_" + dr.GetInt("ID") + ".DB";
                if (!File.Exists(fn))
                {
                    File.Copy(GLB.CFG_DIR + "DataDefine.DB", fn);
                }
                HisConn[devId] = new DBConnect(fn);
            }
            //for (int i = 0; i < 48; i++)
            //{
            //    String fn = GLB.DAT_DIR + "Data_" + i.ToString() + ".DB";
            //    if (!File.Exists(fn))
            //        File.Copy(GLB.CFG_DIR + "DataDefine.DB", fn);
            //    HisConn.Add(new DBConnect(fn));
            //}
            //InitParaItem();
        }

        public static void CloseConn()
        {
            if (SYS != null)
            {
                if (SYS.Conn != null && SYS.Conn.State== ConnectionState.Open)
                {
                    SYS.Conn.Close();
                }
                // SYS.Conn = null;
                //SYS = null;
            }
        }
        //-----------------------------------------------------------------------
        /*private static void InitParaItem()
        {
            //DBConnect DB = new DBConnect(GLB.EXE_DIR + "Settings.DB;");
            DBConnect DB = DBConnect.SYS;
            //初始化设备配置
            DB.BeginTransaction();
            DBReader dr = new DBReader(DB);
            DB.ExecSQL("Delete From ParaItem Where ViewID>=0");
            dr.Select("Select * From ParaItem Where ViewID=-1");
            for (int ViewID = 0; ViewID < 200; ViewID++)
            {
                dr.First();
                while (dr.Read())
                {
                    DB.AddPara("@ID", dr.GetInt("ID") + 1000 * (ViewID + 1));
                    DB.AddPara("@ViewID", ViewID);
                    DB.AddPara("@Path", dr.GetStr("Path"));
                    DB.AddPara("@SN", dr.GetStr("SN"));
                    DB.AddPara("@UnitName", dr.GetStr("UnitName"));
                    DB.AddPara("@Min", dr.GetInt("Min"));
                    DB.AddPara("@Max", dr.GetInt("Max"));
                    DB.AddPara("@Low", dr.GetInt("Low"));
                    DB.AddPara("@High", dr.GetInt("High"));
                    DB.AddPara("@Val", dr.GetInt("Val"));

                    DB.ExecSQL("Insert Into ParaItem (ID, ViewID, Path, SN, UnitName, Min, Max, Low, High, Val) Values (@ID, @ViewID, @Path, @SN, @UnitName, @Min, @Max, @Low, @High, @Val)");//添加
                }
            }
            DB.Commit();
        }*/
        //-----------------------------------------------------------------------------------------
        public SQLiteConnection Conn = null;
        public SQLiteCommand Cmd = null;
        private SQLiteTransaction Trans = null;
        private object _executeLock = new object();
        //-----------------------------------------------------------------------
        /// <summary>
        /// 数据库连接的构造
        /// </summary>
        /// <param name="ConStr">数据库连接字串</param>
        public DBConnect(String ConStr)
        {
            //初始化数据库连接
            Conn = new SQLiteConnection("Data Source = " + ConStr);
            Conn.Open();
            Cmd = new SQLiteCommand();
            Cmd.Connection = Conn;
        }
        //-----------------------------------------------------------------------
        public void ExecSQL(String CommandText)
        {  //执行SQL语句的操作

            if (Cmd != null)
            {
                lock(_executeLock)
                {
                    Cmd.CommandText = CommandText;
                    Cmd.ExecuteNonQuery();
                    Cmd.Parameters.Clear();
                }
            }
        }
        //-----------------------------------------------------------------------
        public SQLiteParameter AddPara(String ParaName, Object Value)
        {
            return Cmd.Parameters.AddWithValue(ParaName, Value);
        }

        public int AddBinaryPara(string paraName, object value)
        {
            return Cmd.Parameters.Add(new SQLiteParameter(paraName, DbType.Binary) { Value = value });
        }
        //-----------------------------------------------------------------------
        public void ExecInsert(String InsertSQL)
        {//定义好Cmd的变量之后，执行插入动作
            Cmd.CommandText = InsertSQL;
            Cmd.ExecuteNonQuery();
        }
        //-----------------------------------------------------------------------
        public void BeginTransaction()
        {
            if (Trans != null) GLB.Dispose(Trans);
            Trans = Conn.BeginTransaction();
        }
        //-----------------------------------------------------------------------
        public void Commit()
        {
            if (Trans == null) return;
            Trans.Commit();
            GLB.Dispose(Trans);
        }
        //-----------------------------------------------------------------------
        public void Free()
        {
            GLB.Dispose(Trans);
            GLB.Dispose(Cmd);
            GLB.Dispose(Conn);
        }
        //-----------------------------------------------------------------------
        public int GetLastInsertID()
        {
            SQLiteDataAdapter Da = new SQLiteDataAdapter("Select Last_Insert_Rowid() As NewID", Conn);
            DataSet Ds = new DataSet();
            Da.Fill(Ds);
            int ReInt = Convert.ToInt32(Ds.Tables[0].Rows[0]["NewID"].ToString());
            //清理
            Da.Dispose();
            Ds.Dispose();
            //返回
            return ReInt;
        }
        //-----------------------------------------------------------------------

    }//End Of class CDB_Conn
}
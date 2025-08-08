using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
//using System.Windows.Forms;
using GlobalClass;
using MGOA_Pack;
using System.Threading;
using System.Data.SQLite;
//----------------------------------------------------------------------------------------------
namespace History_Pack
{
    public class SubmitToReport
    {
        //private static String SqliteConStr = string.Empty;   // 本地DB文件地址
        //private static SQLiteConnection SQLiteConn = null;   // 本地DB连接
        //private static SQLiteTransaction SQLiteTran = null;  // 本地DB事务
        private static SQLiteCommand SQLiteCmd = null;       // 本地DB命令
        //
        private const string UPDATE_DEVICE_SQL = "update Device set DEVICETYPE=@DEVICETYPE, BARCODE=@BARCODE, DEVICEIP=@DEVICEIP, INHOSNUM=@INHOSNUM, ARCHIVESID=@ARCHIVESID, BEDNUM=@BEDNUM, PATIENTNAME=@PATIENTNAME, STATUS='1' where DEVICEID=@DEVICEID";
        private const string INSERT_DEVICE_SQL = "insert into Device(DEVICETYPE,BARCODE,DEVICEIP,INHOSNUM,ARCHIVESID,BEDNUM,PATIENTNAME,DEVICEID,STATUS) values(@DEVICETYPE,@BARCODE,@DEVICEIP,@INHOSNUM,@ARCHIVESID,@BEDNUM,@PATIENTNAME,@DEVICEID,'1')";
        private const string QUERY_V_DEVICE_PATIENT = "select * from v_device_patient";
        private const string UPDATE_DEVICE_STATUS_0 = "update Device set STATUS = '0'";
        private const string UPDATE_DEVICE_STATUS_1 = "update Device set INHOSNUM='', ARCHIVESID='', BEDNUM='', PATIENTNAME='' where STATUS='0'";
        // modify_by_limu_160606
        //private static String ConStr = @"Data Source=orcl; User ID=ics_user; Password=123; Unicode=True";
        private static String ConStr = @"Data Source=ICS; User ID=ics_user; Password=123; Unicode=True";
        //private static String ConStr = @"Data Source=AJSERVER2; User ID=ics_user; Password=123; Unicode=True";
        //private static String ConStr = @"Data Source=DCS_AJSERVER; User ID=ics_user; Password=123; Unicode=True";
        private static OracleConnection OConn = new OracleConnection();
        private static OracleCommand OCom = new OracleCommand();
        private static OracleCommand OComRead = new OracleCommand();
        private static TimeSpan tSpan;
        private static OracleTransaction Transaction = null;
        private static Thread ThrTimer = null;//new Thread(new ThreSadStart(Run));

        public delegate void OnSubmitData();
        public static OnSubmitData SubmitData = null;
        //----------------------------------------------------------------------------------------------
        public static void Initialize()
        {
            ThrTimer = new Thread(new ThreadStart(TimerRun));
            ThrTimer.Start();
        }
        //----------------------------------------------------------------------------------------------
        public SubmitToReport()
        {
        }
        //----------------------------------------------------------------------------------------------
        private static void TimerRun()
        {
            while (true)
            {
                TimerTick();
                Thread.Sleep(10000);
            }
        }
        //----------------------------------------------------------------------------------------------
        private static void TimerTick()
        {
            if (OConn.State != ConnectionState.Open)
            {
                try
                {
                    OConn.ConnectionString = ConStr;
                    Log.d(OConn.ConnectionString);
                    OConn.Open();
                    //MessageBox.Show("数据库连接测试成功...", "提示!");
                    Log.d("数据库连接成功...");
                    OCom.CommandText = "Select CURRENT_TIMESTAMP as ct from dual";
                    OCom.Connection = OConn;
                    OracleDataReader Reader = OCom.ExecuteReader();
                    if (Reader.Read())
                    {
                        DateTime dt = Convert.ToDateTime(Reader["ct"]);
                        tSpan = dt - DateTime.Now;
                    }
                }
                catch
                {
                    Log.d("数据库连接失败!");
                    ;//MessageBox.Show("数据库连接测试失败！请开启数据库服务并重新配置数据库连接信息。", "警告");
                }
            }
            else
            {
                SyncData();
                if (SubmitData != null)
                {
                    //OracleTransaction Trans = OConn.BeginTransaction();
                    SubmitData();
                    //Trans.Commit();
                }
            }
        }
        //----------------------------------------------------------------------------------------------
        /*private static void Save()
        {
            String SelStr = "Insert Into ICU_RealTimeData (BedID, ParaGroup, Val1, Val2, Val3, MeasTime, SubmTime) " +
                "Values('NICU-018', 'NIBP.NIBP', '140', '122', '135', :MeasTime, CURRENT_TIMESTAMP)";
            //OracleCommand OCom = new OracleCommand(SelStr, OConn);                
            OCom.CommandText = SelStr;
            OCom.Connection = OConn;
            //
            OCom.Parameters.Add("MeasTime", OracleType.DateTime);
            OCom.Parameters["MeasTime"].Value = DateTime.Now.Add(tSpan);
            OCom.ExecuteNonQuery();
        }*/
        //----------------------------------------------------------------------------------------------
        public static void Submit(ParaGroup PGroup, String BedID, String InHosNum, String DeviceID, String BoxNum)
        {
            //2020年01月16日 临时保存
            //if (OConn.State != ConnectionState.Open) return;
            ////--- 保存到ORCAL，报表服务器 ---
            //String FStr = "";
            //String VStr = "";
            //for (int i = 0; i < PGroup.PItems.Length; i++)
            //{
            //    FStr += "Val" + (i + 1).ToString() + ",";
            //    VStr += "'" + PGroup.PItems[i].GetValue() + "',";
            //}
            ////
            ////String SelStr = "Insert Into RealTimeData (BedID, ParaGroup, Val_1, Val_2, Val_3, MeasTime, SubmTime) " +
            ////"Values('NICU-018', 'NIBP.NIBP', '140', '122', '135', :MeasTime, CURRENT_TIMESTAMP)";
            ////OracleCommand OCom = new OracleCommand(SelStr, OConn);                
            //OCom.CommandText = "Insert Into ICU_RealTimeData (BedID, ParaGroup, " + FStr + " MeasTime, SubmTime, InHosNum, DeviceID, BoxNum, PortNum) " +
            //    "Values(:BedID, '" + PGroup.FullSN + "', " + VStr + " :MeasTime, CURRENT_TIMESTAMP, :InHosNum, :DeviceID, :BoxNum, 'LAN1')";
            //OCom.Connection = OConn;
            ////
            //OCom.Parameters.Add(":BedID", BedID);
            //OCom.Parameters.Add(":InHosNum", InHosNum);
            //OCom.Parameters.Add(":DeviceID", DeviceID);
            //OCom.Parameters.Add(":BoxNum", BoxNum);
            //OCom.Parameters.Add("MeasTime", OracleType.DateTime);
            //OCom.Parameters["MeasTime"].Value = DateTime.Now.Add(tSpan);
            //OCom.ExecuteNonQuery();
        }
        //----------------------------------------------------------------------------------------------
        public static void BeginTransaction()
        {
            Transaction = OConn.BeginTransaction();
        }
        //----------------------------------------------------------------------------------------------
        public static void Commit()
        {
            if (Transaction == null) return;
            Transaction.Commit();
            Transaction = null;
        }
        //----------------------------------------------------------------------------------------------
        /** 从ORCL同步数据到本地DB **/
        private static void SyncData()
        {
            if (OConn.State != ConnectionState.Open) return;
            // 1.从Oracle数据库读取数据
            OComRead.CommandText = QUERY_V_DEVICE_PATIENT;
            OComRead.Connection = OConn;
            OracleDataReader Reader = OComRead.ExecuteReader();

            if (Reader.HasRows == false)
            {
                if (!Reader.IsClosed)
                    Reader.Close();
                return;
            }

            List<SQLiteParameter> sqlParas = new List<SQLiteParameter>();
            int rowsAffected = 0, updateRowsCount = 0, insertRowsCount = 0;
            bool hasRows = false;
            // 2.同步数据到本地DB（使用事务控制）
            try
            {
                DBConnect.SYS.BeginTransaction();// SQLiteConn.BeginTransaction();   // 开始本地DB事务
                SQLiteCmd = DBConnect.SYS.Cmd;// new SQLiteCommand(DBConnect.SYS);    // 新建本地DB命令
                if (Reader.Read())
                {
                    // 先将Device表的所有记录的标识置为0，后续更新记录时会将该标识置为1，最后将标识为0的记录的设备解绑病人
                    hasRows = true;
                    SQLiteCmd.CommandText = UPDATE_DEVICE_STATUS_0;
                    SQLiteCmd.ExecuteNonQuery();
                }
                do
                {
                    // 先Update，若返回的影响行记录数为0，再考虑是否需要Insert
                    sqlParas.Add(new SQLiteParameter("@DEVICETYPE", Reader.GetValue(Reader.GetOrdinal("DEVICETYPE"))));
                    sqlParas.Add(new SQLiteParameter("@BARCODE", Reader.GetValue(Reader.GetOrdinal("BARCODE"))));
                    sqlParas.Add(new SQLiteParameter("@DEVICEIP", Reader.GetValue(Reader.GetOrdinal("DEVICEIP"))));
                    sqlParas.Add(new SQLiteParameter("@INHOSNUM", Reader.GetValue(Reader.GetOrdinal("INHOSNUM"))));
                    sqlParas.Add(new SQLiteParameter("@ARCHIVESID", Reader.GetValue(Reader.GetOrdinal("ARCHIVESID"))));
                    sqlParas.Add(new SQLiteParameter("@BEDNUM", Reader.GetValue(Reader.GetOrdinal("BEDNUM"))));
                    sqlParas.Add(new SQLiteParameter("@PATIENTNAME", Reader.GetValue(Reader.GetOrdinal("PATIENTNAME"))));
                    sqlParas.Add(new SQLiteParameter("@DEVICEID", Reader.GetValue(Reader.GetOrdinal("DEVICEID"))));

                    SQLiteCmd.CommandText = UPDATE_DEVICE_SQL;
                    SQLiteCmd.Parameters.Clear();
                    SQLiteCmd.Parameters.AddRange(sqlParas.ToArray());
                    rowsAffected = SQLiteCmd.ExecuteNonQuery();
                    updateRowsCount += rowsAffected;

                    // 影响的行记录数为0，进行Insert操作
                    if (rowsAffected == 0)
                    {
                        SQLiteCmd.CommandText = INSERT_DEVICE_SQL;
                        SQLiteCmd.Parameters.Clear();
                        SQLiteCmd.Parameters.AddRange(sqlParas.ToArray());
                        rowsAffected = SQLiteCmd.ExecuteNonQuery();
                        insertRowsCount += rowsAffected;
                    }

                    // 清空临时变量值
                    sqlParas.Clear();
                    rowsAffected = 0;
                } while (Reader.Read());
                if (hasRows == true)
                {
                    // 
                    SQLiteCmd.CommandText = UPDATE_DEVICE_STATUS_1;
                    SQLiteCmd.ExecuteNonQuery();
                }
                DBConnect.SYS.Commit();// SQLiteTran.Commit();   //提交本地DB事务
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
                //DBConnect.SYS.Rollback();
            }
            finally
            {
                //SQLiteTran = null;
                SQLiteCmd = null;
                //Print("updated " + updateRowsCount + " rows, inserted " + insertRowsCount + " rows.");
                if (!Reader.IsClosed)
                    Reader.Close();
            }
        }
        //----------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------
}

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using MGOA_Pack;
using GlobalClass;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
//-----------------------------------------------------------------------------
namespace History_Pack
{
    //Class History-----------------------------------------------------------------------------
    public class History
    {
        //-------------------------------
        private object obj = new object();
        private const int INTE_SEC = 10;
        private const int INTE = INTE_SEC * 1000;
        //private const int P_LMT = 100;// 30 * 24 * 3600 / INTE_SEC;
        //private const int W_LMT = 100;//1 * 24 * 3600 / INTE_SEC;
        //-------------------------------
        private Thread SaveThread = null;
        private DBConnect Conn = null;
        public List<ParaGroup> Para_List = null;
        public List<WaveGroup> Wave_List = null;
        private DateTime now = DateTime.MinValue;
        private DateTime lastTime = DateTime.Now;
        private readonly int TestId = 0;
        //-----------------------------------------------------------------------------
        public History(int ID, List<ParaGroup> paraGroup, List<WaveGroup> waveGroup)
        {//初始化
            TestId = ID;
            Para_List = paraGroup;
            Wave_List = waveGroup;
            //
            Conn = DBConnect.HisConn[ID];

            //String Str = "Create Table IF NOT EXISTS Para (dTime DATETIME";
            //for (int i = 0; i < Para_List.Count; i++)
            //    Str += ", '" + Para_List[i].FullSN + "' BLOB";
            //Str += ")";
            string sql = "CREATE TABLE IF NOT EXISTS Para (dTime DATETIME, FullSN NVARCHAR, Value BLOB, CONSTRAINT Para_PK unique(dTime, FullSN))";
            Conn.ExecSQL(sql);

            //Str = "Create Table IF NOT EXISTS Wave (dTime DATETIME";
            //for (int i = 0; i < Wave_List.Count; i++)
            //    Str += ", '" + Wave_List[i].FullSN + "' BLOB";
            //Str += ")";
            sql = "CREATE TABLE IF NOT EXISTS Wave (dTime DATETIME, FullSN NVARCHAR, Value BLOB, CONSTRAINT Para_PK unique(dTime, FullSN))";
            Conn.ExecSQL(sql);

            sql = "CREATE TABLE IF NOT EXISTS Alarms (dTime DATETIME, FullSN NVARCHAR, Value NVARCHAR)";
            Conn.ExecSQL(sql);
            //
            //SaveThread = new Thread(new ThreadStart(Run));
            //SaveThread.Start();
        }
        //-----------------------------------------------------------------------------
        //private void Run()
        //{
        //    while (true)
        //    {
        //        Thread.Sleep(INTE);
        //        lock (obj)
        //        {

        //            now = DateTime.Now;
        //            Conn.BeginTransaction();
        //            SavePara();
        //            SaveWave();
        //            Conn.Commit();
        //            lastTime = now;
        //        }
        //    }
        //}
        //-----------------------------------------------------------------------------
        public void SavePara(string fullSn, byte[] valBuf)
        {
            if (Para_List == null || valBuf.Length <= 0) return;

            try
            {
                string sql = "INSERT INTO Para([dTime], [FullSN], [Value]) VALUES(@dTime, @FullSN, @Value) ON CONFLICT DO UPDATE SET Value=@Value";
                Conn.AddPara("@dTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Conn.AddPara("@FullSN", fullSn);
                Conn.AddBinaryPara("@Value", valBuf);
                Conn.ExecSQL(sql);
            }
            catch(Exception ex)
            {
                Log.d($"[{TestId}]监测参数保存异常：dTime: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, SN: {fullSn}, ValueSize: {valBuf.Length}");
                Log.d(ex.Message);
                Log.d(ex.StackTrace.ToString());
            }


            //bool needSave = false;
            //Conn.AddPara("@dTime", now.ToString("yyyy-MM-dd HH:mm:ss"));
            ////String Str = "[dTime]=@dTime";
            //string fields = "[dTime]";
            //string values = "@dTime";
            ////SubmitToReport.BeginTransaction();

            //for (int i = 0; i < Para_List.Count; i++)
            //{
            //    ParaGroup PGrp = Para_List[i];
            //    //Str += ",[" + PGrp.FullSN + "]=@V" + i.ToString();
            //    fields += ", [" + PGrp.FullSN + "]";
            //    values += ", @V" + i.ToString();

            //    if (PGrp.NeedSave)
            //    {
            //        Conn.AddBinaryPara("@V" + i.ToString(), PGrp.ValBuf);
            //        //Conn.AddPara("@V" + i.ToString(), PGrp.ValBuf);
            //        //SubmitToReport.Submit(PGrp);
            //        needSave = true;
            //    }
            //    else
            //    {
            //        Conn.AddPara("@V" + i.ToString(), null);
            //    }
            //    PGrp.NeedSave = false;

            //}
            //if (needSave)
            //{
            //    string sql = "INSERT INTO Para (" + fields + ") VALUES(" + values + ")";
            //    Conn.ExecSQL(sql);
            //}
            //Conn.ExecSQL("Update Para Set " + Str + " Where dTime=(SELECT min(dTime) FROM Para)");

            //SubmitToReport.Commit();
        }
        //-----------------------------------------------------------------------------
        public void SaveWave(string fullSn, float[] data)
        {
            if (Wave_List == null || data.Length <= 0) return;
            try
            {
                byte[] bytes = new byte[data.Length * sizeof(float)];
                Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);
                string sql = "INSERT INTO Wave([dTime], [FullSN], [Value]) VALUES(@dTime, @FullSN, @Value)";
                Conn.AddPara("@dTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Conn.AddPara("@FullSN", fullSn);
                Conn.AddBinaryPara("@Value", bytes);
                Conn.ExecSQL(sql);
            }
            catch(Exception ex)
            {
                Log.d($"[{TestId}]波形数据保存异常：dTime: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, SN: {fullSn}, ValueSize: {data.Length}");
                Log.d(ex.Message);
                Log.d(ex.StackTrace.ToString());
            }



            //bool needSave = false;
            ////Conn.AddPara("@dTime", lastTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //Conn.AddPara("@dTime", now.ToString("yyyy-MM-dd HH:mm:ss"));
            ////String Str = " [sTime]=@sTime, [eTime]=@eTime ";
            //string fields = "[dTime]";
            //string values = "@dTime";
            //for (int i = 0; i < Wave_List.Count; i++)
            //{
            //    WaveGroup WGrp = Wave_List[i];
            //    float[] data = WGrp.HistoryOuter.GetAllBuffer();
            //    if (data == null) { continue; }
            //    needSave = true;
            //    //Str += ",[" + WGrp.FullSN + "]=@V" + i.ToString();
            //    fields += ", [" + WGrp.FullSN + "]";
            //    values += ", @V" + i.ToString();
            //    //Conn.AddPara("@V" + i.ToString(), WGrp.HistoryOuter.GetAllBuffer());
            //    byte[] bytes = new byte[data.Length * sizeof(float)];
            //    Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);
            //    Conn.AddBinaryPara("@V" + i.ToString(), bytes);
            //}
            ////Conn.ExecSQL("Update Wave Set " + Str + " Where sTime=(SELECT min(sTime) FROM Wave)");
            //if (needSave)
            //{
            //    string sql = "INSERT INTO Wave (" + fields + ") VALUES (" + values + ")";
            //    Conn.ExecSQL(sql);
            //}
        }
        //-----------------------------------------------------------------------------
        public void Stop()
        {
            SaveThread.Abort();
        }
        //-----------------------------------------------------------------------------
    }
    //Class History-----------------------------------------------------------------------------
}

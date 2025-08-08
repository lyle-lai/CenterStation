using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Text;

namespace GlobalClass
{
    public class OracleHelper
    {
        private OracleConnection OConn = new OracleConnection();
        private static OracleCommand OCom = new OracleCommand();

        public OracleHelper()
        {
            //OConn.ConnectionString = @"Data Source=DCS_180_WAN; User ID=ics_user; Password=123; Unicode=True";
            OConn.ConnectionString = @"Data Source=orcl; User ID=ics_user; Password=123; Unicode=True";
        }

        public DataTable Query(string sql)
        {
            DataTable dt = new DataTable();

            if (OConn.State != ConnectionState.Open)
            {
                try
                {
                    OConn.Open();
                    OCom.CommandText = sql;
                    OCom.Connection = OConn;
                    using (OracleDataReader reader = OCom.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
                catch (Exception e)
                {
                    Log.d("查询 " + sql + " 出错，错误信息：" + e.Message);
                }
                finally
                {
                    OConn.Close();
                }
            }

            return dt;
        }

    }
}

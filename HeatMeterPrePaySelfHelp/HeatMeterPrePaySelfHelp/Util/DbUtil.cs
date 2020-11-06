using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using HeatMeterPrePay.exception;
using HeatMeterPrePaySelfHelp.util;
using MySql.Data.MySqlClient;

namespace HeatMeterPrePay.Util
{
    // Token: 0x0200004F RID: 79
    public class DbUtil : IDisposable
    {
        // Token: 0x06000506 RID: 1286 RVA: 0x00051B60 File Offset: 0x0004FD60
        public DbUtil()
        {
            string datasource = ConfigAppSettings.GetValue("cnstr");
            this.init(datasource);
        }

        // Token: 0x06000507 RID: 1287 RVA: 0x00051B7E File Offset: 0x0004FD7E
        public DbUtil(string datasource)
        {
            this.init(datasource);
        }

        // Token: 0x06000508 RID: 1288 RVA: 0x00051B98 File Offset: 0x0004FD98
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Token: 0x06000509 RID: 1289 RVA: 0x00051BA7 File Offset: 0x0004FDA7
        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.isOpen = false;
                    this.connection.Dispose();
                }
                this.close();
                this.disposed = true;
            }
        }

        // Token: 0x0600050A RID: 1290 RVA: 0x00051BD4 File Offset: 0x0004FDD4
        ~DbUtil()
        {
            this.Dispose(false);
        }

        // Token: 0x0600050B RID: 1291 RVA: 0x00051C04 File Offset: 0x0004FE04
        private void init(string datasource)
        {
            if (null != datasource && "" != datasource)
            {
                this.datasource = datasource;
            }
            this.connection = new MySqlConnection(this.datasource);
            this.parameters = new Dictionary<string, string>();
            this.isOpen = false;
        }

        // Token: 0x0600050C RID: 1292 RVA: 0x00051C52 File Offset: 0x0004FE52
        private bool checkDbExist()
        {
            return File.Exists(this.datasource);
        }

        // Token: 0x0600050D RID: 1293 RVA: 0x00051C64 File Offset: 0x0004FE64
        private void open()
        {
            if (!this.isOpen)
            {
                this.connection.Open();
            }
            this.isOpen = true;
        }

        // Token: 0x0600050E RID: 1294 RVA: 0x00051C93 File Offset: 0x0004FE93
        private void close()
        {
            this.connection.Close();
        }

        // Token: 0x0600050F RID: 1295 RVA: 0x00051CAF File Offset: 0x0004FEAF
        public void AddParameter(string key, string value)
        {
            this.parameters.Add(key, value);
        }
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, Dictionary<string, string> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 300;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (var parameter in cmdParms)
                {
                    if (parameter.Value == null || parameter.Value == "")
                    {
                        cmd.Parameters.AddWithValue(parameter.Key, DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }

                }
            }
        }
        // Token: 0x06000510 RID: 1296 RVA: 0x00051CC0 File Offset: 0x0004FEC0
        public int ExecuteNonQuery(string queryStr)
        {
            using (MySqlConnection connection = new MySqlConnection(this.datasource))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, queryStr, this.parameters);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        this.parameters.Clear();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        // Token: 0x06000511 RID: 1297 RVA: 0x00051DA4 File Offset: 0x0004FFA4
        public long ExecuteNonQueryAndReturnLastInsertRowId(string queryStr)
        {
            using (MySqlConnection connection = new MySqlConnection(this.datasource))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, queryStr, this.parameters);
                        cmd.ExecuteNonQuery();
                        long id = cmd.LastInsertedId;
                        cmd.Parameters.Clear();
                        this.parameters.Clear();
                        return id;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        // Token: 0x06000512 RID: 1298 RVA: 0x00051F6C File Offset: 0x0005016C
        public DataTable ExecuteQuery(string queryStr)
        {
            DataSet dataSet = Query(queryStr);
            if (dataSet == null || dataSet.Tables.Count == 0)
            {
                return null;
            }
            return dataSet.Tables[0];

        }

        public DataSet Query(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(this.datasource))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, this.parameters);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    this.parameters.Clear();
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(this.datasource))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                MySqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }


        // Token: 0x06000513 RID: 1299 RVA: 0x00052064 File Offset: 0x00050264
        public DataRow ExecuteRow(string queryStr)
        {
            DataTable dataTable = ExecuteQuery(queryStr);
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dataTable.Rows[0];
            }
        }

        // Token: 0x0400061C RID: 1564
        private string datasource = "server=127.0.0.1;port=3307;user id=root;password=root;persistsecurityinfo=True;database=hw_wm;connect timeout=300";

        // Token: 0x0400061D RID: 1565
        private bool isOpen;

        // Token: 0x0400061E RID: 1566
        private bool disposed;

        // Token: 0x0400061F RID: 1567
        private MySqlConnection connection;

        // Token: 0x04000620 RID: 1568
        private Dictionary<string, string> parameters;
    }
}

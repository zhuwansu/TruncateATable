/****************************************************************************
*Copyright (c) 2017  All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：ZHUWANSU
*公司名称：
*命名空间：TruncateATable.GetSQL
*文件名：  OracleHelp
*版本号：  V1.0.0.0
*唯一标识：2f6f98a8-9884-49b9-adc0-3b2c9314baf5
*当前的用户域：ZHUWANSU
*创建人：  朱皖苏
*电子邮箱：zhuwansu@dbgo.com
*创建时间：2017/11/17 11:11:15

*描述：
*
*=====================================================================
*修改标记
*修改时间：2017/11/17 11:11:15
*修改人： 朱皖苏
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/

using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TruncateATable.Common
{

    /// <summary>
    /// A helper class used to execute queries against an Oracle database
    /// </summary>
    public partial class OracleHelper
    {

        // Read the connection strings from the configuration file

        //Create a hashtable for the parameter cached
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a database query which does not include a select
        /// </summary>
        /// <param name="connString">Connection string to database</param>
        /// <param name="cmdType">Command type either stored procedure or SQL</param>
        /// <param name="cmdText">Acutall SQL Command</param>
        /// <param name="commandParameters">Parameters to bind to the command</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            // Create a new Oracle command
            OracleCommand cmd = new OracleCommand();

            //Create a connection
            using (OracleConnection connection = new OracleConnection(connectionString))
            {

                //Prepare the command
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);

                //Execute the command
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static DataSet ExecuteDataSet(string connStr, string SQL)
        {
            DataSet ds = null;
            DataSet functionReturnValue = null;

            OracleConnection SQLconn = null;
            OracleDataAdapter sqlda = null;
            try
            {
                SQLconn = new OracleConnection(connStr);
                SQLconn.Open();
                sqlda = new OracleDataAdapter(SQL, SQLconn);
                ds = new DataSet();
                sqlda.Fill(ds);
                functionReturnValue = ds;
            }
            finally
            {
                if ((SQLconn != null))
                    SQLconn.Dispose();
                SQLconn = null;
                if ((sqlda != null))
                    sqlda.Dispose();
                sqlda = null;
            }
            return functionReturnValue;
        }

        /// <summary>
        /// Execute an OracleCommand (that returns no resultset) against an existing database transaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing database transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute an OracleCommand (that returns no resultset) against an existing database connection
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            OracleCommand cmd = new OracleCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a select query that will return a result set
        /// </summary>
        /// <param name="connString">Connection string</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns></returns>
        public static OracleDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            //Create the command and connection
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(connectionString);

            try
            {
                //Prepare the command to execute
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                //Execute the query, stating that the connection should close when the resulting datareader has been read
                OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;

            }
            catch
            {

                //If an error occurs close the connection as the reader will not be used and we expect it to close the connection
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute an OracleCommand that returns the first column of the first record against the database specified in the connection string
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        ///	<summary>
        ///	Execute	a OracleCommand (that returns a 1x1 resultset)	against	the	specified SqlTransaction
        ///	using the provided parameters.
        ///	</summary>
        ///	<param name="transaction">A	valid SqlTransaction</param>
        ///	<param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        ///	<param name="commandText">The stored procedure name	or PL/SQL command</param>
        ///	<param name="commandParameters">An array of	OracleParamters used to execute the command</param>
        ///	<returns>An	object containing the value	in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rollbacked	or commited, please	provide	an open	transaction.", "transaction");

            // Create a	command	and	prepare	it for execution
            OracleCommand cmd = new OracleCommand();

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // Execute the command & return	the	results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters	from the command object, so	they can be	used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// Execute an OracleCommand that returns the first column of the first record against an existing database connection
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  Object obj = ExecuteScalar(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(OracleConnection connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();

            PrepareCommand(cmd, connectionString, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Add a set of parameters to the cached
        /// </summary>
        /// <param name="cacheKey">Key value to look up the parameters</param>
        /// <param name="commandParameters">Actual parameters to cached</param>
        public static void CacheParameters(string cacheKey, params OracleParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Fetch parameters from the cache
        /// </summary>
        /// <param name="cacheKey">Key to look up the parameters</param>
        /// <returns></returns>
        public static OracleParameter[] GetCachedParameters(string cacheKey)
        {
            OracleParameter[] cachedParms = (OracleParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            // If the parameters are in the cache
            OracleParameter[] clonedParms = new OracleParameter[cachedParms.Length];

            // return a copy of the parameters
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Internal function to prepare a command for execution by the database
        /// </summary>
        /// <param name="cmd">Existing command object</param>
        /// <param name="conn">Database connection object</param>
        /// <param name="trans">Optional transaction object</param>
        /// <param name="cmdType">Command type, e.g. stored procedure</param>
        /// <param name="cmdText">Command test</param>
        /// <param name="commandParameters">Parameters for the command</param>
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] commandParameters)
        {

            //Open the connection if required
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //Set up the command
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            //Bind it to the transaction if it exists
            if (trans != null)
                cmd.Transaction = trans;

            // Bind the parameters passed in
            if (commandParameters != null)
            {
                foreach (OracleParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// Converter to use boolean data type with Oracle
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string OraBit(bool value)
        {
            if (value)
                return "Y";
            else
                return "N";
        }

        /// <summary>
        /// Converter to use boolean data type with Oracle
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static bool OraBool(string value)
        {
            if (value.Equals("Y"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnRowData">键-值存储的批量数据：键是列名称，值是对应的数据集合</param>
        /// <param name="conStr">连接字符串</param>
        /// <param name="len">每次批处理数据的大小</param>
        /// <returns></returns>
        public static int BatchInsert(string tableName, string sequence, Dictionary<string, object> columnRowData, string conStr, int len)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("必须指定批量插入的表名称", "tableName");
            }

            if (columnRowData == null || columnRowData.Count < 1)
            {
                throw new ArgumentException("必须指定批量插入的字段名称", "columnRowData");
            }

            int iResult = 0;
            string[] dbColumns = new string[columnRowData.Keys.Count];
            int _tempCount = 0;
            foreach (string key in columnRowData.Keys)
            {
                dbColumns.SetValue(key, _tempCount);
                _tempCount++;
            }
            StringBuilder sbCmdText = new StringBuilder();
            if (columnRowData.Count > 0)
            {
                //准备插入的SQL
                sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
                sbCmdText.Append("Id,");
                sbCmdText.Append(string.Join(",", dbColumns));
                sbCmdText.Append(") VALUES (");
                sbCmdText.Append(sequence + ".nextval,");
                sbCmdText.Append(":" + string.Join(",:", dbColumns));
                sbCmdText.Append(")");

                using (OracleConnection conn = new OracleConnection(conStr))
                {
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        //绑定批处理的行数
                        cmd.ArrayBindCount = len;
                        cmd.BindByName = true;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sbCmdText.ToString();
                        cmd.CommandTimeout = 600;//10分钟

                        //创建参数
                        OracleParameter oraParam;
                        List<IDbDataParameter> cacher = new List<IDbDataParameter>();
                        OracleDbType dbType = OracleDbType.Varchar2;
                        foreach (string colName in dbColumns)
                        {
                            dbType = GetOracleDbType(columnRowData[colName]);
                            oraParam = new OracleParameter(colName, dbType)
                            {
                                Direction = ParameterDirection.Input,
                                OracleDbTypeEx = dbType,
                                Value = columnRowData[colName]
                            };
                            cmd.Parameters.Add(oraParam);
                        }
                        //打开连接
                        conn.Open();

                        /*执行批处理*/
                        var trans = conn.BeginTransaction();
                        try
                        {
                            cmd.Transaction = trans;
                            iResult = cmd.ExecuteNonQuery();
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            if (conn != null) conn.Close();
                        }

                    }
                }
            }
            return iResult;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="trans">一个已存在的事务</param>
        /// <param name="tableName">表名称</param>
        /// <param name="columnRowData">键-值存储的批量数据：键是列名称，值是对应的数据集合</param>
        /// <param name="conStr">连接字符串</param>
        /// <param name="len">每次批处理数据的大小</param>
        public static int BatchInsert(OracleTransaction trans, string tableName, string sequence, Dictionary<string, object> columnRowData, int len)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("必须指定批量插入的表名称", "tableName");
            }

            if (columnRowData == null || columnRowData.Count < 1)
            {
                throw new ArgumentException("必须指定批量插入的字段名称", "columnRowData");
            }

            int iResult = 0;
            string[] dbColumns = new string[columnRowData.Keys.Count];
            int _tempCount = 0;
            foreach (string key in columnRowData.Keys)
            {
                dbColumns.SetValue(key, _tempCount);
                _tempCount++;
            }
            StringBuilder sbCmdText = new StringBuilder();
            if (columnRowData.Count > 0)
            {
                //准备插入的SQL
                sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
                sbCmdText.Append("Id,");
                sbCmdText.Append(string.Join(",", dbColumns));
                sbCmdText.Append(") VALUES (");
                sbCmdText.Append(sequence + ".nextval,");
                sbCmdText.Append(":" + string.Join(",:", dbColumns));
                sbCmdText.Append(")");


                using (OracleCommand cmd = trans.Connection.CreateCommand())
                {
                    //绑定批处理的行数
                    cmd.ArrayBindCount = len;
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sbCmdText.ToString();
                    cmd.CommandTimeout = 6000;//100分钟

                    //创建参数
                    OracleParameter oraParam;
                    List<IDbDataParameter> cacher = new List<IDbDataParameter>();
                    OracleDbType dbType = OracleDbType.Varchar2;
                    foreach (string colName in dbColumns)
                    {
                        dbType = GetOracleDbType(columnRowData[colName]);
                        oraParam = new OracleParameter(colName, dbType)
                        {
                            Direction = ParameterDirection.Input,
                            OracleDbTypeEx = dbType,
                            Value = columnRowData[colName]
                        };
                        cmd.Parameters.Add(oraParam);
                    }
                    /*执行批处理*/
                    try
                    {
                        cmd.Transaction = trans;
                        iResult = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return iResult;
        }

        /// <summary>
        /// 根据数据类型获取OracleDbType
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        private static OracleDbType GetOracleDbType(object value)
        {
            OracleDbType dataType = OracleDbType.Varchar2;
            if (value is string[])
            {
                dataType = OracleDbType.Varchar2;
            }
            else if (value is DateTime[])
            {
                dataType = OracleDbType.TimeStamp;
            }
            else if (value is int[] || value is short[])
            {
                dataType = OracleDbType.Int32;
            }
            else if (value is long[])
            {
                dataType = OracleDbType.Int64;
            }
            else if (value is decimal[] || value is double[] || value is float[])
            {
                dataType = OracleDbType.Decimal;
            }
            else if (value is Guid[])
            {
                dataType = OracleDbType.Varchar2;
            }
            else if (value is bool[] || value is Boolean[])
            {
                dataType = OracleDbType.Byte;
            }
            else if (value is byte[])
            {
                dataType = OracleDbType.Blob;
            }
            else if (value is char[])
            {
                dataType = OracleDbType.Char;
            }
            return dataType;
        }
    }
}

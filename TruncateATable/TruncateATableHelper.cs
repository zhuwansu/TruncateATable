using System;
using System.Data;
using System.Linq;
using TruncateATable.Common;
namespace TruncateATable
{
    /// <summary>
    /// 清除一张表及其关联表的数据
    /// </summary>
    public class TruncateATableHelper
    {
        #region 属性

        /// <summary>
        /// 步骤记录
        /// </summary>
        public string Log { get; set; }

        /// <summary>
        /// 记录步骤的方法
        /// </summary>
        public Action<string> LogAction { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connstr { get; set; }

        #endregion

        #region 构造方法

        public TruncateATableHelper(string log = "", Action<string> logAction = null, string connstr = null)
        {
            this.Log = log;
            this.LogAction = logAction;
            if (connstr != null) this.Connstr = connstr;
        }

        #endregion

        #region Common-Sql
        /// <summary>
        /// 获取 清除数据 的sql
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>清除某张表数据的sql</returns>
        public string GetTruncateTableSql(string tableName)
        {
            return $@"truncate table {tableName} ";
        }

        /// <summary>
        /// 获取 查询主键 的sql
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>查询 OWNER、CONSTRAINT_NAME、TABLE_NAME、COLUMN_NAME、POSITION 的SQL</returns>
        public string GetPrimaryKeySql(string tableName)
        {
            string sql = $@" select cu.* from user_cons_columns cu, user_constraints au where cu.constraint_name = au.constraint_name and au.constraint_type = 'P' and au.table_name = '{tableName.ToUpper()}' ";
            return sql;
        }

        /// <summary>
        /// 获取 查询外键 的sql 取R_CONSTRAINT_NAME
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="PkeyName">外键名</param>
        /// <returns></returns>
        public string GetForeignKeySql(string tableName, string PkeyName)
        {
            string sql = $@"select * from user_constraints a where a.constraint_type = 'R'  and a.table_name = '{tableName}' and a.R_CONSTRAINT_NAME='{PkeyName}'";
            return sql;
        }

        /// <summary>
        /// 获取 被引用的表和依赖列名 的Sql
        /// </summary>
        /// <param name="PKeyName">主键名</param>
        /// <returns>查询 TABLE_NAME、COLUMN_NAME 的SQL</returns>
        public string GetRefKeySql(string PKeyName)
        {
            string sql = $@"select b.table_name,b.column_name from user_constraints a inner join user_cons_columns b on a.constraint_name = b.constraint_name where a.r_constraint_name='{PKeyName.ToUpper()}'";
            return sql;
        }


        /// <summary>
        /// 获取 禁用外键 的SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="FKeyName">外键名</param>
        /// <returns></returns>
        public string GetDisableForeignKeySql(string tableName, string FKeyName)
        {
            string sql = $@"alter table {tableName.ToUpper()} disable constraint {FKeyName.ToUpper()}";
            return sql;
        }

        /// <summary>
        /// 获取 启用外键 的SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="FKeyName">外键名</param>
        /// <returns></returns>
        public string GetEnabledForeignKeySql(string tableName, string FKeyName)
        {
            string sql = $@"alter table {tableName.ToUpper()} enable constraint {FKeyName.ToUpper()}";
            return sql;
        }

        public string GetDataCountSql(string tableName)
        {
            var sql = $"select Count(*) from {tableName}";
            return sql;
        }

        #endregion

        #region Common-ExecuteSql

        /// <summary>
        /// 获取 主键  
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connStr">连接字符串</param>
        /// <returns>返回主键 CONSTRAINT_NAME 值，查询不到返回null</returns>
        public string GetPrimaryKey(string tableName, string connStr)
        {
            string sql = GetPrimaryKeySql(tableName);
            DataTable dt = OracleHelper.ExecuteDataSet(connStr, sql).Tables[0];
            var CONSTRAINT_NAME = dt.Rows?[0].Field<string>("CONSTRAINT_NAME");//主键 一张表只有一个主键
            return CONSTRAINT_NAME;
        }
        /// <summary>
        /// 获取 主键  
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>返回主键 CONSTRAINT_NAME 值，查询不到返回null</returns>
        public string GetPrimaryKey(string tableName)
        {
            if (this.Connstr == null)
            {
                throw new Exception("初始化连接字符串有误");
            }
            string sql = GetPrimaryKeySql(tableName);
            DataTable dt = OracleHelper.ExecuteDataSet(this.Connstr, sql).Tables[0];
            var CONSTRAINT_NAME = dt.Rows?[0].Field<string>("CONSTRAINT_NAME");//主键 一张表只有一个主键
            return CONSTRAINT_NAME;
        }
        /// <summary>
        /// 查询外键 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="PkeyName">外键名</param>
        /// <param name="connStr">连接字符串</param>
        /// <returns>返回外键名</returns>
        public string GetForeignKey(string tableName, string PkeyName, string connStr)
        {
            string sql = GetForeignKeySql(tableName, PkeyName);
            DataTable dt = OracleHelper.ExecuteDataSet(connStr, sql).Tables[0];
            var CONSTRAINT_NAME = dt.Rows?[0].Field<string>("CONSTRAINT_NAME");//外键 两张表一个外键关系
            return CONSTRAINT_NAME;
        }
        /// <summary>
        /// 查询外键 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="PkeyName">外键名</param>
        /// <returns>返回外键名</returns>
        public string GetForeignKey(string tableName, string PkeyName)
        {
            if (this.Connstr == null)
            {
                throw new Exception("初始化连接字符串有误");
            }
            string sql = GetForeignKeySql(tableName, PkeyName);
            DataTable dt = OracleHelper.ExecuteDataSet(Connstr, sql).Tables[0];
            var CONSTRAINT_NAME = dt.Rows?[0].Field<string>("R_CONSTRAINT_NAME");//外键 两张表一个外键关系
            return CONSTRAINT_NAME;
        }

        /// <summary>
        /// 获取 被引用的表   
        /// </summary>
        /// <param name="PKeyName">主键名</param>
        /// <param name="connStr">连接字符串</param>
        /// <returns>返回item1 Ref TABLE_NAME，item2 all Ref TABLE_NAME</returns>
        public Tuple<string, string[]> GetRefKey(string PKeyName, string connStr)
        {
            string sql = GetRefKeySql(PKeyName);
            DataTable dt = OracleHelper.ExecuteDataSet(connStr, sql).Tables[0];
            var refTables = dt.Select().Select(m => m.Field<string>("TABLE_NAME")).ToArray();
            return new Tuple<string, string[]>(string.Join(",", refTables), refTables);
        }

        /// <summary>
        /// 获取 被引用的表   
        /// </summary>
        /// <param name="PKeyName">主键名</param>
        /// <returns>返回item1 Ref TABLE_NAME，item2 all Ref TABLE_NAME</returns>
        public Tuple<string, string[]> GetRefKey(string PKeyName)
        {
            if (this.Connstr == null)
            {
                throw new Exception("初始化连接字符串有误");
            }
            string sql = GetRefKeySql(PKeyName);
            DataTable dt = OracleHelper.ExecuteDataSet(Connstr, sql).Tables[0];
            var CONSTRAINT_NAMEs = dt.Select().Select(m => m.Field<string>("R_CONSTRAINT_NAME")).ToArray();//外键 两张表一个外键关系
            return new Tuple<string, string[]>(string.Join(",", CONSTRAINT_NAMEs), CONSTRAINT_NAMEs);
        }

        /// <summary>
        ///  禁用外键 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="FKeyName">外键名</param>
        /// <param name="connStr">连接字符串</param>
        /// <returns></returns>
        public int GetDisableForeignKey(string tableName, string FKeyName, string connStr)
        {
            string sql = GetDisableForeignKeySql(tableName, FKeyName);
            int i = OracleHelper.ExecuteNonQuery(connStr, CommandType.Text, sql);
            return i;
        }

        /// <summary>
        ///  禁用外键 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="FKeyName">外键名</param>
        /// <returns></returns>
        public int GetDisableForeignKey(string tableName, string FKeyName)
        {
            if (this.Connstr == null)
            {
                throw new Exception("初始化连接字符串有误");
            }
            string sql = GetDisableForeignKeySql(tableName, FKeyName);
            int i = OracleHelper.ExecuteNonQuery(Connstr, CommandType.Text, sql);
            return i;
        }

        /// <summary>
        /// 获取 启用外键 的SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="FKeyName">外键名</param>
        /// <param name="connStr">连接字符串</param>
        /// <returns></returns>
        public int GetEnabledForeignKey(string tableName, string FKeyName, string connStr)
        {
            try
            {
                string sql = GetEnabledForeignKeySql(tableName, FKeyName);
                int i = OracleHelper.ExecuteNonQuery(connStr, CommandType.Text, sql);
                return i;
            }
            catch
            {
                return 0;
            }

        }

        /// <summary>
        /// 获取 启用外键 的SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="FKeyName">外键名</param>
        /// <returns></returns>
        public int GetEnabledForeignKey(string tableName, string FKeyName)
        {
            string sql = GetEnabledForeignKeySql(tableName, FKeyName);
            int i = OracleHelper.ExecuteNonQuery(Connstr, CommandType.Text, sql);
            return i;
        }

        private int TruncateTable(string tableName, string connStr)
        {
            string sql = GetTruncateTableSql(tableName);
            int i = OracleHelper.ExecuteNonQuery(connStr, CommandType.Text, sql);
            return i;
        }

        public long GetDataCount(string tableName, string connStr)
        {
            var sql = GetDataCountSql(tableName);
            long count = Convert.ToInt64(OracleHelper.ExecuteScalar(connStr, CommandType.Text, sql));
            return count;
        }
        #endregion

        #region Core

        /// <summary>
        /// 清除 一张表的数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string TruncateATable(string tableName, string connStr)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return "表名为空";
            }
            tableName = tableName.ToUpper();
            LogInfo($"准备删除表{tableName}数据");
            var count = GetDataCount(tableName, connStr);
            if (count == 0)
            {
                LogInfo($"表{tableName}无数据");
                return "true";
            }
            var pKeyName = GetPrimaryKey(tableName, connStr);
            if (string.IsNullOrEmpty(pKeyName))
            {
                LogInfo("1、表无主键请检查");
                throw new Exception("表无主键请检查");
            }
            LogInfo($"1、查询主键{pKeyName}...");
            var refTables = GetRefKey(pKeyName, connStr);
            if (string.IsNullOrEmpty(refTables.Item1))
            {
                LogInfo("2.1、主键未被引用。");
                var res = TruncateTable(tableName, connStr);
                if (res == -1)
                {
                    LogInfo($"{tableName}表数据已删除");
                    return "true";
                }
            }
            else
            {
                LogInfo($"2.2、主键被表{refTables.Item1}引用。");
                int accumulator = 0;
                foreach (var item in refTables.Item2)
                {
                    LogInfo($"查询{item}数据记录。");
                    var itemCount = GetDataCount(item, connStr);
                    LogInfo($"{item}count{itemCount}条数据记录。");
                    if (itemCount != 0)
                    {
                        string res = TruncateATable(item, connStr);
                        if (res == "true")
                        {
                            LogInfo($"表{item}count{itemCount}条数据记录已被删除。");
                            accumulator++;
                        }
                        else
                        {
                            LogInfo($"表{item}count{itemCount}条数据记录删除失败。");
                            LogInfo("Error:出现意料之外的异常");
                            throw new Exception("意料之外的异常");
                        }
                    }
                    else
                    {
                        accumulator++;
                    }

                }
                if (accumulator == refTables.Item2.Count())
                {
                    LogInfo($"表{tableName}的所有引用表{refTables.Item1}均无数据，可以禁用外键。");
                    LogInfo($"开始禁用表{tableName}对表{refTables.Item1}的外键。");
                    int disableAccumulator = 0;
                    foreach (var item in refTables.Item2)
                    {
                        var fKeyName = GetForeignKey(item, pKeyName, connStr);
                        if (string.IsNullOrEmpty(fKeyName))
                        {
                            LogInfo("Error:出现意料之外的异常");
                            throw new Exception("意料之外的异常");
                        }
                        else
                        {
                            LogInfo($"禁用表{item}的外键{fKeyName}。");
                            int res = GetDisableForeignKey(item, fKeyName, connStr);
                            if (res == -1)
                            {
                                LogInfo($"禁用表{item}的外键{fKeyName}成功。");
                                disableAccumulator++;
                            }
                            else
                            {
                                LogInfo($"禁用表{item}的外键{fKeyName}失败。");
                                LogInfo("Error:出现意料之外的异常");
                                throw new Exception("意料之外的异常");
                            }
                        }
                    }
                    if (disableAccumulator == refTables.Item2.Count())
                    {
                        LogInfo($"禁用表{tableName}对表{refTables.Item1}的外键 成功。");
                        LogInfo($"执行删除计划");
                        var i = TruncateTable(tableName, connStr);
                        if (i == -1)
                        {
                            LogInfo($"表{tableName}删除计划执行成功");
                            LogInfo($"启用表{tableName}对表{refTables.Item1}的外键");
                            int enableAccumulator = 0;
                            foreach (var item in refTables.Item2)
                            {
                                var fKeyName = GetForeignKey(item, pKeyName, connStr);
                                if (string.IsNullOrEmpty(fKeyName))
                                {
                                    LogInfo("Error:出现意料之外的异常");
                                    throw new Exception("意料之外的异常");
                                }
                                else
                                {
                                    LogInfo($"启用表{item}的外键{fKeyName}。");
                                    int res = GetEnabledForeignKey(item, fKeyName, connStr);
                                    if (res == -1)
                                    {
                                        LogInfo($"启用表{item}的外键{fKeyName}成功。");
                                        enableAccumulator++;
                                    }
                                    else
                                    {
                                        if (res == 0)
                                        {
                                            LogInfo($"启用表{item}的外键{fKeyName}失败。");
                                            enableAccumulator++;
                                        }
                                        else
                                        {
                                            LogInfo($"启用表{item}的外键{fKeyName}失败。");
                                            LogInfo("Error:出现意料之外的异常");
                                            throw new Exception("意料之外的异常");
                                        }

                                    }
                                }
                            }
                            if (enableAccumulator == refTables.Item2.Count())
                            {
                                LogInfo($"启用表{tableName}对表{refTables.Item1}的外键 成功。");
                                return "true";
                            }
                            else
                            {
                                LogInfo($"启用表{tableName}对表{refTables.Item1}的外键 部分失败。");
                                LogInfo("Error:出现意料之外的异常");
                                throw new Exception("意料之外的异常");
                            }
                        }
                        else
                        {
                            LogInfo($"表{tableName}删除计划执行失败");
                            LogInfo("Error:出现意料之外的异常");
                            throw new Exception("意料之外的异常");
                        }
                    }
                    else
                    {
                        LogInfo($"禁用表{tableName}对表{refTables.Item1}的外键 部分失败。");
                        LogInfo("Error:出现意料之外的异常");
                        throw new Exception("意料之外的异常");
                    }
                }
                else
                {
                    LogInfo("Error:出现意料之外的异常");
                    throw new Exception("意料之外的异常");
                }
            }

            LogInfo($"表{tableName}删除成功");
            return $"true";
        }

        private void LogInfo(string log)
        {
            string huiche = @"
";
            LogAction?.Invoke(log);
            log += log + huiche;
        }

        #endregion

    }
}

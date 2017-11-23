/****************************************************************************
*Copyright (c) 2017  All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：ZHUWANSU
*公司名称：
*命名空间：TruncateATable.GetValue
*文件名：  OracleHelper
*版本号：  V1.0.0.0
*唯一标识：50c46c5b-4ecc-45de-aa41-6484758a18f8
*当前的用户域：ZHUWANSU
*创建人：  朱皖苏
*电子邮箱：zhuwansu@dbgo.com
*创建时间：2017/11/17 11:29:19

*描述：
*
*=====================================================================
*修改标记
*修改时间：2017/11/17 11:29:19
*修改人： 朱皖苏
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TruncateATable.Common
{
    public partial class OracleHelper
    {
        public string Connstr { get; private set; }

        #region 常量

        #endregion

        #region 静态私有字段

        #endregion

        #region 静态构造函数

        #endregion

        #region 公共静态属性

        #endregion

        #region 公共静态方法

        #endregion

        #region 私有字段

        #endregion

        #region 构造函数

        #endregion

        #region 公共属性

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取 主键  
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connStr">连接字符串</param>
        /// <returns>返回主键 CONSTRAINT_NAME 值，查询不到返回null</returns>
        public string GetPrimaryKey(string tableName, string connStr)
        {
            string sql = this.GetPrimaryKeySql(tableName);
            DataTable dt = ExecuteDataSet(connStr, sql).Tables[0];
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
            string sql = this.GetPrimaryKeySql(tableName);
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
            string sql = this.GetForeignKeySql(tableName, PkeyName);
            DataTable dt = ExecuteDataSet(connStr, sql).Tables[0];
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
            string sql = this.GetForeignKeySql(tableName, PkeyName);
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
            string sql = this.GetRefKeySql(PKeyName);
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
            string sql = this.GetRefKeySql(PKeyName);
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
            string sql = this.GetDisableForeignKeySql(tableName, FKeyName);
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
            string sql = this.GetDisableForeignKeySql(tableName, FKeyName);
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
                string sql = this.GetEnabledForeignKeySql(tableName, FKeyName);
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

        public int TruncateTable(string tableName, string connStr)
        {
            string sql = GetTruncateTableSql(tableName);
            int i = ExecuteNonQuery(connStr, CommandType.Text, sql);
            return i;
        }

        public long GetDataCount(string tableName, string connStr)
        {
            var sql = GetDataCountSql(tableName);
            long count = Convert.ToInt64(ExecuteScalar(connStr, CommandType.Text, sql));
            return count;
        }

        #endregion

        #region 事件

        #endregion

        #region 保护方法

        #endregion

        #region 保护静态方法

        #endregion

        #region 私有静态方法

        #endregion

        #region 内部类

        #endregion

    }
}
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruncateATable.Common
{
    public partial class OracleHelper
    {

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

        /// <summary>
        /// 获取某表数量的sql
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetDataCountSql(string tableName)
        {
            var sql = $"select Count(*) from {tableName}";
            return sql;
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
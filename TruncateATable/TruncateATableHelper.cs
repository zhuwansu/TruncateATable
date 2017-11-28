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

        public OracleHelper OracleHelper { get; set; } = new OracleHelper();

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connstr { get; set; }

        /// <summary>
        /// 记录步骤的方法
        /// </summary>
        public Action<string> LogAction { get; set; }

        #endregion

        #region 构造方法

        public TruncateATableHelper(string log = "", Action<string> logAction = null, string connstr = null)
        {
            this.Log = log;
            this.LogAction = logAction;
            if (connstr != null) this.Connstr = connstr;
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
            var count = OracleHelper.GetDataCount(tableName, connStr);
            if (count == 0)
            {
                LogInfo($"表{tableName}无数据");
                return "true";
            }
            var pKeyName = OracleHelper.GetPrimaryKey(tableName, connStr);
            if (string.IsNullOrEmpty(pKeyName))
            {
                LogInfo("1、表无主键请检查");
                throw new Exception("表无主键请检查");
            }
            LogInfo($"1、查询主键{pKeyName}...");
            var refTables = OracleHelper.GetRefKey(pKeyName, connStr);
            if (string.IsNullOrEmpty(refTables.Item1))
            {
                LogInfo("2.1、主键未被引用。");
                var res = OracleHelper.TruncateTable(tableName, connStr);
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
                    var itemCount = OracleHelper.GetDataCount(item, connStr);
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
                        var fKeyName = OracleHelper.GetForeignKey(item, pKeyName, connStr);
                        if (string.IsNullOrEmpty(fKeyName))
                        {
                            LogInfo("Error:出现意料之外的异常");
                            throw new Exception("意料之外的异常");
                        }
                        else
                        {
                            LogInfo($"禁用表{item}的外键{fKeyName}。");
                            int res = OracleHelper.GetDisableForeignKey(item, fKeyName, connStr);
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
                        var i = OracleHelper.TruncateTable(tableName, connStr);
                        if (i == -1)
                        {
                            LogInfo($"表{tableName}删除计划执行成功");
                            LogInfo($"启用表{tableName}对表{refTables.Item1}的外键");
                            int enableAccumulator = 0;
                            foreach (var item in refTables.Item2)
                            {
                                var fKeyName = OracleHelper.GetForeignKey(item, pKeyName, connStr);
                                if (string.IsNullOrEmpty(fKeyName))
                                {
                                    LogInfo("Error:出现意料之外的异常");
                                    throw new Exception("意料之外的异常");
                                }
                                else
                                {
                                    LogInfo($"启用表{item}的外键{fKeyName}。");
                                    int res = OracleHelper.GetEnabledForeignKey(item, fKeyName, connStr);
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

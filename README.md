# 概述
- 针对数据库 Oracle 快速删除一个表关联链上的所有表的所有数据。即可删除该表的数据及其关联表的数据，如果其关联表自己仍有关联表的话也会被清空数据，再往下亦是如此。
- 是对 truncate 语句的扩展，truncate 语句无法对引用表执行删除操作。

## 快速开始

> **安装**
> 
> ``` nuget
> Install-Package TruncateATable -Version 1.0.0
> ```

> **调用**
> 
> ``` C#
> new TruncateATable.TruncateATableHelper().TruncateATable(tableName,connStr);
> ```

## 应用

1、制作清除数据的工具 。
2、使用清除数据工具，见 release。

## 其他

1、不会重置 sequence 。
2、通过禁用外键后执行 truncate 命令，最后回滚外键关联。

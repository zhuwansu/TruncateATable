using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TruncateATable;

namespace TruncateHelper
{
    /// <summary>
    /// 客户端测试
    /// </summary>
    public partial class TruncateTestClient : Form
    {

        /// <summary>
        /// 连接字符串模板
        /// </summary>
        private string connModel = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=@Host)(PORT=@Port))(CONNECT_DATA=(SERVICE_NAME=@ServerName)));User Id=@UserId;Password=@UserPwd;";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string conn;

        /// <summary>
        /// 登录记录
        /// </summary>
        public Dictionary<string, string> Conns { get; set; } = new Dictionary<string, string>();

        public TruncateATableHelper TruncateATableHelper { get; set; } = new TruncateATableHelper();
        public TruncateTestClient()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExecuteTruncate_Click(object sender, EventArgs e)
        {
            string tableName = this.txtTabNameForSql.Text;
            TruncateATableHelper.LogAction = WriteToResultControl;
            string sql = TruncateATableHelper.GetTruncateTableSql(tableName);
            txtResult2.AppendText($@"--目标sql：
{sql}");
            try
            {
                string sqlSteps = TruncateATableHelper.TruncateATable(tableName, conn);
                txtResult2.AppendText($@"
{sqlSteps}");
                MessageBox.Show("搞定啦！");
            }
            catch (Exception exc)
            {
                txtResult2.AppendText($@"
{ exc.Message}");
            }
        }

        /// <summary>
        /// 记录登录字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignIn_Click(object sender, EventArgs e)
        {
            string dataSource = this.txtDataSource.Text.Trim(),
                   userId = this.txtUserId.Text,
                   userPwd = this.txtPassword.Text,
                   host = "localhost", serverName = "orcl", port = "1521";
            if (dataSource.IndexOf("/") != -1 || dataSource.IndexOf(@"\") != -1)
            {
                string[] dataSource1 = dataSource.Split(@"/\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                host = dataSource1[0];
                serverName = dataSource1[1];
            }

            SetConn(host, port, serverName, userId, userPwd);

            txtResult1.ResetText();
            txtResult1.AppendText($@"
{conn}");
            txtResult1.AppendText($@"
准备完成。。");

        }

        /// <summary>
        /// 根据模板设置 conn
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="serverName"></param>
        /// <param name="userId"></param>
        /// <param name="userPwd"></param>
        public void SetConn(string host, string port, string serverName, string userId, string userPwd)
        {
            conn = connModel.Replace("@Host", host).Replace("@Port", port).Replace("@ServerName", serverName).Replace("@UserId", userId).Replace("@UserPwd", userPwd);
            string key = $"{userId}@{serverName}/{port}";
            if (!Conns.ContainsKey(key))
            {
                Conns.Add(key, conn);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetSign_Click(object sender, EventArgs e)
        {
            txtResult1.ResetText();
            conn = "";
        }

        /// <summary>
        /// 写入结果文本框
        /// </summary>
        /// <param name="text"></param>
        private void WriteToResultControl(string text)
        {
            string huiche = @"
                ";
            this.txtResult2.AppendText(text + huiche);
            txtResult2.SelectionStart = txtResult2.Text.Length;
            txtResult2.ScrollToCaret();
        }
    }
}

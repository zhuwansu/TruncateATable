namespace TruncateHelper
{
    partial class TruncateTestClient
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTabNameForSql = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExecuteTruncate = new System.Windows.Forms.Button();
            this.txtResult2 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtResult1 = new System.Windows.Forms.RichTextBox();
            this.SignOut = new System.Windows.Forms.Button();
            this.SignIn = new System.Windows.Forms.Button();
            this.txtDataSource = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tab1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tab1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTabNameForSql
            // 
            this.txtTabNameForSql.Location = new System.Drawing.Point(112, 23);
            this.txtTabNameForSql.Name = "txtTabNameForSql";
            this.txtTabNameForSql.Size = new System.Drawing.Size(442, 21);
            this.txtTabNameForSql.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Table_Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExecuteTruncate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtTabNameForSql);
            this.groupBox1.Location = new System.Drawing.Point(13, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(579, 114);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Do It";
            // 
            // btnExecuteTruncate
            // 
            this.btnExecuteTruncate.Location = new System.Drawing.Point(27, 63);
            this.btnExecuteTruncate.Name = "btnExecuteTruncate";
            this.btnExecuteTruncate.Size = new System.Drawing.Size(518, 45);
            this.btnExecuteTruncate.TabIndex = 0;
            this.btnExecuteTruncate.Text = "ExecuteTruncate";
            this.btnExecuteTruncate.UseVisualStyleBackColor = true;
            this.btnExecuteTruncate.Click += new System.EventHandler(this.BtnExecuteTruncate_Click);
            // 
            // txtResult2
            // 
            this.txtResult2.Location = new System.Drawing.Point(6, 342);
            this.txtResult2.Name = "txtResult2";
            this.txtResult2.Size = new System.Drawing.Size(616, 96);
            this.txtResult2.TabIndex = 6;
            this.txtResult2.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 327);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Result:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtResult1);
            this.groupBox3.Controls.Add(this.SignOut);
            this.groupBox3.Controls.Add(this.SignIn);
            this.groupBox3.Controls.Add(this.txtDataSource);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtPassword);
            this.groupBox3.Controls.Add(this.txtUserId);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(6, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(623, 185);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "OracleSign";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "Result:";
            // 
            // txtResult1
            // 
            this.txtResult1.Location = new System.Drawing.Point(0, 128);
            this.txtResult1.Name = "txtResult1";
            this.txtResult1.Size = new System.Drawing.Size(616, 57);
            this.txtResult1.TabIndex = 10;
            this.txtResult1.Text = "";
            // 
            // SignOut
            // 
            this.SignOut.Location = new System.Drawing.Point(477, 79);
            this.SignOut.Name = "SignOut";
            this.SignOut.Size = new System.Drawing.Size(75, 23);
            this.SignOut.TabIndex = 9;
            this.SignOut.Text = "Reset";
            this.SignOut.UseVisualStyleBackColor = true;
            this.SignOut.Click += new System.EventHandler(this.ResetSign_Click);
            // 
            // SignIn
            // 
            this.SignIn.Location = new System.Drawing.Point(396, 79);
            this.SignIn.Name = "SignIn";
            this.SignIn.Size = new System.Drawing.Size(75, 23);
            this.SignIn.TabIndex = 8;
            this.SignIn.Text = "Sign In";
            this.SignIn.UseVisualStyleBackColor = true;
            this.SignIn.Click += new System.EventHandler(this.SignIn_Click);
            // 
            // txtDataSource
            // 
            this.txtDataSource.Location = new System.Drawing.Point(103, 81);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(263, 21);
            this.txtDataSource.TabIndex = 7;
            this.txtDataSource.Text = "ORCL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "Data Source:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(103, 50);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(263, 21);
            this.txtPassword.TabIndex = 5;
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(103, 20);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(263, 21);
            this.txtUserId.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "UserName:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Pwd_Cmd:";
            // 
            // tab1
            // 
            this.tab1.Controls.Add(this.tabPage1);
            this.tab1.Location = new System.Drawing.Point(12, 12);
            this.tab1.Name = "tab1";
            this.tab1.SelectedIndex = 0;
            this.tab1.Size = new System.Drawing.Size(647, 488);
            this.tab1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.txtResult2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(639, 462);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "第一版";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TruncateTestClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 501);
            this.Controls.Add(this.tab1);
            this.Name = "TruncateTestClient";
            this.Text = "TruncateHelper";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tab1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtTabNameForSql;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExecuteTruncate;
        private System.Windows.Forms.RichTextBox txtResult2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDataSource;
        private System.Windows.Forms.Button SignOut;
        private System.Windows.Forms.Button SignIn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox txtResult1;
        private System.Windows.Forms.TabControl tab1;
        private System.Windows.Forms.TabPage tabPage1;
    }
}


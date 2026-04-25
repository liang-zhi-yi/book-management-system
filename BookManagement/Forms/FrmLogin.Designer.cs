namespace BookManagement
{
    partial class FrmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblUserName = new Label();
            txtUserName = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            cmbUserType = new ComboBox();
            btnLogin = new Button();
            btnCancel = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Italic, GraphicsUnit.Point, 134);
            lblUserName.Location = new Point(398, 311);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(182, 52);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "用户名：";
            lblUserName.Click += label1_Click;
            // 
            // txtUserName
            // 
            txtUserName.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Italic);
            txtUserName.Location = new Point(651, 305);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(294, 58);
            txtUserName.TabIndex = 1;
            txtUserName.TextChanged += textBox1_TextChanged;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Italic);
            lblPassword.Location = new Point(398, 425);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(142, 52);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "密码：";
            lblPassword.Click += label1_Click_1;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Italic);
            txtPassword.Location = new Point(651, 425);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(294, 58);
            txtPassword.TabIndex = 3;
            // 
            // cmbUserType
            // 
            cmbUserType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUserType.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Italic);
            cmbUserType.FormattingEnabled = true;
            cmbUserType.Location = new Point(651, 551);
            cmbUserType.Name = "cmbUserType";
            cmbUserType.Size = new Size(294, 60);
            cmbUserType.TabIndex = 4;
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Italic);
            btnLogin.Location = new Point(398, 694);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(150, 68);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "登录";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Italic);
            btnCancel.Location = new Point(900, 694);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 68);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Italic);
            label1.Location = new Point(358, 554);
            label1.Name = "label1";
            label1.Size = new Size(222, 52);
            label1.TabIndex = 7;
            label1.Text = "用户类型：";
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1404, 963);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnLogin);
            Controls.Add(cmbUserType);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUserName);
            Controls.Add(lblUserName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "图书管理系统-登录";
            Load += FrmLogin_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUserName;
        private TextBox txtUserName;
        private Label lblPassword;
        private TextBox txtPassword;
        private ComboBox cmbUserType;
        private Button btnLogin;
        private Button btnCancel;
        private Label label1;
    }
}

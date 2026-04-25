using BookManagement.Forms;

namespace BookManagement
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            cmbUserType.Items.Clear();

            // 2. 添加两个选项
            cmbUserType.Items.Add("系统管理员");
            cmbUserType.Items.Add("读者");

            // 3. 设置默认选中第一项（“系统管理员”）
            cmbUserType.SelectedIndex = 0;

            // （可选）设置默认测试账号，方便调试
            txtUserName.Text = "admin";
            txtPassword.Text = "123456";

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim(); // 去除首尾空格
            string password = txtPassword.Text;
            int userType = cmbUserType.SelectedIndex; // 假设下拉框选项顺序：0-管理员, 1-读者

            // 2. 输入验证（前端校验）
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("请输入用户名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus(); // 将光标聚焦到用户名输入框
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // 3. 调用业务逻辑层（BLL）进行登录验证
            try
            {
                BookManagement.BLL.UserManager userManager = new BookManagement.BLL.UserManager();
                BookManagement.Model.User user = userManager.Login(userName, password);

                // 4. 验证用户类型是否匹配
                if (user.UserType != userType)
                {
                    string expectedType = userType == 0 ? "系统管理员" : "读者";
                    string actualType = user.UserType == 0 ? "系统管理员" : "读者";
                    MessageBox.Show($"用户类型不匹配！您选择的是{expectedType}，但该账户是{actualType}。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 5. 登录成功
                MessageBox.Show($"登录成功！欢迎您，{user.RealName}", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 6. 跳转到主界面
                FrmMain mainForm = new FrmMain(user); // 将登录用户信息传递给主窗体
                this.Hide(); // 隐藏登录窗口
                mainForm.ShowDialog(); // 以模态方式显示主窗体
                this.Close(); // 关闭登录窗体
            }
            catch (Exception ex)
            {
                // 捕获BLL层抛出的异常（如：用户名密码错误）
                MessageBox.Show(ex.Message, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

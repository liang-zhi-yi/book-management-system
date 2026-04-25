using System;
using System.Windows.Forms;
using BookManagement.Model;

namespace BookManagement.Forms
{
    public partial class FrmMain : Form
    {
        // 当前登录用户
        private User currentUser;

        // 默认构造函数
        public FrmMain()
        {
            InitializeComponent();
        }

        // 带用户参数的构造函数（登录成功后调用）
        public FrmMain(User user) : this()
        {
            currentUser = user;
        }
        private void label1_Click(object sender, EventArgs e)
        {
            // 这个方法什么都不做，只是为了满足设计器的要求
        }

        // 窗体加载事件
        private void FrmMain_Load(object sender, EventArgs e)
        {
            // 根据用户类型设置界面
            if (currentUser != null)
            {
                if (currentUser.UserType == 0) // 系统管理员
                {
                    this.Text = $"图书管理系统 - 管理员 ({currentUser.RealName})";
                    // 管理员可以看到所有菜单，无需隐藏
                }
                else // 读者
                {
                    this.Text = $"图书管理系统 - 读者 ({currentUser.RealName})";
                    // 读者隐藏系统管理菜单

                }
            }
        }

        // 图书类别管理菜单项点击事件
        private void tsmiBookCategoryManage_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建并显示图书类别管理窗体
                FrmBookCategoryManage frm = new FrmBookCategoryManage();
                frm.ShowDialog(); // 以模态对话框形式打开
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开图书类别管理窗体失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 图书管理菜单项点击事件（预留，待实现）
        private void tsmiBookManage_Click(object sender, EventArgs e)
        {

            FrmBookManage frm = new FrmBookManage();
            frm.ShowDialog();

        }

        // 读者类别管理菜单项点击事件（预留，待实现）
        private void tsmiReaderCategoryManage_Click(object sender, EventArgs e)
        {
            FrmReaderCategoryManage frm = new FrmReaderCategoryManage();
            frm.ShowDialog();  // 或者 frm.Show();
        }


        // 读者管理菜单项点击事件（预留，待实现）
        /// <summary>
        /// 读者管理菜单项点击事件
        /// </summary>
        private void tsmiReaderManage_Click(object sender, EventArgs e)
        {
            try
            {

                // 创建并显示读者管理窗体
                FrmReaderManage frm = new FrmReaderManage();
                frm.ShowDialog();  // 以模态对话框形式打开
            }
            catch (Exception ex)
            {
                // 捕获异常，避免程序崩溃
                MessageBox.Show($"打开读者管理窗体失败：\n{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 图书借阅菜单项点击事件
        private void tsmiBorrowBook_Click(object sender, EventArgs e)
        {
            try
            {
                FrmBorrowBook frm = new FrmBorrowBook();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开图书借阅窗体失败：{ex.Message}", "错误");
            }
        }

        // 图书归还菜单项点击事件
        private void tsmiReturnBook_Click(object sender, EventArgs e)
        {
            try
            {
                FrmReturnBook frm = new FrmReturnBook();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开图书归还窗体失败：{ex.Message}", "错误");
            }
        }

        // 借阅记录菜单项点击事件
        private void tsmiBorrowRecord_Click(object sender, EventArgs e)
        {
            try
            {
                FrmBorrowRecord frm = new FrmBorrowRecord();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开借阅记录窗体失败：{ex.Message}", "错误");
            }
        }


        // 退出系统菜单项点击事件
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            // 确认是否退出
            DialogResult result = MessageBox.Show("确定要退出系统吗？", "确认退出",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit(); // 退出应用程序
            }
        }

        // 窗体关闭事件
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // 确保程序完全退出
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }
    }
}

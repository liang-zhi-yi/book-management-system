using System;
using System.Windows.Forms;
using BookManagement.Forms;  // 添加这行

namespace BookManagement
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 启动登录窗体
            Application.Run(new FrmLogin());
        }
    }
}
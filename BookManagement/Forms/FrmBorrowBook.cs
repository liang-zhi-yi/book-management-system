using System;
using System.Data;
using System.Windows.Forms;
using BookManagement.BLL;
using BookManagement.Model;

namespace BookManagement.Forms
{
    public partial class FrmBorrowBook : Form
    {
        /// <summary>
        /// 安全地从数据库读取字符串
        /// </summary>
        private string GetSafeString(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;
            return value.ToString();
        }

        /// <summary>
        /// 安全地从数据库读取整数
        /// </summary>
        private int GetSafeInt(object value, int defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (int.TryParse(value.ToString(), out int result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 安全地从数据库读取日期
        /// </summary>
        private DateTime GetSafeDateTime(object value, DateTime defaultValue)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (DateTime.TryParse(value.ToString(), out DateTime result))
                return result;

            return defaultValue;
        }


        // 业务逻辑对象
        private BorrowManager borrowManager = new BorrowManager();
        private ReaderManager readerManager = new ReaderManager();
        private BookManager bookManager = new BookManager();

        // 当前选中的读者和图书
        private int currentReaderId = 0;
        private int currentBookId = 0;

        public FrmBorrowBook()
        {
            InitializeComponent();
        }

        private void FrmBorrowBook_Load(object sender, EventArgs e)
        {
            // 设置默认日期
            dtpBorrowDate.Value = DateTime.Now;
            dtpDueDate.Value = DateTime.Now.AddDays(30); // 默认借期30天

            // 初始化界面
            ClearReaderInfo();
            ClearBookInfo();
        }

        // ====== 读者相关功能 ======

        /// <summary>
        /// 查询读者按钮点击事件
        /// </summary>
        private void btnSearchReader_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtReaderId.Text.Trim()))
            {
                MessageBox.Show("请输入读者ID！", "提示");
                txtReaderId.Focus();
                return;
            }

            try
            {
                int readerId = Convert.ToInt32(txtReaderId.Text.Trim());

                // 查询读者信息
                DataTable readerDt = readerManager.GetReaderById(readerId);

                if (readerDt == null || readerDt.Rows.Count == 0)
                {
                    MessageBox.Show("读者不存在！", "提示");
                    ClearReaderInfo();
                    return;
                }

                DataRow readerRow = readerDt.Rows[0];
                currentReaderId = readerId;

                // 安全获取读者姓名
                lblReaderName.Text = GetSafeString(readerRow["ReaderName"]);

                // 安全获取读者类别ID
                int categoryId = GetSafeInt(readerRow["ReaderCategoryId"], 0);

                if (categoryId > 0)
                {
                    DataTable categoryDt = readerManager.GetReaderCategoryById(categoryId);
                    if (categoryDt != null && categoryDt.Rows.Count > 0)
                    {
                        // 显示读者类别名称
                        lblReaderCategory.Text = GetSafeString(categoryDt.Rows[0]["CategoryName"]);

                        // 获取最大借阅数
                        int maxBorrow = GetSafeInt(categoryDt.Rows[0]["MaxBorrowCount"], 5);
                        lblMaxBorrow.Text = maxBorrow.ToString();
                    }
                    else
                    {
                        lblReaderCategory.Text = "未知";
                        lblMaxBorrow.Text = "5"; // 默认值
                    }
                }
                else
                {
                    lblReaderCategory.Text = "未设置";
                    lblMaxBorrow.Text = "5"; // 默认值
                }

                // 获取当前借阅情况
                DataTable borrowDt = borrowManager.GetReaderBorrowInfo(readerId);
                int currentBorrowCount = borrowDt?.Rows.Count ?? 0;
                lblTotalBorrowed.Text = currentBorrowCount.ToString();

                // 计算还可借阅数量
                int maxBorrowValue = GetSafeInt(lblMaxBorrow.Text, 5);
                int availableCount = maxBorrowValue - currentBorrowCount;

                // 确保可借数量不为负数
                if (availableCount < 0) availableCount = 0;
                lblBorrowedCount.Text = availableCount.ToString();

                // 加载当前借阅记录
                LoadCurrentBorrowRecords(borrowDt);

                // 检查读者状态
                int status = GetSafeInt(readerRow["Status"], 0);
                if (status != 0) // 0:正常
                {
                    string statusText = GetReaderStatusText(status);
                    MessageBox.Show($"读者状态：{statusText}，无法借书！", "提示");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("读者ID必须是数字！", "输入错误");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询读者失败：{ex.Message}", "错误");
            }
        }



        /// <summary>
        /// 加载当前借阅记录到DataGridView
        /// </summary>
        private void LoadCurrentBorrowRecords(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                dgvCurrentBorrow.DataSource = null;
                return;
            }

            dgvCurrentBorrow.DataSource = dt;

            // 设置列宽
            dgvCurrentBorrow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 设置列标题
            if (dgvCurrentBorrow.Columns.Contains("BorrowDate"))
                dgvCurrentBorrow.Columns["BorrowDate"].HeaderText = "借阅日期";
            if (dgvCurrentBorrow.Columns.Contains("DueDate"))
                dgvCurrentBorrow.Columns["DueDate"].HeaderText = "应还日期";
            if (dgvCurrentBorrow.Columns.Contains("BookName"))
                dgvCurrentBorrow.Columns["BookName"].HeaderText = "图书名称";
            if (dgvCurrentBorrow.Columns.Contains("ISBN"))
                dgvCurrentBorrow.Columns["ISBN"].HeaderText = "ISBN";

            // 添加状态列
            if (dgvCurrentBorrow.Columns.Contains("Status"))
            {
                dgvCurrentBorrow.Columns["Status"].Visible = false; // 隐藏状态代码
            }
        }

        /// <summary>
        /// 清空读者信息
        /// </summary>
        private void ClearReaderInfo()
        {
            currentReaderId = 0;
            lblReaderName.Text = "";
            lblReaderCategory.Text = "";
            lblBorrowedCount.Text = "";
            lblTotalBorrowed.Text = "0";
            lblMaxBorrow.Text = "0";
            dgvCurrentBorrow.DataSource = null;
        }

        // ====== 图书相关功能 ======

        /// <summary>
        /// 查询图书按钮点击事件
        /// </summary>
        private void btnSearchBook_Click(object sender, EventArgs e)
        {
            // 添加调试信息
            

            if (string.IsNullOrEmpty(txtBookId.Text.Trim()))
            {
                MessageBox.Show("请输入图书ID或ISBN！", "提示");
                txtBookId.Focus();
                return;
            }

            try
            {
                string input = txtBookId.Text.Trim();
                DataTable bookDt = null;

                // 显示正在查询的信息
                

                // 判断输入是ID还是ISBN
                if (int.TryParse(input, out int bookId))
                {
                    // 按图书ID查询
                    bookDt = bookManager.GetBookById(bookId);
                    
                }
                else
                {
                    // 按ISBN查询
                    bookDt = bookManager.GetBookByISBN(input);
                    
                }

                if (bookDt == null)
                {
                    MessageBox.Show("查询结果为空！", "调试");
                    ClearBookInfo();
                    return;
                }

                MessageBox.Show($"查询到 {bookDt.Rows.Count} 条记录", "调试");

                if (bookDt.Rows.Count == 0)
                {
                    MessageBox.Show("图书不存在！", "提示");
                    ClearBookInfo();
                    return;
                }

                // 显示图书信息
                DataRow bookRow = bookDt.Rows[0];
                currentBookId = Convert.ToInt32(bookRow["BookId"]);
                lblBookName.Text = bookRow["BookName"].ToString();
                lblAuthor.Text = bookRow["Author"].ToString();
                lblPublisher.Text = bookRow["Publisher"].ToString();
                lblStock.Text = bookRow["TotalCount"].ToString();
                lblAvailableCount.Text = bookRow["AvailableCount"].ToString();

                MessageBox.Show("图书信息显示完成！", "调试");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询图书失败：{ex.Message}\n{ex.StackTrace}", "错误");
            }
        }

        /// <summary>
        /// 清空图书信息
        /// </summary>
        private void ClearBookInfo()
        {
            currentBookId = 0;
            lblBookName.Text = "";
            lblAuthor.Text = "";
            lblPublisher.Text = "";
            lblStock.Text = "";
            lblAvailableCount.Text = "";
        }

        // ====== 借书功能 ======

        /// <summary>
        /// 借书按钮点击事件
        /// </summary>
        private void btnBorrow_Click(object sender, EventArgs e)
        {
            // 验证读者信息
            if (currentReaderId == 0)
            {
                MessageBox.Show("请先查询读者信息！", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReaderId.Focus();
                return;
            }

            // 验证图书信息
            if (currentBookId == 0)
            {
                MessageBox.Show("请先查询图书信息！", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBookId.Focus();
                return;
            }

            // 验证借阅日期
            if (dtpDueDate.Value <= dtpBorrowDate.Value)
            {
                MessageBox.Show("应还日期必须晚于借阅日期！", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDueDate.Focus();
                return;
            }

            try
            {
                // 执行借书操作
                bool success = borrowManager.BorrowBook(
                    currentReaderId,
                    currentBookId,
                    dtpBorrowDate.Value,
                    dtpDueDate.Value
                );

                if (success)
                {
                    MessageBox.Show("借书成功！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 重置界面
                    ClearBookInfo();
                    txtBookId.Clear();

                    // 刷新读者信息
                    btnSearchReader_Click(null, null);
                }
                else
                {
                    MessageBox.Show("借书失败，请检查读者状态和图书库存！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"借书失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 重置按钮点击事件
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearReaderInfo();
            ClearBookInfo();
            txtReaderId.Clear();
            txtBookId.Clear();
            txtRemark.Clear();
            dtpBorrowDate.Value = DateTime.Now;
            dtpDueDate.Value = DateTime.Now.AddDays(30);
            txtReaderId.Focus();
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 获取读者状态文字
        /// </summary>
        private string GetReaderStatusText(int status)
        {
            switch (status)
            {
                case 0: return "正常";
                case 1: return "挂失";
                case 2: return "注销";
                default: return "未知";
            }
        }

        private void LoadCurrentBorrowRecord(object sender, EventArgs e)
        {

        }
    }
}

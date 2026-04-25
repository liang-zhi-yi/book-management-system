using BookManagement.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookManagement.Forms
{
    public partial class FrmReturnBook : Form
    {
        private BorrowManager borrowManager = new BorrowManager();
        private ReaderManager readerManager = new ReaderManager();
        private BookManager bookManager = new BookManager();

        private int currentBorrowId = 0;  // 当前选中的借阅记录ID
        private int currentReaderId = 0;  // 当前读者ID
        private int currentBookId = 0;    // 当前图书ID
        private DateTime dueDate;         // 应还日期
        private decimal lateFeePerDay = 0.5m;  // 默认逾期费率
        public FrmReturnBook()
        {
            InitializeComponent();
        }

        private void FrmReturnBook_Load(object sender, EventArgs e)
        {
            // 设置默认日期
            dtpReturnDate.Value = DateTime.Now;

            // 初始禁用计算和归还按钮
            btnCalculate.Enabled = false;
            btnReturn.Enabled = false;
            AddTestDataToGridView();
        }

        /// <summary>
        /// 向DataGridView添加测试数据
        /// </summary>
        private void AddTestDataToGridView()
        {
            try
            {
                // 创建DataTable
                DataTable dt = new DataTable();

                // 添加列（与数据库列对应）
                dt.Columns.Add("BorrowId", typeof(int));
                dt.Columns.Add("ReaderName", typeof(string));
                dt.Columns.Add("BookName", typeof(string));
                dt.Columns.Add("ISBN", typeof(string));
                dt.Columns.Add("BorrowDate", typeof(DateTime));
                dt.Columns.Add("DueDate", typeof(DateTime));
                dt.Columns.Add("ReaderId", typeof(int));
                dt.Columns.Add("Status", typeof(string));

                // 添加测试行
                dt.Rows.Add(1, "王五", "C#编程入门", "978-7-111-12345-6",
                    DateTime.Now.AddDays(-10), DateTime.Now.AddDays(5), 1, "借出中");
                dt.Rows.Add(2, "张三", "数据库原理", "978-7-222-23456-7",
                    DateTime.Now.AddDays(-5), DateTime.Now.AddDays(3), 2, "借出中");
                dt.Rows.Add(3, "李四", "数据结构与算法", "978-7-333-34567-8",
                    DateTime.Now.AddDays(-20), DateTime.Now.AddDays(-2), 3, "逾期");
                dt.Rows.Add(4, "赵六", "计算机网络", "978-7-444-45678-9",
                    DateTime.Now.AddDays(-15), DateTime.Now, 4, "即将到期");
                dt.Rows.Add(5, "钱七", "操作系统", "978-7-555-56789-0",
                    DateTime.Now.AddDays(-3), DateTime.Now.AddDays(7), 5, "借出中");
                dgvBorrowList.DataSource = dt;

                // 设置列宽
                dgvBorrowList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // 设置列标题
                dgvBorrowList.Columns["BorrowId"].HeaderText = "借阅号";
                dgvBorrowList.Columns["ReaderName"].HeaderText = "读者姓名";
                dgvBorrowList.Columns["BookName"].HeaderText = "图书名称";
                dgvBorrowList.Columns["ISBN"].HeaderText = "ISBN";
                dgvBorrowList.Columns["BorrowDate"].HeaderText = "借阅日期";
                dgvBorrowList.Columns["DueDate"].HeaderText = "应还日期";
                dgvBorrowList.Columns["Status"].HeaderText = "状态";

                // 隐藏ReaderId列（不需要显示）
                dgvBorrowList.Columns["ReaderId"].Visible = false;

                // 设置日期格式
                dgvBorrowList.Columns["BorrowDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                dgvBorrowList.Columns["DueDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                foreach (DataGridViewRow row in dgvBorrowList.Rows)
                {
                    if (row.Cells["Status"].Value != null)
                    {
                        string status = row.Cells["Status"].Value.ToString();
                        if (status == "逾期")
                        {
                            row.DefaultCellStyle.BackColor = Color.LightPink;
                            row.DefaultCellStyle.ForeColor = Color.DarkRed;
                        }
                        else if (status == "即将到期")
                        {
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            row.DefaultCellStyle.ForeColor = Color.DarkOrange;
                        }
                    }
                }

                MessageBox.Show($"已加载 {dt.Rows.Count} 条测试借阅记录", "测试数据");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加测试数据失败：{ex.Message}", "错误");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            { int? readerId = null;
                string isbn = null;
                int? borrowId = null;
                if (!string.IsNullOrEmpty(txtReaderId.Text.Trim()))
                {
                    if (int.TryParse(txtReaderId.Text.Trim(), out int rid))
                        readerId = rid;
                }

                if (!string.IsNullOrEmpty(txtISBN.Text.Trim()))
                    isbn = txtISBN.Text.Trim();

                if (!string.IsNullOrEmpty(txtBorrowId.Text.Trim()))
                {
                    if (int.TryParse(txtBorrowId.Text.Trim(), out int bid))
                        borrowId = bid;
                }
                if (!readerId.HasValue && string.IsNullOrEmpty(isbn) && !borrowId.HasValue)
                {
                    MessageBox.Show("请输入至少一个查询条件！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 调用BLL层查询
                DataTable dt = borrowManager.SearchBorrowRecords(readerId, isbn, borrowId);
                dgvBorrowList.DataSource = dt;
                dgvBorrowList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("没有找到借阅记录！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void dgvBorrowList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBorrowList.SelectedRows.Count == 0)
                return;

            try
            {
                DataGridViewRow row = dgvBorrowList.SelectedRows[0];

                // 获取借阅记录ID
                currentBorrowId = Convert.ToInt32(row.Cells["BorrowId"].Value);
                currentReaderId = Convert.ToInt32(row.Cells["ReaderId"].Value);

                // 获取读者信息
                DataTable readerDt = readerManager.GetReaderById(currentReaderId);
                if (readerDt != null && readerDt.Rows.Count > 0)
                {
                    lblReaderName.Text = GetSafeString(readerDt.Rows[0]["ReaderName"]);
                }

                // 获取借阅详情
                lblBookName.Text = GetSafeString(row.Cells["BookName"].Value);
                lblBorrowDate.Text = Convert.ToDateTime(row.Cells["BorrowDate"].Value).ToString("yyyy-MM-dd");
                lblDueDate.Text = Convert.ToDateTime(row.Cells["DueDate"].Value).ToString("yyyy-MM-dd");
                dueDate = Convert.ToDateTime(row.Cells["DueDate"].Value);

                // 获取图书ID
                string isbn = GetSafeString(row.Cells["ISBN"].Value);
                DataTable bookDt = bookManager.GetBookByISBN(isbn);
                if (bookDt != null && bookDt.Rows.Count > 0)
                {
                    currentBookId = Convert.ToInt32(bookDt.Rows[0]["BookId"]);
                }

                // 获取读者类别以确定逾期费率
                if (readerDt != null && readerDt.Rows.Count > 0)
                {
                    int categoryId = GetSafeInt(readerDt.Rows[0]["ReaderCategoryId"], 0);
                    if (categoryId > 0)
                    {
                        DataTable categoryDt = readerManager.GetReaderCategoryById(categoryId);
                        if (categoryDt != null && categoryDt.Rows.Count > 0)
                        {
                            lateFeePerDay = GetSafeDecimal(categoryDt.Rows[0]["LateFeePerDay"], 0.5m);
                        }
                    }
                }

                // 启用按钮
                btnCalculate.Enabled = true;
                btnReturn.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载借阅详情失败：{ex.Message}", "错误");
            }

        }

        private void btnResetForm_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 只清空查询条件
                txtReaderId.Clear();      // 清空读者ID
                txtISBN.Clear();          // 清空图书ISBN
                txtBorrowId.Clear();      // 清空借阅号

                // 2. 不清空其他数据（借阅记录列表、借阅详情保持不变）
                // 这样用户可以保留之前的查询结果，只修改查询条件重新查询

                // 3. 将焦点设置到第一个输入框
                txtReaderId.Focus();

                // 4. 显示提示（可选）
                // MessageBox.Show("查询条件已清空，可重新输入", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重置查询条件失败：{ex.Message}", "错误");
            }
        }

        

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime returnDate = dtpReturnDate.Value;

                // 计算逾期天数
                int overdueDays = 0;
                if (returnDate > dueDate)
                {
                    overdueDays = (returnDate - dueDate).Days;
                }

                // 计算逾期费用
                decimal lateFee = overdueDays * lateFeePerDay;

                // 显示结果
                lblOverdueDays.Text = overdueDays.ToString();
                lblLateFee.Text = lateFee.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算逾期费用失败：{ex.Message}", "错误");
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (currentBorrowId == 0)
            {
                MessageBox.Show("请先选择要归还的借阅记录！", "提示");
                return;
            }
            try
            {
                DateTime returnDate = dtpReturnDate.Value;
                string remark = txtRemark.Text.Trim();

                // 计算逾期天数
                int overdueDays = 0;
                decimal lateFee = 0;

                if (returnDate > dueDate)
                {
                    overdueDays = (returnDate - dueDate).Days;
                    lateFee = overdueDays * lateFeePerDay;
                }

                // 执行还书
                var result = borrowManager.ReturnBook(currentBorrowId, returnDate, lateFee, remark);

                if (result.success)
                {
                    MessageBox.Show($"还书成功！\n逾期天数：{overdueDays}天\n逾期费用：{lateFee:0.00}元", "提示");
                    ResetForm();

                    // 重新查询
                    btnSearch_Click(null, null);
                }
                else
                {
                    MessageBox.Show(result.message, "错误");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"还书失败：{ex.Message}", "错误");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResetForm()
        {
            txtReaderId.Clear();
            txtISBN.Clear();
            txtBorrowId.Clear();
            txtRemark.Clear();

            dgvBorrowList.DataSource = null;

            lblReaderName.Text = "";
            lblBookName.Text = "";
            lblBorrowDate.Text = "";
            lblDueDate.Text = "";
            lblOverdueDays.Text = "";
            lblLateFee.Text = "";

            dtpReturnDate.Value = DateTime.Now;

            currentBorrowId = 0;
            currentReaderId = 0;
            currentBookId = 0;

            btnCalculate.Enabled = false;
            btnReturn.Enabled = false;

            txtReaderId.Focus();
        }
        private string GetSafeString(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;
            return value.ToString();
        }

        private int GetSafeInt(object value, int defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (int.TryParse(value.ToString(), out int result))
                return result;

            return defaultValue;
        }

        private decimal GetSafeDecimal(object value, decimal defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            if (decimal.TryParse(value.ToString(), out decimal result))
                return result;

            return defaultValue;
        }
    }
}

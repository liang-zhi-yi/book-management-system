using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BookManagement.BLL;
using BookManagement.Model;

namespace BookManagement.Forms
{
    public partial class FrmBorrowRecord : Form
    {
        // 业务逻辑对象
        private BorrowManager borrowManager = new BorrowManager();
        private BookManager bookManager = new BookManager();

        // 分页相关变量
        private int currentPage = 1;      // 当前页码
        private int pageSize = 20;        // 每页记录数
        private int totalRecords = 0;     // 总记录数
        private int totalPages = 1;       // 总页数
        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitializeComboBoxes()
        {
            try
            {
                // 借阅状态下拉框
                cmbStatus.Items.Clear();
                cmbStatus.Items.Add("全部");
                cmbStatus.Items.Add("借出中");
                cmbStatus.Items.Add("已归还");
                cmbStatus.Items.Add("逾期");

                // 注意：这里不要立即设置SelectedIndex，在Load事件中设置

                // 图书类别下拉框
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("全部");
                // 不在这里设置SelectedIndex
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化下拉框失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public FrmBorrowRecord()
        {
            
            InitializeComponent();
        }

        // ========== 窗体加载事件 ==========
        private void FrmBorrowRecord_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeComboBoxes();     // 初始化下拉框
                SetDefaultDates();          // 设置默认日期
                LoadBookCategories();       // 加载图书类别

                // 在确保下拉框有项目后，再设置选中项
                if (cmbStatus.Items.Count > 0)
                    cmbStatus.SelectedIndex = 0;

                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;

                LoadBorrowRecords();        // 首次加载数据
            }
            catch (Exception ex)
            {
                MessageBox.Show($"窗体加载失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置默认日期范围
        /// </summary>
        private void SetDefaultDates()
        {
            dtpBorrowFrom.Value = DateTime.Now.AddMonths(-1);
            dtpBorrowTo.Value = DateTime.Now;

            // 归还日期：留空表示不过滤
            dtpReturnFrom.Value = DateTime.Now.AddYears(-1);
            dtpReturnTo.Value = DateTime.Now;
        }

        /// <summary>
        /// 加载图书类别到下拉框
        /// </summary>
        /// <summary>
        /// 加载图书类别到下拉框
        /// </summary>
        private void LoadBookCategories()
        {
            try
            {
                // 先清空，但保留"全部"选项
                int selectedIndex = cmbCategory.SelectedIndex;

                // 确保有"全部"选项
                if (!cmbCategory.Items.Contains("全部"))
                {
                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("全部");
                }
                else
                {
                    // 如果已有"全部"，清空其他选项
                    while (cmbCategory.Items.Count > 1)
                    {
                        cmbCategory.Items.RemoveAt(1);
                    }
                }

                DataTable dt = bookManager.GetAllCategories();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["CategoryName"] != null && !string.IsNullOrEmpty(row["CategoryName"].ToString()))
                        {
                            cmbCategory.Items.Add(row["CategoryName"].ToString());
                        }
                    }
                }

                // 恢复选中项，但要确保不超出范围
                if (cmbCategory.Items.Count > 0)
                {
                    if (selectedIndex >= 0 && selectedIndex < cmbCategory.Items.Count)
                    {
                        cmbCategory.SelectedIndex = selectedIndex;
                    }
                    else
                    {
                        cmbCategory.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图书类别失败：{ex.Message}\n\n请检查数据库连接和BookCategory表。", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 确保至少有"全部"选项
                if (cmbCategory.Items.Count == 0)
                {
                    cmbCategory.Items.Add("全部");
                }
            }
        }


        // ========== 数据操作方法 ==========

        /// <summary>
        /// 加载借阅记录
        /// </summary>
        private void LoadBorrowRecords()
        {
            try
            {
                // 显示加载提示
                Cursor = Cursors.WaitCursor;

                var conditions = GetSearchConditions();

                // 调用BLL层查询
                DataTable dt = borrowManager.SearchAllBorrowRecords(
                    conditions.readerId,
                    conditions.readerName,
                    conditions.bookName,
                    conditions.isbn,
                    conditions.status,
                    conditions.category,
                    conditions.borrowFrom,
                    conditions.borrowTo,
                    conditions.returnFrom,
                    conditions.returnTo,
                    currentPage,
                    pageSize,
                    out totalRecords
                ); dgvBorrowRecord.DataSource = dt;

                // 配置DataGridView列
                ConfigureDataGridViewColumns();

                // 更新统计信息
                UpdateStatistics(dt);

                // 更新分页信息
                UpdatePaginationInfo();
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("没有找到符合条件的借阅记录！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载借阅记录失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;  // 恢复光标
            }
        }

        /// <summary
        private (int? readerId, string readerName, string bookName, string isbn,
        int? status, string category,
        DateTime? borrowFrom, DateTime? borrowTo,
        DateTime? returnFrom, DateTime? returnTo) GetSearchConditions()
        {
            int? readerId = null;
            if (!string.IsNullOrEmpty(txtReaderId.Text.Trim()))
            {
                if (int.TryParse(txtReaderId.Text.Trim(), out int id))
                    readerId = id;
            }

            string readerName = txtReaderName.Text.Trim();
            string bookName = txtBookName.Text.Trim();
            string isbn = txtISBN.Text.Trim();

            int? status = null;
            if (cmbStatus.SelectedIndex > 0)
            {
                status = cmbStatus.SelectedIndex - 1; // 0:借出中, 1:已归还, 2:逾期
            }

            string category = null;
            if (cmbCategory.SelectedIndex > 0)
            {
                category = cmbCategory.Text;
            }

            DateTime? borrowFrom = dtpBorrowFrom.Value.Date;
            DateTime? borrowTo = dtpBorrowTo.Value.Date.AddDays(1).AddSeconds(-1);

            DateTime? returnFrom = null;
            DateTime? returnTo = null;

            if (dtpReturnFrom.Value.Year > 1900)
                returnFrom = dtpReturnFrom.Value.Date;
            if (dtpReturnTo.Value.Year > 1900)
                returnTo = dtpReturnTo.Value.Date.AddDays(1).AddSeconds(-1);

            return (readerId, readerName, bookName, isbn, status,
                    category, borrowFrom, borrowTo,
                    returnFrom, returnTo);
        }


        /// <summary>
        /// 更新分页按钮状态
        /// </summary>
        private void ConfigureDataGridViewColumns()
        {
            if (dgvBorrowRecord.Columns.Count == 0)
                return;
            dgvBorrowRecord.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            foreach (DataGridViewColumn column in dgvBorrowRecord.Columns)
            {
                switch (column.Name)
                {
                    case "BorrowId":
                        column.HeaderText = "借阅号";
                        column.Width = 60;
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "ReaderName":
                        column.HeaderText = "读者姓名";
                        column.Width = 100;
                        break;
                    case "BookName":
                        column.HeaderText = "图书名称";
                        column.Width = 150;
                        break;
                    case "ISBN":
                        column.HeaderText = "ISBN";
                        column.Width = 100;
                        break;
                    case "BorrowDate":
                        column.HeaderText = "借阅日期";
                        column.Width = 100;
                        column.DefaultCellStyle.Format = "yyyy-MM-dd";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "DueDate":
                        column.HeaderText = "应还日期";
                        column.Width = 100;
                        column.DefaultCellStyle.Format = "yyyy-MM-dd";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "ReturnDate":
                        column.HeaderText = "归还日期";
                        column.Width = 100;
                        column.DefaultCellStyle.Format = "yyyy-MM-dd";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "StatusText":
                        column.HeaderText = "状态";
                        column.Width = 80;
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "LateFee":
                        column.HeaderText = "逾期费(元)";
                        column.Width = 80;
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        column.DefaultCellStyle.Format = "0.00";
                        break;
                    case "Remark":
                        column.HeaderText = "备注";
                        column.Width = 150;
                        break;
                    default:
                        if (column.Name == "Status" ||
                            column.Name == "ReaderId" ||
                            column.Name == "BookId")
                        {
                            column.Visible = false;
                        }
                        break;
                }
            }
            dgvBorrowRecord.RowHeadersVisible = false;
            dgvBorrowRecord.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            dgvBorrowRecord.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvBorrowRecord.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9, FontStyle.Bold);
            dgvBorrowRecord.DefaultCellStyle.Font = new Font("微软雅黑", 9);
            foreach (DataGridViewRow row in dgvBorrowRecord.Rows)
            {
                if (row.Cells["StatusText"].Value != null)
                {
                    string status = row.Cells["StatusText"].Value.ToString();
                    if (status == "逾期")
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                        row.DefaultCellStyle.Font = new Font(row.DefaultCellStyle.Font, FontStyle.Bold);
                    }
                    else if (status == "借出中")
                    {
                        row.DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }
        }
        private void UpdateStatistics(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    lblTotalRecords.Text = "总记录：0";
                    lblBorrowing.Text = "借出中：0";
                    lblReturned.Text = "已归还：0";
                    lblOverdue.Text = "逾期：0";
                    lblTotalLateFee.Text = "总逾期费：0.00元";
                    return;
                }

                int total = dt.Rows.Count;
                int borrowing = 0;
                int returned = 0;
                int overdue = 0;
                decimal totalLateFee = 0;

                foreach (DataRow row in dt.Rows)
                {
                    int status = GetSafeInt(row["Status"], 0);
                    decimal lateFee = GetSafeDecimal(row["LateFee"], 0);

                    totalLateFee += lateFee;

                    switch (status)
                    {
                        case 0: borrowing++; break;
                        case 1: returned++; break;
                        case 2: overdue++; break;
                    }
                }

                lblTotalRecords.Text = $"总记录：{total}";
                lblBorrowing.Text = $"借出中：{borrowing}";
                lblReturned.Text = $"已归还：{returned}";
                lblOverdue.Text = $"逾期：{overdue}";
                lblTotalLateFee.Text = $"总逾期费：{totalLateFee:0.00}元";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新统计信息失败：{ex.Message}", "错误",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePaginationInfo()
        {
            try
            {
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages < 1) totalPages = 1;

                if (currentPage > totalPages)
                    currentPage = totalPages;
                if (currentPage < 1)
                    currentPage = 1;

                lblPageInfo.Text = $"第{currentPage}页/共{totalPages}页";

                UpdatePaginationButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新分页信息失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdatePaginationButtons()
{
            try
            {
                btnFirst.Enabled = (currentPage > 1);
                btnPrev.Enabled = (currentPage > 1);
                btnNext.Enabled = (currentPage < totalPages);
                btnLast.Enabled = (currentPage < totalPages);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新分页按钮状态失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
private void btnSearch_Click(object sender, EventArgs e)
{
            currentPage = 1;  // 重置到第一页
            LoadBorrowRecords();
        }

/// <summary>
/// 重置按钮点击事件
/// </summary>
private void btnReset_Click(object sender, EventArgs e)
{
            ResetSearchConditions();
        }

        /// <summary>
        /// 重置查询条件
        /// </summary>
        /// <summary>
        /// 重置查询条件
        /// </summary>
        private void ResetSearchConditions()
        {
            try
            {
                // 1. 清空所有查询条件
                txtReaderId.Clear();
                txtReaderName.Clear();
                txtBookName.Clear();
                txtISBN.Clear();

                // 2. 重置下拉框，但先检查是否有项目
                if (cmbStatus.Items.Count > 0)
                {
                    cmbStatus.SelectedIndex = 0;
                }
                else
                {
                    // 如果状态下拉框为空，重新初始化
                    InitializeComboBoxes();
                }

                if (cmbCategory.Items.Count > 0)
                {
                    cmbCategory.SelectedIndex = 0;
                }
                else
                {
                    // 如果图书类别下拉框为空，重新加载
                    LoadBookCategories();
                }

                // 3. 重置日期
                SetDefaultDates();

                // 4. 重置分页
                currentPage = 1;

                // 5. 焦点回到第一个输入框
                txtReaderId.Focus();

                // 6. 可选：清空数据表格
                dgvBorrowRecord.DataSource = null;

                // 7. 清空统计信息
                lblTotalRecords.Text = "总记录：0";
                lblBorrowing.Text = "借出中：0";
                lblReturned.Text = "已归还：0";
                lblOverdue.Text = "逾期：0";
                lblTotalLateFee.Text = "总逾期费：0.00元";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重置失败：{ex.Message}", "错误");
            }
        }




        private void btnExport_Click(object sender, EventArgs e)
{
            ExportToCSV();
        }

/// <summary>
/// 导出数据到CSV文件
/// </summary>
private void ExportToCSV()
{
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = true;
                saveDialog.Title = "导出借阅记录";
                saveDialog.FileName = $"借阅记录_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = dgvBorrowRecord.DataSource as DataTable;
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("没有数据可导出！", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    using (StreamWriter sw = new StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Columns[i].ColumnName == "Status" ||
                                dt.Columns[i].ColumnName == "ReaderId" ||
                                dt.Columns[i].ColumnName == "BookId")
                                continue;

                            sw.Write(dt.Columns[i].ColumnName);
                            if (i < dt.Columns.Count - 1)
                                sw.Write(",");
                        }
                        sw.WriteLine();

                        foreach (DataRow row in dt.Rows)
                        {
                            bool firstColumn = true;
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                if (dt.Columns[i].ColumnName == "Status" ||
                                    dt.Columns[i].ColumnName == "ReaderId" ||
                                    dt.Columns[i].ColumnName == "BookId")
                                    continue;

                                if (!firstColumn)
                                    sw.Write(",");

                                string value = row[i].ToString(); if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                                {
                                    value = "\"" + value.Replace("\"", "\"\"") + "\"";
                                }

                                sw.Write(value);
                                firstColumn = false;
                            }
                            sw.WriteLine();
                        }
                    }

                    MessageBox.Show($"导出成功！文件已保存到：\n{saveDialog.FileName}", "导出完成",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnFirst_Click(object sender, EventArgs e)
{
            currentPage = 1;
            LoadBorrowRecords();
        }

/// <summary>
/// 上一页按钮点击事件
/// </summary>
private void btnPrev_Click(object sender, EventArgs e)
{
            if (currentPage > 1)
            {
                currentPage--;
                LoadBorrowRecords();
            }
        }

/// <summary>
/// 下一页按钮点击事件
/// </summary>
private void btnNext_Click(object sender, EventArgs e)
{
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadBorrowRecords();
            }
        }

/// <summary>
/// 末页按钮点击事件
/// </summary>
private void btnLast_Click(object sender, EventArgs e)
{
            currentPage = totalPages;
            LoadBorrowRecords();
        }

/// <summary>
/// 刷新按钮点击事件
/// </summary>
private void btnRefresh_Click(object sender, EventArgs e)
{
            LoadBorrowRecords();
        
}
private void btnCloseBottom_Click(object sender, EventArgs e)
{
    this.Close();
}

/// <summary>
/// 键盘快捷键处理
/// </summary>
private void FrmBorrowRecord_KeyDown(object sender, KeyEventArgs e)
{
            if (e.Control && e.KeyCode == Keys.F)
            {
                txtReaderId.Focus();
            }
            else if (e.KeyCode == Keys.F5)
            {
                LoadBorrowRecords();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }


        private string GetSafeString(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;
            return value.ToString();
        }

        /// <summary>
        /// 安全获取整数（处理DBNull）
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
/// 安全获取小数（处理DBNull）
/// </summary>
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

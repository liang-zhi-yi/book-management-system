using BookManagement.BLL;
using BookManagement.Model;
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
    public partial class FrmBookManage : Form
    {

        // ========== 1. 先在这里添加字段声明 ==========
        // 业务逻辑对象
        private BookManager bookManager = new BookManager();

        // 当前选中的图书ID（用于修改和删除）
        private int currentBookId = 0;

        // 当前模式：0-无，1-新增，2-修改
        private int currentMode = 0;

        // ========== 2. 构造函数 ==========
        public FrmBookManage()
        {
            InitializeComponent();
        }

        // ========== 3. 下面是其他方法 ==========


        private void FrmBookManage_Load(object sender, EventArgs e)
        {
            // 1. 加载图书列表到表格
            LoadBookData();

            // ！！！ 2. 关键：加载图书类别到下拉框 ！！！
            LoadCategoryDataComboBox();

            // 3. 初始状态：只有“新增/修改/删除”可用
            SetControlsState(false);
        }
        /// <summary>
        /// 从数据库加载图书类别，并绑定到下拉框(ComboBox)
        /// 这是本窗体的关键方法之一
        /// </summary>
        private void LoadCategoryDataComboBox()
        {
           
            try
            {
                BookManager bookManager = new BookManager();
                DataTable dt = bookManager.GetAllCategories();

                

                // 2. 核心三步：绑定数据到下拉框
                if (dt.Rows.Count > 0)
                {
                    // 关键：以下三行代码缺一不可
                    cmbCategory.DataSource = dt;                 // ① 设置数据源
                    cmbCategory.DisplayMember = "CategoryName";  // ② 设置显示的文字（对应您的截图中的列名）
                    cmbCategory.ValueMember = "CategoryId";      // ③ 设置后台实际的值
                    cmbCategory.SelectedIndex = -1;             // 初始不选中任何项
                }
                else
                {
                    MessageBox.Show("数据库中暂无类别数据，请先在'图书类别管理'中添加。", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载类别数据失败：" + ex.Message, "错误");
            }
        }




        // ... 其他方法

        private void LoadBookData()
        {
            try
            {
                // 调用 BookManager.GetAllBooks() 绑定到 dgvBookList.DataSource
                DataTable dt = bookManager.GetAllBooks();
                dgvBookList.DataSource = dt;  // 绑定到DataGridView

                // 可选：设置列宽自适应
                dgvBookList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // 清空当前选择
                dgvBookList.ClearSelection();

                // 清空输入区域
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载图书数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearInput()
        {
            // 清空所有输入框，重置下拉框和数字框
            currentBookId = 0;
            txtBookName.Clear();
            txtAuthor.Clear();
            txtPublisher.Clear();
            txtISBN.Clear();
            numPrice.Value = 0;
            numTotal.Value = 1;

            // 清空下拉框选择
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = -1;

            currentMode = 0;  // 重置为浏览模式
        }

        private void SetControlsState(bool editing)
        {
            // 根据 editing 为 true/false，设置五个按钮的 Enabled 属性
            // 编辑模式时，禁用新增/修改/删除，启用保存/取消
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;

            // 编辑模式时，禁止操作表格（防止误选）
            dgvBookList.Enabled = !editing;
        }





        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // ==== 按钮点击事件 ====
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearInput(); // 清空输入框
            SetControlsState(true); // 进入编辑状态
            currentMode = 1; // 标记为新增模式
            txtBookName.Focus(); // 光标聚焦
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBookList.CurrentRow == null)
            {
                MessageBox.Show("请先选择要修改的图书！", "提示");
                return;
            }
            // 获取选中行的数据，填充到输入框...
            DataGridViewRow row = dgvBookList.CurrentRow;
            currentBookId = Convert.ToInt32(row.Cells["BookId"].Value);
            txtBookName.Text = row.Cells["BookName"].Value.ToString();
            txtAuthor.Text = row.Cells["Author"].Value.ToString();
            // ... (获取其他字段，包括cmbCategory.SelectedValue)
            // 关键：设置下拉框
            if (row.Cells["CategoryId"].Value != DBNull.Value)
            {
                cmbCategory.SelectedValue = Convert.ToInt32(row.Cells["CategoryId"].Value);
            }

            SetControlsState(true);
            currentMode = 2; // 标记为修改模式
            txtBookName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBookList.CurrentRow == null)
            {
                MessageBox.Show("请先选择要删除的图书！", "提示");
                return;
            }
            if (MessageBox.Show("确定要删除吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    int bookId = Convert.ToInt32(dgvBookList.CurrentRow.Cells["BookId"].Value);
                    BookManager manager = new BookManager();
                    bool success = manager.DeleteBook(bookId);
                    if (success)
                    {
                        MessageBox.Show("删除成功！", "提示");
                        LoadBookData(); // 刷新列表
                    }
                }
                catch (Exception ex) { MessageBox.Show("删除失败：" + ex.Message, "错误"); }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. 数据验证
            if (string.IsNullOrEmpty(txtBookName.Text.Trim()))
            {
                MessageBox.Show("书名不能为空！", "提示");
                return;
            }
            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("请选择图书类别！", "提示");
                return;
            }
            try
            {
                // 2. 创建对象
                Book book = new Book
                {
                    BookName = txtBookName.Text.Trim(),
                    Author = txtAuthor.Text.Trim(),
                    CategoryId = Convert.ToInt32(cmbCategory.SelectedValue), // 关键
                    // ... 设置其他属性
                };
                BookManager manager = new BookManager();
                bool success = false;
                // 3. 根据模式保存
                if (currentMode == 1) { success = manager.AddBook(book); }
                else if (currentMode == 2) { book.BookId = currentBookId; success = manager.UpdateBook(book); }

                if (success)
                {
                    MessageBox.Show("保存成功！", "提示");
                    SetControlsState(false); // 退出编辑状态
                    LoadBookData(); // 刷新列表
                }
            }
            catch (Exception ex) { MessageBox.Show("保存失败：" + ex.Message, "错误"); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControlsState(false); // 退出编辑状态
            ClearInput();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

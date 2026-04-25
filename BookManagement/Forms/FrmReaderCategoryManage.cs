using System;
using System.Data;
using System.Windows.Forms;
using BookManagement.BLL;
using BookManagement.Model;

namespace BookManagement.Forms
{
    public partial class FrmReaderCategoryManage : Form
    {
        // 业务逻辑对象
        private ReaderCategoryManager categoryManager = new ReaderCategoryManager();
        // 当前选中的类别ID（用于修改和删除）
        private int currentCategoryId = 0;
        // 当前模式：0-无，1-新增，2-修改
        private int currentMode = 0;

        public FrmReaderCategoryManage()
        {
            InitializeComponent();
        }

        // 窗体加载时，加载数据
        private void FrmReaderCategoryManage_Load(object sender, EventArgs e)
        {// 设置列宽自适应
            dgvCategoryList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadCategoryData();
            SetControlsState(false); // 初始时，保存和取消按钮不可用
        }

        // 加载类别数据到DataGridView
        private void LoadCategoryData()
        {
            try
            {
                DataTable dt = categoryManager.GetAllCategories();
                dgvCategoryList.DataSource = dt;

                dgvCategoryList.ClearSelection();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据失败：" + ex.Message, "错误");
            }
        }



        // 清空输入框
        private void ClearInput()
        {
            currentCategoryId = 0;
            txtCategoryName.Clear();
            // 设置默认值
            numMaxBorrowCount.Value = 5;      // 默认最大借阅数
            numBorrowDays.Value = 30;         // 默认借阅天数
            numLateFeePerDay.Value = 0.5m;    // 默认逾期费
            currentMode = 0;
        }


        // 设置控件状态（是否可编辑）
        private void SetControlsState(bool editing)
        {
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
            dgvCategoryList.Enabled = !editing; // 编辑时不可选择列表
        }

        // 新增按钮点击事件
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearInput();
            SetControlsState(true);
            currentMode = 1; // 进入新增模式
            txtCategoryName.Focus(); // 焦点移到名称输入框
        }

        // 修改按钮点击事件
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCategoryList.CurrentRow == null)
            {
                MessageBox.Show("请先选择要修改的类别！", "提示");
                return;
            }

            // 获取选中行的数据
            DataGridViewRow row = dgvCategoryList.CurrentRow;
            currentCategoryId = Convert.ToInt32(row.Cells["CategoryId"].Value);
            txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();
            // 修改为 ReaderCategory 的特定属性
            numMaxBorrowCount.Value = Convert.ToInt32(row.Cells["MaxBorrowCount"].Value);
            numBorrowDays.Value = Convert.ToInt32(row.Cells["BorrowDays"].Value);
            numLateFeePerDay.Value = Convert.ToDecimal(row.Cells["LateFeePerDay"].Value);


            SetControlsState(true);
            currentMode = 2; // 进入修改模式
            txtCategoryName.Focus();
        }

        // 删除按钮点击事件
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategoryList.CurrentRow == null)
            {
                MessageBox.Show("请先选择要删除的类别！", "提示");
                return;
            }

            if (MessageBox.Show("确定要删除这个类别吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int categoryId = Convert.ToInt32(dgvCategoryList.CurrentRow.Cells["CategoryId"].Value);
                    bool success = categoryManager.DeleteCategory(categoryId);

                    if (success)
                    {
                        MessageBox.Show("删除成功！", "提示");
                        LoadCategoryData(); // 刷新列表
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败：" + ex.Message, "错误");
                }
            }
        }

        // 保存按钮点击事件
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. 输入验证（您已有的代码）
            if (string.IsNullOrEmpty(txtCategoryName.Text.Trim()))
            {
                MessageBox.Show("类别名称不能为空！", "提示");
                txtCategoryName.Focus();
                return;
            }

            try
            {
                // 2. 准备数据（您已有的代码）
                ReaderCategory category = new ReaderCategory
                {
                    CategoryName = txtCategoryName.Text.Trim(),
                    MaxBorrowCount = (int)numMaxBorrowCount.Value,  // ✅
                    BorrowDays = (int)numBorrowDays.Value,          // ✅
                    LateFeePerDay = numLateFeePerDay.Value          // ✅
                };


                bool success = false;
                string operation = "";

                // 3. 根据模式（新增/修改）调用BLL层
                if (currentMode == 1) // 新增模式
                {
                    operation = "新增";
                    success = categoryManager.AddCategory(category); // 调用新增
                }
                else if (currentMode == 2) // 修改模式
                {
                    operation = "修改";
                    category.CategoryId = currentCategoryId;
                    success = categoryManager.UpdateCategory(category); // 调用修改
                }

                // 4. 处理操作结果
                if (success)
                {
                    // 提示成功
                    MessageBox.Show(operation + "成功！", "提示");



                    // ---------- 关键修复点：开始 ----------
                    // 1. 退出编辑状态（恢复按钮）
                    SetControlsState(false);
                    // 2. 重新从数据库加载数据，刷新DataGridView显示
                    LoadCategoryData();
                    // 3. 清空输入框，准备下一次操作
                    ClearInput();
                    // ---------- 关键修复点：结束 ----------
                }
                else
                {
                    MessageBox.Show(operation + "失败！", "提示");
                }
            }
            catch (Exception ex)
            {
                // 5. 异常处理
                MessageBox.Show("保存失败：" + ex.Message, "错误");
            }
        }


        // 取消按钮点击事件
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControlsState(false);
            ClearInput();
        }
    }
}

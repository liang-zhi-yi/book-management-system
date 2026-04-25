using System;
using System.Data;
using System.Windows.Forms;
using BookManagement.BLL;
using BookManagement.Model;

namespace BookManagement.Forms
{
    public partial class FrmReaderManage : Form
    {
        // 业务逻辑对象
        private ReaderManager readerManager = new ReaderManager();
        // 当前选中的读者ID
        private int currentReaderId = 0;
        // 当前模式：0-浏览，1-新增，2-修改
        private int currentMode = 0;

        public FrmReaderManage()
        {
            InitializeComponent();
        }

        private void FrmReaderManage_Load(object sender, EventArgs e)
        {
            // 1. 先初始化固定下拉框（性别、状态）
            InitComboBoxes();

            // 2. 再加载动态数据（读者类别）
            LoadReaderCategoryData();

            // 3. 最后加载读者列表
            LoadReaderData();

            // 4. 设置控件状态
            SetControlsState(false);

            // 5. 设置默认日期
            dtpRegistrationDate.Value = DateTime.Now;
            dtpExpiryDate.Value = DateTime.Now.AddYears(1);
        }


        /// <summary>
        /// 加载读者数据
        /// </summary>
        private void LoadReaderData()
        {
            try
            {
                
                DataTable dt = readerManager.GetAllReaders();
                dgvReaderList.DataSource = dt;
                dgvReaderList.ClearSelection();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载读者数据失败：" + ex.Message, "错误");
            }
        }

        /// <summary>
        /// 加载读者类别到下拉框
        /// </summary>
        private void LoadReaderCategoryData()
        {
            try
            {
                // 获取读者类别数据
                DataTable dt = readerManager.GetAllReaderCategories();

                if (dt == null || dt.Rows.Count == 0)
                {
                    // 如果没有数据，清空下拉框并显示提示
                    cmbReaderCategory.DataSource = null;
                    cmbReaderCategory.Items.Clear();
                    cmbReaderCategory.Items.Add("暂无类别数据");
                    cmbReaderCategory.SelectedIndex = 0;
                    cmbReaderCategory.Enabled = false;  // 禁用下拉框

                    MessageBox.Show("没有读者类别数据，请先添加读者类别！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 有数据时才进行绑定
                cmbReaderCategory.DataSource = dt;
                cmbReaderCategory.DisplayMember = "CategoryName";
                cmbReaderCategory.ValueMember = "CategoryId";

                // 不设置 SelectedIndex，让其保持 -1
                // cmbReaderCategory.SelectedIndex = 0;  // 删除这行

                cmbReaderCategory.Enabled = true;  // 启用下拉框
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载读者类别失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitComboBoxes()
        {
            // 初始化性别下拉框
            cmbGender.Items.Clear();
            cmbGender.Items.Add("男");
            cmbGender.Items.Add("女");
            if (cmbGender.Items.Count > 0)
                cmbGender.SelectedIndex = 0;

            // 初始化状态下拉框
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("正常");
            cmbStatus.Items.Add("挂失");
            cmbStatus.Items.Add("注销");
            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;
        }


        /// <summary>
        /// 清空输入框
        /// </summary>
        private void ClearInput()
        {
            currentReaderId = 0;
            txtReaderName.Clear();

            // 性别下拉框 - 确保有项再设置
            if (cmbGender.Items.Count > 0)
                cmbGender.SelectedIndex = 0;

            // 读者类别下拉框 - 不设置选中项
            if (cmbReaderCategory.Items.Count > 0)
            {
                // 只有当有实际类别数据时才尝试重置
                if (cmbReaderCategory.DataSource != null)
                    cmbReaderCategory.SelectedIndex = -1;  // 设为-1而不是0
            }

            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            dtpRegistrationDate.Value = DateTime.Now;
            dtpExpiryDate.Value = DateTime.Now.AddYears(1);

            // 状态下拉框 - 确保有项再设置
            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            txtRemark.Clear();
            currentMode = 0;
        }


        /// <summary>
        /// 设置控件状态
        /// </summary>
        private void SetControlsState(bool editing)
        {
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
            dgvReaderList.Enabled = !editing;
        }

        // ========== 所有按钮点击事件代码 ==========

        /// <summary>
        /// 【新增】按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearInput();
            SetControlsState(true);
            currentMode = 1;  // 进入新增模式
            txtReaderName.Focus();
        }

        /// <summary>
        /// 【修改】按钮点击事件
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvReaderList.CurrentRow == null)
            {
                MessageBox.Show("请先选择要修改的读者！", "提示");
                return;
            }

            DataGridViewRow row = dgvReaderList.CurrentRow;
            currentReaderId = Convert.ToInt32(row.Cells["ReaderId"].Value);
            txtReaderName.Text = row.Cells["ReaderName"].Value.ToString();
            cmbGender.Text = row.Cells["Gender"].Value.ToString();

            // 设置读者类别 - 确保下拉框有数据
            if (row.Cells["ReaderCategoryId"].Value != DBNull.Value &&
                cmbReaderCategory.Items.Count > 0 &&
                cmbReaderCategory.DataSource != null)
            {
                int categoryId = Convert.ToInt32(row.Cells["ReaderCategoryId"].Value);
                cmbReaderCategory.SelectedValue = categoryId;  // 使用SelectedValue而不是SelectedIndex
            }

            txtPhone.Text = row.Cells["Phone"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
            txtAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";

            if (row.Cells["RegistrationDate"].Value != DBNull.Value)
                dtpRegistrationDate.Value = Convert.ToDateTime(row.Cells["RegistrationDate"].Value);
            if (row.Cells["ExpiryDate"].Value != DBNull.Value)
                dtpExpiryDate.Value = Convert.ToDateTime(row.Cells["ExpiryDate"].Value);

            int status = Convert.ToInt32(row.Cells["Status"].Value);
            if (cmbStatus.Items.Count > status)
                cmbStatus.SelectedIndex = status;

            txtRemark.Text = row.Cells["Remark"].Value?.ToString() ?? "";

            SetControlsState(true);
            currentMode = 2;
            txtReaderName.Focus();
        }


        /// <summary>
        /// 【删除】按钮点击事件
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReaderList.CurrentRow == null)
            {
                MessageBox.Show("请先选择要删除的读者！", "提示");
                return;
            }

            if (MessageBox.Show("确定要删除这个读者吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int readerId = Convert.ToInt32(dgvReaderList.CurrentRow.Cells["ReaderId"].Value);
                    bool success = readerManager.DeleteReader(readerId);

                    if (success)
                    {
                        MessageBox.Show("删除成功！", "提示");
                        LoadReaderData();  // 刷新列表
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败：" + ex.Message, "错误");
                }
            }
        }

        /// <summary>
        /// 【保存】按钮点击事件
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 数据验证
            if (string.IsNullOrEmpty(txtReaderName.Text.Trim()))
            {
                MessageBox.Show("读者姓名不能为空！", "提示");
                txtReaderName.Focus();
                return;
            }
            if (cmbReaderCategory.SelectedValue == null)
            {
                MessageBox.Show("请选择读者类别！", "提示");
                cmbReaderCategory.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                MessageBox.Show("联系电话不能为空！", "提示");
                txtPhone.Focus();
                return;
            }
            if (dtpExpiryDate.Value <= dtpRegistrationDate.Value)
            {
                MessageBox.Show("有效期必须晚于注册日期！", "提示");
                dtpExpiryDate.Focus();
                return;
            }

            try
            {
                // 创建读者对象
                Reader reader = new Reader
                {
                    ReaderName = txtReaderName.Text.Trim(),
                    Gender = cmbGender.Text,
                    ReaderCategoryId = Convert.ToInt32(cmbReaderCategory.SelectedValue),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    RegistrationDate = dtpRegistrationDate.Value,
                    ExpiryDate = dtpExpiryDate.Value,
                    Status = cmbStatus.SelectedIndex,
                    Remark = txtRemark.Text.Trim()
                };

                bool success = false;
                string operation = "";

                if (currentMode == 1)  // 新增
                {
                    operation = "新增";
                    success = readerManager.AddReader(reader);
                }
                else if (currentMode == 2)  // 修改
                {
                    operation = "修改";
                    reader.ReaderId = currentReaderId;
                    success = readerManager.UpdateReader(reader);
                }

                if (success)
                {
                    MessageBox.Show(operation + "成功！", "提示");
                    SetControlsState(false);
                    LoadReaderData();  // 刷新列表
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message, "错误");
            }
        }

        /// <summary>
        /// 【取消】按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControlsState(false);
            ClearInput();
        }
    }
}


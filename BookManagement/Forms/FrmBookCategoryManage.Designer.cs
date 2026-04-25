namespace BookManagement.Forms
{
    partial class FrmBookCategoryManage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvCategoryList = new DataGridView();
            groupBox1 = new GroupBox();
            txtDescription = new TextBox();
            txtCategoryName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCategoryList).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvCategoryList
            // 
            dgvCategoryList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategoryList.Location = new Point(0, 0);
            dgvCategoryList.Name = "dgvCategoryList";
            dgvCategoryList.RowHeadersWidth = 82;
            dgvCategoryList.Size = new Size(1543, 300);
            dgvCategoryList.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtDescription);
            groupBox1.Controls.Add(txtCategoryName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(0, 306);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1543, 334);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "类别信息";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(246, 173);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(1273, 155);
            txtDescription.TabIndex = 3;
            // 
            // txtCategoryName
            // 
            txtCategoryName.Location = new Point(246, 84);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(1273, 38);
            txtCategoryName.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 11F);
            label2.Location = new Point(98, 171);
            label2.Name = "label2";
            label2.Size = new Size(107, 39);
            label2.TabIndex = 1;
            label2.Text = "描述：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 11F);
            label1.Location = new Point(98, 84);
            label1.Name = "label1";
            label1.Size = new Size(167, 39);
            label1.TabIndex = 0;
            label1.Text = "类别名称：";
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Microsoft YaHei UI", 12F);
            btnAdd.Location = new Point(98, 708);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(150, 76);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "新增";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Font = new Font("Microsoft YaHei UI", 12F);
            btnEdit.Location = new Point(375, 708);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(150, 76);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "修改";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Microsoft YaHei UI", 12F);
            btnDelete.Location = new Point(661, 708);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(150, 76);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "删除";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Microsoft YaHei UI", 12F);
            btnSave.Location = new Point(965, 708);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 76);
            btnSave.TabIndex = 5;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Microsoft YaHei UI", 12F);
            btnCancel.Location = new Point(1269, 708);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 76);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // FrmBookCategoryManage
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1545, 898);
            Controls.Add(dgvCategoryList);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(groupBox1);
            Name = "FrmBookCategoryManage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "图书类别管理";
            Load += FrmBookCategoryManage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCategoryList).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvCategoryList;
        private GroupBox groupBox1;
        private TextBox txtDescription;
        private TextBox txtCategoryName;
        private Label label2;
        private Label label1;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;
    }
}
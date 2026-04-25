namespace BookManagement.Forms
{
    partial class FrmReaderCategoryManage
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
            numLateFeePerDay = new NumericUpDown();
            numBorrowDays = new NumericUpDown();
            numMaxBorrowCount = new NumericUpDown();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            txtCategoryName = new TextBox();
            label1 = new Label();
            btnCancel = new Button();
            btnSave = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCategoryList).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numLateFeePerDay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBorrowDays).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxBorrowCount).BeginInit();
            SuspendLayout();
            // 
            // dgvCategoryList
            // 
            dgvCategoryList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategoryList.Location = new Point(-4, -2);
            dgvCategoryList.Margin = new Padding(4, 2, 4, 2);
            dgvCategoryList.Name = "dgvCategoryList";
            dgvCategoryList.RowHeadersWidth = 82;
            dgvCategoryList.Size = new Size(1549, 233);
            dgvCategoryList.TabIndex = 7;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(numLateFeePerDay);
            groupBox1.Controls.Add(numBorrowDays);
            groupBox1.Controls.Add(numMaxBorrowCount);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtCategoryName);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("Microsoft YaHei UI", 11F);
            groupBox1.Location = new Point(-4, 235);
            groupBox1.Margin = new Padding(4, 2, 4, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 2, 4, 2);
            groupBox1.Size = new Size(1549, 524);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "读者类别信息";
            // 
            // numLateFeePerDay
            // 
            numLateFeePerDay.DecimalPlaces = 2;
            numLateFeePerDay.Location = new Point(254, 356);
            numLateFeePerDay.Name = "numLateFeePerDay";
            numLateFeePerDay.Size = new Size(240, 45);
            numLateFeePerDay.TabIndex = 9;
            numLateFeePerDay.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            // 
            // numBorrowDays
            // 
            numBorrowDays.Location = new Point(254, 269);
            numBorrowDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            numBorrowDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numBorrowDays.Name = "numBorrowDays";
            numBorrowDays.Size = new Size(240, 45);
            numBorrowDays.TabIndex = 8;
            numBorrowDays.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // numMaxBorrowCount
            // 
            numMaxBorrowCount.Location = new Point(254, 184);
            numMaxBorrowCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxBorrowCount.Name = "numMaxBorrowCount";
            numMaxBorrowCount.Size = new Size(240, 45);
            numMaxBorrowCount.TabIndex = 7;
            numMaxBorrowCount.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(51, 358);
            label5.Name = "label5";
            label5.Size = new Size(197, 39);
            label5.TabIndex = 6;
            label5.Text = "每日逾期费：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(57, 275);
            label4.Name = "label4";
            label4.Size = new Size(167, 39);
            label4.TabIndex = 5;
            label4.Text = "借阅天数：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(51, 190);
            label3.Name = "label3";
            label3.Size = new Size(197, 39);
            label3.TabIndex = 4;
            label3.Text = "最大借阅数：";
            // 
            // txtCategoryName
            // 
            txtCategoryName.Location = new Point(254, 79);
            txtCategoryName.Margin = new Padding(4, 2, 4, 2);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(1278, 45);
            txtCategoryName.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 11F);
            label1.Location = new Point(51, 79);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(167, 39);
            label1.TabIndex = 0;
            label1.Text = "类别名称：";
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("宋体", 11F);
            btnCancel.Location = new Point(1265, 763);
            btnCancel.Margin = new Padding(4, 2, 4, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(159, 69);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("宋体", 11F);
            btnSave.Location = new Point(960, 763);
            btnSave.Margin = new Padding(4, 2, 4, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(168, 69);
            btnSave.TabIndex = 12;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("宋体", 11F);
            btnDelete.Location = new Point(636, 763);
            btnDelete.Margin = new Padding(4, 2, 4, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(170, 69);
            btnDelete.TabIndex = 11;
            btnDelete.Text = "删除";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.Font = new Font("宋体", 11F);
            btnEdit.Location = new Point(339, 763);
            btnEdit.Margin = new Padding(4, 2, 4, 2);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(161, 69);
            btnEdit.TabIndex = 10;
            btnEdit.Text = "修改";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("宋体", 11F);
            btnAdd.Location = new Point(53, 763);
            btnAdd.Margin = new Padding(4, 2, 4, 2);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(161, 69);
            btnAdd.TabIndex = 9;
            btnAdd.Text = "新增";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // FrmReaderCategoryManage
            // 
            AutoScaleDimensions = new SizeF(15F, 29F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1544, 1050);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(dgvCategoryList);
            Controls.Add(groupBox1);
            Font = new Font("宋体", 11F);
            Margin = new Padding(4, 2, 4, 2);
            Name = "FrmReaderCategoryManage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "读者类别管理";
            Load += FrmReaderCategoryManage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCategoryList).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numLateFeePerDay).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBorrowDays).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxBorrowCount).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvCategoryList;
        private GroupBox groupBox1;
        private TextBox txtCategoryName;
        private Label label1;
        private Button btnCancel;
        private Button btnSave;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnAdd;
        private NumericUpDown numLateFeePerDay;
        private NumericUpDown numBorrowDays;
        private NumericUpDown numMaxBorrowCount;
        private Label label5;
        private Label label4;
        private Label label3;
    }
}
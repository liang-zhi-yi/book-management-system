namespace BookManagement.Forms
{
    partial class FrmBookManage
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
            dgvBookList = new DataGridView();
            groupbox1 = new GroupBox();
            label9 = new Label();
            label8 = new Label();
            cmbCategory = new ComboBox();
            txtISBN = new TextBox();
            txtBookName = new TextBox();
            txtAuthor = new TextBox();
            txtPublisher = new TextBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            numTotal = new NumericUpDown();
            numPrice = new NumericUpDown();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvBookList).BeginInit();
            groupbox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTotal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            SuspendLayout();
            // 
            // dgvBookList
            // 
            dgvBookList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBookList.Location = new Point(0, 0);
            dgvBookList.Name = "dgvBookList";
            dgvBookList.RowHeadersWidth = 82;
            dgvBookList.Size = new Size(1302, 300);
            dgvBookList.TabIndex = 0;
            dgvBookList.CellContentClick += dataGridView1_CellContentClick;
            // 
            // groupbox1
            // 
            groupbox1.Controls.Add(label9);
            groupbox1.Controls.Add(label8);
            groupbox1.Controls.Add(cmbCategory);
            groupbox1.Controls.Add(txtISBN);
            groupbox1.Controls.Add(txtBookName);
            groupbox1.Controls.Add(txtAuthor);
            groupbox1.Controls.Add(txtPublisher);
            groupbox1.Controls.Add(label7);
            groupbox1.Controls.Add(label6);
            groupbox1.Controls.Add(label5);
            groupbox1.Controls.Add(label4);
            groupbox1.Controls.Add(label3);
            groupbox1.Controls.Add(label2);
            groupbox1.Controls.Add(label1);
            groupbox1.Controls.Add(numTotal);
            groupbox1.Controls.Add(numPrice);
            groupbox1.Location = new Point(12, 306);
            groupbox1.Name = "groupbox1";
            groupbox1.Size = new Size(1290, 546);
            groupbox1.TabIndex = 1;
            groupbox1.TabStop = false;
            groupbox1.Text = "图书信息";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("宋体", 11F);
            label9.Location = new Point(1071, 487);
            label9.Name = "label9";
            label9.Size = new Size(43, 30);
            label9.TabIndex = 15;
            label9.Text = "本";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("宋体", 11F);
            label8.Location = new Point(1071, 424);
            label8.Name = "label8";
            label8.Size = new Size(43, 30);
            label8.TabIndex = 14;
            label8.Text = "元";
            // 
            // cmbCategory
            // 
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Font = new Font("宋体", 11F);
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(180, 219);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(934, 37);
            cmbCategory.TabIndex = 13;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            // 
            // txtISBN
            // 
            txtISBN.Font = new Font("Microsoft YaHei UI", 11F);
            txtISBN.Location = new Point(180, 341);
            txtISBN.Name = "txtISBN";
            txtISBN.Size = new Size(934, 45);
            txtISBN.TabIndex = 12;
            // 
            // txtBookName
            // 
            txtBookName.Font = new Font("Microsoft YaHei UI", 11F);
            txtBookName.Location = new Point(180, 76);
            txtBookName.Name = "txtBookName";
            txtBookName.Size = new Size(934, 45);
            txtBookName.TabIndex = 11;
            // 
            // txtAuthor
            // 
            txtAuthor.Font = new Font("Microsoft YaHei UI", 11F);
            txtAuthor.Location = new Point(180, 147);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(934, 45);
            txtAuthor.TabIndex = 10;
            // 
            // txtPublisher
            // 
            txtPublisher.Font = new Font("宋体", 11F);
            txtPublisher.Location = new Point(180, 273);
            txtPublisher.Name = "txtPublisher";
            txtPublisher.Size = new Size(934, 41);
            txtPublisher.TabIndex = 9;
            txtPublisher.TextChanged += textBox1_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("宋体", 11F);
            label7.Location = new Point(50, 502);
            label7.Name = "label7";
            label7.Size = new Size(103, 30);
            label7.TabIndex = 8;
            label7.Text = "总数：";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("宋体", 11F);
            label6.Location = new Point(50, 432);
            label6.Name = "label6";
            label6.Size = new Size(88, 30);
            label6.TabIndex = 7;
            label6.Text = "价格:";
            label6.Click += label6_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("宋体", 11F);
            label5.Location = new Point(50, 356);
            label5.Name = "label5";
            label5.Size = new Size(103, 30);
            label5.TabIndex = 6;
            label5.Text = "ISBN：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("宋体", 11F);
            label4.Location = new Point(50, 284);
            label4.Name = "label4";
            label4.Size = new Size(133, 30);
            label4.TabIndex = 5;
            label4.Text = "出版社：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("宋体", 11F);
            label3.Location = new Point(50, 222);
            label3.Name = "label3";
            label3.Size = new Size(103, 30);
            label3.TabIndex = 4;
            label3.Text = "类别：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("宋体", 11F);
            label2.Location = new Point(50, 162);
            label2.Name = "label2";
            label2.Size = new Size(103, 30);
            label2.TabIndex = 3;
            label2.Text = "作者：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("宋体", 11F);
            label1.Location = new Point(50, 91);
            label1.Name = "label1";
            label1.Size = new Size(103, 30);
            label1.TabIndex = 2;
            label1.Text = "书名：";
            // 
            // numTotal
            // 
            numTotal.Font = new Font("Microsoft YaHei UI", 11F);
            numTotal.Location = new Point(180, 487);
            numTotal.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numTotal.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTotal.Name = "numTotal";
            numTotal.Size = new Size(890, 45);
            numTotal.TabIndex = 1;
            numTotal.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numPrice
            // 
            numPrice.DecimalPlaces = 2;
            numPrice.Font = new Font("Microsoft YaHei UI", 11F);
            numPrice.Location = new Point(180, 417);
            numPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(890, 45);
            numPrice.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Microsoft YaHei UI", 11F);
            btnAdd.Location = new Point(48, 858);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(184, 83);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "新增";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Font = new Font("Microsoft YaHei UI", 11F);
            btnEdit.Location = new Point(308, 858);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(182, 83);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "修改";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Microsoft YaHei UI", 11F);
            btnDelete.Location = new Point(568, 858);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(176, 83);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "删除";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Microsoft YaHei UI", 11F);
            btnSave.Location = new Point(824, 858);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(186, 83);
            btnSave.TabIndex = 5;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Microsoft YaHei UI", 11F);
            btnCancel.Location = new Point(1083, 858);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(179, 83);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // FrmBookManage
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1314, 1269);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnDelete);
            Controls.Add(groupbox1);
            Controls.Add(dgvBookList);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FrmBookManage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "图书管理";
            Load += FrmBookManage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBookList).EndInit();
            groupbox1.ResumeLayout(false);
            groupbox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTotal).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvBookList;
        private GroupBox groupbox1;
        private NumericUpDown numTotal;
        private NumericUpDown numPrice;
        private ComboBox cmbCategory;
        private TextBox txtISBN;
        private TextBox txtBookName;
        private TextBox txtAuthor;
        private TextBox txtPublisher;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;
        private Label label9;
        private Label label8;
    }
}
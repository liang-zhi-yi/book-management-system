namespace BookManagement.Forms
{
    partial class FrmBorrowBook
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
            gbReaderInfo = new GroupBox();
            btnSearchReader = new Button();
            lblMaxBorrow = new Label();
            label15 = new Label();
            lblTotalBorrowed = new Label();
            lblBorrowedCount = new Label();
            lblReaderCategory = new Label();
            lblReaderName = new Label();
            txtReaderId = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            gbBookInfo = new GroupBox();
            lblAvailableCount = new Label();
            lblStock = new Label();
            lblPublisher = new Label();
            lblAuthor = new Label();
            btnSearchBook = new Button();
            txtBookId = new TextBox();
            lblBookName = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            gbBorrowInfo = new GroupBox();
            txtRemark = new TextBox();
            dtpDueDate = new DateTimePicker();
            dtpBorrowDate = new DateTimePicker();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            gbCurrentBorrow = new GroupBox();
            dgvCurrentBorrow = new DataGridView();
            gbReaderInfo.SuspendLayout();
            gbBookInfo.SuspendLayout();
            gbBorrowInfo.SuspendLayout();
            gbCurrentBorrow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCurrentBorrow).BeginInit();
            SuspendLayout();
            // 
            // gbReaderInfo
            // 
            gbReaderInfo.Controls.Add(btnSearchReader);
            gbReaderInfo.Controls.Add(lblMaxBorrow);
            gbReaderInfo.Controls.Add(label15);
            gbReaderInfo.Controls.Add(lblTotalBorrowed);
            gbReaderInfo.Controls.Add(lblBorrowedCount);
            gbReaderInfo.Controls.Add(lblReaderCategory);
            gbReaderInfo.Controls.Add(lblReaderName);
            gbReaderInfo.Controls.Add(txtReaderId);
            gbReaderInfo.Controls.Add(label5);
            gbReaderInfo.Controls.Add(label4);
            gbReaderInfo.Controls.Add(label3);
            gbReaderInfo.Controls.Add(label2);
            gbReaderInfo.Controls.Add(label1);
            gbReaderInfo.Location = new Point(3, 12);
            gbReaderInfo.Name = "gbReaderInfo";
            gbReaderInfo.Size = new Size(1424, 408);
            gbReaderInfo.TabIndex = 0;
            gbReaderInfo.TabStop = false;
            gbReaderInfo.Text = "读者信息";
            // 
            // btnSearchReader
            // 
            btnSearchReader.Location = new Point(579, 61);
            btnSearchReader.Name = "btnSearchReader";
            btnSearchReader.Size = new Size(150, 46);
            btnSearchReader.TabIndex = 12;
            btnSearchReader.Text = "查询";
            btnSearchReader.UseVisualStyleBackColor = true;
            btnSearchReader.Click += btnSearchReader_Click;
            // 
            // lblMaxBorrow
            // 
            lblMaxBorrow.AutoSize = true;
            lblMaxBorrow.BorderStyle = BorderStyle.Fixed3D;
            lblMaxBorrow.Location = new Point(200, 347);
            lblMaxBorrow.Name = "lblMaxBorrow";
            lblMaxBorrow.Size = new Size(30, 33);
            lblMaxBorrow.TabIndex = 11;
            lblMaxBorrow.Text = "0";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(36, 347);
            label15.Name = "label15";
            label15.Size = new Size(24, 31);
            label15.TabIndex = 10;
            label15.Text = "/";
            // 
            // lblTotalBorrowed
            // 
            lblTotalBorrowed.AutoSize = true;
            lblTotalBorrowed.BorderStyle = BorderStyle.Fixed3D;
            lblTotalBorrowed.Location = new Point(200, 288);
            lblTotalBorrowed.Name = "lblTotalBorrowed";
            lblTotalBorrowed.Size = new Size(30, 33);
            lblTotalBorrowed.TabIndex = 9;
            lblTotalBorrowed.Text = "0";
            // 
            // lblBorrowedCount
            // 
            lblBorrowedCount.AutoSize = true;
            lblBorrowedCount.BorderStyle = BorderStyle.Fixed3D;
            lblBorrowedCount.Location = new Point(200, 231);
            lblBorrowedCount.Name = "lblBorrowedCount";
            lblBorrowedCount.Size = new Size(2, 33);
            lblBorrowedCount.TabIndex = 8;
            // 
            // lblReaderCategory
            // 
            lblReaderCategory.AutoSize = true;
            lblReaderCategory.BorderStyle = BorderStyle.Fixed3D;
            lblReaderCategory.Location = new Point(200, 174);
            lblReaderCategory.Name = "lblReaderCategory";
            lblReaderCategory.Size = new Size(2, 33);
            lblReaderCategory.TabIndex = 7;
            // 
            // lblReaderName
            // 
            lblReaderName.AutoSize = true;
            lblReaderName.BorderStyle = BorderStyle.Fixed3D;
            lblReaderName.Location = new Point(203, 122);
            lblReaderName.Name = "lblReaderName";
            lblReaderName.Size = new Size(2, 33);
            lblReaderName.TabIndex = 6;
            // 
            // txtReaderId
            // 
            txtReaderId.Location = new Point(200, 66);
            txtReaderId.Name = "txtReaderId";
            txtReaderId.Size = new Size(313, 38);
            txtReaderId.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(36, 290);
            label5.Name = "label5";
            label5.Size = new Size(134, 31);
            label5.TabIndex = 4;
            label5.Text = "已借数量：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(36, 231);
            label4.Name = "label4";
            label4.Size = new Size(134, 31);
            label4.TabIndex = 3;
            label4.Text = "可借数量：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(36, 174);
            label3.Name = "label3";
            label3.Size = new Size(134, 31);
            label3.TabIndex = 2;
            label3.Text = "读者类别：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(36, 122);
            label2.Name = "label2";
            label2.Size = new Size(134, 31);
            label2.TabIndex = 1;
            label2.Text = "读者姓名：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 69);
            label1.Name = "label1";
            label1.Size = new Size(169, 31);
            label1.TabIndex = 0;
            label1.Text = "读者ID/卡号：";
            // 
            // gbBookInfo
            // 
            gbBookInfo.Controls.Add(lblAvailableCount);
            gbBookInfo.Controls.Add(lblStock);
            gbBookInfo.Controls.Add(lblPublisher);
            gbBookInfo.Controls.Add(lblAuthor);
            gbBookInfo.Controls.Add(btnSearchBook);
            gbBookInfo.Controls.Add(txtBookId);
            gbBookInfo.Controls.Add(lblBookName);
            gbBookInfo.Controls.Add(label11);
            gbBookInfo.Controls.Add(label10);
            gbBookInfo.Controls.Add(label9);
            gbBookInfo.Controls.Add(label8);
            gbBookInfo.Controls.Add(label7);
            gbBookInfo.Controls.Add(label6);
            gbBookInfo.Location = new Point(3, 426);
            gbBookInfo.Name = "gbBookInfo";
            gbBookInfo.Size = new Size(1424, 332);
            gbBookInfo.TabIndex = 1;
            gbBookInfo.TabStop = false;
            gbBookInfo.Text = "图书信息";
            // 
            // lblAvailableCount
            // 
            lblAvailableCount.AutoSize = true;
            lblAvailableCount.BorderStyle = BorderStyle.Fixed3D;
            lblAvailableCount.Location = new Point(200, 274);
            lblAvailableCount.Name = "lblAvailableCount";
            lblAvailableCount.Size = new Size(2, 33);
            lblAvailableCount.TabIndex = 20;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.BorderStyle = BorderStyle.Fixed3D;
            lblStock.Location = new Point(200, 229);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(2, 33);
            lblStock.TabIndex = 19;
            // 
            // lblPublisher
            // 
            lblPublisher.AutoSize = true;
            lblPublisher.BorderStyle = BorderStyle.Fixed3D;
            lblPublisher.Location = new Point(200, 187);
            lblPublisher.Name = "lblPublisher";
            lblPublisher.Size = new Size(2, 33);
            lblPublisher.TabIndex = 18;
            // 
            // lblAuthor
            // 
            lblAuthor.AutoSize = true;
            lblAuthor.BorderStyle = BorderStyle.Fixed3D;
            lblAuthor.Location = new Point(200, 143);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(2, 33);
            lblAuthor.TabIndex = 17;
            // 
            // btnSearchBook
            // 
            btnSearchBook.Location = new Point(579, 52);
            btnSearchBook.Name = "btnSearchBook";
            btnSearchBook.Size = new Size(150, 46);
            btnSearchBook.TabIndex = 16;
            btnSearchBook.Text = "查询";
            btnSearchBook.UseVisualStyleBackColor = true;
            btnSearchBook.Click += btnSearchBook_Click;
            // 
            // txtBookId
            // 
            txtBookId.Location = new Point(203, 57);
            txtBookId.Name = "txtBookId";
            txtBookId.Size = new Size(310, 38);
            txtBookId.TabIndex = 15;
            // 
            // lblBookName
            // 
            lblBookName.AutoSize = true;
            lblBookName.BorderStyle = BorderStyle.Fixed3D;
            lblBookName.Location = new Point(200, 102);
            lblBookName.Name = "lblBookName";
            lblBookName.Size = new Size(2, 33);
            lblBookName.TabIndex = 10;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(36, 57);
            label11.Name = "label11";
            label11.Size = new Size(177, 31);
            label11.TabIndex = 9;
            label11.Text = "图书ID/ISBN：";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(36, 229);
            label10.Name = "label10";
            label10.Size = new Size(134, 31);
            label10.TabIndex = 8;
            label10.Text = "库存数量：";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(36, 276);
            label9.Name = "label9";
            label9.Size = new Size(134, 31);
            label9.TabIndex = 7;
            label9.Text = "可借数量：";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(36, 102);
            label8.Name = "label8";
            label8.Size = new Size(134, 31);
            label8.TabIndex = 6;
            label8.Text = "图书名称：";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(36, 145);
            label7.Name = "label7";
            label7.Size = new Size(86, 31);
            label7.TabIndex = 5;
            label7.Text = "作者：";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(36, 189);
            label6.Name = "label6";
            label6.Size = new Size(110, 31);
            label6.TabIndex = 4;
            label6.Text = "出版社：";
            // 
            // gbBorrowInfo
            // 
            gbBorrowInfo.Controls.Add(txtRemark);
            gbBorrowInfo.Controls.Add(dtpDueDate);
            gbBorrowInfo.Controls.Add(dtpBorrowDate);
            gbBorrowInfo.Controls.Add(label14);
            gbBorrowInfo.Controls.Add(label13);
            gbBorrowInfo.Controls.Add(label12);
            gbBorrowInfo.Location = new Point(3, 764);
            gbBorrowInfo.Name = "gbBorrowInfo";
            gbBorrowInfo.Size = new Size(1424, 226);
            gbBorrowInfo.TabIndex = 2;
            gbBorrowInfo.TabStop = false;
            gbBorrowInfo.Text = "借阅信息";
            // 
            // txtRemark
            // 
            txtRemark.Location = new Point(200, 160);
            txtRemark.Name = "txtRemark";
            txtRemark.Size = new Size(400, 38);
            txtRemark.TabIndex = 9;
            // 
            // dtpDueDate
            // 
            dtpDueDate.Location = new Point(200, 107);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.Size = new Size(400, 38);
            dtpDueDate.TabIndex = 8;
            // 
            // dtpBorrowDate
            // 
            dtpBorrowDate.Location = new Point(200, 51);
            dtpBorrowDate.Name = "dtpBorrowDate";
            dtpBorrowDate.Size = new Size(400, 38);
            dtpBorrowDate.TabIndex = 7;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(36, 57);
            label14.Name = "label14";
            label14.Size = new Size(134, 31);
            label14.TabIndex = 6;
            label14.Text = "借阅日期：";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(36, 107);
            label13.Name = "label13";
            label13.Size = new Size(134, 31);
            label13.TabIndex = 5;
            label13.Text = "应还日期：";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(36, 160);
            label12.Name = "label12";
            label12.Size = new Size(86, 31);
            label12.TabIndex = 4;
            label12.Text = "备注：";
            // 
            // gbCurrentBorrow
            // 
            gbCurrentBorrow.Controls.Add(dgvCurrentBorrow);
            gbCurrentBorrow.Location = new Point(12, 996);
            gbCurrentBorrow.Name = "gbCurrentBorrow";
            gbCurrentBorrow.Size = new Size(1415, 268);
            gbCurrentBorrow.TabIndex = 3;
            gbCurrentBorrow.TabStop = false;
            gbCurrentBorrow.Text = "当前借阅记录";
            // 
            // dgvCurrentBorrow
            // 
            dgvCurrentBorrow.AllowUserToAddRows = false;
            dgvCurrentBorrow.AllowUserToDeleteRows = false;
            dgvCurrentBorrow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCurrentBorrow.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCurrentBorrow.Location = new Point(6, 37);
            dgvCurrentBorrow.MultiSelect = false;
            dgvCurrentBorrow.Name = "dgvCurrentBorrow";
            dgvCurrentBorrow.ReadOnly = true;
            dgvCurrentBorrow.RowHeadersVisible = false;
            dgvCurrentBorrow.RowHeadersWidth = 82;
            dgvCurrentBorrow.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCurrentBorrow.Size = new Size(1403, 225);
            dgvCurrentBorrow.TabIndex = 0;
            // 
            // FrmBorrowBook
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1439, 1275);
            Controls.Add(gbCurrentBorrow);
            Controls.Add(gbBorrowInfo);
            Controls.Add(gbBookInfo);
            Controls.Add(gbReaderInfo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Location = new Point(10, 10);
            Name = "FrmBorrowBook";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "图书借阅";
            gbReaderInfo.ResumeLayout(false);
            gbReaderInfo.PerformLayout();
            gbBookInfo.ResumeLayout(false);
            gbBookInfo.PerformLayout();
            gbBorrowInfo.ResumeLayout(false);
            gbBorrowInfo.PerformLayout();
            gbCurrentBorrow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCurrentBorrow).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbReaderInfo;
        private GroupBox gbBookInfo;
        private GroupBox gbBorrowInfo;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label14;
        private Label label13;
        private Label label12;
        private TextBox txtReaderId;
        private Label lblReaderName;
        private Label lblMaxBorrow;
        private Label label15;
        private Label lblTotalBorrowed;
        private Label lblBorrowedCount;
        private Label lblReaderCategory;
        private Button btnSearchReader;
        private Label lblAuthor;
        private Button btnSearchBook;
        private TextBox txtBookId;
        private Label lblBookName;
        private Label lblAvailableCount;
        private Label lblStock;
        private Label lblPublisher;
        private TextBox txtRemark;
        private DateTimePicker dtpDueDate;
        private DateTimePicker dtpBorrowDate;
        private GroupBox gbCurrentBorrow;
        private DataGridView dgvCurrentBorrow;
    }
}
namespace BookManagement.Forms
{
    partial class FrmReturnBook
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
            dgvBorrowList = new DataGridView();
            gbSearchCondition = new GroupBox();
            txtBorrowId = new TextBox();
            txtISBN = new TextBox();
            txtReaderId = new TextBox();
            lblReaderId = new Label();
            lblBorrowId = new Label();
            lblISBN = new Label();
            btnResetForm = new Button();
            btnSearch = new Button();
            gbBorrowDetail = new GroupBox();
            txtRemark = new TextBox();
            lblRemark = new Label();
            dtpReturnDate = new DateTimePicker();
            lblOverdueDates = new Label();
            lblDetailLateFee = new Label();
            lblDueDate = new Label();
            lblLateFee = new Label();
            lblBorrowDate = new Label();
            lblBookName = new Label();
            lblDetailBookName = new Label();
            lblDetailBorrowDate = new Label();
            lblDetailDueDate = new Label();
            lblReturnDate = new Label();
            lblOverdueDays = new Label();
            lblReaderName = new Label();
            lblDetailReaderName = new Label();
            btnReturn = new Button();
            btnReset = new Button();
            btnCalculate = new Button();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvBorrowList).BeginInit();
            gbSearchCondition.SuspendLayout();
            gbBorrowDetail.SuspendLayout();
            SuspendLayout();
            // 
            // dgvBorrowList
            // 
            dgvBorrowList.AllowUserToAddRows = false;
            dgvBorrowList.AllowUserToDeleteRows = false;
            dgvBorrowList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBorrowList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBorrowList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBorrowList.Location = new Point(6, 6);
            dgvBorrowList.MultiSelect = false;
            dgvBorrowList.Name = "dgvBorrowList";
            dgvBorrowList.ReadOnly = true;
            dgvBorrowList.RowHeadersWidth = 82;
            dgvBorrowList.Size = new Size(1600, 392);
            dgvBorrowList.TabIndex = 0;
            // 
            // gbSearchCondition
            // 
            gbSearchCondition.Controls.Add(txtBorrowId);
            gbSearchCondition.Controls.Add(txtISBN);
            gbSearchCondition.Controls.Add(txtReaderId);
            gbSearchCondition.Controls.Add(lblReaderId);
            gbSearchCondition.Controls.Add(lblBorrowId);
            gbSearchCondition.Controls.Add(lblISBN);
            gbSearchCondition.Controls.Add(btnResetForm);
            gbSearchCondition.Controls.Add(btnSearch);
            gbSearchCondition.Location = new Point(6, 407);
            gbSearchCondition.Name = "gbSearchCondition";
            gbSearchCondition.Size = new Size(730, 431);
            gbSearchCondition.TabIndex = 1;
            gbSearchCondition.TabStop = false;
            gbSearchCondition.Text = "查询条件";
            // 
            // txtBorrowId
            // 
            txtBorrowId.Location = new Point(184, 244);
            txtBorrowId.Name = "txtBorrowId";
            txtBorrowId.Size = new Size(337, 38);
            txtBorrowId.TabIndex = 12;
            // 
            // txtISBN
            // 
            txtISBN.Location = new Point(184, 167);
            txtISBN.Name = "txtISBN";
            txtISBN.Size = new Size(337, 38);
            txtISBN.TabIndex = 11;
            // 
            // txtReaderId
            // 
            txtReaderId.Location = new Point(184, 90);
            txtReaderId.Name = "txtReaderId";
            txtReaderId.Size = new Size(337, 38);
            txtReaderId.TabIndex = 10;
            // 
            // lblReaderId
            // 
            lblReaderId.AutoSize = true;
            lblReaderId.Font = new Font("Microsoft YaHei UI", 11F);
            lblReaderId.Location = new Point(23, 88);
            lblReaderId.Name = "lblReaderId";
            lblReaderId.Size = new Size(139, 39);
            lblReaderId.TabIndex = 9;
            lblReaderId.Text = "读者ID：";
            // 
            // lblBorrowId
            // 
            lblBorrowId.AutoSize = true;
            lblBorrowId.Font = new Font("Microsoft YaHei UI", 11F);
            lblBorrowId.Location = new Point(23, 242);
            lblBorrowId.Name = "lblBorrowId";
            lblBorrowId.Size = new Size(137, 39);
            lblBorrowId.TabIndex = 8;
            lblBorrowId.Text = "借阅号：";
            // 
            // lblISBN
            // 
            lblISBN.AutoSize = true;
            lblISBN.Font = new Font("Microsoft YaHei UI", 11F);
            lblISBN.Location = new Point(23, 167);
            lblISBN.Name = "lblISBN";
            lblISBN.Size = new Size(176, 39);
            lblISBN.TabIndex = 6;
            lblISBN.Text = "图书ISBN：";
            // 
            // btnResetForm
            // 
            btnResetForm.Location = new Point(486, 364);
            btnResetForm.Name = "btnResetForm";
            btnResetForm.Size = new Size(150, 46);
            btnResetForm.TabIndex = 1;
            btnResetForm.Text = "重置";
            btnResetForm.UseVisualStyleBackColor = true;
            btnResetForm.Click += btnResetForm_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(49, 364);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(150, 46);
            btnSearch.TabIndex = 0;
            btnSearch.Text = "查询";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // gbBorrowDetail
            // 
            gbBorrowDetail.Controls.Add(txtRemark);
            gbBorrowDetail.Controls.Add(lblRemark);
            gbBorrowDetail.Controls.Add(dtpReturnDate);
            gbBorrowDetail.Controls.Add(lblOverdueDates);
            gbBorrowDetail.Controls.Add(lblDetailLateFee);
            gbBorrowDetail.Controls.Add(lblDueDate);
            gbBorrowDetail.Controls.Add(lblLateFee);
            gbBorrowDetail.Controls.Add(lblBorrowDate);
            gbBorrowDetail.Controls.Add(lblBookName);
            gbBorrowDetail.Controls.Add(lblDetailBookName);
            gbBorrowDetail.Controls.Add(lblDetailBorrowDate);
            gbBorrowDetail.Controls.Add(lblDetailDueDate);
            gbBorrowDetail.Controls.Add(lblReturnDate);
            gbBorrowDetail.Controls.Add(lblOverdueDays);
            gbBorrowDetail.Controls.Add(lblReaderName);
            gbBorrowDetail.Controls.Add(lblDetailReaderName);
            gbBorrowDetail.Location = new Point(742, 407);
            gbBorrowDetail.Name = "gbBorrowDetail";
            gbBorrowDetail.Size = new Size(858, 431);
            gbBorrowDetail.TabIndex = 2;
            gbBorrowDetail.TabStop = false;
            gbBorrowDetail.Text = "借阅详情";
            // 
            // txtRemark
            // 
            txtRemark.Location = new Point(157, 326);
            txtRemark.Name = "txtRemark";
            txtRemark.Size = new Size(400, 38);
            txtRemark.TabIndex = 22;
            // 
            // lblRemark
            // 
            lblRemark.AutoSize = true;
            lblRemark.Location = new Point(25, 326);
            lblRemark.Name = "lblRemark";
            lblRemark.Size = new Size(86, 31);
            lblRemark.TabIndex = 21;
            lblRemark.Text = "备注：";
            // 
            // dtpReturnDate
            // 
            dtpReturnDate.Format = DateTimePickerFormat.Short;
            dtpReturnDate.Location = new Point(157, 269);
            dtpReturnDate.Name = "dtpReturnDate";
            dtpReturnDate.Size = new Size(400, 38);
            dtpReturnDate.TabIndex = 20;
            // 
            // lblOverdueDates
            // 
            lblOverdueDates.AutoSize = true;
            lblOverdueDates.BorderStyle = BorderStyle.Fixed3D;
            lblOverdueDates.Location = new Point(695, 77);
            lblOverdueDates.Name = "lblOverdueDates";
            lblOverdueDates.Size = new Size(2, 33);
            lblOverdueDates.TabIndex = 19;
            // 
            // lblDetailLateFee
            // 
            lblDetailLateFee.AutoSize = true;
            lblDetailLateFee.Location = new Point(454, 123);
            lblDetailLateFee.Name = "lblDetailLateFee";
            lblDetailLateFee.Size = new Size(206, 31);
            lblDetailLateFee.TabIndex = 18;
            lblDetailLateFee.Text = "逾期费用（元）：";
            // 
            // lblDueDate
            // 
            lblDueDate.AutoSize = true;
            lblDueDate.BorderStyle = BorderStyle.Fixed3D;
            lblDueDate.Location = new Point(157, 216);
            lblDueDate.Name = "lblDueDate";
            lblDueDate.Size = new Size(2, 33);
            lblDueDate.TabIndex = 17;
            // 
            // lblLateFee
            // 
            lblLateFee.AutoSize = true;
            lblLateFee.BorderStyle = BorderStyle.Fixed3D;
            lblLateFee.Location = new Point(695, 123);
            lblLateFee.Name = "lblLateFee";
            lblLateFee.Size = new Size(2, 33);
            lblLateFee.TabIndex = 16;
            // 
            // lblBorrowDate
            // 
            lblBorrowDate.AutoSize = true;
            lblBorrowDate.BorderStyle = BorderStyle.Fixed3D;
            lblBorrowDate.Location = new Point(157, 172);
            lblBorrowDate.Name = "lblBorrowDate";
            lblBorrowDate.Size = new Size(2, 33);
            lblBorrowDate.TabIndex = 15;
            // 
            // lblBookName
            // 
            lblBookName.AutoSize = true;
            lblBookName.BorderStyle = BorderStyle.Fixed3D;
            lblBookName.Location = new Point(157, 123);
            lblBookName.Name = "lblBookName";
            lblBookName.Size = new Size(2, 33);
            lblBookName.TabIndex = 14;
            // 
            // lblDetailBookName
            // 
            lblDetailBookName.AutoSize = true;
            lblDetailBookName.Location = new Point(25, 123);
            lblDetailBookName.Name = "lblDetailBookName";
            lblDetailBookName.Size = new Size(134, 31);
            lblDetailBookName.TabIndex = 13;
            lblDetailBookName.Text = "图书名称：";
            // 
            // lblDetailBorrowDate
            // 
            lblDetailBorrowDate.AutoSize = true;
            lblDetailBorrowDate.Location = new Point(25, 167);
            lblDetailBorrowDate.Name = "lblDetailBorrowDate";
            lblDetailBorrowDate.Size = new Size(134, 31);
            lblDetailBorrowDate.TabIndex = 12;
            lblDetailBorrowDate.Text = "借出日期：";
            // 
            // lblDetailDueDate
            // 
            lblDetailDueDate.AutoSize = true;
            lblDetailDueDate.Location = new Point(25, 216);
            lblDetailDueDate.Name = "lblDetailDueDate";
            lblDetailDueDate.Size = new Size(134, 31);
            lblDetailDueDate.TabIndex = 11;
            lblDetailDueDate.Text = "应还日期：";
            // 
            // lblReturnDate
            // 
            lblReturnDate.AutoSize = true;
            lblReturnDate.Location = new Point(25, 269);
            lblReturnDate.Name = "lblReturnDate";
            lblReturnDate.Size = new Size(134, 31);
            lblReturnDate.TabIndex = 10;
            lblReturnDate.Text = "归还日期：";
            // 
            // lblOverdueDays
            // 
            lblOverdueDays.AutoSize = true;
            lblOverdueDays.Location = new Point(526, 79);
            lblOverdueDays.Name = "lblOverdueDays";
            lblOverdueDays.Size = new Size(134, 31);
            lblOverdueDays.TabIndex = 9;
            lblOverdueDays.Text = "逾期天数：";
            // 
            // lblReaderName
            // 
            lblReaderName.AutoSize = true;
            lblReaderName.BorderStyle = BorderStyle.Fixed3D;
            lblReaderName.Location = new Point(157, 79);
            lblReaderName.Name = "lblReaderName";
            lblReaderName.Size = new Size(2, 33);
            lblReaderName.TabIndex = 8;
            // 
            // lblDetailReaderName
            // 
            lblDetailReaderName.AutoSize = true;
            lblDetailReaderName.Location = new Point(25, 79);
            lblDetailReaderName.Name = "lblDetailReaderName";
            lblDetailReaderName.Size = new Size(134, 31);
            lblDetailReaderName.TabIndex = 7;
            lblDetailReaderName.Text = "读者姓名：";
            // 
            // btnReturn
            // 
            btnReturn.Font = new Font("Microsoft YaHei UI", 11F);
            btnReturn.Location = new Point(492, 844);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(217, 58);
            btnReturn.TabIndex = 3;
            btnReturn.Text = "归还";
            btnReturn.UseVisualStyleBackColor = true;
            btnReturn.Click += btnReturn_Click;
            // 
            // btnReset
            // 
            btnReset.Font = new Font("Microsoft YaHei UI", 11F);
            btnReset.Location = new Point(881, 844);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(208, 58);
            btnReset.TabIndex = 4;
            btnReset.Text = "重置";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnCalculate
            // 
            btnCalculate.Font = new Font("Microsoft YaHei UI", 11F);
            btnCalculate.Location = new Point(76, 844);
            btnCalculate.Name = "btnCalculate";
            btnCalculate.Size = new Size(224, 58);
            btnCalculate.TabIndex = 5;
            btnCalculate.Text = "计算逾期费用";
            btnCalculate.UseVisualStyleBackColor = true;
            btnCalculate.Click += btnCalculate_Click;
            // 
            // btnClose
            // 
            btnClose.Font = new Font("Microsoft YaHei UI", 11F);
            btnClose.Location = new Point(1268, 844);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(201, 58);
            btnClose.TabIndex = 3;
            btnClose.Text = "关闭";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // FrmReturnBook
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1612, 1053);
            Controls.Add(btnClose);
            Controls.Add(btnCalculate);
            Controls.Add(btnReset);
            Controls.Add(btnReturn);
            Controls.Add(gbBorrowDetail);
            Controls.Add(gbSearchCondition);
            Controls.Add(dgvBorrowList);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmReturnBook";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "图书归还";
            ((System.ComponentModel.ISupportInitialize)dgvBorrowList).EndInit();
            gbSearchCondition.ResumeLayout(false);
            gbSearchCondition.PerformLayout();
            gbBorrowDetail.ResumeLayout(false);
            gbBorrowDetail.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvBorrowList;
        private GroupBox gbSearchCondition;
        private Label lblReaderId;
        private Label lblBorrowId;
        private Label lblISBN;
        private Button btnResetForm;
        private Button btnSearch;
        private GroupBox gbBorrowDetail;
        private Label lblDetailReaderName;
        private Button btnReturn;
        private Button btnReset;
        private Button btnCalculate;
        private Button btnClose;
        private Label lblDetailBookName;
        private Label lblDetailBorrowDate;
        private Label lblDetailDueDate;
        private Label lblReturnDate;
        private Label lblOverdueDays;
        private Label lblReaderName;
        private TextBox txtBorrowId;
        private TextBox txtISBN;
        private TextBox txtReaderId;
        private Label lblDueDate;
        private Label lblLateFee;
        private Label lblBorrowDate;
        private Label lblBookName;
        private Label lblDetailLateFee;
        private Label lblOverdueDates;
        private Label lblRemark;
        private DateTimePicker dtpReturnDate;
        private TextBox txtRemark;
    }
}
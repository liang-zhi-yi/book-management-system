namespace BookManagement.Forms
{
    partial class FrmBorrowRecord
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
            gbSearchCondition = new GroupBox();
            btnExport = new Button();
            btnReset = new Button();
            btnClose = new Button();
            btnSearch = new Button();
            dtpReturnTo = new DateTimePicker();
            dtpReturnFrom = new DateTimePicker();
            lblTo2 = new Label();
            dtpBorrowTo = new DateTimePicker();
            dtpBorrowFrom = new DateTimePicker();
            cmbCategory = new ComboBox();
            cmbStatus = new ComboBox();
            txtReaderName = new TextBox();
            txtBookName = new TextBox();
            txtISBN = new TextBox();
            txtReaderId = new TextBox();
            lblReaderName = new Label();
            lblBookName = new Label();
            lblISBN = new Label();
            lblStatus = new Label();
            lblBookCategory = new Label();
            lblBorrowDate = new Label();
            lblTo1 = new Label();
            lblReturnDate = new Label();
            lblReaderId = new Label();
            pnlStatistics = new Panel();
            lblOverdue = new Label();
            lblReturned = new Label();
            lblTotalLateFee = new Label();
            lblBorrowing = new Label();
            lblTotalRecords = new Label();
            dgvBorrowRecord = new DataGridView();
            btnRefresh = new Button();
            btnCloseBottom = new Button();
            lblPageInfo = new Label();
            btnFirst = new Button();
            btnPrev = new Button();
            btnNext = new Button();
            btnLast = new Button();
            gbSearchCondition.SuspendLayout();
            pnlStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBorrowRecord).BeginInit();
            SuspendLayout();
            // 
            // gbSearchCondition
            // 
            gbSearchCondition.Controls.Add(btnExport);
            gbSearchCondition.Controls.Add(btnReset);
            gbSearchCondition.Controls.Add(btnClose);
            gbSearchCondition.Controls.Add(btnSearch);
            gbSearchCondition.Controls.Add(dtpReturnTo);
            gbSearchCondition.Controls.Add(dtpReturnFrom);
            gbSearchCondition.Controls.Add(lblTo2);
            gbSearchCondition.Controls.Add(dtpBorrowTo);
            gbSearchCondition.Controls.Add(dtpBorrowFrom);
            gbSearchCondition.Controls.Add(cmbCategory);
            gbSearchCondition.Controls.Add(cmbStatus);
            gbSearchCondition.Controls.Add(txtReaderName);
            gbSearchCondition.Controls.Add(txtBookName);
            gbSearchCondition.Controls.Add(txtISBN);
            gbSearchCondition.Controls.Add(txtReaderId);
            gbSearchCondition.Controls.Add(lblReaderName);
            gbSearchCondition.Controls.Add(lblBookName);
            gbSearchCondition.Controls.Add(lblISBN);
            gbSearchCondition.Controls.Add(lblStatus);
            gbSearchCondition.Controls.Add(lblBookCategory);
            gbSearchCondition.Controls.Add(lblBorrowDate);
            gbSearchCondition.Controls.Add(lblTo1);
            gbSearchCondition.Controls.Add(lblReturnDate);
            gbSearchCondition.Controls.Add(lblReaderId);
            gbSearchCondition.Location = new Point(12, 12);
            gbSearchCondition.Name = "gbSearchCondition";
            gbSearchCondition.Size = new Size(1371, 374);
            gbSearchCondition.TabIndex = 0;
            gbSearchCondition.TabStop = false;
            gbSearchCondition.Text = "查询条件";
            // 
            // btnExport
            // 
            btnExport.Font = new Font("Microsoft YaHei UI", 11F);
            btnExport.Location = new Point(757, 283);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(150, 46);
            btnExport.TabIndex = 26;
            btnExport.Text = "导出";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // btnReset
            // 
            btnReset.Font = new Font("Microsoft YaHei UI", 11F);
            btnReset.Location = new Point(397, 283);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(150, 46);
            btnReset.TabIndex = 25;
            btnReset.Text = "重置";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnClose
            // 
            btnClose.Font = new Font("Microsoft YaHei UI", 11F);
            btnClose.Location = new Point(1097, 283);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(150, 46);
            btnClose.TabIndex = 24;
            btnClose.Text = "关闭";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnSearch
            // 
            btnSearch.Font = new Font("Microsoft YaHei UI", 11F);
            btnSearch.Location = new Point(72, 283);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(150, 46);
            btnSearch.TabIndex = 23;
            btnSearch.Text = "查询";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // dtpReturnTo
            // 
            dtpReturnTo.Location = new Point(909, 186);
            dtpReturnTo.Name = "dtpReturnTo";
            dtpReturnTo.Size = new Size(242, 38);
            dtpReturnTo.TabIndex = 20;
            // 
            // dtpReturnFrom
            // 
            dtpReturnFrom.Location = new Point(595, 186);
            dtpReturnFrom.Name = "dtpReturnFrom";
            dtpReturnFrom.Size = new Size(242, 38);
            dtpReturnFrom.TabIndex = 19;
            // 
            // lblTo2
            // 
            lblTo2.AutoSize = true;
            lblTo2.Location = new Point(854, 193);
            lblTo2.Name = "lblTo2";
            lblTo2.Size = new Size(38, 31);
            lblTo2.TabIndex = 18;
            lblTo2.Text = "至";
            // 
            // dtpBorrowTo
            // 
            dtpBorrowTo.Location = new Point(909, 146);
            dtpBorrowTo.Name = "dtpBorrowTo";
            dtpBorrowTo.Size = new Size(242, 38);
            dtpBorrowTo.TabIndex = 17;
            // 
            // dtpBorrowFrom
            // 
            dtpBorrowFrom.Location = new Point(595, 145);
            dtpBorrowFrom.Name = "dtpBorrowFrom";
            dtpBorrowFrom.Size = new Size(242, 38);
            dtpBorrowFrom.TabIndex = 16;
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(595, 96);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(242, 39);
            cmbCategory.TabIndex = 15;
            // 
            // cmbStatus
            // 
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(595, 51);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(242, 39);
            cmbStatus.TabIndex = 14;
            // 
            // txtReaderName
            // 
            txtReaderName.Location = new Point(147, 96);
            txtReaderName.Name = "txtReaderName";
            txtReaderName.Size = new Size(200, 38);
            txtReaderName.TabIndex = 13;
            // 
            // txtBookName
            // 
            txtBookName.Location = new Point(147, 146);
            txtBookName.Name = "txtBookName";
            txtBookName.Size = new Size(200, 38);
            txtBookName.TabIndex = 12;
            // 
            // txtISBN
            // 
            txtISBN.Location = new Point(147, 190);
            txtISBN.Name = "txtISBN";
            txtISBN.Size = new Size(200, 38);
            txtISBN.TabIndex = 11;
            // 
            // txtReaderId
            // 
            txtReaderId.Location = new Point(147, 47);
            txtReaderId.Name = "txtReaderId";
            txtReaderId.Size = new Size(200, 38);
            txtReaderId.TabIndex = 9;
            // 
            // lblReaderName
            // 
            lblReaderName.AutoSize = true;
            lblReaderName.Location = new Point(19, 99);
            lblReaderName.Name = "lblReaderName";
            lblReaderName.Size = new Size(134, 31);
            lblReaderName.TabIndex = 8;
            lblReaderName.Text = "读者姓名：";
            // 
            // lblBookName
            // 
            lblBookName.AutoSize = true;
            lblBookName.Location = new Point(19, 146);
            lblBookName.Name = "lblBookName";
            lblBookName.Size = new Size(134, 31);
            lblBookName.TabIndex = 7;
            lblBookName.Text = "图书名称：";
            // 
            // lblISBN
            // 
            lblISBN.AutoSize = true;
            lblISBN.Location = new Point(19, 193);
            lblISBN.Name = "lblISBN";
            lblISBN.Size = new Size(94, 31);
            lblISBN.TabIndex = 6;
            lblISBN.Text = "ISBN：";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(475, 54);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(134, 31);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "借阅状态：";
            // 
            // lblBookCategory
            // 
            lblBookCategory.AutoSize = true;
            lblBookCategory.Location = new Point(475, 99);
            lblBookCategory.Name = "lblBookCategory";
            lblBookCategory.Size = new Size(134, 31);
            lblBookCategory.TabIndex = 4;
            lblBookCategory.Text = "图书类别：";
            // 
            // lblBorrowDate
            // 
            lblBorrowDate.AutoSize = true;
            lblBorrowDate.Location = new Point(475, 146);
            lblBorrowDate.Name = "lblBorrowDate";
            lblBorrowDate.Size = new Size(134, 31);
            lblBorrowDate.TabIndex = 3;
            lblBorrowDate.Text = "借阅日期：";
            // 
            // lblTo1
            // 
            lblTo1.AutoSize = true;
            lblTo1.Location = new Point(854, 149);
            lblTo1.Name = "lblTo1";
            lblTo1.Size = new Size(38, 31);
            lblTo1.TabIndex = 2;
            lblTo1.Text = "至";
            // 
            // lblReturnDate
            // 
            lblReturnDate.AutoSize = true;
            lblReturnDate.Location = new Point(475, 193);
            lblReturnDate.Name = "lblReturnDate";
            lblReturnDate.Size = new Size(134, 31);
            lblReturnDate.TabIndex = 1;
            lblReturnDate.Text = "归还日期：";
            // 
            // lblReaderId
            // 
            lblReaderId.AutoSize = true;
            lblReaderId.Location = new Point(19, 54);
            lblReaderId.Name = "lblReaderId";
            lblReaderId.Size = new Size(111, 31);
            lblReaderId.TabIndex = 0;
            lblReaderId.Text = "读者ID：";
            // 
            // pnlStatistics
            // 
            pnlStatistics.BorderStyle = BorderStyle.FixedSingle;
            pnlStatistics.Controls.Add(lblOverdue);
            pnlStatistics.Controls.Add(lblReturned);
            pnlStatistics.Controls.Add(lblTotalLateFee);
            pnlStatistics.Controls.Add(lblBorrowing);
            pnlStatistics.Controls.Add(lblTotalRecords);
            pnlStatistics.Location = new Point(12, 392);
            pnlStatistics.Name = "pnlStatistics";
            pnlStatistics.Size = new Size(1368, 99);
            pnlStatistics.TabIndex = 1;
            // 
            // lblOverdue
            // 
            lblOverdue.AutoSize = true;
            lblOverdue.Location = new Point(765, 35);
            lblOverdue.Name = "lblOverdue";
            lblOverdue.Size = new Size(100, 31);
            lblOverdue.TabIndex = 4;
            lblOverdue.Text = "逾期：0";
            // 
            // lblReturned
            // 
            lblReturned.AutoSize = true;
            lblReturned.Location = new Point(515, 35);
            lblReturned.Name = "lblReturned";
            lblReturned.Size = new Size(124, 31);
            lblReturned.TabIndex = 3;
            lblReturned.Text = "已归还：0";
            // 
            // lblTotalLateFee
            // 
            lblTotalLateFee.AutoSize = true;
            lblTotalLateFee.Location = new Point(1006, 35);
            lblTotalLateFee.Name = "lblTotalLateFee";
            lblTotalLateFee.Size = new Size(206, 31);
            lblTotalLateFee.TabIndex = 2;
            lblTotalLateFee.Text = "总逾期费：0.00元";
            // 
            // lblBorrowing
            // 
            lblBorrowing.AutoSize = true;
            lblBorrowing.Location = new Point(273, 35);
            lblBorrowing.Name = "lblBorrowing";
            lblBorrowing.Size = new Size(124, 31);
            lblBorrowing.TabIndex = 1;
            lblBorrowing.Text = "借出中：0";
            // 
            // lblTotalRecords
            // 
            lblTotalRecords.AutoSize = true;
            lblTotalRecords.Location = new Point(47, 35);
            lblTotalRecords.Name = "lblTotalRecords";
            lblTotalRecords.Size = new Size(124, 31);
            lblTotalRecords.TabIndex = 0;
            lblTotalRecords.Text = "总记录：0";
            // 
            // dgvBorrowRecord
            // 
            dgvBorrowRecord.AllowUserToAddRows = false;
            dgvBorrowRecord.AllowUserToDeleteRows = false;
            dgvBorrowRecord.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBorrowRecord.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBorrowRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBorrowRecord.Location = new Point(12, 497);
            dgvBorrowRecord.MultiSelect = false;
            dgvBorrowRecord.Name = "dgvBorrowRecord";
            dgvBorrowRecord.ReadOnly = true;
            dgvBorrowRecord.RowHeadersVisible = false;
            dgvBorrowRecord.RowHeadersWidth = 82;
            dgvBorrowRecord.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBorrowRecord.Size = new Size(1371, 298);
            dgvBorrowRecord.TabIndex = 2;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1013, 831);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(150, 46);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "刷新";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnCloseBottom
            // 
            btnCloseBottom.Location = new Point(1200, 831);
            btnCloseBottom.Name = "btnCloseBottom";
            btnCloseBottom.Size = new Size(150, 46);
            btnCloseBottom.TabIndex = 28;
            btnCloseBottom.Text = "关闭";
            btnCloseBottom.UseVisualStyleBackColor = true;
            btnCloseBottom.Click += btnCloseBottom_Click;
            // 
            // lblPageInfo
            // 
            lblPageInfo.AutoSize = true;
            lblPageInfo.Location = new Point(31, 839);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(168, 31);
            lblPageInfo.TabIndex = 3;
            lblPageInfo.Text = "第一页/共一页";
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(221, 831);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(150, 46);
            btnFirst.TabIndex = 4;
            btnFirst.Text = "首页";
            btnFirst.UseVisualStyleBackColor = true;
            btnFirst.Click += btnFirst_Click;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(420, 831);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(150, 46);
            btnPrev.TabIndex = 6;
            btnPrev.Text = "上页";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += btnPrev_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(620, 831);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(150, 46);
            btnNext.TabIndex = 7;
            btnNext.Text = "下页";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(812, 831);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(150, 46);
            btnLast.TabIndex = 27;
            btnLast.Text = "末页";
            btnLast.UseVisualStyleBackColor = true;
            btnLast.Click += btnLast_Click;
            // 
            // FrmBorrowRecord
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1392, 985);
            Controls.Add(btnCloseBottom);
            Controls.Add(btnLast);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnRefresh);
            Controls.Add(btnFirst);
            Controls.Add(lblPageInfo);
            Controls.Add(dgvBorrowRecord);
            Controls.Add(pnlStatistics);
            Controls.Add(gbSearchCondition);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmBorrowRecord";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "借阅记录查询";
            gbSearchCondition.ResumeLayout(false);
            gbSearchCondition.PerformLayout();
            pnlStatistics.ResumeLayout(false);
            pnlStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBorrowRecord).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox gbSearchCondition;
        private Panel pnlStatistics;
        private Label lblReaderName;
        private Label lblBookName;
        private Label lblISBN;
        private Label lblStatus;
        private Label lblBookCategory;
        private Label lblBorrowDate;
        private Label lblTo1;
        private Label lblReturnDate;
        private Label lblReaderId;
        private DateTimePicker dtpBorrowTo;
        private DateTimePicker dtpBorrowFrom;
        private ComboBox cmbCategory;
        private ComboBox cmbStatus;
        private TextBox txtReaderName;
        private TextBox txtBookName;
        private TextBox txtISBN;
        private TextBox txtReaderId;
        private Button btnExport;
        private Button btnReset;
        private Button btnClose;
        private Button btnSearch;
        private ComboBox cmbOverdue;
        private Label lblOverdue;
        private DateTimePicker dtpReturnTo;
        private DateTimePicker dtpReturnFrom;
        private Label lblTo2;
        private Label lblTotalRecords;
        private Label label5;
        private Label lblReturned;
        private Label lblTotalLateFee;
        private Label lblBorrowing;
        private DataGridView dgvBorrowRecord;
        private Button btnRefresh;
        private Button btnCloseBottom;
        private Label lblPageInfo;
        private Button btnFirst;
        private Button btnPrev;
        private Button btnNext;
        private Button btnLast;
    }
}
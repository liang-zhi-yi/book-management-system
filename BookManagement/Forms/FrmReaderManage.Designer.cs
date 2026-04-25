namespace BookManagement.Forms
{
    partial class FrmReaderManage
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
            dgvReaderList = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            btnCancel = new Label();
            label6 = new Label();
            btnSave = new Label();
            btnDelete = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            btnAdd = new Label();
            label14 = new Label();
            btnEdit = new Label();
            groupBox1 = new GroupBox();
            cmbStatus = new ComboBox();
            cmbReaderCategory = new ComboBox();
            cmbGender = new ComboBox();
            txtAddress = new RichTextBox();
            dtpExpiryDate = new DateTimePicker();
            dtpRegistrationDate = new DateTimePicker();
            txtRemark = new RichTextBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();
            txtReaderName = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvReaderList).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvReaderList
            // 
            dgvReaderList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReaderList.Location = new Point(891, 1);
            dgvReaderList.Name = "dgvReaderList";
            dgvReaderList.RowHeadersWidth = 82;
            dgvReaderList.Size = new Size(621, 705);
            dgvReaderList.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 442);
            label1.Name = "label1";
            label1.Size = new Size(134, 31);
            label1.TabIndex = 1;
            label1.Text = "注册日期：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 143);
            label2.Name = "label2";
            label2.Size = new Size(68, 31);
            label2.TabIndex = 2;
            label2.Text = "性别:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 379);
            label3.Name = "label3";
            label3.Size = new Size(86, 31);
            label3.TabIndex = 3;
            label3.Text = "地址：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 199);
            label4.Name = "label4";
            label4.Size = new Size(134, 31);
            label4.TabIndex = 4;
            label4.Text = "读者类别：";
            // 
            // btnCancel
            // 
            btnCancel.AutoSize = true;
            btnCancel.BorderStyle = BorderStyle.FixedSingle;
            btnCancel.Font = new Font("Microsoft YaHei UI", 15F);
            btnCancel.ForeColor = Color.Red;
            btnCancel.Location = new Point(1410, 807);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(104, 54);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "取消";
            btnCancel.Click += btnCancel_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 568);
            label6.Name = "label6";
            label6.Size = new Size(86, 31);
            label6.TabIndex = 6;
            label6.Text = "状态：";
            // 
            // btnSave
            // 
            btnSave.AutoSize = true;
            btnSave.BorderStyle = BorderStyle.FixedSingle;
            btnSave.Font = new Font("Microsoft YaHei UI", 15F);
            btnSave.ForeColor = SystemColors.ActiveCaption;
            btnSave.Location = new Point(1059, 807);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(104, 54);
            btnSave.TabIndex = 7;
            btnSave.Text = "保存";
            btnSave.Click += btnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.AutoSize = true;
            btnDelete.BorderStyle = BorderStyle.FixedSingle;
            btnDelete.Font = new Font("Microsoft YaHei UI", 15F);
            btnDelete.ForeColor = Color.Red;
            btnDelete.Location = new Point(736, 807);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(104, 54);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "删除";
            btnDelete.Click += btnDelete_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(16, 259);
            label9.Name = "label9";
            label9.Size = new Size(86, 31);
            label9.TabIndex = 9;
            label9.Text = "电话：";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(16, 317);
            label10.Name = "label10";
            label10.Size = new Size(86, 31);
            label10.TabIndex = 10;
            label10.Text = "邮箱：";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(16, 504);
            label11.Name = "label11";
            label11.Size = new Size(134, 31);
            label11.TabIndex = 11;
            label11.Text = "有效期至：";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(16, 90);
            label12.Name = "label12";
            label12.Size = new Size(86, 31);
            label12.TabIndex = 12;
            label12.Text = "姓名：";
            // 
            // btnAdd
            // 
            btnAdd.AutoSize = true;
            btnAdd.BorderStyle = BorderStyle.FixedSingle;
            btnAdd.Font = new Font("Microsoft YaHei UI", 15F);
            btnAdd.ForeColor = SystemColors.ActiveCaption;
            btnAdd.Location = new Point(109, 807);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(104, 54);
            btnAdd.TabIndex = 13;
            btnAdd.Text = "新增";
            btnAdd.Click += btnAdd_Click;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(16, 622);
            label14.Name = "label14";
            label14.Size = new Size(86, 31);
            label14.TabIndex = 14;
            label14.Text = "备注：";
            // 
            // btnEdit
            // 
            btnEdit.AutoSize = true;
            btnEdit.BorderStyle = BorderStyle.FixedSingle;
            btnEdit.Font = new Font("Microsoft YaHei UI", 15F);
            btnEdit.ForeColor = SystemColors.ActiveCaption;
            btnEdit.Location = new Point(419, 807);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(104, 54);
            btnEdit.TabIndex = 15;
            btnEdit.Text = "修改";
            btnEdit.Click += btnEdit_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cmbStatus);
            groupBox1.Controls.Add(cmbReaderCategory);
            groupBox1.Controls.Add(cmbGender);
            groupBox1.Controls.Add(txtAddress);
            groupBox1.Controls.Add(dtpExpiryDate);
            groupBox1.Controls.Add(dtpRegistrationDate);
            groupBox1.Controls.Add(txtRemark);
            groupBox1.Controls.Add(txtPhone);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(txtReaderName);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(-4, 1);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(889, 705);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "读者信息";
            // 
            // cmbStatus
            // 
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(143, 568);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(242, 39);
            cmbStatus.TabIndex = 24;
            // 
            // cmbReaderCategory
            // 
            cmbReaderCategory.FormattingEnabled = true;
            cmbReaderCategory.Location = new Point(143, 196);
            cmbReaderCategory.Name = "cmbReaderCategory";
            cmbReaderCategory.Size = new Size(242, 39);
            cmbReaderCategory.TabIndex = 23;
            // 
            // cmbGender
            // 
            cmbGender.FormattingEnabled = true;
            cmbGender.Location = new Point(143, 143);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(242, 39);
            cmbGender.TabIndex = 22;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(143, 379);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(666, 45);
            txtAddress.TabIndex = 21;
            txtAddress.Text = "";
            // 
            // dtpExpiryDate
            // 
            dtpExpiryDate.Location = new Point(143, 504);
            dtpExpiryDate.Name = "dtpExpiryDate";
            dtpExpiryDate.Size = new Size(400, 38);
            dtpExpiryDate.TabIndex = 20;
            // 
            // dtpRegistrationDate
            // 
            dtpRegistrationDate.Location = new Point(143, 442);
            dtpRegistrationDate.Name = "dtpRegistrationDate";
            dtpRegistrationDate.Size = new Size(400, 38);
            dtpRegistrationDate.TabIndex = 19;
            // 
            // txtRemark
            // 
            txtRemark.Location = new Point(143, 622);
            txtRemark.Name = "txtRemark";
            txtRemark.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtRemark.Size = new Size(638, 66);
            txtRemark.TabIndex = 18;
            txtRemark.Text = "";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(143, 259);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(638, 38);
            txtPhone.TabIndex = 17;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(143, 317);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(638, 38);
            txtEmail.TabIndex = 16;
            // 
            // txtReaderName
            // 
            txtReaderName.Location = new Point(143, 90);
            txtReaderName.Name = "txtReaderName";
            txtReaderName.Size = new Size(242, 38);
            txtReaderName.TabIndex = 15;
            // 
            // FrmReaderManage
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1908, 1043);
            Controls.Add(groupBox1);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(dgvReaderList);
            ForeColor = SystemColors.ActiveCaption;
            Name = "FrmReaderManage";
            Text = "读者管理";
            Load += FrmReaderManage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvReaderList).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvReaderList;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label btnCancel;
        private Label label6;
        private Label btnSave;
        private Label btnDelete;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label btnAdd;
        private Label label14;
        private Label btnEdit;
        private GroupBox groupBox1;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtReaderName;
        private RichTextBox txtRemark;
        private RichTextBox txtAddress;
        private DateTimePicker dtpExpiryDate;
        private DateTimePicker dtpRegistrationDate;
        private ComboBox cmbStatus;
        private ComboBox cmbReaderCategory;
        private ComboBox cmbGender;
    }
}
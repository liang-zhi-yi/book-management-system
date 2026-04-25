namespace BookManagement.Forms
{
    partial class FrmMain
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
            menuStrip1 = new MenuStrip();
            系统管理SToolStripMenuItem = new ToolStripMenuItem();
            tsmiBookCategoryManage = new ToolStripMenuItem();
            tsmiBookManage = new ToolStripMenuItem();
            tsmiReaderCategoryManage = new ToolStripMenuItem();
            tsmiReaderManage = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            tsmiBorrowBook = new ToolStripMenuItem();
            tsmiReturnBook = new ToolStripMenuItem();
            tmsiBorrowRecord = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripSeparator();
            tsmiExit = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { 系统管理SToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1235, 39);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // 系统管理SToolStripMenuItem
            // 
            系统管理SToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tsmiBookCategoryManage, tsmiBookManage, tsmiReaderCategoryManage, tsmiReaderManage, toolStripMenuItem1, tsmiBorrowBook, tsmiReturnBook, tmsiBorrowRecord, toolStripMenuItem6, tsmiExit });
            系统管理SToolStripMenuItem.Name = "系统管理SToolStripMenuItem";
            系统管理SToolStripMenuItem.Size = new Size(192, 35);
            系统管理SToolStripMenuItem.Text = "系统管理（&S）";
            // 
            // tsmiBookCategoryManage
            // 
            tsmiBookCategoryManage.Name = "tsmiBookCategoryManage";
            tsmiBookCategoryManage.Size = new Size(359, 44);
            tsmiBookCategoryManage.Text = "图书类别管理";
            tsmiBookCategoryManage.Click += tsmiBookCategoryManage_Click;
            // 
            // tsmiBookManage
            // 
            tsmiBookManage.Name = "tsmiBookManage";
            tsmiBookManage.Size = new Size(359, 44);
            tsmiBookManage.Text = "图书管理（&B)";
            tsmiBookManage.Click += tsmiBookManage_Click;
            // 
            // tsmiReaderCategoryManage
            // 
            tsmiReaderCategoryManage.Name = "tsmiReaderCategoryManage";
            tsmiReaderCategoryManage.Size = new Size(359, 44);
            tsmiReaderCategoryManage.Text = "读者类别管理（&R）";
            tsmiReaderCategoryManage.Click += tsmiReaderCategoryManage_Click;
            // 
            // tsmiReaderManage
            // 
            tsmiReaderManage.Name = "tsmiReaderManage";
            tsmiReaderManage.Size = new Size(359, 44);
            tsmiReaderManage.Text = "读者管理（&D）";
            tsmiReaderManage.Click += tsmiReaderManage_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(356, 6);
            // 
            // tsmiBorrowBook
            // 
            tsmiBorrowBook.Name = "tsmiBorrowBook";
            tsmiBorrowBook.Size = new Size(359, 44);
            tsmiBorrowBook.Text = "图书借阅（&L）";
            tsmiBorrowBook.Click += tsmiBorrowBook_Click;
            // 
            // tsmiReturnBook
            // 
            tsmiReturnBook.Name = "tsmiReturnBook";
            tsmiReturnBook.Size = new Size(359, 44);
            tsmiReturnBook.Text = "图书归还（&R）";
            tsmiReturnBook.Click += tsmiReturnBook_Click;
            // 
            // tmsiBorrowRecord
            // 
            tmsiBorrowRecord.Name = "tmsiBorrowRecord";
            tmsiBorrowRecord.Size = new Size(359, 44);
            tmsiBorrowRecord.Text = "借阅记录（&H）";
            tmsiBorrowRecord.Click += tsmiBorrowRecord_Click;
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(356, 6);
            // 
            // tsmiExit
            // 
            tsmiExit.Name = "tsmiExit";
            tsmiExit.Size = new Size(359, 44);
            tsmiExit.Text = "退出系统（&X)";
            tsmiExit.Click += tsmiExit_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1235, 687);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FrmMain";
            Text = "FrmMain";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 系统管理SToolStripMenuItem;
        private ToolStripMenuItem tsmiBookCategoryManage;
        private ToolStripMenuItem tsmiBookManage;
        private ToolStripMenuItem tsmiReaderCategoryManage;
        private ToolStripMenuItem tsmiReaderManage;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem tsmiExit;
        private ToolStripMenuItem tsmiBorrowBook;
        private ToolStripMenuItem tsmiReturnBook;
        private ToolStripMenuItem tmsiBorrowRecord;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripSeparator toolStripMenuItem2;

    }
}
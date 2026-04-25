import tkinter as tk
from tkinter import ttk, messagebox
from ui.book_category import FrmBookCategoryManage
from ui.book_manage import FrmBookManage
from ui.reader_category import FrmReaderCategoryManage
from ui.reader_manage import FrmReaderManage
from ui.borrow_book import FrmBorrowBook
from ui.return_book import FrmReturnBook
from ui.borrow_record import FrmBorrowRecord


class FrmMain:
    def __init__(self, user):
        self.current_user = user
        self.root = tk.Tk()
        self.root.title(f"图书管理系统 - 管理员 ({user.real_name})" if user.user_type == 0 else f"图书管理系统 - 读者 ({user.real_name})")
        self.root.geometry("800x600")

        self.setup_menu()

        if user.user_type == 1:
            self.menu_bar.delete(0, 4)

    def setup_menu(self):
        self.menu_bar = tk.Menu(self.root)
        self.root.config(menu=self.menu_bar)

        self.menu_book = tk.Menu(self.menu_bar, tearoff=0)
        self.menu_bar.add_cascade(label="图书管理", menu=self.menu_book)
        self.menu_book.add_command(label="图书类别管理", command=self.open_book_category)
        self.menu_book.add_command(label="图书管理", command=self.open_book_manage)

        self.menu_reader = tk.Menu(self.menu_bar, tearoff=0)
        self.menu_bar.add_cascade(label="读者管理", menu=self.menu_reader)
        self.menu_reader.add_command(label="读者类别管理", command=self.open_reader_category)
        self.menu_reader.add_command(label="读者管理", command=self.open_reader_manage)

        self.menu_borrow = tk.Menu(self.menu_bar, tearoff=0)
        self.menu_bar.add_cascade(label="借阅管理", menu=self.menu_borrow)
        self.menu_borrow.add_command(label="借阅图书", command=self.open_borrow_book)
        self.menu_borrow.add_command(label="归还图书", command=self.open_return_book)
        self.menu_borrow.add_command(label="借阅记录", command=self.open_borrow_record)

        self.menu_bar.add_command(label="退出", command=self.exit_app)

        tk.Label(self.root, text=f"欢迎使用图书管理系统！\n当前用户：{self.current_user.real_name}",
                 font=("Arial", 16)).place(x=250, y=250)

    def open_book_category(self):
        FrmBookCategoryManage()

    def open_book_manage(self):
        FrmBookManage()

    def open_reader_category(self):
        FrmReaderCategoryManage()

    def open_reader_manage(self):
        FrmReaderManage()

    def open_borrow_book(self):
        FrmBorrowBook(self.current_user)

    def open_return_book(self):
        FrmReturnBook()

    def open_borrow_record(self):
        FrmBorrowRecord()

    def exit_app(self):
        result = messagebox.askyesno("确认退出", "确定要退出系统吗？")
        if result:
            self.root.destroy()

    def show(self):
        self.root.mainloop()
import tkinter as tk
from tkinter import ttk, messagebox
from bll.borrow_manager import BorrowManager
from bll.reader_manager import ReaderManager
from bll.book_manager import BookManager
from datetime import date, timedelta


class FrmBorrowBook:
    def __init__(self, user):
        self.user = user
        self.borrow_manager = BorrowManager()
        self.reader_manager = ReaderManager()
        self.book_manager = BookManager()
        self.top = tk.Toplevel()
        self.top.title("借阅图书")
        self.top.geometry("800x500")
        self.setup_ui()
        self.load_data()

    def setup_ui(self):
        left_frame = tk.Frame(self.top)
        left_frame.pack(side=tk.LEFT, fill=tk.BOTH, expand=True, padx=10, pady=10)

        tk.Label(left_frame, text="读者信息", font=("Arial", 12)).pack()
        reader_columns = ("编号", "姓名", "类别", "电话", "状态")
        self.reader_tree = ttk.Treeview(left_frame, columns=reader_columns, show="headings", height=8)
        self.reader_tree.pack(fill=tk.X, pady=5)
        widths = [60, 80, 80, 100, 60]
        for col, w in zip(reader_columns, widths):
            self.reader_tree.column(col, width=w)
            self.reader_tree.heading(col, text=col)
        self.reader_tree.bind("<<TreeviewSelect>>", self.on_reader_select)

        tk.Label(left_frame, text="图书信息", font=("Arial", 12)).pack(pady=(20, 5))
        book_columns = ("编号", "书名", "ISBN", "可借数量")
        self.book_tree = ttk.Treeview(left_frame, columns=book_columns, show="headings", height=8)
        self.book_tree.pack(fill=tk.X, pady=5)
        widths = [60, 150, 120, 80]
        for col, w in zip(book_columns, widths):
            self.book_tree.column(col, width=w)
            self.book_tree.heading(col, text=col)
        self.book_tree.bind("<<TreeviewSelect>>", self.on_book_select)

        right_frame = tk.Frame(self.top)
        right_frame.pack(side=tk.RIGHT, fill=tk.BOTH, padx=10, pady=10)

        tk.Label(right_frame, text="借阅信息", font=("Arial", 12)).pack()
        info_frame = tk.Frame(right_frame)
        info_frame.pack(pady=10)

        tk.Label(info_frame, text="读者ID:").grid(row=0, column=0, padx=5, pady=5, sticky=tk.W)
        self.lbl_reader_id = tk.Label(info_frame, text="", bg="#f0f0f0", width=15)
        self.lbl_reader_id.grid(row=0, column=1, padx=5, pady=5)

        tk.Label(info_frame, text="图书ID:").grid(row=1, column=0, padx=5, pady=5, sticky=tk.W)
        self.lbl_book_id = tk.Label(info_frame, text="", bg="#f0f0f0", width=15)
        self.lbl_book_id.grid(row=1, column=1, padx=5, pady=5)

        tk.Label(info_frame, text="借阅日期:").grid(row=2, column=0, padx=5, pady=5, sticky=tk.W)
        self.lbl_borrow_date = tk.Label(info_frame, text=date.today().strftime('%Y-%m-%d'), bg="#f0f0f0", width=15)
        self.lbl_borrow_date.grid(row=2, column=1, padx=5, pady=5)

        tk.Label(info_frame, text="应还日期:").grid(row=3, column=0, padx=5, pady=5, sticky=tk.W)
        self.lbl_due_date = tk.Label(info_frame, text=(date.today() + timedelta(days=30)).strftime('%Y-%m-%d'), bg="#f0f0f0", width=15)
        self.lbl_due_date.grid(row=3, column=1, padx=5, pady=5)

        self.btn_borrow = tk.Button(right_frame, text="确认借阅", font=("Arial", 12), width=12, command=self.borrow)
        self.btn_borrow.pack(pady=20)

    def load_data(self):
        for item in self.reader_tree.get_children():
            self.reader_tree.delete(item)
        rows = self.reader_manager.get_all_readers()
        status_text = ["正常", "挂失", "注销"]
        for row in rows:
            self.reader_tree.insert("", tk.END, values=(
                row['ReaderId'], row['ReaderName'], row['ReaderCategoryName'],
                row['Phone'], status_text[row['Status']] if row['Status'] < len(status_text) else "未知"
            ))

        for item in self.book_tree.get_children():
            self.book_tree.delete(item)
        rows = self.book_manager.get_all_books()
        for row in rows:
            if row['AvailableCount'] > 0:
                self.book_tree.insert("", tk.END, values=(
                    row['BookId'], row['BookName'], row['ISBN'], row['AvailableCount']
                ))

    def on_reader_select(self, event):
        selection = self.reader_tree.selection()
        if selection:
            values = self.reader_tree.item(selection[0], "values")
            self.lbl_reader_id.config(text=values[0])
            self.selected_reader_id = int(values[0])

    def on_book_select(self, event):
        selection = self.book_tree.selection()
        if selection:
            values = self.book_tree.item(selection[0], "values")
            self.lbl_book_id.config(text=values[0])
            self.selected_book_id = int(values[0])

    def borrow(self):
        if not hasattr(self, 'selected_reader_id') or not hasattr(self, 'selected_book_id'):
            messagebox.showwarning("提示", "请先选择读者和图书！")
            return
        try:
            borrow_date = self.lbl_borrow_date.cget("text")
            due_date = self.lbl_due_date.cget("text")
            if self.borrow_manager.borrow_book(self.selected_reader_id, self.selected_book_id, borrow_date, due_date):
                messagebox.showinfo("成功", "借阅成功！")
                self.load_data()
            else:
                messagebox.showerror("失败", "借阅失败！")
        except Exception as ex:
            messagebox.showerror("失败", str(ex))
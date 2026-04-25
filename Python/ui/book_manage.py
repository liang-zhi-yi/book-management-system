import tkinter as tk
from tkinter import ttk, messagebox
from bll.book_manager import BookManager
from bll.book_category_manager import BookCategoryManager
from models.book import Book


class FrmBookManage:
    def __init__(self):
        self.manager = BookManager()
        self.category_manager = BookCategoryManager()
        self.top = tk.Toplevel()
        self.top.title("图书管理")
        self.top.geometry("900x600")
        self.setup_ui()
        self.load_categories()
        self.load_data()

    def setup_ui(self):
        search_frame = tk.Frame(self.top)
        search_frame.pack(fill=tk.X, padx=10, pady=5)
        tk.Label(search_frame, text="图书名称:").pack(side=tk.LEFT)
        self.txt_search_name = tk.Entry(search_frame, width=15)
        self.txt_search_name.pack(side=tk.LEFT, padx=5)
        tk.Label(search_frame, text="ISBN:").pack(side=tk.LEFT)
        self.txt_search_isbn = tk.Entry(search_frame, width=15)
        self.txt_search_isbn.pack(side=tk.LEFT, padx=5)
        tk.Button(search_frame, text="查询", command=self.search).pack(side=tk.LEFT, padx=5)
        tk.Button(search_frame, text="刷新", command=self.load_data).pack(side=tk.LEFT)

        input_frame = tk.LabelFrame(self.top, text="图书信息")
        input_frame.pack(fill=tk.X, padx=10, pady=5)

        row1 = tk.Frame(input_frame)
        row1.pack(fill=tk.X, pady=2)
        tk.Label(row1, text="书名:").pack(side=tk.LEFT, padx=5)
        self.txt_book_name = tk.Entry(row1, width=20)
        self.txt_book_name.pack(side=tk.LEFT, padx=5)
        tk.Label(row1, text="作者:").pack(side=tk.LEFT, padx=5)
        self.txt_author = tk.Entry(row1, width=20)
        self.txt_author.pack(side=tk.LEFT, padx=5)
        tk.Label(row1, text="出版社:").pack(side=tk.LEFT, padx=5)
        self.txt_publisher = tk.Entry(row1, width=20)
        self.txt_publisher.pack(side=tk.LEFT, padx=5)

        row2 = tk.Frame(input_frame)
        row2.pack(fill=tk.X, pady=2)
        tk.Label(row2, text="ISBN:").pack(side=tk.LEFT, padx=5)
        self.txt_isbn = tk.Entry(row2, width=20)
        self.txt_isbn.pack(side=tk.LEFT, padx=5)
        tk.Label(row2, text="类别:").pack(side=tk.LEFT, padx=5)
        self.cmb_category = ttk.Combobox(row2, width=18, state="readonly")
        self.cmb_category.pack(side=tk.LEFT, padx=5)
        tk.Label(row2, text="单价:").pack(side=tk.LEFT, padx=5)
        self.txt_price = tk.Entry(row2, width=15)
        self.txt_price.pack(side=tk.LEFT, padx=5)

        row3 = tk.Frame(input_frame)
        row3.pack(fill=tk.X, pady=2)
        tk.Label(row3, text="总数量:").pack(side=tk.LEFT, padx=5)
        self.txt_total_count = tk.Entry(row3, width=15)
        self.txt_total_count.pack(side=tk.LEFT, padx=5)
        self.current_id = None

        btn_frame = tk.Frame(self.top)
        btn_frame.pack(pady=5)
        tk.Button(btn_frame, text="添加", width=8, command=self.add).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="修改", width=8, command=self.update).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="删除", width=8, command=self.delete).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="清空", width=8, command=self.clear).pack(side=tk.LEFT, padx=5)

        columns = ("编号", "书名", "作者", "出版社", "ISBN", "类别", "总数量", "可借", "单价")
        self.tree = ttk.Treeview(self.top, columns=columns, show="headings", height=18)
        self.tree.pack(fill=tk.BOTH, expand=True, padx=10, pady=5)
        widths = [50, 120, 80, 100, 100, 80, 60, 60, 80]
        for col, w in zip(columns, widths):
            self.tree.column(col, width=w)
            self.tree.heading(col, text=col)
        self.tree.bind("<<TreeviewSelect>>", self.on_select)

    def load_categories(self):
        rows = self.category_manager.get_all_categories()
        self.categories = {row['CategoryName']: row['CategoryId'] for row in rows}
        self.cmb_category['values'] = list(self.categories.keys())

    def load_data(self):
        for item in self.tree.get_children():
            self.tree.delete(item)
        rows = self.manager.get_all_books()
        for row in rows:
            self.tree.insert("", tk.END, values=(
                row['BookId'], row['BookName'], row['Author'], row['Publisher'],
                row['ISBN'], row['CategoryName'], row['TotalCount'], row['AvailableCount'], row['Price']
            ))

    def search(self):
        name = self.txt_search_name.get().strip()
        isbn = self.txt_search_isbn.get().strip()
        for item in self.tree.get_children():
            self.tree.delete(item)
        rows = self.manager.get_all_books()
        for row in rows:
            if (not name or name in row['BookName']) and (not isbn or isbn in row['ISBN']):
                self.tree.insert("", tk.END, values=(
                    row['BookId'], row['BookName'], row['Author'], row['Publisher'],
                    row['ISBN'], row['CategoryName'], row['TotalCount'], row['AvailableCount'], row['Price']
                ))

    def on_select(self, event):
        selection = self.tree.selection()
        if selection:
            values = self.tree.item(selection[0], "values")
            self.current_id = values[0]
            self.txt_book_name.delete(0, tk.END)
            self.txt_book_name.insert(0, values[1])
            self.txt_author.delete(0, tk.END)
            self.txt_author.insert(0, values[2])
            self.txt_publisher.delete(0, tk.END)
            self.txt_publisher.insert(0, values[3])
            self.txt_isbn.delete(0, tk.END)
            self.txt_isbn.insert(0, values[4])
            self.cmb_category.set(values[5])
            self.txt_total_count.delete(0, tk.END)
            self.txt_total_count.insert(0, values[6])
            self.txt_price.delete(0, tk.END)
            self.txt_price.insert(0, values[8])

    def add(self):
        book = Book()
        book.book_name = self.txt_book_name.get().strip()
        book.author = self.txt_author.get().strip()
        book.publisher = self.txt_publisher.get().strip()
        book.isbn = self.txt_isbn.get().strip()
        book.category_id = self.categories.get(self.cmb_category.get(), 0)
        book.total_count = int(self.txt_total_count.get().strip() or "0")
        book.available_count = book.total_count
        book.price = float(self.txt_price.get().strip() or "0")
        try:
            if self.manager.add_book(book):
                messagebox.showinfo("成功", "添加成功！")
                self.load_data()
                self.clear()
        except Exception as ex:
            messagebox.showerror("失败", str(ex))

    def update(self):
        if not self.current_id:
            messagebox.showwarning("提示", "请先选择要修改的记录！")
            return
        book = Book()
        book.book_id = self.current_id
        book.book_name = self.txt_book_name.get().strip()
        book.author = self.txt_author.get().strip()
        book.publisher = self.txt_publisher.get().strip()
        book.isbn = self.txt_isbn.get().strip()
        book.category_id = self.categories.get(self.cmb_category.get(), 0)
        book.total_count = int(self.txt_total_count.get().strip() or "0")
        book.available_count = int(self.tree.item(self.tree.selection()[0])["values"][7])
        book.price = float(self.txt_price.get().strip() or "0")
        try:
            if self.manager.update_book(book):
                messagebox.showinfo("成功", "修改成功！")
                self.load_data()
                self.clear()
        except Exception as ex:
            messagebox.showerror("失败", str(ex))

    def delete(self):
        if not self.current_id:
            messagebox.showwarning("提示", "请先选择要删除的记录！")
            return
        if messagebox.askyesno("确认", "确定要删除吗？"):
            try:
                if self.manager.delete_book(self.current_id):
                    messagebox.showinfo("成功", "删除成功！")
                    self.load_data()
                    self.clear()
            except Exception as ex:
                messagebox.showerror("失败", str(ex))

    def clear(self):
        self.txt_book_name.delete(0, tk.END)
        self.txt_author.delete(0, tk.END)
        self.txt_publisher.delete(0, tk.END)
        self.txt_isbn.delete(0, tk.END)
        self.cmb_category.set("")
        self.txt_total_count.delete(0, tk.END)
        self.txt_price.delete(0, tk.END)
        self.current_id = None
import tkinter as tk
from tkinter import ttk, messagebox
from bll.reader_category_manager import ReaderCategoryManager
from models.reader_category import ReaderCategory


class FrmReaderCategoryManage:
    def __init__(self):
        self.manager = ReaderCategoryManager()
        self.top = tk.Toplevel()
        self.top.title("读者类别管理")
        self.top.geometry("650x450")
        self.setup_ui()
        self.load_data()

    def setup_ui(self):
        tk.Label(self.top, text="类别名称:").grid(row=0, column=0, padx=10, pady=10)
        self.txt_category_name = tk.Entry(self.top, width=15)
        self.txt_category_name.grid(row=0, column=1, padx=10, pady=10)

        tk.Label(self.top, text="最大借阅数量:").grid(row=0, column=2, padx=10, pady=10)
        self.txt_max_borrow = tk.Entry(self.top, width=10)
        self.txt_max_borrow.grid(row=0, column=3, padx=10, pady=10)

        tk.Label(self.top, text="借阅天数:").grid(row=1, column=0, padx=10, pady=10)
        self.txt_borrow_days = tk.Entry(self.top, width=10)
        self.txt_borrow_days.grid(row=1, column=1, padx=10, pady=10)

        tk.Label(self.top, text="逾期日费率:").grid(row=1, column=2, padx=10, pady=10)
        self.txt_late_fee = tk.Entry(self.top, width=10)
        self.txt_late_fee.grid(row=1, column=3, padx=10, pady=10)

        btn_frame = tk.Frame(self.top)
        btn_frame.grid(row=2, column=0, columnspan=4, pady=10)
        tk.Button(btn_frame, text="添加", width=8, command=self.add).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="修改", width=8, command=self.update).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="删除", width=8, command=self.delete).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="清空", width=8, command=self.clear).pack(side=tk.LEFT, padx=5)

        columns = ("类别ID", "类别名称", "最大借阅", "借阅天数", "逾期日费")
        self.tree = ttk.Treeview(self.top, columns=columns, show="headings", height=15)
        self.tree.grid(row=3, column=0, columnspan=4, padx=10, pady=10)
        widths = [80, 150, 100, 100, 100]
        for col, w in zip(columns, widths):
            self.tree.column(col, width=w)
            self.tree.heading(col, text=col)
        self.tree.bind("<<TreeviewSelect>>", self.on_select)

    def load_data(self):
        for item in self.tree.get_children():
            self.tree.delete(item)
        rows = self.manager.get_all_categories()
        for row in rows:
            self.tree.insert("", tk.END, values=(
                row['CategoryId'], row['CategoryName'], row['MaxBorrowCount'],
                row['BorrowDays'], row['LateFeePerDay']
            ))

    def on_select(self, event):
        selection = self.tree.selection()
        if selection:
            values = self.tree.item(selection[0], "values")
            self.current_id = values[0]
            self.txt_category_name.delete(0, tk.END)
            self.txt_category_name.insert(0, values[1])
            self.txt_max_borrow.delete(0, tk.END)
            self.txt_max_borrow.insert(0, values[2])
            self.txt_borrow_days.delete(0, tk.END)
            self.txt_borrow_days.insert(0, values[3])
            self.txt_late_fee.delete(0, tk.END)
            self.txt_late_fee.insert(0, values[4])

    def add(self):
        category = ReaderCategory()
        category.category_name = self.txt_category_name.get().strip()
        category.max_borrow_count = int(self.txt_max_borrow.get().strip() or "0")
        category.borrow_days = int(self.txt_borrow_days.get().strip() or "0")
        category.late_fee_per_day = float(self.txt_late_fee.get().strip() or "0")
        try:
            if self.manager.add_category(category):
                messagebox.showinfo("成功", "添加成功！")
                self.load_data()
                self.clear()
        except Exception as ex:
            messagebox.showerror("失败", str(ex))

    def update(self):
        if not hasattr(self, 'current_id'):
            messagebox.showwarning("提示", "请先选择要修改的记录！")
            return
        category = ReaderCategory()
        category.category_id = self.current_id
        category.category_name = self.txt_category_name.get().strip()
        category.max_borrow_count = int(self.txt_max_borrow.get().strip() or "0")
        category.borrow_days = int(self.txt_borrow_days.get().strip() or "0")
        category.late_fee_per_day = float(self.txt_late_fee.get().strip() or "0")
        try:
            if self.manager.update_category(category):
                messagebox.showinfo("成功", "修改成功！")
                self.load_data()
                self.clear()
        except Exception as ex:
            messagebox.showerror("失败", str(ex))

    def delete(self):
        if not hasattr(self, 'current_id'):
            messagebox.showwarning("提示", "请先选择要删除的记录！")
            return
        if messagebox.askyesno("确认", "确定要删除吗？"):
            try:
                if self.manager.delete_category(self.current_id):
                    messagebox.showinfo("成功", "删除成功！")
                    self.load_data()
                    self.clear()
            except Exception as ex:
                messagebox.showerror("失败", str(ex))

    def clear(self):
        self.txt_category_name.delete(0, tk.END)
        self.txt_max_borrow.delete(0, tk.END)
        self.txt_borrow_days.delete(0, tk.END)
        self.txt_late_fee.delete(0, tk.END)
        if hasattr(self, 'current_id'):
            del self.current_id
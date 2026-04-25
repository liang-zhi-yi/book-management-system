import tkinter as tk
from tkinter import ttk, messagebox
from bll.book_category_manager import BookCategoryManager
from models.book_category import BookCategory


class FrmBookCategoryManage:
    def __init__(self):
        self.manager = BookCategoryManager()
        self.top = tk.Toplevel()
        self.top.title("图书类别管理")
        self.top.geometry("600x400")
        self.setup_ui()
        self.load_data()

    def setup_ui(self):
        tk.Label(self.top, text="类别名称:").grid(row=0, column=0, padx=10, pady=10)
        self.txt_category_name = tk.Entry(self.top, width=20)
        self.txt_category_name.grid(row=0, column=1, padx=10, pady=10)

        tk.Label(self.top, text="描述:").grid(row=0, column=2, padx=10, pady=10)
        self.txt_description = tk.Entry(self.top, width=20)
        self.txt_description.grid(row=0, column=3, padx=10, pady=10)

        btn_frame = tk.Frame(self.top)
        btn_frame.grid(row=1, column=0, columnspan=4, pady=10)
        tk.Button(btn_frame, text="添加", width=8, command=self.add).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="修改", width=8, command=self.update).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="删除", width=8, command=self.delete).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="清空", width=8, command=self.clear).pack(side=tk.LEFT, padx=5)

        columns = ("类别ID", "类别名称", "描述")
        self.tree = ttk.Treeview(self.top, columns=columns, show="headings", height=15)
        self.tree.grid(row=2, column=0, columnspan=4, padx=10, pady=10)
        self.tree.column("类别ID", width=80)
        self.tree.column("类别名称", width=150)
        self.tree.column("描述", width=300)
        self.tree.heading("类别ID", text="类别ID")
        self.tree.heading("类别名称", text="类别名称")
        self.tree.heading("描述", text="描述")
        self.tree.bind("<<TreeviewSelect>>", self.on_select)

    def load_data(self):
        for item in self.tree.get_children():
            self.tree.delete(item)
        rows = self.manager.get_all_categories()
        for row in rows:
            self.tree.insert("", tk.END, values=(row['CategoryId'], row['CategoryName'], row['Description'] or ""))

    def on_select(self, event):
        selection = self.tree.selection()
        if selection:
            values = self.tree.item(selection[0], "values")
            self.txt_category_name.delete(0, tk.END)
            self.txt_category_name.insert(0, values[1])
            self.txt_description.delete(0, tk.END)
            self.txt_description.insert(0, values[2] if len(values) > 2 else "")
            self.current_id = values[0]

    def add(self):
        category = BookCategory()
        category.category_name = self.txt_category_name.get().strip()
        category.description = self.txt_description.get().strip()
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
        category = BookCategory()
        category.category_id = self.current_id
        category.category_name = self.txt_category_name.get().strip()
        category.description = self.txt_description.get().strip()
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
        self.txt_description.delete(0, tk.END)
        if hasattr(self, 'current_id'):
            del self.current_id
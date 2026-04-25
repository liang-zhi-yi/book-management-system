import tkinter as tk
from tkinter import ttk, messagebox
from bll.reader_manager import ReaderManager
from bll.reader_category_manager import ReaderCategoryManager
from models.reader import Reader
from datetime import date


class FrmReaderManage:
    def __init__(self):
        self.manager = ReaderManager()
        self.category_manager = ReaderCategoryManager()
        self.top = tk.Toplevel()
        self.top.title("读者管理")
        self.top.geometry("950x600")
        self.setup_ui()
        self.load_categories()
        self.load_data()

    def setup_ui(self):
        input_frame = tk.LabelFrame(self.top, text="读者信息")
        input_frame.pack(fill=tk.X, padx=10, pady=5)

        row1 = tk.Frame(input_frame)
        row1.pack(fill=tk.X, pady=2)
        tk.Label(row1, text="姓名:").pack(side=tk.LEFT, padx=5)
        self.txt_reader_name = tk.Entry(row1, width=12)
        self.txt_reader_name.pack(side=tk.LEFT, padx=5)
        tk.Label(row1, text="性别:").pack(side=tk.LEFT, padx=5)
        self.cmb_gender = ttk.Combobox(row1, width=8, state="readonly")
        self.cmb_gender['values'] = ("男", "女")
        self.cmb_gender.current(0)
        self.cmb_gender.pack(side=tk.LEFT, padx=5)
        tk.Label(row1, text="类别:").pack(side=tk.LEFT, padx=5)
        self.cmb_category = ttk.Combobox(row1, width=12, state="readonly")
        self.cmb_category.pack(side=tk.LEFT, padx=5)
        tk.Label(row1, text="电话:").pack(side=tk.LEFT, padx=5)
        self.txt_phone = tk.Entry(row1, width=15)
        self.txt_phone.pack(side=tk.LEFT, padx=5)

        row2 = tk.Frame(input_frame)
        row2.pack(fill=tk.X, pady=2)
        tk.Label(row2, text="邮箱:").pack(side=tk.LEFT, padx=5)
        self.txt_email = tk.Entry(row2, width=20)
        self.txt_email.pack(side=tk.LEFT, padx=5)
        tk.Label(row2, text="地址:").pack(side=tk.LEFT, padx=5)
        self.txt_address = tk.Entry(row2, width=25)
        self.txt_address.pack(side=tk.LEFT, padx=5)
        tk.Label(row2, text="状态:").pack(side=tk.LEFT, padx=5)
        self.cmb_status = ttk.Combobox(row2, width=10, state="readonly")
        self.cmb_status['values'] = ("正常", "挂失", "注销")
        self.cmb_status.current(0)
        self.cmb_status.pack(side=tk.LEFT, padx=5)

        row3 = tk.Frame(input_frame)
        row3.pack(fill=tk.X, pady=2)
        tk.Label(row3, text="注册日期:").pack(side=tk.LEFT, padx=5)
        self.txt_reg_date = tk.Entry(row3, width=12)
        self.txt_reg_date.pack(side=tk.LEFT, padx=5)
        self.txt_reg_date.insert(0, date.today().strftime('%Y-%m-%d'))
        tk.Label(row3, text="有效期至:").pack(side=tk.LEFT, padx=5)
        self.txt_expiry_date = tk.Entry(row3, width=12)
        self.txt_expiry_date.pack(side=tk.LEFT, padx=5)
        self.txt_expiry_date.insert(0, date.today().strftime('%Y-%m-%d'))
        tk.Label(row3, text="备注:").pack(side=tk.LEFT, padx=5)
        self.txt_remark = tk.Entry(row3, width=15)
        self.txt_remark.pack(side=tk.LEFT, padx=5)
        self.current_id = None

        btn_frame = tk.Frame(self.top)
        btn_frame.pack(pady=5)
        tk.Button(btn_frame, text="添加", width=8, command=self.add).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="修改", width=8, command=self.update).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="删除", width=8, command=self.delete).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="清空", width=8, command=self.clear).pack(side=tk.LEFT, padx=5)

        columns = ("编号", "姓名", "性别", "类别", "电话", "邮箱", "地址", "注册日期", "有效期至", "状态", "备注")
        self.tree = ttk.Treeview(self.top, columns=columns, show="headings", height=16)
        self.tree.pack(fill=tk.BOTH, expand=True, padx=10, pady=5)
        widths = [50, 70, 50, 80, 90, 130, 100, 80, 80, 60, 80]
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
        rows = self.manager.get_all_readers()
        status_text = ["正常", "挂失", "注销"]
        for row in rows:
            self.tree.insert("", tk.END, values=(
                row['ReaderId'], row['ReaderName'], row['Gender'],
                row['ReaderCategoryName'], row['Phone'], row['Email'],
                row['Address'], row['RegistrationDate'], row['ExpiryDate'],
                status_text[row['Status']] if row['Status'] < len(status_text) else "未知",
                row['Remark'] or ""
            ))

    def on_select(self, event):
        selection = self.tree.selection()
        if selection:
            values = self.tree.item(selection[0], "values")
            self.current_id = values[0]
            self.txt_reader_name.delete(0, tk.END)
            self.txt_reader_name.insert(0, values[1])
            self.cmb_gender.set(values[2])
            self.cmb_category.set(values[3])
            self.txt_phone.delete(0, tk.END)
            self.txt_phone.insert(0, values[4])
            self.txt_email.delete(0, tk.END)
            self.txt_email.insert(0, values[5])
            self.txt_address.delete(0, tk.END)
            self.txt_address.insert(0, values[6])
            self.txt_reg_date.delete(0, tk.END)
            self.txt_reg_date.insert(0, values[7])
            self.txt_expiry_date.delete(0, tk.END)
            self.txt_expiry_date.insert(0, values[8])
            self.cmb_status.set(values[9])
            self.txt_remark.delete(0, tk.END)
            self.txt_remark.insert(0, values[10])

    def add(self):
        reader = Reader()
        reader.reader_name = self.txt_reader_name.get().strip()
        reader.gender = self.cmb_gender.get()
        reader.reader_category_id = self.categories.get(self.cmb_category.get(), 0)
        reader.phone = self.txt_phone.get().strip()
        reader.email = self.txt_email.get().strip()
        reader.address = self.txt_address.get().strip()
        reader.registration_date = date.today()
        from datetime import timedelta
        reader.expiry_date = date.today() + timedelta(days=365)
        reader.status = self.cmb_status.current()
        reader.remark = self.txt_remark.get().strip()
        try:
            if self.manager.add_reader(reader):
                messagebox.showinfo("成功", "添加成功！")
                self.load_data()
                self.clear()
        except Exception as ex:
            messagebox.showerror("失败", str(ex))

    def update(self):
        if not self.current_id:
            messagebox.showwarning("提示", "请先选择要修改的记录！")
            return
        reader = Reader()
        reader.reader_id = self.current_id
        reader.reader_name = self.txt_reader_name.get().strip()
        reader.gender = self.cmb_gender.get()
        reader.reader_category_id = self.categories.get(self.cmb_category.get(), 0)
        reader.phone = self.txt_phone.get().strip()
        reader.email = self.txt_email.get().strip()
        reader.address = self.txt_address.get().strip()
        reader.registration_date = date.today()
        from datetime import timedelta
        reader.expiry_date = date.today() + timedelta(days=365)
        reader.status = self.cmb_status.current()
        reader.remark = self.txt_remark.get().strip()
        try:
            if self.manager.update_reader(reader):
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
                if self.manager.delete_reader(self.current_id):
                    messagebox.showinfo("成功", "删除成功！")
                    self.load_data()
                    self.clear()
            except Exception as ex:
                messagebox.showerror("失败", str(ex))

    def clear(self):
        self.txt_reader_name.delete(0, tk.END)
        self.cmb_gender.set("")
        self.cmb_category.set("")
        self.txt_phone.delete(0, tk.END)
        self.txt_email.delete(0, tk.END)
        self.txt_address.delete(0, tk.END)
        self.cmb_status.set("")
        self.txt_remark.delete(0, tk.END)
        self.current_id = None
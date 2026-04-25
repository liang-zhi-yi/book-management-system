import tkinter as tk
from tkinter import ttk, messagebox
from bll.borrow_manager import BorrowManager
from datetime import date


class FrmReturnBook:
    def __init__(self):
        self.borrow_manager = BorrowManager()
        self.top = tk.Toplevel()
        self.top.title("归还图书")
        self.top.geometry("800x500")
        self.setup_ui()
        self.load_data()

    def setup_ui(self):
        top_frame = tk.Frame(self.top)
        top_frame.pack(fill=tk.X, padx=10, pady=5)
        tk.Label(top_frame, text="借阅记录查询", font=("Arial", 12)).pack(side=tk.LEFT)

        search_frame = tk.Frame(self.top)
        search_frame.pack(fill=tk.X, padx=10, pady=5)
        tk.Label(search_frame, text="读者ID:").pack(side=tk.LEFT, padx=5)
        self.txt_reader_id = tk.Entry(search_frame, width=10)
        self.txt_reader_id.pack(side=tk.LEFT, padx=5)
        tk.Label(search_frame, text="ISBN:").pack(side=tk.LEFT, padx=5)
        self.txt_isbn = tk.Entry(search_frame, width=15)
        self.txt_isbn.pack(side=tk.LEFT, padx=5)
        tk.Button(search_frame, text="查询", command=self.search).pack(side=tk.LEFT, padx=5)
        tk.Button(search_frame, text="刷新", command=self.load_data).pack(side=tk.LEFT)

        columns = ("借阅ID", "读者ID", "读者姓名", "图书名称", "ISBN", "借阅日期", "应还日期", "状态")
        self.tree = ttk.Treeview(self.top, columns=columns, show="headings", height=12)
        self.tree.pack(fill=tk.BOTH, expand=True, padx=10, pady=5)
        widths = [70, 60, 80, 120, 100, 90, 90, 70]
        for col, w in zip(columns, widths):
            self.tree.column(col, width=w)
            self.tree.heading(col, text=col)
        self.tree.bind("<<TreeviewSelect>>", self.on_select)

        info_frame = tk.LabelFrame(self.top, text="还书信息")
        info_frame.pack(fill=tk.X, padx=10, pady=5)
        row = tk.Frame(info_frame)
        row.pack(pady=5)
        tk.Label(row, text="借阅ID:").grid(row=0, column=0, padx=5)
        self.lbl_borrow_id = tk.Label(row, text="", bg="#f0f0f0", width=10)
        self.lbl_borrow_id.grid(row=0, column=1, padx=5)
        tk.Label(row, text="还书日期:").grid(row=0, column=2, padx=5)
        self.txt_return_date = tk.Entry(row, width=12)
        self.txt_return_date.grid(row=0, column=3, padx=5)
        self.txt_return_date.insert(0, date.today().strftime('%Y-%m-%d'))
        tk.Label(row, text="逾期费用:").grid(row=0, column=4, padx=5)
        self.txt_late_fee = tk.Entry(row, width=10)
        self.txt_late_fee.grid(row=0, column=5, padx=5)
        tk.Label(row, text="备注:").grid(row=0, column=6, padx=5)
        self.txt_remark = tk.Entry(row, width=15)
        self.txt_remark.grid(row=0, column=7, padx=5)

        btn_frame = tk.Frame(self.top)
        btn_frame.pack(pady=10)
        tk.Button(btn_frame, text="计算逾期费", width=10, command=self.calculate_fee).pack(side=tk.LEFT, padx=5)
        tk.Button(btn_frame, text="确认还书", width=10, command=self.return_book).pack(side=tk.LEFT, padx=5)

    def load_data(self):
        for item in self.tree.get_children():
            self.tree.delete(item)
        rows = self.borrow_manager.search_borrow_records()
        status_text = ["借出中", "已归还", "逾期"]
        for row in rows:
            self.tree.insert("", tk.END, values=(
                row['BorrowId'], row['ReaderId'], row['ReaderName'],
                row['BookName'], row['ISBN'], row['BorrowDate'],
                row['DueDate'], status_text[row['Status']] if row['Status'] < len(status_text) else "未知"
            ))

    def search(self):
        reader_id = self.txt_reader_id.get().strip()
        isbn = self.txt_isbn.get().strip()
        for item in self.tree.get_children():
            self.tree.delete(item)
        rows = self.borrow_manager.search_borrow_records(
            reader_id=int(reader_id) if reader_id else None,
            isbn=isbn if isbn else None
        )
        status_text = ["借出中", "已归还", "逾期"]
        for row in rows:
            self.tree.insert("", tk.END, values=(
                row['BorrowId'], row['ReaderId'], row['ReaderName'],
                row['BookName'], row['ISBN'], row['BorrowDate'],
                row['DueDate'], status_text[row['Status']] if row['Status'] < len(status_text) else "未知"
            ))

    def on_select(self, event):
        selection = self.tree.selection()
        if selection:
            values = self.tree.item(selection[0], "values")
            self.lbl_borrow_id.config(text=values[0])
            self.selected_borrow_id = int(values[0])

    def calculate_fee(self):
        if not hasattr(self, 'selected_borrow_id'):
            messagebox.showwarning("提示", "请先选择借阅记录！")
            return
        try:
            return_date = self.txt_return_date.get().strip()
            fee = self.borrow_manager.calculate_late_fee(self.selected_borrow_id, return_date)
            self.txt_late_fee.delete(0, tk.END)
            self.txt_late_fee.insert(0, f"{fee:.2f}")
        except Exception as ex:
            messagebox.showerror("错误", str(ex))

    def return_book(self):
        if not hasattr(self, 'selected_borrow_id'):
            messagebox.showwarning("提示", "请先选择借阅记录！")
            return
        try:
            return_date = self.txt_return_date.get().strip()
            late_fee = float(self.txt_late_fee.get().strip() or "0")
            remark = self.txt_remark.get().strip()
            success, message, fee = self.borrow_manager.return_book(self.selected_borrow_id, return_date, late_fee, remark)
            if success:
                messagebox.showinfo("成功", f"{message}！\n逾期费用：{fee:.2f}元")
                self.load_data()
                self.clear()
            else:
                messagebox.showerror("失败", message)
        except Exception as ex:
            messagebox.showerror("失败", str(ex))

    def clear(self):
        self.lbl_borrow_id.config(text="")
        self.txt_late_fee.delete(0, tk.END)
        self.txt_remark.delete(0, tk.END)
        if hasattr(self, 'selected_borrow_id'):
            del self.selected_borrow_id
import tkinter as tk
from tkinter import ttk, messagebox
from bll.borrow_manager import BorrowManager
from datetime import date


class FrmBorrowRecord:
    def __init__(self):
        self.borrow_manager = BorrowManager()
        self.top = tk.Toplevel()
        self.top.title("借阅记录")
        self.top.geometry("1000x550")
        self.setup_ui()
        self.load_data()

    def setup_ui(self):
        search_frame = tk.LabelFrame(self.top, text="查询条件")
        search_frame.pack(fill=tk.X, padx=10, pady=5)

        row1 = tk.Frame(search_frame)
        row1.pack(fill=tk.X, pady=2)
        tk.Label(row1, text="读者ID:").pack(side=tk.LEFT, padx=5)
        self.txt_reader_id = tk.Entry(row1, width=8)
        self.txt_reader_id.pack(side=tk.LEFT, padx=5)
        tk.Label(row1, text="读者姓名:").pack(side=tk.LEFT, padx=5)
        self.txt_reader_name = tk.Entry(row1, width=10)
        self.txt_reader_name.pack(side=tk.LEFT, padx=5)
        tk.Label(row1, text="图书名称:").pack(side=tk.LEFT, padx=5)
        self.txt_book_name = tk.Entry(row1, width=12)
        self.txt_book_name.pack(side=tk.LEFT, padx=5)
        tk.Label(row1, text="ISBN:").pack(side=tk.LEFT, padx=5)
        self.txt_isbn = tk.Entry(row1, width=12)
        self.txt_isbn.pack(side=tk.LEFT, padx=5)

        row2 = tk.Frame(search_frame)
        row2.pack(fill=tk.X, pady=2)
        tk.Label(row2, text="状态:").pack(side=tk.LEFT, padx=5)
        self.cmb_status = ttk.Combobox(row2, width=8, state="readonly")
        self.cmb_status['values'] = ("全部", "借出中", "已归还", "逾期")
        self.cmb_status.current(0)
        self.cmb_status.pack(side=tk.LEFT, padx=5)
        tk.Label(row2, text="借阅日期:").pack(side=tk.LEFT, padx=5)
        self.txt_borrow_from = tk.Entry(row2, width=10)
        self.txt_borrow_from.pack(side=tk.LEFT, padx=5)
        tk.Label(row2, text="至:").pack(side=tk.LEFT)
        self.txt_borrow_to = tk.Entry(row2, width=10)
        self.txt_borrow_to.pack(side=tk.LEFT, padx=5)

        row3 = tk.Frame(search_frame)
        row3.pack(fill=tk.X, pady=2)
        tk.Label(row3, text="归还日期:").pack(side=tk.LEFT, padx=5)
        self.txt_return_from = tk.Entry(row3, width=10)
        self.txt_return_from.pack(side=tk.LEFT, padx=5)
        tk.Label(row3, text="至:").pack(side=tk.LEFT)
        self.txt_return_to = tk.Entry(row3, width=10)
        self.txt_return_to.pack(side=tk.LEFT, padx=5)
        tk.Button(row3, text="查询", width=8, command=self.search).pack(side=tk.LEFT, padx=20)
        tk.Button(row3, text="刷新", width=8, command=self.load_data).pack(side=tk.LEFT)

        columns = ("借阅ID", "读者ID", "读者姓名", "图书名称", "ISBN", "借阅日期", "应还日期", "归还日期", "状态", "逾期费", "备注")
        self.tree = ttk.Treeview(self.top, columns=columns, show="headings", height=16)
        self.tree.pack(fill=tk.BOTH, expand=True, padx=10, pady=5)
        widths = [60, 60, 80, 120, 90, 85, 85, 85, 70, 70, 100]
        for col, w in zip(columns, widths):
            self.tree.column(col, width=w)
            self.tree.heading(col, text=col)

        page_frame = tk.Frame(self.top)
        page_frame.pack(fill=tk.X, padx=10, pady=5)
        tk.Label(page_frame, text="第").pack(side=tk.LEFT)
        self.txt_page = tk.Entry(page_frame, width=5)
        self.txt_page.pack(side=tk.LEFT)
        self.txt_page.insert(0, "1")
        tk.Label(page_frame, text="页").pack(side=tk.LEFT)
        tk.Label(page_frame, text="每页").pack(side=tk.LEFT, padx=5)
        self.txt_page_size = tk.Entry(page_frame, width=5)
        self.txt_page_size.pack(side=tk.LEFT)
        self.txt_page_size.insert(0, "10")
        tk.Label(page_frame, text="条").pack(side=tk.LEFT)
        tk.Button(page_frame, text="跳转", command=self.goto_page).pack(side=tk.LEFT, padx=10)
        self.lbl_total = tk.Label(page_frame, text="共 0 条记录")
        self.lbl_total.pack(side=tk.LEFT, padx=20)

    def load_data(self):
        self.page_index = 1
        self.page_size = int(self.txt_page_size.get().strip() or "10")
        self.search()

    def search(self):
        reader_id = self.txt_reader_id.get().strip()
        reader_name = self.txt_reader_name.get().strip()
        book_name = self.txt_book_name.get().strip()
        isbn = self.txt_isbn.get().strip()
        status_map = {"全部": None, "借出中": 0, "已归还": 1, "逾期": 2}
        status = status_map.get(self.cmb_status.get(), None)
        borrow_from = self.txt_borrow_from.get().strip() or None
        borrow_to = self.txt_borrow_to.get().strip() or None
        return_from = self.txt_return_from.get().strip() or None
        return_to = self.txt_return_to.get().strip() or None

        for item in self.tree.get_children():
            self.tree.delete(item)

        rows, total = self.borrow_manager.search_all_borrow_records(
            reader_id=int(reader_id) if reader_id else None,
            reader_name=reader_name if reader_name else None,
            book_name=book_name if book_name else None,
            isbn=isbn if isbn else None,
            status=status,
            borrow_from=borrow_from,
            borrow_to=borrow_to,
            return_from=return_from,
            return_to=return_to,
            page_index=self.page_index,
            page_size=self.page_size
        )

        self.lbl_total.config(text=f"共 {total} 条记录")
        self.total_records = total

        for row in rows:
            self.tree.insert("", tk.END, values=(
                row['BorrowId'], row['ReaderId'], row['ReaderName'],
                row['BookName'], row['ISBN'], row['BorrowDate'],
                row['DueDate'], row['ReturnDate'] or "",
                row['StatusText'], f"{row['LateFee']:.2f}" if row['LateFee'] else "0.00",
                row['Remark'] or ""
            ))

    def goto_page(self):
        try:
            self.page_index = int(self.txt_page.get().strip() or "1")
            if self.page_index < 1:
                self.page_index = 1
            self.search()
        except:
            messagebox.showerror("错误", "请输入有效的页码！")
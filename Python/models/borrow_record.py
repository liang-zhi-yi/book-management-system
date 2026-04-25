class BorrowRecord:
    def __init__(self):
        self.borrow_id = 0
        self.reader_id = 0
        self.book_id = 0
        self.reader_name = ""
        self.book_name = ""
        self.borrow_date = None
        self.due_date = None
        self.return_date = None
        self.status = 0
        self.late_fee = 0.0
        self.remark = ""
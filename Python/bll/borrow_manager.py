from dal.borrow_service import BorrowService
from dal.reader_service import ReaderService
from dal.book_service import BookService
from datetime import datetime


class BorrowManager:
    def __init__(self):
        self.borrow_service = BorrowService()
        self.reader_service = ReaderService()
        self.book_service = BookService()

    def validate_borrow(self, reader_id: int, book_id: int) -> tuple:
        try:
            reader_rows = self.reader_service.get_reader_by_id(reader_id)
            if not reader_rows:
                return False, "读者不存在！"

            reader_status = reader_rows[0]['Status']
            if reader_status != 0:
                return False, "读者状态异常，无法借书！"

            book_rows = self.book_service.get_book_by_id(book_id)
            if not book_rows:
                return False, "图书不存在！"

            available_count = book_rows[0]['AvailableCount']
            if available_count <= 0:
                return False, "该图书已全部借出！"

            category_id = reader_rows[0]['ReaderCategoryId']
            category_rows = self.reader_service.get_reader_category_by_id(category_id)
            if category_rows:
                max_borrow = category_rows[0]['MaxBorrowCount']
                borrow_rows = self.borrow_service.get_reader_borrow_info(reader_id)
                current_borrow_count = len(borrow_rows) if borrow_rows else 0
                if current_borrow_count >= max_borrow:
                    return False, f"已达到最大借阅数量（{max_borrow}本）！"

            return True, "可以借阅"
        except Exception as ex:
            return False, f"验证失败：{str(ex)}"

    def borrow_book(self, reader_id: int, book_id: int, borrow_date: str, due_date: str) -> bool:
        can_borrow, message = self.validate_borrow(reader_id, book_id)
        if not can_borrow:
            raise Exception(message)
        return self.borrow_service.borrow_book(reader_id, book_id, borrow_date, due_date)

    def get_reader_borrow_info(self, reader_id: int):
        return self.borrow_service.get_reader_borrow_info(reader_id)

    def search_borrow_records(self, reader_id: int = None, isbn: str = None, borrow_id: int = None, status: int = None):
        return self.borrow_service.search_borrow_records(reader_id, isbn, borrow_id, status)

    def search_all_borrow_records(self, reader_id: int = None, reader_name: str = None,
                                  book_name: str = None, isbn: str = None,
                                  status: int = None, borrow_from: str = None,
                                  borrow_to: str = None, return_from: str = None,
                                  return_to: str = None, page_index: int = 1,
                                  page_size: int = 10) -> tuple:
        return self.borrow_service.search_all_borrow_records(
            reader_id, reader_name, book_name, isbn, status,
            borrow_from, borrow_to, return_from, return_to, page_index, page_size
        )

    def calculate_late_fee(self, borrow_id: int, actual_return_date: str) -> float:
        try:
            borrow_rows = self.borrow_service.get_borrow_record_by_id(borrow_id)
            if not borrow_rows:
                raise Exception("借阅记录不存在")

            due_date_str = borrow_rows[0]['DueDate']
            due_date = datetime.strptime(due_date_str, '%Y-%m-%d')
            actual_date = datetime.strptime(actual_return_date, '%Y-%m-%d')

            overdue_days = 0
            if actual_date > due_date:
                overdue_days = (actual_date - due_date).days

            reader_category_id = borrow_rows[0]['ReaderCategoryId']
            category_rows = self.reader_service.get_reader_category_by_id(reader_category_id)
            if not category_rows:
                raise Exception("读者类别不存在")

            late_fee_per_day = category_rows[0]['LateFeePerDay']
            return overdue_days * late_fee_per_day
        except Exception as ex:
            raise Exception(f"计算逾期费用失败：{str(ex)}")

    def return_book(self, borrow_id: int, return_date: str, late_fee: float, remark: str) -> tuple:
        try:
            calculated_late_fee = self.calculate_late_fee(borrow_id, return_date)
            if late_fee == 0 and calculated_late_fee > 0:
                late_fee = calculated_late_fee

            result = self.borrow_service.return_book(borrow_id, return_date, late_fee, remark)
            if result:
                return True, "还书成功", late_fee
            else:
                return False, "还书失败", 0
        except Exception as ex:
            return False, f"还书失败：{str(ex)}", 0

    def get_borrow_record_by_id(self, borrow_id: int):
        return self.borrow_service.get_borrow_record_by_id(borrow_id)
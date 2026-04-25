from dal.db_helper import DBHelper
from models.reader_category import ReaderCategory


class ReaderCategoryService:
    def __init__(self):
        self.db = DBHelper()

    def get_all_categories(self) -> list:
        sql = "SELECT CategoryId, CategoryName, MaxBorrowCount, BorrowDays, LateFeePerDay FROM ReaderCategory ORDER BY CategoryId"
        rows, _ = self.db.execute_data_set(sql)
        return rows

    def add_category(self, category: ReaderCategory) -> int:
        sql = """
            INSERT INTO ReaderCategory (CategoryName, MaxBorrowCount, BorrowDays, LateFeePerDay)
            VALUES (?, ?, ?, ?)
        """
        params = (category.category_name, category.max_borrow_count, category.borrow_days, category.late_fee_per_day)
        return self.db.execute_non_query(sql, params)

    def update_category(self, category: ReaderCategory) -> int:
        sql = """
            UPDATE ReaderCategory
            SET CategoryName=?, MaxBorrowCount=?, BorrowDays=?, LateFeePerDay=?
            WHERE CategoryId=?
        """
        params = (category.category_name, category.max_borrow_count, category.borrow_days,
                  category.late_fee_per_day, category.category_id)
        return self.db.execute_non_query(sql, params)

    def delete_category(self, category_id: int) -> int:
        sql = "DELETE FROM ReaderCategory WHERE CategoryId=?"
        return self.db.execute_non_query(sql, (category_id,))
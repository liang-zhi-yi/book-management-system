from dal.db_helper import DBHelper
from models.book_category import BookCategory


class BookCategoryService:
    def __init__(self):
        self.db = DBHelper()

    def get_all_categories(self) -> list:
        sql = "SELECT CategoryId, CategoryName, Description FROM BookCategory ORDER BY CategoryId"
        rows, _ = self.db.execute_data_set(sql)
        return rows

    def add_category(self, category: BookCategory) -> int:
        sql = "INSERT INTO BookCategory (CategoryName, Description) VALUES (?, ?)"
        return self.db.execute_non_query(sql, (category.category_name, category.description))

    def update_category(self, category: BookCategory) -> int:
        sql = "UPDATE BookCategory SET CategoryName=?, Description=? WHERE CategoryId=?"
        return self.db.execute_non_query(sql, (category.category_name, category.description, category.category_id))

    def delete_category(self, category_id: int) -> int:
        sql = "DELETE FROM BookCategory WHERE CategoryId=?"
        return self.db.execute_non_query(sql, (category_id,))
from dal.db_helper import DBHelper
from models.book import Book


class BookService:
    def __init__(self):
        self.db = DBHelper()

    def get_book_by_id(self, book_id: int) -> list:
        sql = "SELECT b.*, c.CategoryName FROM Book b LEFT JOIN BookCategory c ON b.CategoryId = c.CategoryId WHERE BookId=?"
        rows, _ = self.db.execute_data_set(sql, (book_id,))
        return rows

    def get_book_by_isbn(self, isbn: str) -> list:
        sql = "SELECT b.*, c.CategoryName FROM Book b LEFT JOIN BookCategory c ON b.CategoryId = c.CategoryId WHERE ISBN=?"
        rows, _ = self.db.execute_data_set(sql, (isbn,))
        return rows

    def get_all_books(self) -> list:
        sql = """
            SELECT b.BookId, b.BookName, b.Author, b.Publisher, b.ISBN,
                   b.CategoryId, c.CategoryName, b.TotalCount, b.AvailableCount, b.Price
            FROM Book b
            LEFT JOIN BookCategory c ON b.CategoryId = c.CategoryId
            ORDER BY b.BookId
        """
        rows, _ = self.db.execute_data_set(sql)
        return rows

    def add_book(self, book: Book) -> int:
        sql = """
            INSERT INTO Book (BookName, Author, Publisher, ISBN, CategoryId, TotalCount, AvailableCount, Price)
            VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        """
        params = (book.book_name, book.author, book.publisher, book.isbn,
                  book.category_id, book.total_count, book.available_count, book.price)
        return self.db.execute_non_query(sql, params)

    def update_book(self, book: Book) -> int:
        sql = """
            UPDATE Book
            SET BookName=?, Author=?, Publisher=?, ISBN=?, CategoryId=?,
                TotalCount=?, AvailableCount=?, Price=?
            WHERE BookId=?
        """
        params = (book.book_name, book.author, book.publisher, book.isbn,
                  book.category_id, book.total_count, book.available_count, book.price, book.book_id)
        return self.db.execute_non_query(sql, params)

    def delete_book(self, book_id: int) -> int:
        sql = "DELETE FROM Book WHERE BookId=?"
        return self.db.execute_non_query(sql, (book_id,))

    def get_all_categories(self) -> list:
        sql = "SELECT CategoryId, CategoryName FROM BookCategory ORDER BY CategoryId"
        rows, _ = self.db.execute_data_set(sql)
        return rows
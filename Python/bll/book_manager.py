from dal.book_service import BookService
from models.book import Book


class BookManager:
    def __init__(self):
        self.book_service = BookService()

    def get_book_by_id(self, book_id: int):
        return self.book_service.get_book_by_id(book_id)

    def get_book_by_isbn(self, isbn: str):
        return self.book_service.get_book_by_isbn(isbn)

    def get_all_books(self):
        return self.book_service.get_all_books()

    def get_all_categories(self):
        return self.book_service.get_all_categories()

    def add_book(self, book: Book) -> bool:
        if not book.book_name:
            raise Exception("书名不能为空！")
        if book.category_id <= 0:
            raise Exception("请选择图书类别！")
        if book.total_count <= 0:
            raise Exception("图书总数必须大于0！")
        return self.book_service.add_book(book) > 0

    def update_book(self, book: Book) -> bool:
        if not book.book_name:
            raise Exception("书名不能为空！")
        if book.category_id <= 0:
            raise Exception("请选择图书类别！")
        return self.book_service.update_book(book) > 0

    def delete_book(self, book_id: int) -> bool:
        return self.book_service.delete_book(book_id) > 0
from dal.book_category_service import BookCategoryService
from models.book_category import BookCategory


class BookCategoryManager:
    def __init__(self):
        self.category_service = BookCategoryService()

    def get_all_categories(self):
        return self.category_service.get_all_categories()

    def add_category(self, category: BookCategory) -> bool:
        if not category.category_name:
            raise Exception("类别名称不能为空！")
        return self.category_service.add_category(category) > 0

    def update_category(self, category: BookCategory) -> bool:
        if not category.category_name:
            raise Exception("类别名称不能为空！")
        return self.category_service.update_category(category) > 0

    def delete_category(self, category_id: int) -> bool:
        return self.category_service.delete_category(category_id) > 0
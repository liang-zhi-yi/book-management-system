from dal.reader_category_service import ReaderCategoryService
from models.reader_category import ReaderCategory


class ReaderCategoryManager:
    def __init__(self):
        self.category_service = ReaderCategoryService()

    def get_all_categories(self):
        return self.category_service.get_all_categories()

    def add_category(self, category: ReaderCategory) -> bool:
        if not category.category_name:
            raise Exception("类别名称不能为空！")
        if category.max_borrow_count <= 0:
            raise Exception("最大借阅数量必须大于0！")
        if category.borrow_days <= 0:
            raise Exception("借阅天数必须大于0！")
        if category.late_fee_per_day < 0:
            raise Exception("逾期费用不能为负数！")
        return self.category_service.add_category(category) > 0

    def update_category(self, category: ReaderCategory) -> bool:
        if not category.category_name:
            raise Exception("类别名称不能为空！")
        return self.category_service.update_category(category) > 0

    def delete_category(self, category_id: int) -> bool:
        return self.category_service.delete_category(category_id) > 0
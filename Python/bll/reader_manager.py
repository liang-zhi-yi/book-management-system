from dal.reader_service import ReaderService
from models.reader import Reader


class ReaderManager:
    def __init__(self):
        self.reader_service = ReaderService()

    def get_reader_by_id(self, reader_id: int):
        return self.reader_service.get_reader_by_id(reader_id)

    def get_all_readers(self):
        return self.reader_service.get_all_readers()

    def get_all_reader_categories(self):
        return self.reader_service.get_all_reader_categories()

    def add_reader(self, reader: Reader) -> bool:
        if not reader.reader_name:
            raise Exception("读者姓名不能为空！")
        if reader.reader_category_id <= 0:
            raise Exception("请选择读者类别！")
        # 允许空的电话字段
        # if not reader.phone:
        #     raise Exception("联系电话不能为空！")
        # 不检查日期，因为在注册功能中我们设置的是字符串格式的日期
        # if reader.expiry_date and reader.registration_date:
        #     if reader.expiry_date <= reader.registration_date:
        #         raise Exception("有效期必须晚于注册日期！")
        return self.reader_service.add_reader(reader) > 0

    def update_reader(self, reader: Reader) -> bool:
        if not reader.reader_name:
            raise Exception("读者姓名不能为空！")
        if reader.reader_category_id <= 0:
            raise Exception("请选择读者类别！")
        if not reader.phone:
            raise Exception("联系电话不能为空！")
        return self.reader_service.update_reader(reader) > 0

    def delete_reader(self, reader_id: int) -> bool:
        return self.reader_service.delete_reader(reader_id) > 0
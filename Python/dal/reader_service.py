from dal.db_helper import DBHelper
from models.reader import Reader


class ReaderService:
    def __init__(self):
        self.db = DBHelper()

    def get_reader_by_id(self, reader_id: int) -> list:
        sql = "SELECT * FROM Reader WHERE ReaderId=?"
        rows, _ = self.db.execute_data_set(sql, (reader_id,))
        return rows

    def get_reader_category_by_id(self, category_id: int) -> list:
        sql = "SELECT * FROM ReaderCategory WHERE CategoryId=?"
        rows, _ = self.db.execute_data_set(sql, (category_id,))
        return rows

    def get_all_readers(self) -> list:
        sql = """
            SELECT r.*, rc.CategoryName as ReaderCategoryName
            FROM Reader r
            LEFT JOIN ReaderCategory rc ON r.ReaderCategoryId = rc.CategoryId
            ORDER BY r.ReaderId
        """
        rows, _ = self.db.execute_data_set(sql)
        return rows

    def add_reader(self, reader: Reader) -> int:
        sql = """
            INSERT INTO Reader (UserId, ReaderName, Gender, ReaderCategoryId, Phone, Email, Address, RegistrationDate, ExpiryDate, Status, Remark)
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """
        params = (
            reader.user_id,
            reader.reader_name, reader.gender, reader.reader_category_id,
            reader.phone, reader.email, reader.address,
            reader.registration_date if reader.registration_date else '',
            reader.expiry_date if reader.expiry_date else '',
            reader.status, reader.remark
        )
        return self.db.execute_non_query(sql, params)

    def update_reader(self, reader: Reader) -> int:
        sql = """
            UPDATE Reader
            SET UserId=?, ReaderName=?, Gender=?, ReaderCategoryId=?, Phone=?, Email=?, Address=?,
                RegistrationDate=?, ExpiryDate=?, Status=?, Remark=?
            WHERE ReaderId=?
        """
        params = (
            reader.user_id,
            reader.reader_name, reader.gender, reader.reader_category_id,
            reader.phone, reader.email, reader.address,
            reader.registration_date if reader.registration_date else '',
            reader.expiry_date if reader.expiry_date else '',
            reader.status, reader.remark, reader.reader_id
        )
        return self.db.execute_non_query(sql, params)

    def delete_reader(self, reader_id: int) -> int:
        sql = "DELETE FROM Reader WHERE ReaderId=?"
        return self.db.execute_non_query(sql, (reader_id,))

    def get_all_reader_categories(self) -> list:
        sql = "SELECT CategoryId, CategoryName FROM ReaderCategory ORDER BY CategoryId"
        rows, _ = self.db.execute_data_set(sql)
        return rows
import sqlite3
from typing import Optional, List, Tuple, Any
import os


class DBHelper:
    _instance = None
    _connection_string = ""

    def __new__(cls):
        if cls._instance is None:
            cls._instance = super().__new__(cls)
        return cls._instance

    def __init__(self):
        if not self._connection_string:
            db_path = os.path.join(os.path.dirname(os.path.dirname(__file__)), "data", "bookmanager.db")
            os.makedirs(os.path.dirname(db_path), exist_ok=True)
            self._connection_string = db_path

    def get_connection(self) -> sqlite3.Connection:
        conn = sqlite3.connect(self._connection_string)
        conn.row_factory = sqlite3.Row
        return conn

    def execute_non_query(self, sql: str, params: Tuple = ()) -> int:
        conn = self.get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute(sql, params)
            conn.commit()
            return cursor.rowcount
        except Exception as e:
            print(f"数据库操作错误：{e}")
            return -1
        finally:
            conn.close()

    def execute_scalar(self, sql: str, params: Tuple = ()) -> Any:
        conn = self.get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute(sql, params)
            result = cursor.fetchone()
            return result[0] if result else None
        except Exception as e:
            print(f"数据库查询错误：{e}")
            return None
        finally:
            conn.close()

    def execute_query(self, sql: str, params: Tuple = ()) -> List[sqlite3.Row]:
        conn = self.get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute(sql, params)
            return cursor.fetchall()
        except Exception as e:
            print(f"数据库查询错误：{e}")
            return []
        finally:
            conn.close()

    def execute_data_set(self, sql: str, params: Tuple = ()) -> Tuple[List[sqlite3.Row], List[str]]:
        conn = self.get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute(sql, params)
            rows = cursor.fetchall()
            columns = [desc[0] for desc in cursor.description] if cursor.description else []
            return rows, columns
        except Exception as e:
            print(f"数据库查询错误：{e}")
            return [], []
        finally:
            conn.close()

    def init_database(self):
        conn = self.get_connection()
        cursor = conn.cursor()

        cursor.execute("""
            CREATE TABLE IF NOT EXISTS Users (
                UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                UserName TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                UserType INTEGER NOT NULL DEFAULT 0,
                RealName TEXT NOT NULL,
                CreateTime TEXT NOT NULL
            )
        """)

        cursor.execute("""
            CREATE TABLE IF NOT EXISTS BookCategory (
                CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
                CategoryName TEXT NOT NULL,
                Description TEXT
            )
        """)

        cursor.execute("""
            CREATE TABLE IF NOT EXISTS Book (
                BookId INTEGER PRIMARY KEY AUTOINCREMENT,
                BookName TEXT NOT NULL,
                Author TEXT,
                Publisher TEXT,
                ISBN TEXT,
                CategoryId INTEGER,
                TotalCount INTEGER NOT NULL DEFAULT 0,
                AvailableCount INTEGER NOT NULL DEFAULT 0,
                Price REAL NOT NULL DEFAULT 0,
                FOREIGN KEY (CategoryId) REFERENCES BookCategory(CategoryId)
            )
        """)

        cursor.execute("""
            CREATE TABLE IF NOT EXISTS ReaderCategory (
                CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
                CategoryName TEXT NOT NULL,
                MaxBorrowCount INTEGER NOT NULL DEFAULT 5,
                BorrowDays INTEGER NOT NULL DEFAULT 30,
                LateFeePerDay REAL NOT NULL DEFAULT 0.1
            )
        """)

        cursor.execute("""
            CREATE TABLE IF NOT EXISTS Reader (
                ReaderId INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER,
                ReaderName TEXT NOT NULL,
                Gender TEXT,
                ReaderCategoryId INTEGER,
                Phone TEXT,
                Email TEXT,
                Address TEXT,
                RegistrationDate TEXT,
                ExpiryDate TEXT,
                Status INTEGER NOT NULL DEFAULT 0,
                Remark TEXT,
                FOREIGN KEY (ReaderCategoryId) REFERENCES ReaderCategory(CategoryId),
                FOREIGN KEY (UserId) REFERENCES Users(UserId)
            )
        """)

        cursor.execute("""
            CREATE TABLE IF NOT EXISTS BorrowRecord (
                BorrowId INTEGER PRIMARY KEY AUTOINCREMENT,
                ReaderId INTEGER NOT NULL,
                BookId INTEGER NOT NULL,
                BorrowDate TEXT NOT NULL,
                DueDate TEXT NOT NULL,
                ReturnDate TEXT,
                Status INTEGER NOT NULL DEFAULT 0,
                LateFee REAL DEFAULT 0,
                Remark TEXT,
                FOREIGN KEY (ReaderId) REFERENCES Reader(ReaderId),
                FOREIGN KEY (BookId) REFERENCES Book(BookId)
            )
        """)

        cursor.execute("SELECT COUNT(*) FROM Users")
        if cursor.fetchone()[0] == 0:
            cursor.execute("""
                INSERT INTO Users (UserName, Password, UserType, RealName, CreateTime)
                VALUES ('admin', '123456', 0, '管理员', datetime('now'))
            """)
            cursor.execute("""
                INSERT INTO Users (UserName, Password, UserType, RealName, CreateTime)
                VALUES ('reader1', '123456', 1, '读者张三', datetime('now'))
            """)

        cursor.execute("SELECT COUNT(*) FROM BookCategory")
        if cursor.fetchone()[0] == 0:
            cursor.execute("INSERT INTO BookCategory (CategoryName, Description) VALUES ('文学类', '文学作品书籍')")
            cursor.execute("INSERT INTO BookCategory (CategoryName, Description) VALUES ('科技类', '科学技术书籍')")
            cursor.execute("INSERT INTO BookCategory (CategoryName, Description) VALUES ('历史类', '历史书籍')")

        cursor.execute("SELECT COUNT(*) FROM ReaderCategory")
        if cursor.fetchone()[0] == 0:
            cursor.execute("INSERT INTO ReaderCategory (CategoryName, MaxBorrowCount, BorrowDays, LateFeePerDay) VALUES ('普通读者', 5, 30, 0.5)")
            cursor.execute("INSERT INTO ReaderCategory (CategoryName, MaxBorrowCount, BorrowDays, LateFeePerDay) VALUES ('VIP读者', 10, 60, 0.3)")
            cursor.execute("INSERT INTO ReaderCategory (CategoryName, MaxBorrowCount, BorrowDays, LateFeePerDay) VALUES ('学生读者', 3, 15, 0.2)")

        cursor.execute("SELECT COUNT(*) FROM Book")
        if cursor.fetchone()[0] == 0:
            cursor.execute("INSERT INTO Book (BookName, Author, Publisher, ISBN, CategoryId, TotalCount, AvailableCount, Price) VALUES ('Python编程', '张三', '清华大学出版社', '978-7-302-52998-6', 2, 10, 10, 59.00)")
            cursor.execute("INSERT INTO Book (BookName, Author, Publisher, ISBN, CategoryId, TotalCount, AvailableCount, Price) VALUES ('数据结构', '李四', '高等教育出版社', '978-7-040-53789-2', 2, 5, 5, 45.00)")
            cursor.execute("INSERT INTO Book (BookName, Author, Publisher, ISBN, CategoryId, TotalCount, AvailableCount, Price) VALUES ('红楼梦', '曹雪芹', '人民文学出版社', '978-7-020-10672-1', 1, 3, 3, 68.00)")

        cursor.execute("SELECT COUNT(*) FROM Reader")
        if cursor.fetchone()[0] == 0:
            cursor.execute("INSERT INTO Reader (ReaderName, Gender, ReaderCategoryId, Phone, Email, Address, RegistrationDate, ExpiryDate, Status, Remark) VALUES ('张三', '男', 1, '13800138000', 'zhangsan@example.com', '北京市海淀区', date('now'), date('now', '+1 year'), 0, '测试读者')")

        conn.commit()
        conn.close()
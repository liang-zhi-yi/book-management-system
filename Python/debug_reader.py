import sys
import os

parent_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
sys.path.insert(0, parent_dir)

from dal.db_helper import DBHelper
from bll.reader_manager import ReaderManager
from bll.book_manager import BookManager

db = DBHelper()
db.init_database()

reader_manager = ReaderManager()
book_manager = BookManager()

# 测试获取读者信息
reader_id = 2
readers = reader_manager.get_reader_by_id(reader_id)
print(f"读者查询结果数量: {len(readers)}")

if readers and len(readers) > 0:
    reader = readers[0]
    print(f"读者数据类型: {type(reader)}")
    print(f"读者数据: {reader}")
    print(f"是否是dict: {isinstance(reader, dict)}")

    if isinstance(reader, dict):
        print(f"Status (dict): {reader.get('Status', 'NOT_FOUND')}")
        print(f"RealName (dict): {reader.get('RealName', 'NOT_FOUND')}")
    else:
        print(f"Length: {len(reader)}")
        print(f"Index 0 (ReaderId): {reader[0]}")
        print(f"Index 1 (ReaderName): {reader[1]}")
        print(f"Index 2 (Gender): {reader[2]}")
        print(f"Index 3 (ReaderCategoryId): {reader[3]}")
        print(f"Index 4 (Phone): {reader[4]}")
        print(f"Index 5 (Email): {reader[5]}")
        print(f"Index 6 (Address): {reader[6]}")
        print(f"Index 7 (RegistrationDate): {reader[7]}")
        print(f"Index 8 (ExpiryDate): {reader[8]}")
        print(f"Index 9 (Status): {reader[9]}")
        print(f"Index 10 (Remark): {reader[10]}")
        print(f"Index 11 (UserId): {reader[11]}")

# 测试获取图书信息
book_id = 1
books = book_manager.get_book_by_id(book_id)
print(f"\n图书查询结果数量: {len(books)}")

if books and len(books) > 0:
    book = books[0]
    print(f"图书数据类型: {type(book)}")
    print(f"图书数据: {book}")
    print(f"是否是dict: {isinstance(book, dict)}")

    if isinstance(book, dict):
        print(f"AvailableCount (dict): {book.get('AvailableCount', 'NOT_FOUND')}")
    else:
        print(f"Length: {len(book)}")
        print(f"Index 0 (BookId): {book[0]}")
        print(f"Index 1 (BookName): {book[1]}")
        print(f"Index 7 (AvailableCount): {book[7]}")
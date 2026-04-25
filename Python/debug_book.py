import sys
import os

parent_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
sys.path.insert(0, parent_dir)

from bll.book_manager import BookManager

book_manager = BookManager()

# 测试获取图书信息
book_id = 1963
books = book_manager.get_book_by_id(book_id)
print(f"图书查询结果数量: {len(books)}")

if books and len(books) > 0:
    book = books[0]
    print(f"图书数据类型: {type(book)}")
    print(f"图书数据: {book}")
    print(f"是否是dict: {isinstance(book, dict)}")

    if isinstance(book, dict):
        print(f"键值对:")
        for key, value in book.items():
            print(f"  {key}: {value}")
    else:
        print(f"长度: {len(book)}")
        print(f"索引 0 (BookId): {book[0]}")
        print(f"索引 1 (BookName): {book[1]}")
        print(f"索引 2: {book[2]}")
        print(f"索引 3: {book[3]}")
        print(f"索引 4: {book[4]}")
        print(f"索引 5: {book[5]}")
        print(f"索引 6: {book[6]}")
        print(f"索引 7: {book[7]}")
        print(f"索引 8: {book[8]}")
        print(f"索引 9: {book[9]}")
        print(f"索引 10: {book[10]}")
        print(f"索引 11: {book[11]}")
        print(f"索引 12: {book[12]}")
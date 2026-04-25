import sqlite3
import random
import os

db_path = r"c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db"

# 连接数据库
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# 生成随机ISBN
def generate_isbn():
    """生成随机ISBN-13格式"""
    prefix = "978"
    body = ''.join([str(random.randint(0, 9)) for _ in range(9)])
    # 计算校验码
    digits = list(prefix + body)
    check_sum = 0
    for i, digit in enumerate(digits):
        if i % 2 == 0:
            check_sum += int(digit)
        else:
            check_sum += int(digit) * 3
    check_digit = (10 - (check_sum % 10)) % 10
    return f"{prefix}-{body[:3]}-{body[3:6]}-{body[6:9]}-{check_digit}"

# 为没有ISBN的图书生成ISBN
print("为图书生成ISBN...")
cursor.execute("SELECT BookId, BookName FROM Book WHERE ISBN IS NULL OR ISBN = ''")
books_without_isbn = cursor.fetchall()
for book_id, book_name in books_without_isbn:
    isbn = generate_isbn()
    cursor.execute("UPDATE Book SET ISBN = ? WHERE BookId = ?", (isbn, book_id))
    print(f"  为图书 {book_name} (ID: {book_id}) 生成ISBN: {isbn}")

# 为没有UserId的读者补充UserId
print("\n为读者补充UserId...")
cursor.execute("SELECT ReaderId, ReaderName FROM Reader WHERE UserId IS NULL OR UserId = ''")
readers_without_userid = cursor.fetchall()
for reader_id, reader_name in readers_without_userid:
    # 查找对应的用户
    cursor.execute("SELECT UserId FROM Users WHERE UserName = ?", (reader_name,))
    user = cursor.fetchone()
    if user:
        user_id = user[0]
        cursor.execute("UPDATE Reader SET UserId = ? WHERE ReaderId = ?", (user_id, reader_id))
        print(f"  为读者 {reader_name} (ID: {reader_id}) 补充UserId: {user_id}")
    else:
        print(f"  未找到读者 {reader_name} 对应的用户记录")

# 提交更改
conn.commit()

# 验证结果
print("\n验证结果:")
cursor.execute("SELECT BookId, BookName, ISBN FROM Book WHERE BookId IN (1963, 1964, 1006)")
books = cursor.fetchall()
print("图书信息:")
for book in books:
    print(f"  BookId={book[0]}, BookName={book[1]}, ISBN={book[2]}")

cursor.execute("SELECT ReaderId, ReaderName, UserId FROM Reader")
readers = cursor.fetchall()
print("\n读者信息:")
for reader in readers:
    print(f"  ReaderId={reader[0]}, ReaderName={reader[1]}, UserId={reader[2]}")

conn.close()
print("\n数据更新完成！")
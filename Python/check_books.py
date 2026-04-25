import sqlite3

db_path = r"c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db"
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

cursor.execute("SELECT * FROM Book")
books = cursor.fetchall()
print(f"图书数量: {len(books)}")
print("所有图书:")
for book in books:
    print(f"  BookId={book[0]}, BookName={book[1]}, AvailableCount={book[7]}")

conn.close()
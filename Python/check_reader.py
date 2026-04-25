import sqlite3

db_path = r"c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db"
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# 检查Users表中的黄添用户
cursor.execute("SELECT * FROM Users WHERE UserName='黄添'")
users = cursor.fetchall()
print("黄添用户:")
for user in users:
    print(f"  {user}")

# 检查Reader表中是否有对应的读者记录
cursor.execute("SELECT * FROM Reader WHERE UserId=8")
readers = cursor.fetchall()
print("\n对应的读者记录:")
for reader in readers:
    print(f"  {reader}")

# 检查所有读者记录
cursor.execute("SELECT * FROM Reader")
all_readers = cursor.fetchall()
print("\n所有读者记录:")
for reader in all_readers:
    print(f"  {reader}")

conn.close()
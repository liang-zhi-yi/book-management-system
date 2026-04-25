import sqlite3

db_path = r"c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db"
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# 为黄添的读者记录添加UserId
cursor.execute("UPDATE Reader SET UserId=8 WHERE ReaderId=2")
print(f"更新的行数: {cursor.rowcount}")

conn.commit()

# 验证结果
cursor.execute("SELECT * FROM Reader WHERE ReaderId=2")
reader = cursor.fetchone()
print(f"更新后的读者记录: {reader}")

conn.close()
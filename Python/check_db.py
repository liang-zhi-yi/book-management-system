import sqlite3
import os

db_path = r"c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db"
print(f"数据库路径: {db_path}")

if os.path.exists(db_path):
    print(f"数据库文件存在: {db_path}")

    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # 检查Reader表的结构
    cursor.execute("PRAGMA table_info(Reader)")
    columns = cursor.fetchall()
    print("\nReader表结构:")
    for col in columns:
        print(f"  {col}")

    # 检查Users表的结构
    cursor.execute("PRAGMA table_info(Users)")
    columns = cursor.fetchall()
    print("\nUsers表结构:")
    for col in columns:
        print(f"  {col}")

    # 检查所有表
    cursor.execute("SELECT name FROM sqlite_master WHERE type='table'")
    tables = cursor.fetchall()
    print("\n所有表:")
    for table in tables:
        print(f"  {table[0]}")

    conn.close()
else:
    print(f"数据库文件不存在: {db_path}")
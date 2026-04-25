import sqlite3
import os
import shutil

# 备份数据库
db_path = r'c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db'
backup_path = r'c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager_backup.db'

print(f"备份数据库到 {backup_path}...")
shutil.copy2(db_path, backup_path)
print("备份完成！")

# 连接数据库
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# 查询所有用户
print("\n当前所有用户:")
cursor.execute("SELECT UserId, UserName, UserType, RealName FROM Users")
users = cursor.fetchall()
for user in users:
    print(f"  UserId={user[0]}, UserName={user[1]}, UserType={user[2]}, RealName={user[3]}")

# 查询所有读者
print("\n当前所有读者:")
cursor.execute("SELECT ReaderId, ReaderName, UserId FROM Reader")
readers = cursor.fetchall()
for reader in readers:
    print(f"  ReaderId={reader[0]}, ReaderName={reader[1]}, UserId={reader[2]}")

# 关闭连接
conn.close()
print("\n检查完成！")
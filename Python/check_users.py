import sqlite3

conn = sqlite3.connect(r'c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db')
cursor = conn.cursor()

# 查询所有读者用户
cursor.execute("SELECT UserName, UserType, RealName FROM Users WHERE UserType=1")
users = cursor.fetchall()

print('所有读者用户:')
for u in users:
    print(f'  UserName={u[0]}, UserType={u[1]}, RealName={u[2]}')

# 查询所有管理员用户
cursor.execute("SELECT UserName, UserType, RealName FROM Users WHERE UserType=0")
admins = cursor.fetchall()

print('\n所有管理员用户:')
for a in admins:
    print(f'  UserName={a[0]}, UserType={a[1]}, RealName={a[2]}')

conn.close()
import sqlite3

# 连接数据库
conn = sqlite3.connect(r'c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db')
cursor = conn.cursor()

# 要保留的用户ID
keep_user_ids = [1, 8]  # 1是管理员，8是黄添

# 开始事务
conn.execute('BEGIN TRANSACTION')

try:
    # 1. 删除多余的用户
    print("删除多余的用户...")
    # 构建要删除的用户ID列表
    delete_user_ids = []
    cursor.execute("SELECT UserId FROM Users WHERE UserId NOT IN (?, ?)", (1, 8))
    for (user_id,) in cursor.fetchall():
        delete_user_ids.append(user_id)
    
    if delete_user_ids:
        placeholders = ','.join('?' for _ in delete_user_ids)
        cursor.execute(f"DELETE FROM Users WHERE UserId IN ({placeholders})", delete_user_ids)
        print(f"删除了 {len(delete_user_ids)} 个用户")
    
    # 2. 删除多余的读者记录
    print("\n删除多余的读者记录...")
    # 构建要保留的读者记录（只保留黄添的）
    cursor.execute("DELETE FROM Reader WHERE ReaderId != 2")  # 2是黄添的读者记录
    print("只保留了黄添的读者记录")
    
    # 3. 重置读者ID为1
    print("\n重置读者ID为1...")
    # 创建临时表
    cursor.execute('''
        CREATE TABLE Reader_temp (
            ReaderId INTEGER PRIMARY KEY AUTOINCREMENT,
            ReaderName TEXT,
            Gender TEXT,
            ReaderCategoryId INTEGER,
            Phone TEXT,
            Email TEXT,
            Address TEXT,
            RegistrationDate TEXT,
            ExpiryDate TEXT,
            Status INTEGER,
            Remark TEXT,
            UserId INTEGER
        )
    ''')
    
    # 复制数据到临时表，ReaderId从1开始
    cursor.execute('''
        INSERT INTO Reader_temp (ReaderName, Gender, ReaderCategoryId, Phone, Email, Address, RegistrationDate, ExpiryDate, Status, Remark, UserId)
        SELECT ReaderName, Gender, ReaderCategoryId, Phone, Email, Address, RegistrationDate, ExpiryDate, Status, Remark, 8
        FROM Reader
        WHERE ReaderId = 2
    ''')
    
    # 删除原表
    cursor.execute('DROP TABLE Reader')
    
    # 重命名临时表为原表
    cursor.execute('ALTER TABLE Reader_temp RENAME TO Reader')
    
    # 4. 验证结果
    print("\n验证结果:")
    print("\n保留的用户:")
    cursor.execute("SELECT UserId, UserName, UserType, RealName FROM Users")
    users = cursor.fetchall()
    for user in users:
        print(f"  UserId={user[0]}, UserName={user[1]}, UserType={user[2]}, RealName={user[3]}")
    
    print("\n保留的读者:")
    cursor.execute("SELECT ReaderId, ReaderName, UserId FROM Reader")
    readers = cursor.fetchall()
    for reader in readers:
        print(f"  ReaderId={reader[0]}, ReaderName={reader[1]}, UserId={reader[2]}")
    
    # 提交事务
    conn.commit()
    print("\n操作成功完成！")
    
except Exception as e:
    # 回滚事务
    conn.rollback()
    print(f"\n操作失败: {e}")
finally:
    # 关闭连接
    conn.close()
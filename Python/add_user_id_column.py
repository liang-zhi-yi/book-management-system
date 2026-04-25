import sqlite3

db_path = r"c:\Users\hungt\Desktop\BookManagement程序\Python\data\bookmanager.db"
print(f"数据库路径: {db_path}")

conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# 检查Reader表是否有UserId列
cursor.execute("PRAGMA table_info(Reader)")
columns = cursor.fetchall()
has_user_id = False
for col in columns:
    if col[1] == 'UserId':
        has_user_id = True
        break

print(f"Reader表是否有UserId列: {has_user_id}")

if not has_user_id:
    # 添加UserId列
    try:
        cursor.execute("ALTER TABLE Reader ADD COLUMN UserId INTEGER")
        print("已添加UserId列到Reader表")
    except Exception as e:
        print(f"添加UserId列失败: {e}")
else:
    print("UserId列已存在")

conn.commit()

# 验证结果
cursor.execute("PRAGMA table_info(Reader)")
columns = cursor.fetchall()
print("\nReader表最终结构:")
for col in columns:
    print(f"  {col}")

conn.close()
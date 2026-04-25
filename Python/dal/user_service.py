from dal.db_helper import DBHelper
from models.user import User


class UserService:
    def __init__(self):
        self.db = DBHelper()

    def user_login(self, user_name: str, password: str) -> User:
        # 如果密码为空，只检查用户名是否存在
        if not password:
            sql = "SELECT * FROM Users WHERE UserName=?"
            rows, _ = self.db.execute_data_set(sql, (user_name,))
        else:
            sql = "SELECT * FROM Users WHERE UserName=? AND Password=?"
            rows, _ = self.db.execute_data_set(sql, (user_name, password))
        
        if rows:
            row = rows[0]
            user = User()
            user.user_id = row['UserId']
            user.user_name = row['UserName']
            user.password = row['Password']
            user.user_type = row['UserType']
            user.real_name = row['RealName']
            user.create_time = row['CreateTime']
            return user
        return None

    def check_user_name(self, user_name: str) -> bool:
        sql = "SELECT COUNT(*) FROM Users WHERE UserName=?"
        result = self.db.execute_scalar(sql, (user_name,))
        return result is not None and result > 0
    
    def add_user(self, user: User) -> int:
        # 检查用户名是否已存在
        if self.check_user_name(user.user_name):
            return 0
        
        # 使用同一个连接执行插入和获取ID
        conn = self.db.get_connection()
        cursor = conn.cursor()
        try:
            sql = """
                INSERT INTO Users (UserName, Password, UserType, RealName, CreateTime)
                VALUES (?, ?, ?, ?, datetime('now'))
            """
            params = (user.user_name, user.password, user.user_type, user.real_name)
            cursor.execute(sql, params)
            conn.commit()
            
            # 获取新创建的用户ID
            cursor.execute("SELECT last_insert_rowid()")
            user_id = cursor.fetchone()[0]
            return user_id
        except Exception as e:
            return 0
        finally:
            conn.close()
    
    def update_password(self, user_id: int, old_password: str, new_password: str) -> bool:
        # 检查旧密码是否正确
        sql = "SELECT * FROM Users WHERE UserId=? AND Password=?"
        rows, _ = self.db.execute_data_set(sql, (user_id, old_password))
        if not rows:
            return False
        
        # 更新密码
        sql = "UPDATE Users SET Password=? WHERE UserId=?"
        result = self.db.execute_non_query(sql, (new_password, user_id))
        return result > 0
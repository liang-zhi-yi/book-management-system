from dal.user_service import UserService
from models.user import User


class UserManager:
    def __init__(self):
        self.user_service = UserService()

    def login(self, user_name: str, password: str) -> User:
        if not user_name:
            raise Exception("用户名不能为空！")
        if not password:
            raise Exception("密码不能为空！")

        user = self.user_service.user_login(user_name, password)
        if not user:
            raise Exception("用户名或密码错误！")
        return user
    
    def add_user(self, user: User) -> int:
        if not user.user_name:
            raise Exception("用户名不能为空！")
        if not user.password:
            raise Exception("密码不能为空！")
        if not user.real_name:
            raise Exception("真实姓名不能为空！")

        return self.user_service.add_user(user)
    
    def change_password(self, user_id: int, old_password: str, new_password: str) -> bool:
        if not old_password:
            raise Exception("旧密码不能为空！")
        if not new_password:
            raise Exception("新密码不能为空！")

        return self.user_service.update_password(user_id, old_password, new_password)
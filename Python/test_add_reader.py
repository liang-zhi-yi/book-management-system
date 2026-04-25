import sys
import os

parent_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
sys.path.insert(0, parent_dir)

from dal.db_helper import DBHelper
from bll.user_manager import UserManager
from bll.reader_manager import ReaderManager
from models.reader import Reader
from models.user import User

# 初始化数据库
db = DBHelper()
db.init_database()

# 创建用户管理器
user_manager = UserManager()
reader_manager = ReaderManager()

# 测试创建读者
user = User()
user.user_name = '黄添'
user.password = '123456'
user.user_type = 1  # 读者类型
user.real_name = '黄添'

try:
    user_id = user_manager.add_user(user)
    print(f'创建用户成功，用户ID: {user_id}')

    reader = Reader()
    reader.user_id = user_id
    reader.reader_name = '黄添'
    reader.gender = '男'
    reader.reader_category_id = 1
    reader.phone = '13800138000'
    reader.email = ''
    reader.address = ''
    reader.registration_date = '2026-04-25'
    reader.expiry_date = '2027-04-25'
    reader.status = 0
    reader.remark = '测试用户'

    result = reader_manager.add_reader(reader)
    print(f'添加读者结果: {result}')
except Exception as e:
    print(f'错误: {e}')
    import traceback
    traceback.print_exc()
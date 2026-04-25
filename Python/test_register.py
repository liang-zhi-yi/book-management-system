from bll.user_manager import UserManager
from bll.reader_manager import ReaderManager
from models.user import User
from models.reader import Reader
import datetime

# 测试注册功能
def test_register():
    print("开始测试注册功能...")
    
    # 创建用户管理器和读者管理器
    user_manager = UserManager()
    reader_manager = ReaderManager()
    
    # 测试用户数据
    user_name = "test_user_123"
    identity = "student"
    phone = "13800138000"
    password = "123456"
    
    try:
        # 创建用户对象
        user = User()
        user.user_name = user_name
        user.password = password
        user.user_type = 1  # 读者类型
        user.real_name = user_name  # 使用用户名作为真实姓名
        
        # 添加用户
        print("添加用户...")
        user_id = user_manager.add_user(user)
        print(f"添加用户结果: user_id = {user_id}")
        
        if user_id > 0:
            # 创建读者记录
            print("添加读者记录...")
            reader = Reader()
            reader.user_id = user_id
            reader.reader_name = user_name  # 使用用户名作为读者姓名
            reader.gender = ''
            reader.reader_category_id = 1  # 默认普通读者
            reader.phone = phone
            reader.email = ''
            reader.address = ''
            reader.registration_date = datetime.datetime.now().strftime('%Y-%m-%d')
            reader.expiry_date = (datetime.datetime.now() + datetime.timedelta(days=365)).strftime('%Y-%m-%d')
            reader.status = 0  # 正常状态
            reader.remark = f'身份：{identity}'
            
            # 添加读者
            result = reader_manager.add_reader(reader)
            print(f"添加读者结果: {result}")
            
            if result:
                print("注册成功！")
            else:
                print("注册失败：创建读者记录失败！")
        else:
            print("注册失败：创建用户失败！")
    except Exception as e:
        print(f"注册失败：{str(e)}")

if __name__ == "__main__":
    test_register()
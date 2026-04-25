import sys
sys.path.insert(0, r'c:\Users\hungt\Desktop\BookManagement程序\Python\web')

from bll.reader_manager import ReaderManager
from models.reader import Reader

# 测试添加读者
reader_manager = ReaderManager()
reader = Reader()
reader.user_id = 100
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

try:
    result = reader_manager.add_reader(reader)
    print(f'添加读者结果: {result}')
except Exception as e:
    print(f'添加读者失败: {e}')
    import traceback
    traceback.print_exc()
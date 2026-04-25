import sys
import os

# 添加父目录到路径
parent_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
sys.path.insert(0, parent_dir)

print(f"Parent directory: {parent_dir}")
print(f"Python path: {sys.path[:3]}")

# 尝试导入
try:
    from dal.db_helper import DBHelper
    print("DBHelper imported successfully")
except Exception as e:
    print(f"Failed to import DBHelper: {e}")

try:
    from bll.user_manager import UserManager
    print("UserManager imported successfully")
except Exception as e:
    print(f"Failed to import UserManager: {e}")

try:
    from bll.reader_manager import ReaderManager
    print("ReaderManager imported successfully")
except Exception as e:
    print(f"Failed to import ReaderManager: {e}")

try:
    from models.reader import Reader
    print("Reader imported successfully")
except Exception as e:
    print(f"Failed to import Reader: {e}")
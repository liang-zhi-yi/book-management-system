import os
import sys
from app import app

# 使用Flask的测试客户端
test_client = app.test_client()

# 测试登录
login_data = {
    "user_name": "admin",
    "password": "123456",
    "user_type": 0
}

response = test_client.post('/login', json=login_data)
print("Login response:")
print(response.status_code)
print(response.json)

# 测试测试路由
response = test_client.get('/api/test')
print("\nTest route response:")
print(response.status_code)
print(response.data)

# 测试借阅申请审核API
response = test_client.get('/api/borrow/applications')
print("\nBorrow applications response:")
print(response.status_code)
print(response.data)

# 测试其他API作为对比
response = test_client.get('/api/book_categories')
print("\nBook categories response:")
print(response.status_code)
print(response.json)
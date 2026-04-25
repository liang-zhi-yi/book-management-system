import requests
import json

# 测试登录
session = requests.Session()
login_data = {
    "user_name": "admin",
    "password": "123456",
    "user_type": 0
}

response = session.post('http://127.0.0.1:5000/login', json=login_data)
print("Login response:")
print(response.status_code)
print(response.json())
print("Login headers:")
print(dict(response.headers))
print("Session cookies:")
print(dict(session.cookies))

# 测试测试路由
print("\nTesting test route...")
response = session.get('http://127.0.0.1:5000/api/test')
print("Test route response:")
print(response.status_code)
print(response.text)
print("Test route headers:")
print(dict(response.headers))

# 测试借阅申请审核API
print("\nTesting borrow applications API...")
response = session.get('http://127.0.0.1:5000/api/borrow/applications')
print("Borrow applications response:")
print(response.status_code)
print(response.text)
print("Borrow applications headers:")
print(dict(response.headers))

# 测试其他API作为对比
print("\nTesting book categories API...")
response = session.get('http://127.0.0.1:5000/api/book_categories')
print("Book categories response:")
print(response.status_code)
print(response.json())
print("Book categories headers:")
print(dict(response.headers))
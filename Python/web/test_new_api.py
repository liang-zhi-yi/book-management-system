import requests

# 测试新的测试路由
response = requests.get('http://127.0.0.1:5001/api/test')
print("Test route response:")
print(response.status_code)
print(response.json())

# 测试借阅申请审核API
response = requests.get('http://127.0.0.1:5001/api/borrow/applications')
print("\nBorrow applications response:")
print(response.status_code)
print(response.json())
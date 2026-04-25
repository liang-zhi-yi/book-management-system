import requests

# 测试新的测试路由
response = requests.get('http://127.0.0.1:5000/test')
print("Test route response:")
print(response.status_code)
print(response.text)

# 测试根路径
response = requests.get('http://127.0.0.1:5000/')
print("\nRoot path response:")
print(response.status_code)
print(response.text[:100])  # 只打印前100个字符
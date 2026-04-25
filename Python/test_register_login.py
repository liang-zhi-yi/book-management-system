import requests
import json

# 测试注册和登录功能
def test_register_and_login():
    print("开始测试注册和登录功能...")
    
    # 生成一个唯一的用户名
    import time
    unique_suffix = int(time.time()) % 10000
    user_name = f"test_user_{unique_suffix}"
    password = "123456"
    phone = f"13800138{unique_suffix:04d}"
    
    print(f"测试用户: {user_name}")
    print(f"密码: {password}")
    print(f"手机号: {phone}")
    
    # 1. 测试注册
    print("\n=== 测试注册 ===")
    register_data = {
        "user_name": user_name,
        "identity": "student",
        "phone": phone,
        "password": password
    }
    
    try:
        response = requests.post(
            "http://127.0.0.1:5000/register",
            headers={"Content-Type": "application/json"},
            data=json.dumps(register_data)
        )
        
        print(f"注册响应状态码: {response.status_code}")
        print(f"注册响应内容: {response.json()}")
        
        if response.json().get("success"):
            print("注册成功！")
        else:
            print(f"注册失败: {response.json().get('message')}")
            return
    except Exception as e:
        print(f"注册请求失败: {str(e)}")
        return
    
    # 2. 测试登录
    print("\n=== 测试登录 ===")
    login_data = {
        "user_name": user_name,
        "password": password,
        "user_type": "1"  # 1表示读者
    }
    
    try:
        response = requests.post(
            "http://127.0.0.1:5000/login",
            headers={"Content-Type": "application/json"},
            data=json.dumps(login_data)
        )
        
        print(f"登录响应状态码: {response.status_code}")
        print(f"登录响应内容: {response.json()}")
        
        if response.json().get("success"):
            print("登录成功！")
            print(f"重定向地址: {response.json().get('redirect_url')}")
        else:
            print(f"登录失败: {response.json().get('message')}")
    except Exception as e:
        print(f"登录请求失败: {str(e)}")

if __name__ == "__main__":
    test_register_and_login()
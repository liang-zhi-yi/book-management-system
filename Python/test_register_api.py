import requests
import json

# 测试注册功能
def test_register():
    print("开始测试注册功能...")
    
    # 注册数据
    register_data = {
        "user_name": "test_user_456",
        "identity": "student",
        "phone": "13900139000",
        "password": "123456"
    }
    
    # 发送注册请求
    try:
        response = requests.post(
            "http://127.0.0.1:5000/register",
            headers={"Content-Type": "application/json"},
            data=json.dumps(register_data)
        )
        
        # 打印响应
        print(f"响应状态码: {response.status_code}")
        print(f"响应内容: {response.json()}")
        
        if response.json().get("success"):
            print("注册成功！")
        else:
            print(f"注册失败: {response.json().get('message')}")
    except Exception as e:
        print(f"请求失败: {str(e)}")

if __name__ == "__main__":
    test_register()
# Read the file
with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\app.py', 'r', encoding='utf-8') as f:
    content = f.read()

# Find and replace the login response
old_pattern = '''            # 根据用户类型返回不同的跳转路径
            redirect_url = '/' if user.user_type == 0 else '/reader'
            return jsonify({'success': True, 'message': f'登录成功！欢迎 {user.real_name}！', 'redirect_url': redirect_url})'''

new_pattern = '''            # 根据用户类型返回不同的跳转路径
            redirect_url = '/' if user.user_type == 0 else '/reader'
            return jsonify({
                'success': True,
                'message': f'登录成功！欢迎 {user.real_name}！',
                'redirect_url': redirect_url,
                'user_id': user.user_id
            })'''

if old_pattern in content:
    content = content.replace(old_pattern, new_pattern)
    with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\app.py', 'w', encoding='utf-8') as f:
        f.write(content)
    print("Successfully updated login API to return user_id!")
else:
    print("Pattern not found for login API!")
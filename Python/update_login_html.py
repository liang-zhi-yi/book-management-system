# Read the file
with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\templates\login.html', 'r', encoding='utf-8') as f:
    content = f.read()

# Find and replace the sessionStorage save
old_pattern = '''                if (result.success) {
                    messageDiv.className = 'message success';
                    messageDiv.textContent = result.message;
                    // 保存用户信息到 sessionStorage
                    sessionStorage.setItem('user', JSON.stringify({
                        user_name: userName,
                        real_name: userName,
                        user_type: parseInt(userType)
                    }));'''

new_pattern = '''                if (result.success) {
                    messageDiv.className = 'message success';
                    messageDiv.textContent = result.message;
                    // 保存用户信息到 sessionStorage
                    sessionStorage.setItem('user', JSON.stringify({
                        user_id: result.user_id,
                        user_name: userName,
                        real_name: userName,
                        user_type: parseInt(userType)
                    }));'''

if old_pattern in content:
    content = content.replace(old_pattern, new_pattern)
    with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\templates\login.html', 'w', encoding='utf-8') as f:
        f.write(content)
    print("Successfully updated login.html to save user_id!")
else:
    print("Pattern not found for login.html!")
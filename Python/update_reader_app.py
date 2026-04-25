import re

# Read the file
with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\static\js\reader_app.js', 'r', encoding='utf-8') as f:
    content = f.read()

# Find and replace the problematic section
old_pattern = '''        // 从数据库获取最新的读者信息
        const response = await api('/api/readers');
        if (response.success && response.data.length > 0) {
            // 查找当前用户对应的读者信息
            let readerInfo = response.data[0]; // 默认使用第一个读者
            if (userInfo.user_name) {
                readerInfo = response.data.find(r => r.ReaderName === userInfo.user_name) || readerInfo;
            }
            
            // 设置表单值'''

new_pattern = '''        // 从数据库获取最新的读者信息
        const response = await api('/api/readers');
        if (response.success && response.data.length > 0) {
            // 查找当前用户对应的读者信息
            // 优先使用UserId匹配，其次使用ReaderName匹配
            let readerInfo = null;

            // 首先尝试使用UserId匹配（最准确）
            if (userInfo.user_id) {
                readerInfo = response.data.find(r => r.UserId == userInfo.user_id);
            }

            // 如果没有找到，使用ReaderName匹配
            if (!readerInfo && userInfo.user_name) {
                readerInfo = response.data.find(r => r.ReaderName === userInfo.user_name);
            }

            // 如果仍然没有找到，使用第一个读者（不应该发生）
            if (!readerInfo) {
                readerInfo = response.data[0];
            }

            // 设置表单值'''

if old_pattern in content:
    content = content.replace(old_pattern, new_pattern)
    with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\static\js\reader_app.js', 'w', encoding='utf-8') as f:
        f.write(content)
    print("Successfully updated the file!")
else:
    print("Pattern not found!")
    # Show what we're looking for
    print("\nLooking for pattern:")
    print(old_pattern[:200])
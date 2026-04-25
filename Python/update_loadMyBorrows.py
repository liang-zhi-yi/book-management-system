# Read the file
with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\static\js\reader_app.js', 'r', encoding='utf-8') as f:
    content = f.read()

# Find and replace the loadMyBorrows function
old_pattern = '''async function loadMyBorrows() {
    const result = await api('/api/borrow/records');
    if (result.success) {
        allBorrowRecords = result.data;
        updateBorrowRecords();
    }
}'''

new_pattern = '''async function loadMyBorrows() {
    // 确保currentReader已经加载
    if (!currentReader || !currentReader.ReaderId) {
        // 等待currentReader加载
        await new Promise(resolve => setTimeout(resolve, 500));
    }

    const result = await api(`/api/borrow/records?reader_id=${currentReader ? currentReader.ReaderId : ''}`);
    if (result.success) {
        allBorrowRecords = result.data;
        updateBorrowRecords();
    }
}'''

if old_pattern in content:
    content = content.replace(old_pattern, new_pattern)
    with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\static\js\reader_app.js', 'w', encoding='utf-8') as f:
        f.write(content)
    print("Successfully updated loadMyBorrows!")
else:
    print("Pattern not found for loadMyBorrows!")
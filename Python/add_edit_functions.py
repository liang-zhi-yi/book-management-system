# Read the file
with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\static\js\reader_app.js', 'r', encoding='utf-8') as f:
    content = f.read()

# Find the updateCurrentReader function and add new functions after it
old_pattern = '''// 更新currentReader变量
function updateCurrentReader() {
    const userInfo = JSON.parse(sessionStorage.getItem('user')) || {};
    currentReader = {
        ReaderId: currentReader ? currentReader.ReaderId : 1,
        ReaderName: currentReader ? currentReader.ReaderName : (userInfo.real_name || userInfo.user_name || '读者'),
        Gender: currentReader ? currentReader.Gender : '男',
        ReaderCategoryId: currentReader ? currentReader.ReaderCategoryId : 1,
        RemainingBorrowCount: currentReader ? currentReader.RemainingBorrowCount : 10
    };
    console.log('Updated currentReader:', currentReader);
}'''

new_pattern = '''// 更新currentReader变量
function updateCurrentReader() {
    const userInfo = JSON.parse(sessionStorage.getItem('user')) || {};
    currentReader = {
        ReaderId: currentReader ? currentReader.ReaderId : 1,
        ReaderName: currentReader ? currentReader.ReaderName : (userInfo.real_name || userInfo.user_name || '读者'),
        Gender: currentReader ? currentReader.Gender : '男',
        ReaderCategoryId: currentReader ? currentReader.ReaderCategoryId : 1,
        RemainingBorrowCount: currentReader ? currentReader.RemainingBorrowCount : 10
    };
    console.log('Updated currentReader:', currentReader);
}

// 切换编辑模式
function toggleEditMode() {
    const editSection = document.getElementById('edit-section');
    const editBtn = document.getElementById('edit-info-btn');
    const readerNameInput = document.getElementById('personal-reader-name');
    const genderSelect = document.getElementById('personal-reader-gender');
    const categorySelect = document.getElementById('personal-reader-category');
    const phoneInput = document.getElementById('personal-edit-phone');
    const emailInput = document.getElementById('personal-edit-email');
    const addressInput = document.getElementById('personal-edit-address');

    if (editSection.style.display === 'none') {
        // 切换到编辑模式
        editSection.style.display = 'block';
        editBtn.innerHTML = '<i class="fas fa-times"></i> 取消编辑';
        readerNameInput.removeAttribute('readonly');
        readerNameInput.style.background = 'rgba(255, 255, 255, 0.8)';
        readerNameInput.style.cursor = 'text';
        genderSelect.removeAttribute('disabled');
        genderSelect.style.background = 'rgba(255, 255, 255, 0.8)';
        genderSelect.style.cursor = 'pointer';
        categorySelect.removeAttribute('disabled');
        categorySelect.style.background = 'rgba(255, 255, 255, 0.8)';
        categorySelect.style.cursor = 'pointer';

        // 填充编辑字段的当前值
        phoneInput.value = currentReader && currentReader.Phone ? currentReader.Phone : '';
        emailInput.value = currentReader && currentReader.Email ? currentReader.Email : '';
        addressInput.value = currentReader && currentReader.Address ? currentReader.Address : '';
    } else {
        // 切换回只读模式
        editSection.style.display = 'none';
        editBtn.innerHTML = '<i class="fas fa-edit"></i> 修改信息';
        readerNameInput.setAttribute('readonly', 'true');
        readerNameInput.style.background = 'rgba(245, 245, 245, 0.8)';
        readerNameInput.style.cursor = 'not-allowed';
        genderSelect.setAttribute('disabled', 'true');
        genderSelect.style.background = 'rgba(245, 245, 245, 0.8)';
        genderSelect.style.cursor = 'not-allowed';
        categorySelect.setAttribute('disabled', 'true');
        categorySelect.style.background = 'rgba(245, 245, 245, 0.8)';
        categorySelect.style.cursor = 'not-allowed';
    }
}

// 取消编辑
function cancelEdit() {
    const editSection = document.getElementById('edit-section');
    const editBtn = document.getElementById('edit-info-btn');
    const readerNameInput = document.getElementById('personal-reader-name');
    const genderSelect = document.getElementById('personal-reader-gender');
    const categorySelect = document.getElementById('personal-reader-category');

    editSection.style.display = 'none';
    editBtn.innerHTML = '<i class="fas fa-edit"></i> 修改信息';
    readerNameInput.setAttribute('readonly', 'true');
    readerNameInput.style.background = 'rgba(245, 245, 245, 0.8)';
    readerNameInput.style.cursor = 'not-allowed';
    genderSelect.setAttribute('disabled', 'true');
    genderSelect.style.background = 'rgba(245, 245, 245, 0.8)';
    genderSelect.style.cursor = 'not-allowed';
    categorySelect.setAttribute('disabled', 'true');
    categorySelect.style.background = 'rgba(245, 245, 245, 0.8)';
    categorySelect.style.cursor = 'not-allowed';

    // 重新加载个人信息以恢复原始值
    loadPersonalInfo();
}

// 保存个人信息
async function savePersonalInfo() {
    const readerId = parseInt(document.getElementById('personal-reader-id').value);
    const readerName = document.getElementById('personal-reader-name').value.trim();
    const gender = document.getElementById('personal-reader-gender').value;
    const categoryName = document.getElementById('personal-reader-category').value;
    const phone = document.getElementById('personal-edit-phone').value.trim();
    const email = document.getElementById('personal-edit-email').value.trim();
    const address = document.getElementById('personal-edit-address').value.trim();

    // 验证必填字段
    if (!readerName) {
        alert('请输入姓名');
        return;
    }
    if (!gender) {
        alert('请选择性别');
        return;
    }
    if (!categoryName) {
        alert('请选择类别');
        return;
    }

    // 转换类别名称到ID
    let readerCategoryId = 1;
    if (categoryName === '教师') readerCategoryId = 2;
    else if (categoryName === '职工') readerCategoryId = 3;

    try {
        const response = await api(`/api/readers/${readerId}`, 'PUT', {
            reader_name: readerName,
            gender: gender,
            reader_category_id: readerCategoryId,
            phone: phone,
            email: email,
            address: address,
            status: 0
        });

        if (response.success) {
            alert('个人信息更新成功！');
            // 取消编辑模式
            cancelEdit();
            // 重新加载个人信息
            loadPersonalInfo();
        } else {
            alert('更新失败：' + response.message);
        }
    } catch (error) {
        alert('请求失败：' + error.message);
    }
}'''

if old_pattern in content:
    content = content.replace(old_pattern, new_pattern)
    with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\static\js\reader_app.js', 'w', encoding='utf-8') as f:
        f.write(content)
    print("Successfully added toggleEditMode, cancelEdit, and savePersonalInfo functions!")
else:
    print("Pattern not found for adding new functions!")
    # Debug: show what we're looking for
    if "updateCurrentReader" in content:
        print("Found 'updateCurrentReader' in content")
    else:
        print("'updateCurrentReader' NOT found in content")
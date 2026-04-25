# Read the file
with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\templates\reader_index.html', 'r', encoding='utf-8') as f:
    content = f.read()

# Find and replace the personal info section
old_pattern = '''                        <div class="info-card">
                            <h4>个人信息</h4>
                            <div class="personal-profile" style="text-align: center; margin-bottom: 30px;">
                                <div class="profile-avatar" style="width: 120px; height: 120px; border-radius: 50%; background: linear-gradient(135deg, #ffb69f 0%, #fcb69f 100%); margin: 0 auto 20px; display: flex; align-items: center; justify-content: center; font-size: 48px; color: white;">
                                    <i class="fas fa-user"></i>
                                </div>
                                <div class="profile-info" style="text-align: left; max-width: 300px; margin: 0 auto;">
                                    <div style="margin: 10px 0; font-size: 14px; color: #8d6e63;">
                                        <strong style="color: #5d4037;">读者ID:</strong>
                                        <input type="text" id="personal-reader-id" value="-" style="margin-left: 10px; padding: 6px 10px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 8px; font-size: 14px; background: rgba(245, 245, 245, 0.8); color: #5d4037; width: 180px; cursor: not-allowed;" readonly>
                                    </div>
                                    <div style="margin: 10px 0; font-size: 14px; color: #8d6e63;">
                                        <strong style="color: #5d4037;">姓名:</strong>
                                        <input type="text" id="personal-reader-name" value="-" style="margin-left: 10px; padding: 6px 10px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 8px; font-size: 14px; background: rgba(255, 255, 255, 0.8); color: #5d4037; width: 180px;">
                                    </div>
                                    <div style="margin: 10px 0; font-size: 14px; color: #8d6e63;">
                                        <strong style="color: #5d4037;">性别:</strong>
                                        <select id="personal-reader-gender" style="margin-left: 10px; padding: 6px 10px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 8px; font-size: 14px; background: rgba(255, 255, 255, 0.8); color: #5d4037; width: 180px;">
                                            <option value="">请选择</option>
                                            <option value="男">男</option>
                                            <option value="女">女</option>
                                        </select>
                                    </div>
                                    <div style="margin: 10px 0; font-size: 14px; color: #8d6e63;">
                                        <strong style="color: #5d4037;">类别:</strong>
                                        <select id="personal-reader-category" style="margin-left: 10px; padding: 6px 10px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 8px; font-size: 14px; background: rgba(255, 255, 255, 0.8); color: #5d4037; width: 180px;">
                                            <option value="">请选择</option>
                                            <option value="学生">学生</option>
                                            <option value="教师">教师</option>
                                            <option value="职工">职工</option>
                                        </select>
                                    </div>
                                    <p style="margin: 10px 0; font-size: 14px; color: #8d6e63;"><strong style="color: #5d4037;">可借数量上限:</strong> <span id="personal-reader-borrow-limit">10</span> 本</p>
                                    <p style="margin: 10px 0; font-size: 14px; color: #8d6e63;"><strong style="color: #5d4037;">借阅期限:</strong> <span id="personal-reader-borrow-days">30</span> 天</p>
                                    <p style="margin: 10px 0; font-size: 14px; color: #8d6e63;"><strong style="color: #5d4037;">注册日期:</strong> <span id="personal-reader-registration-date">-</span></p>
                                    <p style="margin: 10px 0; font-size: 14px; color: #8d6e63;"><strong style="color: #5d4037;">状态:</strong> <span id="personal-reader-status">-</span></p>
                                </div>
                            </div>
                            <h4>修改信息</h4>
                            <div class="form-row" style="flex-direction: column; align-items: stretch; gap: 15px;">
                                <input type="text" id="personal-edit-phone" placeholder="联系电话" style="padding: 12px 16px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 12px; font-size: 14px; transition: all 0.3s ease; background: rgba(255, 255, 255, 0.8); color: #5d4037;">
                                <input type="email" id="personal-edit-email" placeholder="邮箱" style="padding: 12px 16px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 12px; font-size: 14px; transition: all 0.3s ease; background: rgba(255, 255, 255, 0.8); color: #5d4037;">
                                <input type="text" id="personal-edit-address" placeholder="地址" style="padding: 12px 16px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 12px; font-size: 14px; transition: all 0.3s ease; background: rgba(255, 255, 255, 0.8); color: #5d4037;">
                                <button onclick="updatePersonalInfo()" class="btn-primary" style="width: 100%;">
                                    <i class="fas fa-save"></i> 保存修改
                                </button>
                            </div>
                        </div>'''

new_pattern = '''                        <div class="info-card">
                            <h4>个人信息</h4>
                            <div class="personal-profile" style="text-align: center; margin-bottom: 30px;">
                                <div class="profile-avatar" style="width: 120px; height: 120px; border-radius: 50%; background: linear-gradient(135deg, #ffb69f 0%, #fcb69f 100%); margin: 0 auto 20px; display: flex; align-items: center; justify-content: center; font-size: 48px; color: white;">
                                    <i class="fas fa-user"></i>
                                </div>
                                <div class="profile-info" style="text-align: left; max-width: 300px; margin: 0 auto;">
                                    <div style="margin: 10px 0; font-size: 14px; color: #8d6e63;">
                                        <strong style="color: #5d4037;">读者ID:</strong>
                                        <input type="text" id="personal-reader-id" value="-" style="margin-left: 10px; padding: 6px 10px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 8px; font-size: 14px; background: rgba(245, 245, 245, 0.8); color: #5d4037; width: 180px; cursor: not-allowed;" readonly>
                                    </div>
                                    <div style="margin: 10px 0; font-size: 14px; color: #8d6e63;">
                                        <strong style="color: #5d4037;">姓名:</strong>
                                        <input type="text" id="personal-reader-name" value="-" style="margin-left: 10px; padding: 6px 10px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 8px; font-size: 14px; background: rgba(245, 245, 245, 0.8); color: #5d4037; width: 180px; cursor: not-allowed;" readonly>
                                    </div>
                                    <div style="margin: 10px 0; font-size: 14px; color: #8d6e63;">
                                        <strong style="color: #5d4037;">性别:</strong>
                                        <select id="personal-reader-gender" disabled style="margin-left: 10px; padding: 6px 10px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 8px; font-size: 14px; background: rgba(245, 245, 245, 0.8); color: #5d4037; width: 180px; cursor: not-allowed;">
                                            <option value="">请选择</option>
                                            <option value="男">男</option>
                                            <option value="女">女</option>
                                        </select>
                                    </div>
                                    <div style="margin: 10px 0; font-size: 14px; color: #8d6e63;">
                                        <strong style="color: #5d4037;">类别:</strong>
                                        <select id="personal-reader-category" disabled style="margin-left: 10px; padding: 6px 10px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 8px; font-size: 14px; background: rgba(245, 245, 245, 0.8); color: #5d4037; width: 180px; cursor: not-allowed;">
                                            <option value="">请选择</option>
                                            <option value="学生">学生</option>
                                            <option value="教师">教师</option>
                                            <option value="职工">职工</option>
                                        </select>
                                    </div>
                                    <p style="margin: 10px 0; font-size: 14px; color: #8d6e63;"><strong style="color: #5d4037;">可借数量上限:</strong> <span id="personal-reader-borrow-limit">10</span> 本</p>
                                    <p style="margin: 10px 0; font-size: 14px; color: #8d6e63;"><strong style="color: #5d4037;">借阅期限:</strong> <span id="personal-reader-borrow-days">30</span> 天</p>
                                    <p style="margin: 10px 0; font-size: 14px; color: #8d6e63;"><strong style="color: #5d4037;">注册日期:</strong> <span id="personal-reader-registration-date">-</span></p>
                                    <p style="margin: 10px 0; font-size: 14px; color: #8d6e63;"><strong style="color: #5d4037;">状态:</strong> <span id="personal-reader-status">-</span></p>
                                </div>
                            </div>
                            <button id="edit-info-btn" onclick="toggleEditMode()" class="btn-primary" style="width: 100%; margin-bottom: 15px;">
                                <i class="fas fa-edit"></i> 修改信息
                            </button>
                            <div id="edit-section" style="display: none;">
                                <h4>编辑个人信息</h4>
                                <div class="form-row" style="flex-direction: column; align-items: stretch; gap: 15px;">
                                    <input type="text" id="personal-edit-phone" placeholder="联系电话" style="padding: 12px 16px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 12px; font-size: 14px; transition: all 0.3s ease; background: rgba(255, 255, 255, 0.8); color: #5d4037;">
                                    <input type="email" id="personal-edit-email" placeholder="邮箱" style="padding: 12px 16px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 12px; font-size: 14px; transition: all 0.3s ease; background: rgba(255, 255, 255, 0.8); color: #5d4037;">
                                    <input type="text" id="personal-edit-address" placeholder="地址" style="padding: 12px 16px; border: 2px solid rgba(255, 203, 173, 0.5); border-radius: 12px; font-size: 14px; transition: all 0.3s ease; background: rgba(255, 255, 255, 0.8); color: #5d4037;">
                                    <div style="display: flex; gap: 10px;">
                                        <button onclick="savePersonalInfo()" class="btn-primary" style="flex: 1;">
                                            <i class="fas fa-save"></i> 保存
                                        </button>
                                        <button onclick="cancelEdit()" class="btn-secondary" style="flex: 1;">
                                            <i class="fas fa-times"></i> 取消
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>'''

if old_pattern in content:
    content = content.replace(old_pattern, new_pattern)
    with open(r'c:\Users\hungt\Desktop\BookManagement程序\Python\web\templates\reader_index.html', 'w', encoding='utf-8') as f:
        f.write(content)
    print("Successfully updated reader_index.html!")
else:
    print("Pattern not found for reader_index.html!")
    # Try to find partial match
    if "读者ID:" in content:
        print("Found '读者ID:' in content")
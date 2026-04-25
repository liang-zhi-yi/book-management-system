let currentUser = null;

// 页面加载完成后执行
document.addEventListener('DOMContentLoaded', async () => {
    await checkLogin();
    setupMenu();
    setDefaultDates();
    loadMyBorrows();
    loadBookCategories();
    // 取消注释，使用修改后的 loadPersonalInfo() 函数
    await loadPersonalInfo();
    await loadBooksForReader();
    await loadBooksForApply();
    await loadCategoriesForApply();
    await loadCategoriesForReader();
    await loadApplicationRecords();
});

// 检查登录状态
async function checkLogin() {
    try {
        const response = await fetch('/');
        if (response.url.includes('login')) {
            window.location.href = '/login';
        } else {
            const html = await response.text();
            const userMatch = html.match(/user\.real_name\s*=\s*['"]([^'"]+)['"]/);
            if (userMatch) {
                document.getElementById('currentUser').textContent = userMatch[1];
                currentUser = userMatch[1];
            }
        }
    } catch (e) {
        console.log('Not logged in');
        window.location.href = '/login';
    }
}

// 设置菜单导航
function setupMenu() {
    document.querySelectorAll('.menu-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            document.querySelectorAll('.menu-btn').forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            document.querySelectorAll('.page').forEach(p => p.style.display = 'none');
            const pageId = 'page-' + btn.dataset.page;
            document.getElementById(pageId).style.display = 'block';
        });
    });
}

// 设置默认日期
function setDefaultDates() {
    const today = new Date().toISOString().split('T')[0];
    const nextMonth = new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
    
    // 只在元素存在时设置内容
    const borrowDateEl = document.getElementById('apply-borrow-date');
    if (borrowDateEl) borrowDateEl.textContent = today;
    
    const borrowDueDateEl = document.getElementById('apply-borrow-due-date');
    if (borrowDueDateEl) borrowDueDateEl.textContent = nextMonth;
}

// API请求函数
async function api(url, method = 'GET', body = null) {
    // 使用相对路径，确保在不同环境下都能正常工作
    const fullUrl = url;
    
    const options = {
        method,
        headers: {'Content-Type': 'application/json'},
        credentials: 'include' // 包含 cookies
    };
    if (body) options.body = JSON.stringify(body);
    
    try {
        const response = await fetch(fullUrl, options);
        
        // 检查响应状态
        if (!response.ok) {
            // 尝试获取响应文本
            const errorText = await response.text();
            // 抛出更详细的错误信息
            throw new Error(`HTTP error! status: ${response.status}, message: ${errorText.substring(0, 100)}...`);
        }
        
        // 尝试解析 JSON 响应
        try {
            return await response.json();
        } catch (error) {
            // 如果解析失败，抛出更详细的错误信息
            const errorText = await response.text();
            throw new Error(`Invalid JSON response: ${errorText.substring(0, 100)}...`);
        }
    } catch (error) {
        // 抛出更详细的错误信息
        throw new Error(`请求失败：${error.message}，URL: ${fullUrl}`);
    }
}

// 加载图书类别
async function loadBookCategories() {
    const result = await api('/api/book_categories');
    if (result.success) {
        const categorySelects = [
            document.getElementById('reader-book-category'),
            document.getElementById('apply-book-category')
        ];
        
        categorySelects.forEach(select => {
            if (select) {
                select.innerHTML = '<option value="">选择类别</option>' +
                    result.data.map(c => `<option value="${c.CategoryId}">${c.CategoryName}</option>`).join('');
            }
        });
    }
}

// 全局变量
let allBorrowRecords = [];
let showReturned = true;

// 加载我的借阅记录
async function loadMyBorrows() {
    // 确保currentReader已经加载
    if (!currentReader || !currentReader.ReaderId || currentReader.ReaderId === 0) {
        // 等待currentReader加载
        await new Promise(resolve => setTimeout(resolve, 500));
        // 再次检查
        if (!currentReader || !currentReader.ReaderId || currentReader.ReaderId === 0) {
            console.error('无法获取有效的读者ID');
            allBorrowRecords = [];
            updateBorrowRecords();
            return;
        }
    }

    const result = await api(`/api/borrow/records?reader_id=${currentReader.ReaderId}`);
    if (result.success) {
        allBorrowRecords = result.data;
        updateBorrowRecords();
    }
}

// 更新借阅记录显示
function updateBorrowRecords() {
    const statusFilter = document.getElementById('status-filter').value;
    const startDate = document.getElementById('start-date').value;
    const endDate = document.getElementById('end-date').value;
    const searchTerm = document.getElementById('reader-borrow-search').value.trim();
    
    let filtered = allBorrowRecords;
    
    // 状态筛选
    if (statusFilter !== 'all') {
        switch (statusFilter) {
            case 'borrowed':
                filtered = filtered.filter(record => record.Status === 0);
                break;
            case 'returned':
                filtered = filtered.filter(record => record.Status === 1);
                break;
            case 'overdue':
                filtered = filtered.filter(record => record.Status === 2);
                break;
        }
    }
    
    // 时间范围筛选
    if (startDate) {
        filtered = filtered.filter(record => record.BorrowDate >= startDate);
    }
    if (endDate) {
        filtered = filtered.filter(record => record.BorrowDate <= endDate);
    }
    
    // 搜索筛选
    if (searchTerm) {
        filtered = filtered.filter(record => 
            record.BookName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            record.ISBN.toLowerCase().includes(searchTerm.toLowerCase())
        );
    }
    
    // 归档筛选（已还记录）
    if (!showReturned) {
        filtered = filtered.filter(record => record.Status !== 1);
    }
    
    // 渲染记录
    const tbody = document.getElementById('reader-borrow-table');
    tbody.innerHTML = filtered.map(record => `
        <tr class="${record.Status === 1 ? 'returned-record' : ''}">
            <td>${record.BookName}</td>
            <td>${record.ISBN}</td>
            <td>${record.BorrowDate}</td>
            <td>${record.DueDate}</td>
            <td><span class="status-tag ${getStatusClass(record.Status)}">${getStatusText(record.Status)}</span></td>
            <td>
                <button class="btn-renew" onclick="renewBook(${record.BorrowId})" ${record.Status !== 0 ? 'disabled' : ''}>
                    <i class="fas fa-redo"></i> 续借
                </button>
                <button class="btn-detail" onclick="viewBorrowDetail(${record.BorrowId})"><i class="fas fa-eye"></i> 查看详情</button>
            </td>
        </tr>
    `).join('');
    
    // 更新逾期提示
    updateOverdueAlert();
}

// 搜索我的借阅记录
async function searchMyBorrows() {
    updateBorrowRecords();
}

// 切换已还记录归档
function toggleArchive() {
    showReturned = !showReturned;
    const toggleBtn = document.getElementById('toggle-archive');
    if (showReturned) {
        toggleBtn.innerHTML = '<i class="fas fa-chevron-down"></i> 展开已还记录';
        // 当展开已还记录时，自动将状态筛选设置为"all"，确保能看到所有记录
        document.getElementById('status-filter').value = 'all';
    } else {
        toggleBtn.innerHTML = '<i class="fas fa-chevron-up"></i> 收起已还记录';
    }
    updateBorrowRecords();
}

// 更新逾期提示
function updateOverdueAlert() {
    const overdueRecords = allBorrowRecords.filter(record => record.Status === 2);
    const overdueCount = overdueRecords.length;
    const overdueAlert = document.getElementById('overdue-alert');
    const overdueCountEl = document.getElementById('overdue-count');
    
    if (overdueCount > 0) {
        overdueCountEl.textContent = overdueCount;
        overdueAlert.style.display = 'flex';
    } else {
        overdueAlert.style.display = 'none';
    }
}

// 续借图书
async function renewBook(borrowId) {
    try {
        const response = await api(`/api/borrow/renew/${borrowId}`, 'POST');
        if (response.success) {
            alert('续借成功！');
            loadMyBorrows();
        } else {
            alert('续借失败：' + response.message);
        }
    } catch (error) {
        alert('请求失败：' + error.message);
    }
}

// 查看借阅详情
function viewBorrowDetail(borrowId) {
    const record = allBorrowRecords.find(r => r.BorrowId === borrowId);
    if (record) {
        alert(`借阅详情：\n` +
              `借阅ID: ${record.BorrowId}\n` +
              `图书名称: ${record.BookName}\n` +
              `ISBN: ${record.ISBN}\n` +
              `借阅日期: ${record.BorrowDate}\n` +
              `应还日期: ${record.DueDate}\n` +
              `归还日期: ${record.ReturnDate || '未归还'}\n` +
              `状态: ${getStatusText(record.Status)}\n` +
              `逾期费: ${record.LateFee || 0}`);
    }
}

// 状态筛选变化事件
document.getElementById('status-filter').addEventListener('change', updateBorrowRecords);

// 日期筛选变化事件
document.getElementById('start-date').addEventListener('change', updateBorrowRecords);
document.getElementById('end-date').addEventListener('change', updateBorrowRecords);

// 全局变量
let allBooks = [];
let currentPage = 1;
const pageSize = 12;
let currentView = 'grid';

// 加载图书（读者浏览）
async function loadBooksForReader() {
    const result = await api('/api/books');
    if (result.success) {
        allBooks = result.data;
        sortBooks();
        updateBookDisplay();
    }
}

// 搜索图书（读者浏览）
async function searchBooksForReader() {
    const searchTerm = document.getElementById('reader-book-search').value.trim();
    const categoryId = document.getElementById('reader-book-category').value;
    const result = await api('/api/books');
    if (result.success) {
        allBooks = result.data;
        
        // 搜索筛选
        if (searchTerm) {
            allBooks = allBooks.filter(book => 
                book.BookName.toLowerCase().includes(searchTerm.toLowerCase()) ||
                (book.Author && book.Author.toLowerCase().includes(searchTerm.toLowerCase())) ||
                (book.Publisher && book.Publisher.toLowerCase().includes(searchTerm.toLowerCase())) ||
                book.ISBN.toLowerCase().includes(searchTerm.toLowerCase())
            );
        }
        
        // 类别筛选
        if (categoryId) {
            allBooks = allBooks.filter(book => book.CategoryId == categoryId);
        }
        
        // 限制搜索结果数量，最多显示100本图书
        if (allBooks.length > 100) {
            allBooks = allBooks.slice(0, 100);
            alert(`搜索结果过多，仅显示前100本图书`);
        }
        
        sortBooks();
        currentPage = 1;
        updateBookDisplay();
    }
}

// 排序图书
function sortBooks() {
    const sortBy = document.getElementById('reader-book-sort').value;
    switch (sortBy) {
        case 'borrow_count':
            allBooks.sort((a, b) => (b.BorrowCount || 0) - (a.BorrowCount || 0));
            break;
        case 'publish_date':
            allBooks.sort((a, b) => new Date(b.PublishDate || '1900-01-01') - new Date(a.PublishDate || '1900-01-01'));
            break;
        case 'add_date':
            allBooks.sort((a, b) => new Date(b.AddDate || '1900-01-01') - new Date(a.AddDate || '1900-01-01'));
            break;
    }
}

// 切换视图
function switchView(view) {
    currentView = view;
    document.getElementById('grid-view-btn').classList.toggle('active', view === 'grid');
    document.getElementById('list-view-btn').classList.toggle('active', view === 'list');
    document.getElementById('reader-book-grid').style.display = view === 'grid' ? 'grid' : 'none';
    document.getElementById('reader-book-list').style.display = view === 'list' ? 'block' : 'none';
    updateBookDisplay();
}

// 更新图书显示
function updateBookDisplay() {
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const paginatedBooks = allBooks.slice(startIndex, endIndex);
    
    if (currentView === 'grid') {
        renderBookGridForReader(paginatedBooks, 'reader-book-grid');
    } else {
        renderBookListForReader(paginatedBooks, 'reader-book-list');
    }
    
    updatePagination();
}

// 更新分页控件
function updatePagination() {
    const totalPages = Math.ceil(allBooks.length / pageSize);
    document.getElementById('current-page').textContent = currentPage;
    document.getElementById('total-pages').textContent = totalPages;
    document.getElementById('prev-page').disabled = currentPage === 1;
    document.getElementById('next-page').disabled = currentPage === totalPages;
}

// 切换页码
function changePage(direction) {
    const totalPages = Math.ceil(allBooks.length / pageSize);
    currentPage += direction;
    if (currentPage < 1) currentPage = 1;
    if (currentPage > totalPages) currentPage = totalPages;
    updateBookDisplay();
}

// 排序方式变化事件
document.getElementById('reader-book-sort').addEventListener('change', function() {
    sortBooks();
    currentPage = 1;
    updateBookDisplay();
});

// 加载图书（借阅申请）
async function loadBooksForApply() {
    const result = await api('/api/books');
    if (result.success) {
        applyBooks = result.data;
        currentApplyPage = 1;
        updateApplyBookDisplay();
    }
}

// 更新借阅申请图书显示
function updateApplyBookDisplay() {
    const startIndex = (currentApplyPage - 1) * applyPageSize;
    const endIndex = startIndex + applyPageSize;
    const paginatedBooks = applyBooks.slice(startIndex, endIndex);
    renderBookGridForApply(paginatedBooks, 'apply-book-grid');
    updateApplyPagination();
}

// 更新借阅申请分页控件
function updateApplyPagination() {
    const totalPages = Math.ceil(applyBooks.length / applyPageSize);
    document.getElementById('apply-current-page').textContent = currentApplyPage;
    document.getElementById('apply-total-pages').textContent = totalPages;
    document.getElementById('apply-prev-page').disabled = currentApplyPage === 1;
    document.getElementById('apply-next-page').disabled = currentApplyPage === totalPages;
}

// 切换借阅申请页码
function changeApplyPage(direction) {
    const totalPages = Math.ceil(applyBooks.length / applyPageSize);
    currentApplyPage += direction;
    if (currentApplyPage < 1) currentApplyPage = 1;
    if (currentApplyPage > totalPages) currentApplyPage = totalPages;
    updateApplyBookDisplay();
}

// 加载图书类别（借阅申请）
async function loadCategoriesForApply() {
    const result = await api('/api/book_categories');
    if (result.success) {
        const select = document.getElementById('apply-book-category');
        select.innerHTML = '<option value="">全部分类</option>' +
            result.data.map(c => `<option value="${c.CategoryId}">${c.CategoryName}</option>`).join('');
    }
}

// 加载图书类别（读者浏览）
async function loadCategoriesForReader() {
    const result = await api('/api/book_categories');
    if (result.success) {
        const select = document.getElementById('reader-book-category');
        select.innerHTML = '<option value="">全部分类</option>' +
            result.data.map(c => `<option value="${c.CategoryId}">${c.CategoryName}</option>`).join('');
    }
}

// 加载图书类别
async function loadBookCategories() {
    const result = await api('/api/book_categories');
    if (result.success) {
        // 这里可以加载图书类别到需要的地方
    }
}

// 搜索图书（借阅申请）
async function searchBooksForApply() {
    const searchTerm = document.getElementById('apply-book-search').value.trim();
    const categoryId = document.getElementById('apply-book-category').value;
    const result = await api('/api/books');
    if (result.success) {
        let filtered = result.data;
        if (searchTerm) {
            filtered = filtered.filter(book => 
                book.BookName.toLowerCase().includes(searchTerm.toLowerCase()) ||
                book.ISBN.toLowerCase().includes(searchTerm.toLowerCase())
            );
        }
        if (categoryId) {
            filtered = filtered.filter(book => book.CategoryId == categoryId);
        }
        applyBooks = filtered;
        currentApplyPage = 1;
        updateApplyBookDisplay();
    }
}

// 渲染图书网格（读者浏览）
function renderBookGridForReader(books, containerId) {
    const grid = document.getElementById(containerId);
    grid.innerHTML = books.map(b => {
        const availablePercent = (b.AvailableCount / b.TotalCount) * 100;
        return `
            <div class="book-card">
                <div class="book-cover">
                    <i class="fas fa-book"></i>
                </div>
                <h4>${b.BookName}</h4>
                <p>作者: ${b.Author || '未知'}</p>
                <p>出版社: ${b.Publisher || '未知'}</p>
                <div class="stock-info">
                    <div class="stock-label">
                        <span>可借数量</span>
                        <span>${b.AvailableCount}/${b.TotalCount}</span>
                    </div>
                    <div class="stock-bar">
                        <div class="stock-fill" style="width: ${availablePercent}%"></div>
                    </div>
                </div>
                <button class="btn-borrow" onclick="showBookInfo(${b.BookId}, '${b.BookName}', '${b.ISBN}', '${b.Author}', '${b.Publisher}', '${b.CategoryName}', ${b.AvailableCount}, ${b.TotalCount}, ${b.Price || 0})">
                    查看详情
                </button>
            </div>
        `;
    }).join('');
}

// 渲染图书列表（读者浏览）
function renderBookListForReader(books, containerId) {
    const list = document.getElementById(containerId);
    list.innerHTML = books.map(b => {
        const availablePercent = (b.AvailableCount / b.TotalCount) * 100;
        return `
            <div class="book-list-item">
                <div class="book-list-cover">
                    <i class="fas fa-book"></i>
                </div>
                <div class="book-list-info">
                    <h4>${b.BookName}</h4>
                    <p>ISBN: ${b.ISBN}</p>
                    <p>作者: ${b.Author || '未知'}</p>
                    <p>出版社: ${b.Publisher || '未知'}</p>
                    <p>类别: ${b.CategoryName || '未知'}</p>
                    <div class="book-list-stock">
                        <span class="stock-label">可借数量: ${b.AvailableCount}/${b.TotalCount}</span>
                        <div class="stock-bar">
                            <div class="stock-fill" style="width: ${availablePercent}%"></div>
                        </div>
                    </div>
                </div>
                <div class="book-list-actions">
                    <button class="btn-borrow" onclick="showBookInfo(${b.BookId}, '${b.BookName}', '${b.ISBN}', '${b.Author}', '${b.Publisher}', '${b.CategoryName}', ${b.AvailableCount}, ${b.TotalCount}, ${b.Price || 0})">
                        查看详情
                    </button>
                </div>
            </div>
        `;
    }).join('');
}

// 全局变量
let selectedBooks = [];
let currentStep = 1;
let currentReader = null;
// 借阅申请分页变量
let applyBooks = [];
let currentApplyPage = 1;
const applyPageSize = 12;

// 渲染图书网格（借阅申请）
function renderBookGridForApply(books, containerId) {
    const grid = document.getElementById(containerId);
    // 设置网格布局样式
    grid.style.display = 'grid';
    grid.style.gridTemplateColumns = 'repeat(auto-fill, minmax(280px, 1fr))';
    grid.style.gap = '20px';
    grid.style.alignItems = 'start';
    
    grid.innerHTML = books.map(b => {
        const availablePercent = (b.AvailableCount / b.TotalCount) * 100;
        const isSelected = selectedBooks.some(book => book.bookId === b.BookId);
        return `
            <div class="book-card" style="display: flex; flex-direction: column; height: 100%;">
                <div class="book-checkbox">
                    <input type="checkbox" data-book-id="${b.BookId}" data-book-name="${escapeHtml(b.BookName)}" data-isbn="${b.ISBN}" data-category-name="${escapeHtml(b.CategoryName || '未知')}" data-available-count="${b.AvailableCount}" ${isSelected ? 'checked' : ''}>
                </div>
                <div class="book-cover">
                    <i class="fas fa-book"></i>
                </div>
                <h4 style="margin-bottom: 10px; color: #5d4037; font-size: 16px; font-weight: 600; line-height: 1.4; flex: 1;">${escapeHtml(b.BookName)}</h4>
                <p style="margin: 5px 0; color: #8d6e63; font-size: 14px;">ISBN: ${b.ISBN}</p>
                <p style="margin: 5px 0; color: #8d6e63; font-size: 14px;">作者: ${escapeHtml(b.Author || '未知')}</p>
                <p style="margin: 5px 0; color: #8d6e63; font-size: 14px;">出版社: ${escapeHtml(b.Publisher || '未知')}</p>
                <p style="margin: 5px 0; color: #8d6e63; font-size: 14px;">类别: ${escapeHtml(b.CategoryName || '未知')}</p>
                <div class="stock-info" style="margin-top: 15px;">
                    <div class="stock-label">
                        <span>可借数量</span>
                        <span>${b.AvailableCount}/${b.TotalCount}</span>
                    </div>
                    <div class="stock-bar">
                        <div class="stock-fill" style="width: ${availablePercent}%"></div>
                    </div>
                </div>
            </div>
        `;
    }).join('');
    
    // 添加事件监听器
    grid.querySelectorAll('input[type="checkbox"]').forEach(checkbox => {
        checkbox.addEventListener('change', function() {
            const bookId = parseInt(this.dataset.bookId);
            const bookName = this.dataset.bookName;
            const isbn = this.dataset.isbn;
            const categoryName = this.dataset.categoryName;
            const availableCount = parseInt(this.dataset.availableCount);
            toggleBookSelection(this, bookId, bookName, isbn, categoryName, availableCount);
        });
    });
}

// HTML转义函数
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// 切换图书选择
function toggleBookSelection(checkbox, bookId, bookName, isbn, categoryName, availableCount) {
    const index = selectedBooks.findIndex(book => book.bookId === bookId);
    if (index > -1) {
        // 移除已选图书
        selectedBooks.splice(index, 1);
    } else {
        // 检查读者可借数量上限
        if (currentReader && selectedBooks.length >= currentReader.RemainingBorrowCount) {
            alert(`您最多只能借阅 ${currentReader.RemainingBorrowCount} 本图书`);
            checkbox.checked = false;
            return;
        }
        // 检查图书可借数量
        if (availableCount <= 0) {
            alert('该图书暂无可借数量');
            checkbox.checked = false;
            return;
        }
        // 添加新图书
        selectedBooks.push({ bookId, bookName, isbn, categoryName });
    }
    // 更新下一步按钮状态
    document.getElementById('next-to-step-2').disabled = selectedBooks.length === 0;
    // 更新已选图书列表
    updateSelectedBooksList();
}

// 更新已选图书列表
function updateSelectedBooksList() {
    const list = document.getElementById('selected-books-list');
    if (selectedBooks.length === 0) {
        list.innerHTML = '<p style="color: #8d6e63; text-align: center; margin: 20px 0;">暂无选中图书</p>';
        document.getElementById('next-to-step-3').disabled = true;
    } else {
        list.innerHTML = selectedBooks.map(book => `
            <div class="selected-book-item">
                <div class="selected-book-info">
                    <div class="selected-book-title">${book.bookName}</div>
                    <div class="selected-book-isbn">ISBN: ${book.isbn}</div>
                </div>
                <button class="remove-book-btn" onclick="removeBookFromSelection(${book.bookId})"><i class="fas fa-times"></i></button>
            </div>
        `).join('');
        document.getElementById('next-to-step-3').disabled = false;
    }
}

// 从选择中移除图书
function removeBookFromSelection(bookId) {
    const index = selectedBooks.findIndex(book => book.bookId === bookId);
    if (index > -1) {
        selectedBooks.splice(index, 1);
        // 更新图书网格中的复选框状态
        loadBooksForApply();
        // 更新下一步按钮状态
        document.getElementById('next-to-step-2').disabled = selectedBooks.length === 0;
        // 更新已选图书列表
        updateSelectedBooksList();
    }
}

// 步骤导航
function goToStep(step) {
    // 隐藏所有步骤内容
    document.querySelectorAll('.step-content').forEach(content => {
        content.style.display = 'none';
    });
    // 移除所有步骤的活动状态
    document.querySelectorAll('.step').forEach(s => {
        s.classList.remove('active', 'completed');
    });
    // 显示目标步骤内容
    document.getElementById(`step-${step}-content`).style.display = 'block';
    // 更新步骤状态
    for (let i = 1; i <= 3; i++) {
        if (i < step) {
            document.getElementById(`step-${i}`).classList.add('completed');
        } else if (i === step) {
            document.getElementById(`step-${i}`).classList.add('active');
        }
    }
    // 更新当前步骤
    currentStep = step;
    // 如果是第二步，更新借阅信息
    if (step === 2) {
        updateBorrowInfo();
    }
    // 如果是第三步，更新最终申请信息
    if (step === 3) {
        updateFinalApplicationInfo();
    }
}

// 更新借阅信息
function updateBorrowInfo() {
    // 从个人信息管理界面中获取读者信息
    updateCurrentReader();
    
    // 设置默认日期
    const today = new Date();
    const borrowDate = today.toISOString().split('T')[0];
    const dueDate = new Date(today);
    dueDate.setDate(today.getDate() + 30); // 默认30天
    const dueDateStr = dueDate.toISOString().split('T')[0];
    
    document.getElementById('apply-borrow-date').value = borrowDate;
    document.getElementById('apply-borrow-due-date').value = dueDateStr;
    
    // 更新读者信息
    if (currentReader) {
        document.getElementById('apply-reader-id').textContent = currentReader.ReaderId || '-';
        document.getElementById('apply-reader-name').textContent = currentReader.ReaderName || '-';
        document.getElementById('apply-reader-category').textContent = currentReader.CategoryName || '-';
        document.getElementById('apply-reader-remaining').textContent = currentReader.RemainingBorrowCount || '10';
    } else {
        document.getElementById('apply-reader-id').textContent = '-';
        document.getElementById('apply-reader-name').textContent = '-';
        document.getElementById('apply-reader-category').textContent = '-';
        document.getElementById('apply-reader-remaining').textContent = '10';
    }
    
    // 更新已选图书列表
    updateSelectedBooksList();
}

// 更新最终申请信息
function updateFinalApplicationInfo() {
    // 从个人信息管理界面中获取读者信息
    updateCurrentReader();
    
    const info = document.getElementById('final-application-info');
    const borrowDate = document.getElementById('apply-borrow-date').value;
    const dueDate = document.getElementById('apply-borrow-due-date').value;
    
    info.innerHTML = `
        <div class="final-info-section">
            <h4>读者信息</h4>
            <p>读者ID: ${currentReader ? currentReader.ReaderId : '-'}</p>
            <p>姓名: ${currentReader ? currentReader.ReaderName : '-'}</p>
            <p>类别: ${currentReader ? currentReader.CategoryName : '-'}</p>
        </div>
        <div class="final-info-section">
            <h4>借阅图书</h4>
            ${selectedBooks.map(book => `<p>${book.bookName} (ISBN: ${book.isbn})</p>`).join('')}
        </div>
        <div class="final-info-section">
            <h4>借阅详情</h4>
            <p>借阅日期: ${borrowDate}</p>
            <p>应还日期: ${dueDate}</p>
        </div>
    `;
}

// 提交申请
async function submitApplication() {
    if (selectedBooks.length === 0) {
        alert('请选择要借阅的图书');
        return;
    }
    
    // 从个人信息管理界面中获取读者信息
    updateCurrentReader();
    
    if (!currentReader) {
        alert('无法获取读者信息，请重新登录');
        return;
    }
    
    const borrowDate = document.getElementById('apply-borrow-date').value;
    const dueDate = document.getElementById('apply-borrow-due-date').value;
    
    try {
        const response = await api('/api/borrow/apply', 'POST', {
            reader_id: currentReader.ReaderId,
            books: selectedBooks.map(book => book.bookId),
            borrow_date: borrowDate,
            due_date: dueDate
        });
        
        if (response.success) {
            alert('借阅申请提交成功！');
            // 重置表单
            selectedBooks = [];
            goToStep(1);
            loadBooksForApply();
            loadApplicationRecords();
        } else {
            alert('借阅申请失败：' + response.message);
        }
    } catch (error) {
        alert('请求失败：' + error.message);
    }
}

// 加载申请记录
async function loadApplicationRecords() {
    try {
        if (!currentReader || !currentReader.ReaderId) {
            console.error('无法获取读者信息');
            const list = document.getElementById('application-records-list');
            list.innerHTML = '<p style="color: #8d6e63; text-align: center; margin: 20px 0;">无法获取读者信息</p>';
            return;
        }
        
        const response = await api(`/api/borrow/applications/reader/${currentReader.ReaderId}`);
        if (response.success) {
            const records = response.data;
            const list = document.getElementById('application-records-list');
            
            if (records.length === 0) {
                list.innerHTML = '<p style="color: #8d6e63; text-align: center; margin: 20px 0;">暂无借阅申请记录</p>';
                return;
            }
            
            list.innerHTML = records.map(record => {
                let statusClass = '';
                let statusText = '';
                
                switch (record.Status) {
                    case 0:
                        statusClass = 'pending';
                        statusText = '待审核';
                        break;
                    case 1:
                        statusClass = 'approved';
                        statusText = '审核通过';
                        break;
                    case 2:
                        statusClass = 'rejected';
                        statusText = '已驳回';
                        break;
                }
                
                return `
                    <div class="application-record-item">
                        <div class="application-record-header">
                            <div class="application-record-id">申请编号: ${record.ApplicationId}</div>
                            <div class="application-record-status ${statusClass}">${statusText}</div>
                        </div>
                        <div class="application-record-dates">
                            <p>申请时间: ${record.ApplicationTime}</p>
                            <p>预计借阅天数: ${record.ExpectedBorrowDays} 天</p>
                            <p>借阅日期: ${record.BorrowDate}</p>
                            <p>应还日期: ${record.DueDate}</p>
                        </div>
                        <div class="application-record-books">
                            <h4>借阅图书</h4>
                            <div class="application-record-book">${record.BookName} (ISBN: ${record.ISBN})</div>
                        </div>
                        ${record.RejectReason ? `
                            <div class="application-record-remark">
                                <h4>驳回原因</h4>
                                <p>${record.RejectReason}</p>
                            </div>
                        ` : ''}
                    </div>
                `;
            }).join('');
        }
    } catch (error) {
        console.error('加载借阅申请记录失败:', error);
        const list = document.getElementById('application-records-list');
        list.innerHTML = '<p style="color: #8d6e63; text-align: center; margin: 20px 0;">加载借阅申请记录失败</p>';
    }
}

// 加载个人信息
async function loadPersonalInfo() {
    console.log('loadPersonalInfo function called');
    try {
        // 从session获取用户信息
        const userInfo = JSON.parse(sessionStorage.getItem('user')) || {};
        console.log('User info from session:', userInfo);
        
        // 获取个人信息元素
        const readerIdInput = document.getElementById('personal-reader-id');
        const readerNameInput = document.getElementById('personal-reader-name');
        const genderSelect = document.getElementById('personal-reader-gender');
        const categorySelect = document.getElementById('personal-reader-category');
        const registrationDateSpan = document.getElementById('personal-reader-registration-date');
        const statusSpan = document.getElementById('personal-reader-status');
        const phoneSpan = document.getElementById('personal-reader-phone');
        const emailSpan = document.getElementById('personal-reader-email');
        const addressSpan = document.getElementById('personal-reader-address');
        
        // 从数据库获取最新的读者信息
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

            // 设置表单值
            if (readerIdInput) {
                readerIdInput.value = readerInfo.ReaderId || '1';
            }
            if (readerNameInput) {
                readerNameInput.value = readerInfo.ReaderName || userInfo.user_name || userInfo.real_name || '';
            }
            if (genderSelect) {
                genderSelect.value = readerInfo.Gender || '男';
            }
            if (categorySelect) {
                categorySelect.value = readerInfo.ReaderCategoryId === 3 ? '学生' : readerInfo.ReaderCategoryId === 1 ? '教师' : '职工';
            }
            if (registrationDateSpan) {
                registrationDateSpan.textContent = readerInfo.RegistrationDate || '-';
            }
            if (statusSpan) {
                statusSpan.textContent = readerInfo.Status === 0 ? '正常' : '异常';
            }
            if (phoneSpan) {
                phoneSpan.value = readerInfo.Phone || '-';
            }
            if (emailSpan) {
                emailSpan.value = readerInfo.Email || '-';
            }
            if (addressSpan) {
                addressSpan.value = readerInfo.Address || '-';
            }
        } else {
            // 设置默认值
            if (readerIdInput) {
                readerIdInput.value = '1'; // 读者ID默认为1
            }
            if (readerNameInput) {
                readerNameInput.value = userInfo.user_name || userInfo.real_name || ''; // 姓名更新为登录时的用户名
            }
            if (genderSelect) {
                genderSelect.value = '男'; // 性别默认为男
            }
            if (categorySelect) {
                categorySelect.value = '学生'; // 类别默认为学生
            }
            if (phoneSpan) {
                phoneSpan.value = '-';
            }
            if (emailSpan) {
                emailSpan.value = '-';
            }
            if (addressSpan) {
                addressSpan.value = '-';
            }
        }
        
        // 更新currentReader变量
        updateCurrentReader();
        
        // 加载借阅统计
        loadBorrowStats();
    } catch (error) {
        console.error('Error in loadPersonalInfo:', error);
        // 出错时设置默认值
        const readerIdInput = document.getElementById('personal-reader-id');
        const readerNameInput = document.getElementById('personal-reader-name');
        const genderSelect = document.getElementById('personal-reader-gender');
        const categorySelect = document.getElementById('personal-reader-category');
        const phoneSpan = document.getElementById('personal-reader-phone');
        const emailSpan = document.getElementById('personal-reader-email');
        const addressSpan = document.getElementById('personal-reader-address');
        
        if (readerIdInput) readerIdInput.value = '1';
        if (readerNameInput) readerNameInput.value = userInfo.user_name || userInfo.real_name || '';
        if (genderSelect) genderSelect.value = '男';
        if (categorySelect) categorySelect.value = '学生';
        if (phoneSpan) phoneSpan.value = '-';
        if (emailSpan) emailSpan.value = '-';
        if (addressSpan) addressSpan.value = '-';
        
        updateCurrentReader();
        loadBorrowStats();
    }
}

// 更新currentReader变量
function updateCurrentReader() {
    // 从个人信息管理界面中获取读者信息
    const readerIdInput = document.getElementById('personal-reader-id');
    const readerNameInput = document.getElementById('personal-reader-name');
    const categorySelect = document.getElementById('personal-reader-category');
    const phoneInput = document.getElementById('personal-reader-phone');
    const emailInput = document.getElementById('personal-reader-email');
    const addressInput = document.getElementById('personal-reader-address');
    
    // 更新currentReader变量
    currentReader = {
        ReaderId: readerIdInput ? parseInt(readerIdInput.value) : 1,
        ReaderName: readerNameInput ? readerNameInput.value : '',
        CategoryName: categorySelect ? categorySelect.value : '学生',
        Phone: phoneInput ? phoneInput.value : '',
        Email: emailInput ? emailInput.value : '',
        Address: addressInput ? addressInput.value : '',
        RemainingBorrowCount: 10 // 默认可借数量上限
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
    const phoneInput = document.getElementById('personal-reader-phone');
    const emailInput = document.getElementById('personal-reader-email');
    const addressInput = document.getElementById('personal-reader-address');

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
        phoneInput.removeAttribute('readonly');
        phoneInput.style.background = 'rgba(255, 255, 255, 0.8)';
        phoneInput.style.cursor = 'text';
        emailInput.removeAttribute('readonly');
        emailInput.style.background = 'rgba(255, 255, 255, 0.8)';
        emailInput.style.cursor = 'text';
        addressInput.removeAttribute('readonly');
        addressInput.style.background = 'rgba(255, 255, 255, 0.8)';
        addressInput.style.cursor = 'text';
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
        phoneInput.setAttribute('readonly', 'true');
        phoneInput.style.background = 'rgba(245, 245, 245, 0.8)';
        phoneInput.style.cursor = 'not-allowed';
        emailInput.setAttribute('readonly', 'true');
        emailInput.style.background = 'rgba(245, 245, 245, 0.8)';
        emailInput.style.cursor = 'not-allowed';
        addressInput.setAttribute('readonly', 'true');
        addressInput.style.background = 'rgba(245, 245, 245, 0.8)';
        addressInput.style.cursor = 'not-allowed';
    }
}

// 取消编辑
function cancelEdit() {
    const editSection = document.getElementById('edit-section');
    const editBtn = document.getElementById('edit-info-btn');
    const readerNameInput = document.getElementById('personal-reader-name');
    const genderSelect = document.getElementById('personal-reader-gender');
    const categorySelect = document.getElementById('personal-reader-category');
    const phoneInput = document.getElementById('personal-reader-phone');
    const emailInput = document.getElementById('personal-reader-email');
    const addressInput = document.getElementById('personal-reader-address');

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
    phoneInput.setAttribute('readonly', 'true');
    phoneInput.style.background = 'rgba(245, 245, 245, 0.8)';
    phoneInput.style.cursor = 'not-allowed';
    emailInput.setAttribute('readonly', 'true');
    emailInput.style.background = 'rgba(245, 245, 245, 0.8)';
    emailInput.style.cursor = 'not-allowed';
    addressInput.setAttribute('readonly', 'true');
    addressInput.style.background = 'rgba(245, 245, 245, 0.8)';
    addressInput.style.cursor = 'not-allowed';

    // 重新加载个人信息以恢复原始值
    loadPersonalInfo();
}

// 保存个人信息
async function savePersonalInfo() {
    const readerId = parseInt(document.getElementById('personal-reader-id').value);
    const readerName = document.getElementById('personal-reader-name').value.trim();
    const gender = document.getElementById('personal-reader-gender').value;
    const categoryName = document.getElementById('personal-reader-category').value;
    const phone = document.getElementById('personal-reader-phone').value.trim();
    const email = document.getElementById('personal-reader-email').value.trim();
    const address = document.getElementById('personal-reader-address').value.trim();

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
    let readerCategoryId = 3;
    if (categoryName === '教师') readerCategoryId = 1;
    else if (categoryName === '职工') readerCategoryId = 2;

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
}

// 加载借阅统计
async function loadBorrowStats() {
    // 确保currentReader已经加载
    if (!currentReader || !currentReader.ReaderId) {
        // 等待currentReader加载
        await new Promise(resolve => setTimeout(resolve, 500));
        // 再次检查
        if (!currentReader || !currentReader.ReaderId || currentReader.ReaderId === 0) {
            console.error('无法获取有效的读者ID');
            return;
        }
    }

    const result = await api(`/api/borrow/records?reader_id=${currentReader.ReaderId}`);
    if (result.success) {
        const records = result.data;
        document.getElementById('total-borrows').textContent = records.length;
        document.getElementById('current-borrows').textContent = records.filter(r => r.Status == 0).length;
        document.getElementById('overdue-borrows').textContent = records.filter(r => r.Status == 2).length;
        document.getElementById('total-fee').textContent = records.reduce((sum, r) => sum + (r.LateFee || 0), 0);
    }
}

// 更新个人信息
async function updatePersonalInfo() {
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
    
    // 转换类别名称为类别ID
    let readerCategoryId = 1; // 默认学生
    if (categoryName === '教师') {
        readerCategoryId = 2;
    } else if (categoryName === '职工') {
        readerCategoryId = 3;
    }
    
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
            loadPersonalInfo();
        } else {
            alert('更新失败：' + response.message);
        }
    } catch (error) {
        alert('请求失败：' + error.message);
    }
}

// 检查密码强度
function checkPasswordStrength() {
    const password = document.getElementById('new-password').value;
    const strengthBar = document.getElementById('password-strength');
    const strengthText = document.getElementById('password-strength-text');
    
    let strength = 0;
    
    // 长度检查
    if (password.length >= 8) strength += 25;
    
    // 包含数字
    if (/(\d)/.test(password)) strength += 25;
    
    // 包含小写字母
    if (/[a-z]/.test(password)) strength += 25;
    
    // 包含大写字母
    if (/[A-Z]/.test(password)) strength += 25;
    
    // 更新强度条
    strengthBar.style.width = `${strength}%`;
    
    // 更新强度文本
    if (strength < 25) {
        strengthText.textContent = '弱';
        strengthText.style.color = '#f44336';
    } else if (strength < 50) {
        strengthText.textContent = '一般';
        strengthText.style.color = '#ff9800';
    } else if (strength < 75) {
        strengthText.textContent = '良好';
        strengthText.style.color = '#4caf50';
    } else {
        strengthText.textContent = '强';
        strengthText.style.color = '#2196f3';
    }
}

// 修改密码
async function changePassword() {
    const oldPassword = document.getElementById('old-password').value;
    const newPassword = document.getElementById('new-password').value;
    const confirmPassword = document.getElementById('confirm-password').value;
    const readerId = parseInt(document.getElementById('personal-reader-id').value);
    
    if (!oldPassword) {
        alert('请输入旧密码');
        return;
    }
    
    if (!newPassword) {
        alert('请输入新密码');
        return;
    }
    
    if (newPassword !== confirmPassword) {
        alert('两次输入的新密码不一致');
        return;
    }
    
    // 检查密码强度
    const passwordStrength = document.getElementById('password-strength').style.width;
    const strengthPercentage = parseInt(passwordStrength);
    if (strengthPercentage < 50) {
        if (!confirm('密码强度较弱，确定要使用此密码吗？')) {
            return;
        }
    }
    
    try {
        // 构建API请求，发送旧密码和新密码
        const response = await api(`/api/readers/${readerId}/password`, 'PUT', {
            old_password: oldPassword,
            new_password: newPassword
        });
        
        if (response.success) {
            alert('密码修改成功！');
            // 清空密码输入框
            document.getElementById('old-password').value = '';
            document.getElementById('new-password').value = '';
            document.getElementById('confirm-password').value = '';
            // 重置密码强度条
            document.getElementById('password-strength').style.width = '0%';
            document.getElementById('password-strength-text').textContent = '弱';
            document.getElementById('password-strength-text').style.color = '#f44336';
        } else {
            alert('密码修改失败：' + response.message);
        }
    } catch (error) {
        alert('请求失败：' + error.message);
    }
}

// 获取状态文本
function getStatusText(status) {
    switch (status) {
        case 0: return '已借';
        case 1: return '已还';
        case 2: return '逾期';
        default: return '未知';
    }
}

// 获取状态类名
function getStatusClass(status) {
    switch (status) {
        case 0: return 'borrowed';
        case 1: return 'returned';
        case 2: return 'overdue';
        default: return '';
    }
}

// 显示图书信息模态框
function showBookInfo(bookId, bookName, isbn, author, publisher, category, available, total, price) {
    // 创建隐藏的input元素存储bookId
    let bookIdInput = document.getElementById('modal-book-id');
    if (!bookIdInput) {
        bookIdInput = document.createElement('input');
        bookIdInput.type = 'hidden';
        bookIdInput.id = 'modal-book-id';
        document.getElementById('book-info-modal').appendChild(bookIdInput);
    }
    bookIdInput.value = bookId;
    
    // 填充模态框内容
    document.getElementById('modal-book-name').textContent = bookName;
    document.getElementById('modal-book-isbn').textContent = isbn;
    document.getElementById('modal-book-author').textContent = author || '未知';
    document.getElementById('modal-book-publisher').textContent = publisher || '未知';
    document.getElementById('modal-book-category').textContent = category || '未知';
    document.getElementById('modal-book-available').textContent = available;
    document.getElementById('modal-book-total').textContent = total;
    document.getElementById('modal-book-price').textContent = price;
    
    // 生成随机的内容简介
    const descriptions = [
        '这是一本内容丰富的图书，涵盖了相关领域的基础知识和最新发展。作者通过深入浅出的方式，为读者提供了全面的知识体系。',
        '本书是该领域的经典之作，通过大量实例和案例分析，帮助读者理解核心概念和应用方法。',
        '这是一本权威的专业书籍，由行业专家撰写，内容详实，适合作为教材或参考资料。',
        '本书采用现代化的编写方式，结合实际应用场景，为读者提供了实用的知识和技能。',
        '这是一本深入浅出的入门书籍，适合初学者和有一定基础的读者阅读，内容涵盖了该领域的主要知识点。'
    ];
    const randomDescription = descriptions[Math.floor(Math.random() * descriptions.length)];
    document.getElementById('modal-book-description').textContent = randomDescription;
    
    // 显示模态框
    document.getElementById('book-info-modal').style.display = 'block';
}

// 关闭图书信息模态框
function closeBookInfoModal() {
    document.getElementById('book-info-modal').style.display = 'none';
}

// 复制书名
function copyBookName() {
    const bookName = document.getElementById('modal-book-name').textContent;
    navigator.clipboard.writeText(bookName).then(function() {
        alert('书名已复制到剪贴板！');
    }).catch(function(err) {
        alert('复制失败，请手动复制书名。');
    });
}

// 选择图书并跳转到借阅申请第二步
function selectBookForApply(bookId, bookName, isbn, categoryName) {
    // 清除当前选中的图书
    selectedBooks = [];
    
    // 添加当前图书到选中列表
    selectedBooks.push({ bookId: parseInt(bookId), bookName, isbn, categoryName });
    
    // 更新已选图书列表
    updateSelectedBooksList();
    
    // 跳转到借阅申请页面
    document.querySelectorAll('.menu-btn').forEach(btn => {
        if (btn.dataset.page === 'borrowApply') {
            btn.click();
        }
    });
    
    // 等待页面切换后，跳转到第二步
    setTimeout(() => {
        document.getElementById('next-to-step-2').click();
    }, 500);
}

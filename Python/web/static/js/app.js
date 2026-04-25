let currentUser = null;
let currentPage = 1;
let pageSize = 10;
let selectedBorrowReaderId = null;
let selectedBorrowBookId = null;
let selectedReturnBorrowId = null;
let borrowTrendChart = null;

// 图书管理相关变量
let allBooks = []; // 存储所有图书数据
let totalBooks = 0; // 总图书数量
let totalPages = 1; // 总页数

document.addEventListener('DOMContentLoaded', async () => {
    await checkLogin();
    setupMenu();
    setDefaultDates();
    await loadDashboardData();
    initBorrowTrendChart();
    setupChartPeriodChange();
});

async function checkLogin() {
    try {
        const response = await fetch('/');
        if (response.url.includes('login')) {
            window.location.href = '/login';
        } else {
            const html = await response.text();
            const userMatch = html.match(/user\.real_name\s*=\s*['"]([^'"]+)['"]/);
            if (userMatch) {
                document.getElementById('currentUser').textContent = '欢迎, ' + userMatch[1];
            }
            loadBookCategories();
            loadBooks();
            loadReaderCategories();
            loadReaders();
            loadBorrowRecords();
            loadBorrowReaders();
            loadBorrowBooks();
            loadReturnRecords();
            loadBorrowApplications();
        }
    } catch (e) {
        console.log('Not logged in');
    }
}

function setupMenu() {
    document.querySelectorAll('.menu-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            document.querySelectorAll('.menu-btn').forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            document.querySelectorAll('.page').forEach(p => p.style.display = 'none');
            const pageId = 'page-' + btn.dataset.page;
            document.getElementById(pageId).style.display = 'block';
            
            // 更新面包屑导航
            updateBreadcrumb(btn.textContent.trim());
        });
    });
}

function updateBreadcrumb(pageName) {
    document.getElementById('currentPageName').textContent = pageName;
}

function setDefaultDates() {
    const today = new Date().toISOString().split('T')[0];
    const nextMonth = new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
    
    // 只在元素存在时设置内容
    const borrowDateEl = document.getElementById('borrow-date');
    if (borrowDateEl) borrowDateEl.textContent = today;
    
    const borrowDueDateEl = document.getElementById('borrow-due-date');
    if (borrowDueDateEl) borrowDueDateEl.textContent = nextMonth;
    
    const returnDateEl = document.getElementById('return-date');
    if (returnDateEl) returnDateEl.value = today;
}

async function loadDashboardData() {
    try {
        // 模拟数据加载
        document.getElementById('totalBooks').textContent = '1,248';
        document.getElementById('totalReaders').textContent = '864';
        document.getElementById('currentBorrows').textContent = '127';
        document.getElementById('overdueBooks').textContent = '18';
    } catch (error) {
        console.error('加载仪表盘数据失败:', error);
    }
}

function initBorrowTrendChart() {
    const ctx = document.getElementById('borrowTrendChart').getContext('2d');
    
    // 模拟数据
    const labels = ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'];
    const data = [65, 59, 80, 81, 56, 55, 72, 78, 85, 90, 95, 100];
    
    borrowTrendChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: '借阅数量',
                data: data,
                borderColor: '#1976d2',
                backgroundColor: 'rgba(25, 118, 210, 0.1)',
                tension: 0.4,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: '借阅数量'
                    }
                },
                x: {
                    title: {
                        display: true,
                        text: '月份'
                    }
                }
            }
        }
    });
}

function setupChartPeriodChange() {
    document.getElementById('chartPeriod').addEventListener('change', function() {
        updateBorrowTrendChart(this.value);
    });
}

function updateBorrowTrendChart(period) {
    let labels, data;
    
    switch(period) {
        case 'week':
            labels = ['周一', '周二', '周三', '周四', '周五', '周六', '周日'];
            data = [12, 19, 15, 17, 22, 28, 25];
            break;
        case 'month':
            labels = ['第1周', '第2周', '第3周', '第4周'];
            data = [65, 59, 80, 81];
            break;
        case 'year':
            labels = ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'];
            data = [65, 59, 80, 81, 56, 55, 72, 78, 85, 90, 95, 100];
            break;
    }
    
    borrowTrendChart.data.labels = labels;
    borrowTrendChart.data.datasets[0].data = data;
    borrowTrendChart.update();
}

function exportData() {
    alert('导出功能开发中...');
}

function showHelp() {
    alert('使用帮助：\n1. 点击左侧菜单切换功能模块\n2. 点击修改按钮可编辑现有记录\n3. 点击删除按钮可删除记录\n4. 支持多条件搜索和分页查询\n5. 仪表盘显示系统概览数据');
}

async function api(url, method = 'GET', body = null) {
    let fullUrl = url;
    console.log('API call:', method, fullUrl, body);
    const options = {
        method,
        headers: {'Content-Type': 'application/json'}
    };
    if (body) options.body = JSON.stringify(body);
    try {
        const response = await fetch(fullUrl, options);
        console.log('API response status:', response.status);
        const data = await response.json();
        console.log('API response data:', data);
        return data;
    } catch (error) {
        console.error('API error:', error);
        throw error;
    }
}

async function loadBookCategories() {
    const result = await api('/api/book_categories');
    if (result.success) {
        const tbody = document.getElementById('bc-table');
        tbody.innerHTML = result.data.map(c => `
            <tr>
                <td>${c.CategoryId}</td>
                <td>${c.CategoryName}</td>
                <td>${c.Description || ''}</td>
                <td>
                    <div class="action-buttons">
                        <button class="edit-btn" onclick="editBookCategory(${c.CategoryId}, '${c.CategoryName}', '${c.Description || ''}')">
                            <i class="fas fa-edit"></i> 修改
                        </button>
                        <button class="delete-btn" onclick="deleteBookCategory(${c.CategoryId})">
                            <i class="fas fa-trash"></i> 删除
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');
    }
}

async function addBookCategory() {
    const name = document.getElementById('bc-name').value.trim();
    const desc = document.getElementById('bc-desc').value.trim();
    if (!name) { alert('请输入类别名称'); return; }
    const result = await api('/api/book_categories', 'POST', {category_name: name, description: desc});
    alert(result.message);
    if (result.success) {
        clearBookCategory();
        loadBookCategories();
    }
}

function editBookCategory(id, name, desc) {
    document.getElementById('bc-name').value = name;
    document.getElementById('bc-desc').value = desc;
    const tbody = document.getElementById('bc-table');
    tbody.dataset.editId = id;
}

async function deleteBookCategory(id) {
    if (!confirm('确定要删除吗？')) return;
    const result = await api(`/api/book_categories/${id}`, 'DELETE');
    alert(result.message);
    if (result.success) loadBookCategories();
}

function clearBookCategory() {
    document.getElementById('bc-name').value = '';
    document.getElementById('bc-desc').value = '';
    delete document.getElementById('bc-table').dataset.editId;
}

async function loadBookCategoryOptions() {
    const result = await api('/api/book_categories');
    if (result.success) {
        document.getElementById('b-category').innerHTML =
            '<option value="">选择类别</option>' +
            result.data.map(c => `<option value="${c.CategoryId}">${c.CategoryName}</option>`).join('');
    }
}

// 图书管理相关功能
let selectedBooks = [];
let searchHistory = [];

async function loadBooks() {
    const result = await api('/api/books');
    if (result.success) {
        await loadBookCategoryOptions();
        await loadBookSearchCategoryOptions();
        allBooks = result.data;
        totalBooks = allBooks.length;
        totalPages = Math.ceil(totalBooks / pageSize);
        currentPage = 1; // 重置为第一页
        renderPagedBooks();
        updatePaginationInfo();
        setupBookCheckboxes();
        loadSearchHistory();
        checkNotifications();
    }
}

function renderPagedBooks() {
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const pagedBooks = allBooks.slice(startIndex, endIndex);
    renderBooks(pagedBooks);
}

function updatePaginationInfo() {
    const pageInfo = document.getElementById('page-info');
    if (pageInfo) {
        pageInfo.textContent = `第 ${currentPage} 页，共 ${totalPages} 页`;
    }
    
    // 更新上一页/下一页按钮状态
    const prevBtn = document.getElementById('prev-page');
    const nextBtn = document.getElementById('next-page');
    
    if (prevBtn) {
        prevBtn.disabled = currentPage === 1;
        if (currentPage === 1) {
            prevBtn.style.opacity = '0.6';
            prevBtn.style.cursor = 'not-allowed';
        } else {
            prevBtn.style.opacity = '1';
            prevBtn.style.cursor = 'pointer';
        }
    }
    
    if (nextBtn) {
        nextBtn.disabled = currentPage === totalPages;
        if (currentPage === totalPages) {
            nextBtn.style.opacity = '0.6';
            nextBtn.style.cursor = 'not-allowed';
        } else {
            nextBtn.style.opacity = '1';
            nextBtn.style.cursor = 'pointer';
        }
    }
}

function changePage(direction) {
    const newPage = currentPage + direction;
    if (newPage >= 1 && newPage <= totalPages) {
        currentPage = newPage;
        renderPagedBooks();
        updatePaginationInfo();
    }
}

function renderBooks(books) {
    const tbody = document.getElementById('b-table');
    tbody.innerHTML = books.map(b => `
        <tr data-id="${b.BookId}">
            <td><input type="checkbox" class="book-checkbox" value="${b.BookId}"></td>
            <td>${b.BookId}</td>
            <td>${b.BookName}</td>
            <td>${b.Author || ''}</td>
            <td>${b.Publisher || ''}</td>
            <td>${b.ISBN || ''}</td>
            <td>${b.CategoryName || ''}</td>
            <td>${b.TotalCount}</td>
            <td>${b.AvailableCount}</td>
            <td>${b.Price}</td>
            <td>
                <div class="action-buttons">
                    <button class="edit-btn" onclick="editBook(${b.BookId}, '${b.BookName}', '${b.Author || ''}', '${b.Publisher || ''}', '${b.ISBN || ''}', ${b.CategoryId}, ${b.TotalCount}, ${b.AvailableCount}, ${b.Price}, '${b.Description || ''}')">
                        <i class="fas fa-edit"></i> 修改
                    </button>
                    <button class="delete-btn" onclick="deleteBook(${b.BookId})">
                        <i class="fas fa-trash"></i> 删除
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

async function loadBookSearchCategoryOptions() {
    const result = await api('/api/book_categories');
    if (result.success) {
        document.getElementById('b-search-category').innerHTML =
            '<option value="">选择类别</option>' +
            result.data.map(c => `<option value="${c.CategoryId}">${c.CategoryName}</option>`).join('');
    }
}

function setupBookCheckboxes() {
    // 全选/取消全选
    document.getElementById('select-all-books').addEventListener('change', function() {
        const checkboxes = document.querySelectorAll('.book-checkbox');
        checkboxes.forEach(checkbox => {
            checkbox.checked = this.checked;
        });
        updateSelectedBooks();
    });
    
    // 单个复选框
    document.querySelectorAll('.book-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', updateSelectedBooks);
    });
}

function updateSelectedBooks() {
    const checkboxes = document.querySelectorAll('.book-checkbox:checked');
    selectedBooks = Array.from(checkboxes).map(cb => parseInt(cb.value));
}

async function batchDeleteBooks() {
    if (selectedBooks.length === 0) {
        alert('请选择要删除的图书');
        return;
    }
    
    if (!confirm(`确定要删除选中的 ${selectedBooks.length} 本图书吗？`)) {
        return;
    }
    
    try {
        const result = await api('/api/books/batch', 'DELETE', { book_ids: selectedBooks });
        alert(result.message);
        if (result.success) {
            loadBooks();
            selectedBooks = [];
        }
    } catch (error) {
        alert('批量删除失败：' + error.message);
    }
}

function batchUpdateBooks() {
    if (selectedBooks.length === 0) {
        alert('请选择要修改的图书');
        return;
    }
    
    const categoryId = prompt('请输入新的类别ID：');
    if (!categoryId) return;
    
    batchUpdateBookCategory(selectedBooks, categoryId);
}

async function batchUpdateBookCategory(bookIds, categoryId) {
    try {
        const result = await api('/api/books/batch/category', 'PUT', {
            book_ids: bookIds,
            category_id: categoryId
        });
        alert(result.message);
        if (result.success) {
            loadBooks();
            selectedBooks = [];
        }
    } catch (error) {
        alert('批量修改失败：' + error.message);
    }
}

function exportBooks() {
    alert('导出功能开发中...');
}

// 高级搜索功能
async function advancedSearchBooks() {
    const name = document.getElementById('b-search-name').value.trim();
    const author = document.getElementById('b-search-author').value.trim();
    const publisher = document.getElementById('b-search-publisher').value.trim();
    const isbn = document.getElementById('b-search-isbn').value.trim();
    const categoryId = document.getElementById('b-search-category').value;
    const stockStatus = document.getElementById('b-search-stock').value;
    
    const result = await api('/api/books');
    if (result.success) {
        let filtered = result.data;
        
        if (name) {
            filtered = filtered.filter(b => b.BookName.toLowerCase().includes(name.toLowerCase()));
        }
        if (author) {
            filtered = filtered.filter(b => b.Author && b.Author.toLowerCase().includes(author.toLowerCase()));
        }
        if (publisher) {
            filtered = filtered.filter(b => b.Publisher && b.Publisher.toLowerCase().includes(publisher.toLowerCase()));
        }
        if (isbn) {
            filtered = filtered.filter(b => b.ISBN && b.ISBN.toLowerCase().includes(isbn.toLowerCase()));
        }
        if (categoryId) {
            filtered = filtered.filter(b => b.CategoryId == categoryId);
        }
        if (stockStatus) {
            switch (stockStatus) {
                case 'available':
                    filtered = filtered.filter(b => b.AvailableCount > 0);
                    break;
                case 'unavailable':
                    filtered = filtered.filter(b => b.AvailableCount == 0);
                    break;
                case 'low':
                    filtered = filtered.filter(b => b.AvailableCount < 3);
                    break;
            }
        }
        
        // 更新图书数据和分页信息
        allBooks = filtered;
        totalBooks = allBooks.length;
        totalPages = Math.ceil(totalBooks / pageSize);
        currentPage = 1; // 重置为第一页
        
        renderPagedBooks();
        updatePaginationInfo();
        setupBookCheckboxes();
        saveSearchHistory({ name, author, publisher, isbn, categoryId, stockStatus });
    }
}

function clearSearch() {
    document.getElementById('b-search-name').value = '';
    document.getElementById('b-search-author').value = '';
    document.getElementById('b-search-publisher').value = '';
    document.getElementById('b-search-isbn').value = '';
    document.getElementById('b-search-category').value = '';
    document.getElementById('b-search-stock').value = '';
    loadBooks(); // 重新加载所有图书并更新分页
}

function saveSearchHistory(searchParams) {
    const searchString = Object.entries(searchParams)
        .filter(([_, value]) => value)
        .map(([key, value]) => `${key}: ${value}`)
        .join(', ');
    
    if (searchString) {
        searchHistory.unshift(searchString);
        searchHistory = searchHistory.slice(0, 5); // 只保留最近5条
        localStorage.setItem('bookSearchHistory', JSON.stringify(searchHistory));
        loadSearchHistory();
    }
}

function loadSearchHistory() {
    const savedHistory = localStorage.getItem('bookSearchHistory');
    if (savedHistory) {
        searchHistory = JSON.parse(savedHistory);
    }
    
    const historyContainer = document.getElementById('search-history');
    if (historyContainer) {
        historyContainer.innerHTML = searchHistory.map((item, index) => 
            `<span style="display: inline-block; padding: 2px 8px; margin: 0 5px 5px 0; background: #e9ecef; border-radius: 12px; cursor: pointer;" onclick="loadSearchHistoryItem(${index})">${item}</span>`
        ).join('');
    }
}

function loadSearchHistoryItem(index) {
    const historyItem = searchHistory[index];
    // 这里可以解析历史记录并填充搜索表单
    alert('加载搜索历史：' + historyItem);
}

// 通知系统
async function checkNotifications() {
    // 检查逾期图书
    const overdueResult = await api('/api/borrow/overdue');
    if (overdueResult.success && overdueResult.data.length > 0) {
        showNotification('逾期提醒', `有 ${overdueResult.data.length} 本图书已逾期`, 'warning');
    }
    
    // 检查库存不足
    const lowStockResult = await api('/api/books/low_stock');
    if (lowStockResult.success && lowStockResult.data.length > 0) {
        showNotification('库存预警', `有 ${lowStockResult.data.length} 本图书库存不足`, 'info');
    }
}

function showNotification(title, message, type = 'info') {
    // 创建通知元素
    const notification = document.createElement('div');
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: ${type === 'warning' ? '#ffc107' : type === 'error' ? '#dc3545' : '#17a2b8'};
        color: ${type === 'warning' ? '#212529' : 'white'};
        padding: 15px 20px;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        z-index: 10000;
        max-width: 300px;
        animation: slideIn 0.3s ease-out;
    `;
    
    notification.innerHTML = `
        <h4 style="margin: 0 0 10px 0; font-size: 16px;">${title}</h4>
        <p style="margin: 0; font-size: 14px;">${message}</p>
        <button onclick="this.parentElement.remove()" style="
            position: absolute;
            top: 5px;
            right: 10px;
            background: none;
            border: none;
            font-size: 16px;
            cursor: pointer;
            ${type === 'warning' ? 'color: #212529;' : 'color: white;'}
        ">&times;</button>
    `;
    
    document.body.appendChild(notification);
    
    // 3秒后自动关闭
    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease-in';
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

// 添加CSS动画
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    @keyframes slideOut {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(style);

// 导入功能
document.getElementById('book-import').addEventListener('change', function(e) {
    const file = e.target.files[0];
    if (file) {
        handleBookImport(file);
    }
});

function handleBookImport(file) {
    alert('导入功能开发中...');
    // 这里可以实现文件读取和解析逻辑
    document.getElementById('book-import').value = ''; // 重置文件输入
}

async function searchBooks() {
    const search = document.getElementById('b-search').value.trim().toLowerCase();
    const result = await api('/api/books');
    if (result.success) {
        const filtered = result.data.filter(b =>
            b.BookName.toLowerCase().includes(search) || (b.ISBN && b.ISBN.toLowerCase().includes(search))
        );
        renderBooks(filtered);
    }
}

async function addBook() {
    const book = {
        book_name: document.getElementById('b-name').value.trim(),
        author: document.getElementById('b-author').value.trim(),
        publisher: document.getElementById('b-publisher').value.trim(),
        isbn: document.getElementById('b-isbn').value.trim(),
        category_id: document.getElementById('b-category').value,
        price: document.getElementById('b-price').value || 0,
        total_count: document.getElementById('b-total').value || 0,
        description: document.getElementById('b-description').value.trim()
    };
    if (!book.book_name) { alert('请输入书名'); return; }
    if (!book.category_id) { alert('请选择类别'); return; }
    const result = await api('/api/books', 'POST', book);
    alert(result.message);
    if (result.success) {
        clearBook();
        loadBooks();
    }
}

function editBook(id, name, author, publisher, isbn, categoryId, total, available, price, description) {
    document.getElementById('b-name').value = name;
    document.getElementById('b-author').value = author;
    document.getElementById('b-publisher').value = publisher;
    document.getElementById('b-isbn').value = isbn;
    document.getElementById('b-category').value = categoryId;
    document.getElementById('b-total').value = total;
    document.getElementById('b-price').value = price;
    document.getElementById('b-description').value = description || '';
    document.getElementById('b-table').dataset.editId = id;
    document.getElementById('b-table').dataset.availableCount = available;
}

async function updateBook(id) {
    const availableCount = document.getElementById('b-table').dataset.availableCount;
    const book = {
        book_name: document.getElementById('b-name').value.trim(),
        author: document.getElementById('b-author').value.trim(),
        publisher: document.getElementById('b-publisher').value.trim(),
        isbn: document.getElementById('b-isbn').value.trim(),
        category_id: document.getElementById('b-category').value,
        price: document.getElementById('b-price').value || 0,
        total_count: document.getElementById('b-total').value || 0,
        available_count: availableCount,
        description: document.getElementById('b-description').value.trim()
    };
    const result = await api(`/api/books/${id}`, 'PUT', book);
    alert(result.message);
    if (result.success) {
        clearBook();
        loadBooks();
    }
}

async function deleteBook(id) {
    if (!confirm('确定要删除吗？')) return;
    const result = await api(`/api/books/${id}`, 'DELETE');
    alert(result.message);
    if (result.success) loadBooks();
}

function clearBook() {
    ['b-name', 'b-author', 'b-publisher', 'b-isbn', 'b-price', 'b-total', 'b-description'].forEach(id => document.getElementById(id).value = '');
    document.getElementById('b-category').value = '';
    delete document.getElementById('b-table').dataset.editId;
}

// 生成随机图书描述
function generateBookDescription() {
    const descriptions = [
        '这是一本内容丰富的图书，涵盖了相关领域的基础知识和最新发展。作者通过深入浅出的方式，为读者提供了全面的知识体系。',
        '本书是该领域的经典之作，通过大量实例和案例分析，帮助读者理解核心概念和应用方法。',
        '这是一本权威的专业书籍，由行业专家撰写，内容详实，适合作为教材或参考资料。',
        '本书采用现代化的编写方式，结合实际应用场景，为读者提供了实用的知识和技能。',
        '这是一本深入浅出的入门书籍，适合初学者和有一定基础的读者阅读，内容涵盖了该领域的主要知识点。',
        '本书通过生动的语言和丰富的插图，使复杂的概念变得易于理解，是学习该领域知识的理想选择。',
        '这是一本综合性的专业书籍，不仅介绍了理论知识，还提供了大量的实践案例和操作指南。',
        '本书由多位专家合作撰写，汇集了各方面的专业知识，是该领域的权威参考资料。',
        '这是一本与时俱进的书籍，反映了该领域的最新发展趋势和研究成果，适合专业人士阅读。',
        '本书以实用为导向，注重培养读者的实际应用能力，是一本不可多得的实用指南。'
    ];
    const randomDescription = descriptions[Math.floor(Math.random() * descriptions.length)];
    document.getElementById('b-description').value = randomDescription;
}

async function loadReaderCategories() {
    const result = await api('/api/reader_categories');
    if (result.success) {
        const tbody = document.getElementById('rc-table');
        tbody.innerHTML = result.data.map(c => `
            <tr>
                <td>${c.CategoryId}</td>
                <td>${c.CategoryName}</td>
                <td>${c.MaxBorrowCount}</td>
                <td>${c.BorrowDays}</td>
                <td>${c.LateFeePerDay}</td>
                <td>
                    <div class="action-buttons">
                        <button class="edit-btn" onclick="editReaderCategory(${c.CategoryId}, '${c.CategoryName}', ${c.MaxBorrowCount}, ${c.BorrowDays}, ${c.LateFeePerDay})">
                            <i class="fas fa-edit"></i> 修改
                        </button>
                        <button class="delete-btn" onclick="deleteReaderCategory(${c.CategoryId})">
                            <i class="fas fa-trash"></i> 删除
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');
    }
}

async function addReaderCategory() {
    const category = {
        category_name: document.getElementById('rc-name').value.trim(),
        max_borrow_count: document.getElementById('rc-max').value || 0,
        borrow_days: document.getElementById('rc-days').value || 0,
        late_fee_per_day: document.getElementById('rc-fee').value || 0
    };
    if (!category.category_name) { alert('请输入类别名称'); return; }
    const result = await api('/api/reader_categories', 'POST', category);
    alert(result.message);
    if (result.success) {
        clearReaderCategory();
        loadReaderCategories();
    }
}

function editReaderCategory(id, name, max, days, fee) {
    document.getElementById('rc-name').value = name;
    document.getElementById('rc-max').value = max;
    document.getElementById('rc-days').value = days;
    document.getElementById('rc-fee').value = fee;
    document.getElementById('rc-table').dataset.editId = id;
}

async function deleteReaderCategory(id) {
    if (!confirm('确定要删除吗？')) return;
    const result = await api(`/api/reader_categories/${id}`, 'DELETE');
    alert(result.message);
    if (result.success) loadReaderCategories();
}

function clearReaderCategory() {
    ['rc-name', 'rc-max', 'rc-days', 'rc-fee'].forEach(id => document.getElementById(id).value = '');
    delete document.getElementById('rc-table').dataset.editId;
}

async function loadReaderCategoryOptions() {
    const result = await api('/api/reader_categories');
    if (result.success) {
        document.getElementById('r-category').innerHTML =
            '<option value="">选择类别</option>' +
            result.data.map(c => `<option value="${c.CategoryId}">${c.CategoryName}</option>`).join('');
    }
}

async function loadReaders() {
    const result = await api('/api/readers');
    if (result.success) {
        await loadReaderCategoryOptions();
        const statusText = ['正常', '挂失', '注销'];
        const tbody = document.getElementById('r-table');
        tbody.innerHTML = result.data.map(r => `
            <tr>
                <td>${r.ReaderId}</td>
                <td>${r.ReaderName}</td>
                <td>${r.Gender || '男'}</td>
                <td>${r.ReaderCategoryName || ''}</td>
                <td>${r.Phone || '-'}</td>
                <td>${r.Email || '-'}</td>
                <td>${r.Address || '-'}</td>
                <td>${statusText[r.Status] || ''}</td>
                <td>
                    <div class="action-buttons">
                        <button class="edit-btn" onclick="editReader(${r.ReaderId}, '${r.ReaderName}', '${r.Gender || '男'}', ${r.ReaderCategoryId}, '${r.Phone || ''}', ${r.Email || ''}, '${r.Address || ''}', ${r.Status})">
                            <i class="fas fa-edit"></i> 修改
                        </button>
                        <button class="delete-btn" onclick="deleteReader(${r.ReaderId})">
                            <i class="fas fa-trash"></i> 删除
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');
    }
}

async function searchReaders() {
    const search = document.getElementById('r-search').value.trim().toLowerCase();
    const result = await api('/api/readers');
    if (result.success) {
        const statusText = ['正常', '挂失', '注销'];
        const filtered = result.data.filter(r =>
            r.ReaderName.toLowerCase().includes(search) || (r.Phone && r.Phone.includes(search))
        );
        const tbody = document.getElementById('r-table');
        tbody.innerHTML = filtered.map(r => `
            <tr>
                <td>${r.ReaderId}</td>
                <td>${r.ReaderName}</td>
                <td>${r.Gender || '男'}</td>
                <td>${r.ReaderCategoryName || ''}</td>
                <td>${r.Phone || '-'}</td>
                <td>${r.Email || '-'}</td>
                <td>${r.Address || '-'}</td>
                <td>${statusText[r.Status] || ''}</td>
                <td>
                    <div class="action-buttons">
                        <button class="edit-btn" onclick="editReader(${r.ReaderId}, '${r.ReaderName}', '${r.Gender || '男'}', ${r.ReaderCategoryId}, '${r.Phone || ''}', ${r.Email || ''}, '${r.Address || ''}', ${r.Status})">
                            <i class="fas fa-edit"></i> 修改
                        </button>
                        <button class="delete-btn" onclick="deleteReader(${r.ReaderId})">
                            <i class="fas fa-trash"></i> 删除
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');
    }
}

async function addReader() {
    const reader = {
        reader_name: document.getElementById('r-name').value.trim(),
        gender: document.getElementById('r-gender').value,
        reader_category_id: document.getElementById('r-category').value,
        phone: document.getElementById('r-phone').value.trim(),
        email: document.getElementById('r-email').value.trim(),
        address: document.getElementById('r-address').value.trim(),
        status: document.getElementById('r-status').value,
        remark: ''
    };
    if (!reader.reader_name) { alert('请输入姓名'); return; }
    if (!reader.reader_category_id) { alert('请选择类别'); return; }
    const result = await api('/api/readers', 'POST', reader);
    alert(result.message);
    if (result.success) {
        clearReader();
        loadReaders();
    }
}

function editReader(id, name, gender, categoryId, phone, email, address, status) {
    document.getElementById('r-name').value = name;
    document.getElementById('r-gender').value = gender;
    document.getElementById('r-category').value = categoryId;
    document.getElementById('r-phone').value = phone;
    document.getElementById('r-email').value = email;
    document.getElementById('r-address').value = address;
    document.getElementById('r-status').value = status;
    document.getElementById('r-table').dataset.editId = id;
}

async function deleteReader(id) {
    if (!confirm('确定要删除吗？')) return;
    const result = await api(`/api/readers/${id}`, 'DELETE');
    alert(result.message);
    if (result.success) loadReaders();
}

function clearReader() {
    ['r-name', 'r-phone', 'r-email', 'r-address'].forEach(id => document.getElementById(id).value = '');
    document.getElementById('r-gender').value = '男';
    document.getElementById('r-category').value = '';
    document.getElementById('r-status').value = '0';
    delete document.getElementById('r-table').dataset.editId;
}

// 借阅图书相关功能
let currentStep = 1;
let selectedReaderData = null;
let selectedBookData = null;
let currentView = 'grid';
// 分页相关变量
let borrowBooksCurrentPage = 1;
let borrowBooksPageSize = 10;
let borrowBooksTotalBooks = 0;
let borrowBooksTotalPages = 1;
let borrowBooksAllBooks = [];

async function loadBorrowReaders() {
    const result = await api('/api/readers');
    if (result.success) {
        const statusText = ['正常', '挂失', '注销'];
        const tbody = document.getElementById('borrow-reader-table');
        tbody.innerHTML = result.data.map(r => `
            <tr data-id="${r.ReaderId}" data-name="${r.ReaderName}" data-category="${r.ReaderCategoryName}" data-phone="${r.Phone}" data-status="${r.Status}" class="${selectedBorrowReaderId == r.ReaderId ? 'selected' : ''}">
                <td>${r.ReaderId}</td>
                <td>${r.ReaderName}</td>
                <td>${r.ReaderCategoryName || ''}</td>
                <td>${r.Phone || ''}</td>
                <td>${statusText[r.Status] || ''}</td>
            </tr>
        `).join('');
    }
}

async function loadBorrowBooks() {
    const result = await api('/api/books');
    if (result.success) {
        // 筛选可借图书
        borrowBooksAllBooks = result.data.filter(b => b.AvailableCount > 0);
        borrowBooksTotalBooks = borrowBooksAllBooks.length;
        borrowBooksTotalPages = Math.ceil(borrowBooksTotalBooks / borrowBooksPageSize);
        borrowBooksCurrentPage = 1; // 重置为第一页
        
        // 加载当前页的图书
        renderPagedBorrowBooks();
        updateBorrowBooksPaginationInfo();
    }
}

function renderPagedBorrowBooks() {
    const startIndex = (borrowBooksCurrentPage - 1) * borrowBooksPageSize;
    const endIndex = startIndex + borrowBooksPageSize;
    const pagedBooks = borrowBooksAllBooks.slice(startIndex, endIndex);
    
    // 加载列表视图
    const tbody = document.getElementById('borrow-book-table');
    tbody.innerHTML = pagedBooks.map(b => `
        <tr data-id="${b.BookId}" data-name="${b.BookName}" data-isbn="${b.ISBN}" data-category="${b.CategoryName}" data-available="${b.AvailableCount}" data-total="${b.TotalCount}" class="${selectedBorrowBookId == b.BookId ? 'selected' : ''}">
            <td>${b.BookId}</td>
            <td>${b.BookName}</td>
            <td>${b.ISBN || ''}</td>
            <td>${b.AvailableCount}</td>
            <td>${b.TotalCount}</td>
        </tr>
    `).join('');
    
    // 加载网格视图
    renderBookGrid(pagedBooks);
}

function updateBorrowBooksPaginationInfo() {
    // 创建分页信息和控制按钮
    const paginationContainer = document.getElementById('borrow-books-pagination');
    if (!paginationContainer) {
        // 如果不存在，创建一个
        const step2Content = document.getElementById('step-2');
        const paginationDiv = document.createElement('div');
        paginationDiv.id = 'borrow-books-pagination';
        paginationDiv.className = 'pagination';
        paginationDiv.style.cssText = `
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 20px;
            padding: 10px;
            background: rgba(255, 255, 255, 0.8);
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        `;
        
        paginationDiv.innerHTML = `
            <div id="borrow-books-page-info" style="font-size: 14px; color: #8d6e63;"></div>
            <div style="display: flex; gap: 10px; align-items: center;">
                <button id="borrow-books-prev-page" onclick="changeBorrowBooksPage(-1)" style="
                    padding: 8px 16px;
                    border: none;
                    border-radius: 8px;
                    cursor: pointer;
                    font-size: 14px;
                    transition: all 0.3s ease;
                    background: linear-gradient(135deg, #ffecd2 0%, #fcb69f 100%);
                    color: #5d4037;
                    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                ">
                    <i class="fas fa-chevron-left"></i> 上一页
                </button>
                <button id="borrow-books-next-page" onclick="changeBorrowBooksPage(1)" style="
                    padding: 8px 16px;
                    border: none;
                    border-radius: 8px;
                    cursor: pointer;
                    font-size: 14px;
                    transition: all 0.3s ease;
                    background: linear-gradient(135deg, #ffecd2 0%, #fcb69f 100%);
                    color: #5d4037;
                    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                ">
                    下一页 <i class="fas fa-chevron-right"></i>
                </button>
            </div>
        `;
        
        step2Content.appendChild(paginationDiv);
    }
    
    // 更新分页信息
    const pageInfo = document.getElementById('borrow-books-page-info');
    if (pageInfo) {
        pageInfo.textContent = `第 ${borrowBooksCurrentPage} 页，共 ${borrowBooksTotalPages} 页`;
    }
    
    // 更新上一页/下一页按钮状态
    const prevBtn = document.getElementById('borrow-books-prev-page');
    const nextBtn = document.getElementById('borrow-books-next-page');
    
    if (prevBtn) {
        prevBtn.disabled = borrowBooksCurrentPage === 1;
        if (borrowBooksCurrentPage === 1) {
            prevBtn.style.opacity = '0.6';
            prevBtn.style.cursor = 'not-allowed';
        } else {
            prevBtn.style.opacity = '1';
            prevBtn.style.cursor = 'pointer';
        }
    }
    
    if (nextBtn) {
        nextBtn.disabled = borrowBooksCurrentPage === borrowBooksTotalPages;
        if (borrowBooksCurrentPage === borrowBooksTotalPages) {
            nextBtn.style.opacity = '0.6';
            nextBtn.style.cursor = 'not-allowed';
        } else {
            nextBtn.style.opacity = '1';
            nextBtn.style.cursor = 'pointer';
        }
    }
}

function changeBorrowBooksPage(direction) {
    const newPage = borrowBooksCurrentPage + direction;
    if (newPage >= 1 && newPage <= borrowBooksTotalPages) {
        borrowBooksCurrentPage = newPage;
        renderPagedBorrowBooks();
        updateBorrowBooksPaginationInfo();
    }
}

function renderBookGrid(books) {
    const grid = document.getElementById('borrow-book-grid');
    grid.innerHTML = books.map(b => {
        const availablePercent = (b.AvailableCount / b.TotalCount) * 100;
        return `
            <div class="book-card" data-id="${b.BookId}" data-name="${b.BookName}" data-isbn="${b.ISBN}" data-category="${b.CategoryName}" data-available="${b.AvailableCount}" data-total="${b.TotalCount}">
                <div class="book-cover">
                    <i class="fas fa-book"></i>
                </div>
                <h4>${b.BookName}</h4>
                <p>ISBN: ${b.ISBN}</p>
                <p>类别: ${b.CategoryName}</p>
                <div class="stock-info">
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
    
    // 添加卡片点击事件
    document.querySelectorAll('.book-card').forEach(card => {
        card.addEventListener('click', function() {
            selectBorrowBookCard(this);
        });
    });
}

function selectBorrowReader(e) {
    const tr = e.target.closest('tr');
    if (!tr || !tr.dataset.id) return;
    selectedBorrowReaderId = parseInt(tr.dataset.id);
    selectedReaderData = {
        id: parseInt(tr.dataset.id),
        name: tr.dataset.name,
        category: tr.dataset.category,
        phone: tr.dataset.phone,
        status: parseInt(tr.dataset.status)
    };
    
    // 启用下一步按钮
    const nextBtn = document.getElementById('next-to-step-2');
    nextBtn.disabled = false;
    nextBtn.style.opacity = '1';
    nextBtn.style.cursor = 'pointer';
    
    tr.parentElement.querySelectorAll('tr').forEach(r => r.classList.remove('selected'));
    tr.classList.add('selected');
}

function selectBorrowBook(e) {
    const tr = e.target.closest('tr');
    if (!tr || !tr.dataset.id) return;
    selectedBorrowBookId = parseInt(tr.dataset.id);
    selectedBookData = {
        id: parseInt(tr.dataset.id),
        name: tr.dataset.name,
        isbn: tr.dataset.isbn,
        category: tr.dataset.category,
        available: parseInt(tr.dataset.available),
        total: parseInt(tr.dataset.total)
    };
    
    // 启用下一步按钮
    const nextBtn = document.getElementById('next-to-step-3');
    nextBtn.disabled = false;
    nextBtn.style.opacity = '1';
    nextBtn.style.cursor = 'pointer';
    
    tr.parentElement.querySelectorAll('tr').forEach(r => r.classList.remove('selected'));
    tr.classList.add('selected');
}

function selectBorrowBookCard(card) {
    document.querySelectorAll('.book-card').forEach(c => c.classList.remove('selected'));
    card.classList.add('selected');
    card.style.borderColor = '#ffb69f';
    card.style.boxShadow = '0 8px 16px rgba(255, 182, 159, 0.25)';
    
    selectedBorrowBookId = parseInt(card.dataset.id);
    selectedBookData = {
        id: parseInt(card.dataset.id),
        name: card.dataset.name,
        isbn: card.dataset.isbn,
        category: card.dataset.category,
        available: parseInt(card.dataset.available),
        total: parseInt(card.dataset.total)
    };
    
    // 启用下一步按钮
    const nextBtn = document.getElementById('next-to-step-3');
    nextBtn.disabled = false;
    nextBtn.style.opacity = '1';
    nextBtn.style.cursor = 'pointer';
}

function goToStep(step) {
    // 隐藏所有步骤内容
    document.querySelectorAll('.step-content').forEach(content => {
        content.style.display = 'none';
    });
    
    // 显示当前步骤内容
    document.getElementById(`step-${step}`).style.display = 'block';
    
    // 更新步骤条状态
    document.querySelectorAll('.step').forEach((stepEl, index) => {
        const stepNum = index + 1;
        if (stepNum < step) {
            // 已完成步骤
            stepEl.classList.add('completed');
            stepEl.classList.remove('active');
            const stepNumber = stepEl.querySelector('.step-number');
            stepNumber.innerHTML = '<i class="fas fa-check"></i>';
            stepNumber.style.background = '#e0e0e0';
            stepNumber.style.color = '#999';
            stepEl.querySelector('.step-title').style.color = '#999';
        } else if (stepNum === step) {
            // 当前步骤
            stepEl.classList.add('active');
            stepEl.classList.remove('completed');
            const stepNumber = stepEl.querySelector('.step-number');
            stepNumber.textContent = stepNum;
            stepNumber.style.background = 'linear-gradient(135deg, #ffb69f 0%, #fcb69f 100%)';
            stepNumber.style.color = '#5d4037';
            stepNumber.style.boxShadow = '0 2px 8px rgba(255, 182, 159, 0.3)';
            stepEl.querySelector('.step-title').style.color = '#5d4037';
        } else {
            // 未完成步骤
            stepEl.classList.remove('active', 'completed');
            const stepNumber = stepEl.querySelector('.step-number');
            stepNumber.textContent = stepNum;
            stepNumber.style.background = '#e0e0e0';
            stepNumber.style.color = '#999';
            stepNumber.style.boxShadow = 'none';
            stepEl.querySelector('.step-title').style.color = '#999';
        }
    });
    
    // 更新步骤线条
    document.querySelectorAll('.step-line').forEach((line, index) => {
        if (index < step - 1) {
            line.classList.add('active');
            line.style.background = 'linear-gradient(135deg, #ffb69f 0%, #fcb69f 100%)';
        } else {
            line.classList.remove('active');
            line.style.background = '#e0e0e0';
        }
    });
    
    // 更新当前步骤
    currentStep = step;
    
    // 如果是第三步，填充预览信息
    if (step === 3) {
        fillPreviewInfo();
    }
}

function fillPreviewInfo() {
    if (selectedReaderData) {
        document.getElementById('preview-reader-id').textContent = selectedReaderData.id;
        document.getElementById('preview-reader-name').textContent = selectedReaderData.name;
        document.getElementById('preview-reader-category').textContent = selectedReaderData.category;
        document.getElementById('preview-reader-phone').textContent = selectedReaderData.phone;
    }
    
    if (selectedBookData) {
        document.getElementById('preview-book-id').textContent = selectedBookData.id;
        document.getElementById('preview-book-name').textContent = selectedBookData.name;
        document.getElementById('preview-book-isbn').textContent = selectedBookData.isbn;
        document.getElementById('preview-book-category').textContent = selectedBookData.category;
    }
    
    const today = new Date().toISOString().split('T')[0];
    const nextMonth = new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
    document.getElementById('preview-borrow-date').textContent = today;
    document.getElementById('preview-borrow-due-date').textContent = nextMonth;
}

function switchView(view) {
    currentView = view;
    
    // 更新视图按钮状态
    document.querySelectorAll('.view-btn').forEach(btn => {
        btn.classList.remove('active');
        btn.style.background = 'rgba(255, 255, 255, 0.8)';
        btn.style.color = '#8d6e63';
        btn.style.boxShadow = '0 2px 8px rgba(0, 0, 0, 0.1)';
    });
    
    const activeBtn = document.querySelector(`button[onclick="switchView('${view}')"]`);
    activeBtn.classList.add('active');
    activeBtn.style.background = 'linear-gradient(135deg, #ffb69f 0%, #fcb69f 100%)';
    activeBtn.style.color = '#5d4037';
    activeBtn.style.boxShadow = '0 4px 12px rgba(255, 182, 159, 0.3)';
    
    // 显示对应视图
    document.getElementById('book-grid-view').style.display = view === 'grid' ? 'block' : 'none';
    document.getElementById('book-list-view').style.display = view === 'list' ? 'block' : 'none';
}

async function searchReadersForBorrow() {
    const search = document.getElementById('reader-search').value.trim().toLowerCase();
    const status = document.getElementById('reader-status').value;
    
    const result = await api('/api/readers');
    if (result.success) {
        let filtered = result.data;
        
        if (search) {
            filtered = filtered.filter(r => 
                r.ReaderName.toLowerCase().includes(search) || 
                r.Phone.toLowerCase().includes(search)
            );
        }
        
        if (status) {
            filtered = filtered.filter(r => r.Status == status);
        }
        
        const statusText = ['正常', '挂失', '注销'];
        const tbody = document.getElementById('borrow-reader-table');
        tbody.innerHTML = filtered.map(r => `
            <tr data-id="${r.ReaderId}" data-name="${r.ReaderName}" data-category="${r.ReaderCategoryName}" data-phone="${r.Phone}" data-status="${r.Status}">
                <td>${r.ReaderId}</td>
                <td>${r.ReaderName}</td>
                <td>${r.ReaderCategoryName || ''}</td>
                <td>${r.Phone || ''}</td>
                <td>${statusText[r.Status] || ''}</td>
            </tr>
        `).join('');
    }
}

async function searchBooksForBorrow() {
    const search = document.getElementById('book-search').value.trim().toLowerCase();
    const categoryId = document.getElementById('book-category').value;
    
    const result = await api('/api/books');
    if (result.success) {
        let filtered = result.data.filter(b => b.AvailableCount > 0);
        
        if (search) {
            filtered = filtered.filter(b => 
                b.BookName.toLowerCase().includes(search) || 
                b.ISBN.toLowerCase().includes(search)
            );
        }
        
        if (categoryId) {
            filtered = filtered.filter(b => b.CategoryId == categoryId);
        }
        
        // 更新图书数据和分页信息
        borrowBooksAllBooks = filtered;
        borrowBooksTotalBooks = borrowBooksAllBooks.length;
        borrowBooksTotalPages = Math.ceil(borrowBooksTotalBooks / borrowBooksPageSize);
        borrowBooksCurrentPage = 1; // 重置为第一页
        
        // 加载当前页的图书
        renderPagedBorrowBooks();
        updateBorrowBooksPaginationInfo();
    }
}

async function loadBookCategoriesForBorrow() {
    const result = await api('/api/book_categories');
    if (result.success) {
        document.getElementById('book-category').innerHTML =
            '<option value="">选择类别</option>' +
            result.data.map(c => `<option value="${c.CategoryId}">${c.CategoryName}</option>`).join('');
    }
}

async function confirmBorrow() {
    if (!selectedBorrowReaderId || !selectedBorrowBookId) {
        alert('请先选择读者和图书！');
        return;
    }
    
    const borrow_date = document.getElementById('preview-borrow-date').textContent;
    const due_date = document.getElementById('preview-borrow-due-date').textContent;
    
    const result = await api('/api/borrow/book', 'POST', {
        reader_id: selectedBorrowReaderId,
        book_id: selectedBorrowBookId,
        borrow_date,
        due_date
    });
    
    alert(result.message);
    if (result.success) {
        // 重置状态
        selectedBorrowReaderId = null;
        selectedBorrowBookId = null;
        selectedReaderData = null;
        selectedBookData = null;
        
        // 重置表单
        document.getElementById('reader-search').value = '';
        document.getElementById('reader-status').value = '';
        document.getElementById('book-search').value = '';
        document.getElementById('book-category').value = '';
        
        // 重新加载数据
        loadBorrowReaders();
        loadBorrowBooks();
        loadBookCategoriesForBorrow();
        
        // 回到第一步
        goToStep(1);
        
        // 禁用下一步按钮
        document.getElementById('next-to-step-2').disabled = true;
        document.getElementById('next-to-step-2').style.opacity = '0.6';
        document.getElementById('next-to-step-2').style.cursor = 'not-allowed';
        document.getElementById('next-to-step-3').disabled = true;
        document.getElementById('next-to-step-3').style.opacity = '0.6';
        document.getElementById('next-to-step-3').style.cursor = 'not-allowed';
    }
}

// 借阅申请审核相关功能
let applyCurrentPage = 1;
let applyPageSize = 10;
let applyTotalApplications = 0;
let applyTotalPages = 1;
let applyAllApplications = [];

async function loadBorrowApplications() {
    try {
        console.log('Loading borrow applications...');
        const result = await api('/api/borrow/applications');
        console.log('Borrow applications result:', result);
        if (result.success) {
            applyAllApplications = result.data;
            applyTotalApplications = applyAllApplications.length;
            applyTotalPages = Math.ceil(applyTotalApplications / applyPageSize);
            applyCurrentPage = 1; // 重置为第一页
            
            // 加载当前页的申请
            renderPagedBorrowApplications();
            updateApplyPaginationInfo();
        }
    } catch (error) {
        console.error('Error loading borrow applications:', error);
    }
}

function renderPagedBorrowApplications() {
    const startIndex = (applyCurrentPage - 1) * applyPageSize;
    const endIndex = startIndex + applyPageSize;
    const pagedApplications = applyAllApplications.slice(startIndex, endIndex);
    renderBorrowApplications(pagedApplications);
}

function updateApplyPaginationInfo() {
    // 更新分页信息
    const pageInfo = document.getElementById('apply-page-info');
    if (pageInfo) {
        pageInfo.textContent = `第 ${applyCurrentPage} 页，共 ${applyTotalPages} 页`;
    }
    
    // 更新上一页/下一页按钮状态
    const prevBtn = document.getElementById('prev-apply-page');
    const nextBtn = document.getElementById('next-apply-page');
    
    if (prevBtn) {
        prevBtn.disabled = applyCurrentPage === 1;
        if (applyCurrentPage === 1) {
            prevBtn.style.opacity = '0.6';
            prevBtn.style.cursor = 'not-allowed';
        } else {
            prevBtn.style.opacity = '1';
            prevBtn.style.cursor = 'pointer';
        }
    }
    
    if (nextBtn) {
        nextBtn.disabled = applyCurrentPage === applyTotalPages;
        if (applyCurrentPage === applyTotalPages) {
            nextBtn.style.opacity = '0.6';
            nextBtn.style.cursor = 'not-allowed';
        } else {
            nextBtn.style.opacity = '1';
            nextBtn.style.cursor = 'pointer';
        }
    }
}

function changeApplyPage(direction) {
    const newPage = applyCurrentPage + direction;
    if (newPage >= 1 && newPage <= applyTotalPages) {
        applyCurrentPage = newPage;
        renderPagedBorrowApplications();
        updateApplyPaginationInfo();
    }
}

function renderBorrowApplications(applications) {
    const tbody = document.getElementById('apply-table');
    tbody.innerHTML = applications.map(a => {
        let statusLabel = '';
        switch (a.Status) {
            case 0:
                statusLabel = '<span class="status-badge pending"><i class="fas fa-clock"></i> 待审核</span>';
                break;
            case 1:
                statusLabel = '<span class="status-badge approved"><i class="fas fa-check"></i> 审核通过</span>';
                break;
            case 2:
                statusLabel = '<span class="status-badge rejected"><i class="fas fa-times"></i> 已驳回</span>';
                break;
        }
        
        let actions = '';
        if (a.Status === 0) {
            actions = `
                <div class="apply-action-buttons">
                    <button class="apply-action-btn approve" onclick="approveApplication(${a.ApplicationId})">
                        <i class="fas fa-check"></i> 审核通过
                    </button>
                    <button class="apply-action-btn reject" onclick="rejectApplication(${a.ApplicationId})">
                        <i class="fas fa-times"></i> 驳回
                    </button>
                </div>
            `;
        } else {
            actions = '<span class="apply-action-placeholder">-</span>';
        }
        
        return `
            <tr data-id="${a.ApplicationId}">
                <td>${a.ApplicationId}</td>
                <td>${a.ReaderId}</td>
                <td><strong>${a.ReaderName}</strong></td>
                <td><strong>${a.BookName}</strong></td>
                <td>${a.ISBN}</td>
                <td>${a.ApplicationTime}</td>
                <td>${a.ExpectedBorrowDays}</td>
                <td>${statusLabel}</td>
                <td>${actions}</td>
            </tr>
        `;
    }).join('');
}

async function searchBorrowApplications() {
    const dateFrom = document.getElementById('apply-date-from').value;
    const dateTo = document.getElementById('apply-date-to').value;
    const status = document.getElementById('apply-status').value;
    const readerName = document.getElementById('apply-reader-name').value.trim().toLowerCase();
    const bookName = document.getElementById('apply-book-name').value.trim().toLowerCase();
    
    const result = await api('/api/borrow/applications');
    if (result.success) {
        let filtered = result.data;
        
        if (dateFrom) {
            filtered = filtered.filter(a => a.ApplicationTime >= dateFrom);
        }
        if (dateTo) {
            filtered = filtered.filter(a => a.ApplicationTime <= dateTo);
        }
        if (status) {
            filtered = filtered.filter(a => a.Status == status);
        }
        if (readerName) {
            filtered = filtered.filter(a => a.ReaderName.toLowerCase().includes(readerName));
        }
        if (bookName) {
            filtered = filtered.filter(a => a.BookName.toLowerCase().includes(bookName));
        }
        
        // 更新申请数据和分页信息
        applyAllApplications = filtered;
        applyTotalApplications = applyAllApplications.length;
        applyTotalPages = Math.ceil(applyTotalApplications / applyPageSize);
        applyCurrentPage = 1; // 重置为第一页
        
        // 加载当前页的申请
        renderPagedBorrowApplications();
        updateApplyPaginationInfo();
    }
}

async function approveApplication(applicationId) {
    if (!confirm('确定要审核通过该申请吗？')) return;
    
    const result = await api(`/api/borrow/applications/${applicationId}/approve`, 'POST');
    alert(result.message);
    if (result.success) {
        loadBorrowApplications();
    }
}

function rejectApplication(applicationId) {
    document.getElementById('reject-apply-id').value = applicationId;
    document.getElementById('reject-reason').value = '';
    document.getElementById('reject-modal').style.display = 'block';
}

function closeRejectModal() {
    document.getElementById('reject-modal').style.display = 'none';
}

async function confirmReject() {
    const applicationId = document.getElementById('reject-apply-id').value;
    const reason = document.getElementById('reject-reason').value.trim();
    
    if (!reason) {
        alert('请输入驳回原因');
        return;
    }
    
    const result = await api(`/api/borrow/applications/${applicationId}/reject`, 'POST', { reason });
    alert(result.message);
    if (result.success) {
        closeRejectModal();
        loadBorrowApplications();
    }
}

// 点击模态框外部关闭
window.onclick = function(event) {
    const modal = document.getElementById('reject-modal');
    if (event.target == modal) {
        modal.style.display = 'none';
    }
}

// 测试函数
window.testBorrowApplications = async function() {
    console.log('Testing borrow applications...');
    try {
        const result = await api('/api/borrow/applications');
        console.log('Borrow applications result:', result);
        if (result.success) {
            console.log('Borrow applications data:', result.data);
        }
    } catch (error) {
        console.error('Error testing borrow applications:', error);
    }
}

async function loadReturnRecords() {
    const result = await api('/api/borrow/active');
    if (result.success) {
        const statusText = ['借出中', '已归还', '逾期'];
        const tbody = document.getElementById('return-table');
        tbody.innerHTML = result.data.map(r => `
            <tr data-id="${r.BorrowId}" class="${selectedReturnBorrowId == r.BorrowId ? 'selected' : ''}">
                <td>${r.BorrowId}</td>
                <td>${r.ReaderId}</td>
                <td>${r.ReaderName || ''}</td>
                <td>${r.BookName || ''}</td>
                <td>${r.ISBN || ''}</td>
                <td>${r.BorrowDate || ''}</td>
                <td>${r.DueDate || ''}</td>
                <td>${statusText[r.Status] || ''}</td>
            </tr>
        `).join('');
    }
}

async function searchReturnRecords() {
    const readerId = document.getElementById('return-reader-id').value;
    const isbn = document.getElementById('return-isbn').value.trim();
    const result = await api('/api/borrow/active' + (readerId || isbn ? `?reader_id=${readerId}&isbn=${isbn}` : ''));
    if (result.success) {
        const statusText = ['借出中', '已归还', '逾期'];
        const tbody = document.getElementById('return-table');
        tbody.innerHTML = result.data.map(r => `
            <tr data-id="${r.BorrowId}" class="${selectedReturnBorrowId == r.BorrowId ? 'selected' : ''}">
                <td>${r.BorrowId}</td>
                <td>${r.ReaderId}</td>
                <td>${r.ReaderName || ''}</td>
                <td>${r.BookName || ''}</td>
                <td>${r.ISBN || ''}</td>
                <td>${r.BorrowDate || ''}</td>
                <td>${r.DueDate || ''}</td>
                <td>${statusText[r.Status] || ''}</td>
            </tr>
        `).join('');
    }
}

function selectReturnRecord(e) {
    const tr = e.target.closest('tr');
    if (!tr || !tr.dataset.id) return;
    selectedReturnBorrowId = parseInt(tr.dataset.id);
    document.getElementById('return-borrow-id').textContent = selectedReturnBorrowId;
    document.getElementById('return-form').style.display = 'block';
    tr.parentElement.querySelectorAll('tr').forEach(r => r.classList.remove('selected'));
    tr.classList.add('selected');
}

async function calculateLateFee() {
    if (!selectedReturnBorrowId) { alert('请选择借阅记录！'); return; }
    const return_date = document.getElementById('return-date').value;
    const result = await api('/api/calculate_late_fee', 'POST', {
        borrow_id: selectedReturnBorrowId,
        return_date
    });
    if (result.success) {
        document.getElementById('return-fee').value = result.data.late_fee.toFixed(2);
    } else {
        alert(result.message);
    }
}

async function confirmReturn() {
    if (!selectedReturnBorrowId) { alert('请选择借阅记录！'); return; }
    const return_date = document.getElementById('return-date').value;
    const late_fee = document.getElementById('return-fee').value || 0;
    const remark = document.getElementById('return-remark').value.trim();
    const result = await api('/api/return/book', 'POST', {
        borrow_id: selectedReturnBorrowId,
        return_date,
        late_fee: parseFloat(late_fee),
        remark
    });
    alert(result.message);
    if (result.success) {
        selectedReturnBorrowId = null;
        document.getElementById('return-borrow-id').textContent = '-';
        document.getElementById('return-form').style.display = 'none';
        document.getElementById('return-fee').value = '';
        document.getElementById('return-remark').value = '';
        loadReturnRecords();
        loadBorrowBooks();
    }
}

async function loadBorrowRecords() {
    currentPage = parseInt(document.getElementById('page-num').value) || 1;
    const readerId = document.getElementById('br-reader-id').value;
    const result = await api(`/api/borrow/records?page_index=${currentPage}&page_size=${pageSize}&reader_id=${readerId || ''}`);
    if (result.success) {
        const tbody = document.getElementById('br-table');
        tbody.innerHTML = result.data.map(r => `
            <tr>
                <td>${r.BorrowId}</td>
                <td>${r.ReaderId}</td>
                <td>${r.ReaderName || ''}</td>
                <td>${r.BookName || ''}</td>
                <td>${r.ISBN || ''}</td>
                <td>${r.BorrowDate || ''}</td>
                <td>${r.DueDate || ''}</td>
                <td>${r.ReturnDate || ''}</td>
                <td>${r.StatusText || ''}</td>
                <td>${r.LateFee || 0}</td>
                <td>${r.Remark || ''}</td>
            </tr>
        `).join('');
        document.getElementById('total-records').textContent = `共 ${result.total} 条`;
    }
}

async function searchBorrowRecords() {
    await loadBorrowRecords();
}

function goToPage(delta) {
    const newPage = currentPage + delta;
    if (newPage < 1) return;
    document.getElementById('page-num').value = newPage;
    loadBorrowRecords();
}

async function updateBookCategory(id) {
    const name = document.getElementById('bc-name').value.trim();
    const desc = document.getElementById('bc-desc').value.trim();
    const result = await api(`/api/book_categories/${id}`, 'PUT', {category_name: name, description: desc});
    alert(result.message);
    if (result.success) {
        clearBookCategory();
        loadBookCategories();
    }
}

async function updateReader(id) {
    const reader = {
        reader_name: document.getElementById('r-name').value.trim(),
        gender: document.getElementById('r-gender').value,
        reader_category_id: document.getElementById('r-category').value,
        phone: document.getElementById('r-phone').value.trim(),
        email: document.getElementById('r-email').value.trim(),
        address: document.getElementById('r-address').value.trim(),
        status: document.getElementById('r-status').value
    };
    const result = await api(`/api/readers/${id}`, 'PUT', reader);
    alert(result.message);
    if (result.success) {
        clearReader();
        loadReaders();
    }
}

async function updateReaderCategory(id) {
    const category = {
        category_name: document.getElementById('rc-name').value.trim(),
        max_borrow_count: document.getElementById('rc-max').value || 0,
        borrow_days: document.getElementById('rc-days').value || 0,
        late_fee_per_day: document.getElementById('rc-fee').value || 0
    };
    const result = await api(`/api/reader_categories/${id}`, 'PUT', category);
    alert(result.message);
    if (result.success) {
        clearReaderCategory();
        loadReaderCategories();
    }
}

function updateBookCategoryAction() {
    const editId = document.getElementById('bc-table').dataset.editId;
    if (!editId) {
        alert('请先选择要修改的记录！');
        return;
    }
    updateBookCategory(editId);
}

function updateBookAction() {
    const editId = document.getElementById('b-table').dataset.editId;
    if (!editId) {
        alert('请先选择要修改的记录！');
        return;
    }
    updateBook(editId);
}

function updateReaderCategoryAction() {
    const editId = document.getElementById('rc-table').dataset.editId;
    if (!editId) {
        alert('请先选择要修改的记录！');
        return;
    }
    updateReaderCategory(editId);
}

function updateReaderAction() {
    const editId = document.getElementById('r-table').dataset.editId;
    if (!editId) {
        alert('请先选择要修改的记录！');
        return;
    }
    updateReader(editId);
}
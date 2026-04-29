from flask import Flask, render_template, request, jsonify, session, redirect, url_for
import datetime
import sys
import os

app_dir = os.path.dirname(os.path.abspath(__file__))
python_dir = os.path.dirname(app_dir)
sys.path.insert(0, python_dir)

from dal.db_helper import DBHelper
from bll.user_manager import UserManager
from bll.book_manager import BookManager
from bll.book_category_manager import BookCategoryManager
from bll.reader_manager import ReaderManager
from bll.reader_category_manager import ReaderCategoryManager
from bll.borrow_manager import BorrowManager
from models.book import Book
from models.book_category import BookCategory
from models.reader import Reader
from models.reader_category import ReaderCategory
from models.user import User

template_folder = os.path.join(app_dir, 'templates')
static_folder = os.path.join(app_dir, 'static')
app = Flask(__name__, template_folder=template_folder, static_folder=static_folder)
app.secret_key = 'book_management_secret_key_2024'
app.config['JSON_AS_ASCII'] = False

# 测试路由
@app.route('/test', methods=['GET'])
def test():
    return 'Test route works!'

db = DBHelper()
db.init_database()

user_manager = UserManager()
book_manager = BookManager()
book_category_manager = BookCategoryManager()
reader_manager = ReaderManager()
reader_category_manager = ReaderCategoryManager()
borrow_manager = BorrowManager()


@app.route('/')
def index():
    if 'user' not in session:
        return redirect(url_for('login'))
    # 如果是读者，跳转到读者端界面
    if session['user']['user_type'] == 1:
        return redirect(url_for('reader_index'))
    return render_template('index.html', user=session['user'])


@app.route('/reader')
def reader_index():
    if 'user' not in session:
        return redirect(url_for('login'))
    # 如果是管理员，跳转到管理员界面
    if session['user']['user_type'] == 0:
        return redirect(url_for('index'))
    return render_template('reader_index.html', user=session['user'])


@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        data = request.get_json()
        user_name = data.get('user_name', '').strip()
        password = data.get('password', '')
        user_type = int(data.get('user_type', 0))

        try:
            user = user_manager.login(user_name, password)
            if user.user_type != user_type:
                return jsonify({'success': False, 'message': f'用户类型不匹配！您选择的是{"系统管理员" if user_type == 0 else "读者"}，但该账户是{"系统管理员" if user.user_type == 0 else "读者"}！'})
            session['user'] = {
                'user_id': user.user_id,
                'user_name': user.user_name,
                'real_name': user.real_name,
                'user_type': user.user_type
            }
            # 根据用户类型返回不同的跳转路径
            redirect_url = '/' if user.user_type == 0 else '/reader'
            return jsonify({
                'success': True,
                'message': f'登录成功！欢迎 {user.real_name}！',
                'redirect_url': redirect_url,
                'user_id': user.user_id
            })
        except Exception as ex:
            # 登录失败，返回错误信息
            error_message = str(ex)
            if '用户名或密码错误' in error_message:
                return jsonify({'success': False, 'message': '用户名或密码错误！'})
            else:
                return jsonify({'success': False, 'message': f'登录失败：{error_message}'})
    return render_template('login.html')


@app.route('/register', methods=['POST'])
def register():
    if request.method == 'POST':
        try:
            data = request.get_json()
            if not data:
                return jsonify({'success': False, 'message': '请求数据为空！'})
            
            user_name = data.get('user_name', '').strip()
            identity = data.get('identity', 'student')
            phone = data.get('phone', '').strip()
            password = data.get('password', '')

            # 验证用户名
            if not user_name:
                return jsonify({'success': False, 'message': '用户名不能为空！'})
            if not password:
                return jsonify({'success': False, 'message': '密码不能为空！'})
            if not phone:
                return jsonify({'success': False, 'message': '电话号码不能为空！'})
            
            # 检查用户名是否已存在
            from dal.user_service import UserService
            user_service = UserService()
            if user_service.check_user_name(user_name):
                return jsonify({'success': False, 'message': '注册失败：用户名已存在！'})
            
            # 创建用户
            user = User()
            user.user_name = user_name
            user.password = password
            user.user_type = 1  # 读者类型
            user.real_name = user_name  # 使用用户名作为真实姓名
            
            # 添加用户
            user_id = user_manager.add_user(user)
            if user_id > 0:
                # 创建读者记录
                reader = Reader()
                reader.user_id = user_id
                reader.reader_name = user_name  # 使用用户名作为读者姓名
                reader.gender = '男'  # 默认性别为男
                reader.reader_category_id = 3  # 默认学生类别
                reader.phone = phone
                reader.email = ''
                reader.address = ''
                reader.registration_date = datetime.datetime.now().strftime('%Y-%m-%d')
                reader.expiry_date = (datetime.datetime.now() + datetime.timedelta(days=365)).strftime('%Y-%m-%d')
                reader.status = 0  # 正常状态
                reader.remark = f'身份：{identity}'
                
                try:
                    if reader_manager.add_reader(reader):
                        return jsonify({'success': True, 'message': '注册成功！请使用用户名登录。'})
                    else:
                        return jsonify({'success': False, 'message': '注册失败：创建读者记录失败！'})
                except Exception as e:
                    return jsonify({'success': False, 'message': f'注册失败：{str(e)}'})
            else:
                return jsonify({'success': False, 'message': '注册失败：创建用户失败！'})
        except Exception as ex:
            return jsonify({'success': False, 'message': f'注册失败：{str(ex)}'})
    return jsonify({'success': False, 'message': '请求方法错误！'})


@app.route('/api/check_user', methods=['POST'])
def check_user():
    if request.method == 'POST':
        data = request.get_json()
        user_name = data.get('user_name', '').strip()
        user_type = data.get('user_type', 0)

        try:
            # 检查用户是否存在
            user = user_manager.login(user_name, '')
            if user:
                # 用户存在
                return jsonify({'success': True, 'exists': True})
            else:
                # 用户不存在
                return jsonify({'success': True, 'exists': False})
        except Exception as ex:
            return jsonify({'success': False, 'message': str(ex)})
    return jsonify({'success': False, 'message': '请求方法错误！'})


@app.route('/logout')
def logout():
    session.pop('user', None)
    return redirect(url_for('login'))


@app.route('/api/book_categories', methods=['GET'])
def api_get_book_categories():
    try:
        rows = book_category_manager.get_all_categories()
        data = [dict(row) for row in rows]
        return jsonify({'success': True, 'data': data})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/book_categories', methods=['POST'])
def api_add_book_category():
    try:
        data = request.get_json()
        category = BookCategory()
        category.category_name = data.get('category_name', '')
        category.description = data.get('description', '')
        if book_category_manager.add_category(category):
            return jsonify({'success': True, 'message': '添加成功！'})
        return jsonify({'success': False, 'message': '添加失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/book_categories/<int:category_id>', methods=['PUT'])
def api_update_book_category(category_id):
    try:
        data = request.get_json()
        category = BookCategory()
        category.category_id = category_id
        category.category_name = data.get('category_name', '')
        category.description = data.get('description', '')
        if book_category_manager.update_category(category):
            return jsonify({'success': True, 'message': '修改成功！'})
        return jsonify({'success': False, 'message': '修改失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/book_categories/<int:category_id>', methods=['DELETE'])
def api_delete_book_category(category_id):
    try:
        if book_category_manager.delete_category(category_id):
            return jsonify({'success': True, 'message': '删除成功！'})
        return jsonify({'success': False, 'message': '删除失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/books', methods=['GET'])
def api_get_books():
    try:
        rows = book_manager.get_all_books()
        data = [dict(row) for row in rows]
        return jsonify({'success': True, 'data': data})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/books', methods=['POST'])
def api_add_book():
    try:
        data = request.get_json()
        book = Book()
        book.book_name = data.get('book_name', '')
        book.author = data.get('author', '')
        book.publisher = data.get('publisher', '')
        book.isbn = data.get('isbn', '')
        book.category_id = int(data.get('category_id', 0))
        book.total_count = int(data.get('total_count', 0))
        book.available_count = book.total_count
        book.price = float(data.get('price', 0))
        if book_manager.add_book(book):
            return jsonify({'success': True, 'message': '添加成功！'})
        return jsonify({'success': False, 'message': '添加失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/books/<int:book_id>', methods=['PUT'])
def api_update_book(book_id):
    try:
        data = request.get_json()
        book = Book()
        book.book_id = book_id
        book.book_name = data.get('book_name', '')
        book.author = data.get('author', '')
        book.publisher = data.get('publisher', '')
        book.isbn = data.get('isbn', '')
        book.category_id = int(data.get('category_id', 0))
        book.total_count = int(data.get('total_count', 0))
        book.available_count = int(data.get('available_count', 0))
        book.price = float(data.get('price', 0))
        if book_manager.update_book(book):
            return jsonify({'success': True, 'message': '修改成功！'})
        return jsonify({'success': False, 'message': '修改失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/books/<int:book_id>', methods=['DELETE'])
def api_delete_book(book_id):
    try:
        if book_manager.delete_book(book_id):
            return jsonify({'success': True, 'message': '删除成功！'})
        return jsonify({'success': False, 'message': '删除失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/books/batch', methods=['DELETE'])
def api_batch_delete_books():
    try:
        data = request.get_json()
        book_ids = data.get('book_ids', [])
        if not book_ids:
            return jsonify({'success': False, 'message': '请选择要删除的图书！'})
        
        success_count = 0
        for book_id in book_ids:
            if book_manager.delete_book(book_id):
                success_count += 1
        
        if success_count > 0:
            return jsonify({'success': True, 'message': f'成功删除 {success_count} 本图书！'})
        return jsonify({'success': False, 'message': '删除失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/books/batch/category', methods=['PUT'])
def api_batch_update_book_category():
    try:
        data = request.get_json()
        book_ids = data.get('book_ids', [])
        category_id = data.get('category_id', 0)
        
        if not book_ids:
            return jsonify({'success': False, 'message': '请选择要修改的图书！'})
        if not category_id:
            return jsonify({'success': False, 'message': '请选择目标类别！'})
        
        success_count = 0
        for book_id in book_ids:
            book = Book()
            book.book_id = book_id
            book.category_id = category_id
            # 获取其他字段保持不变
            existing_book = book_manager.get_book_by_id(book_id)
            if existing_book:
                book.book_name = existing_book[1]  # BookName
                book.author = existing_book[2]  # Author
                book.publisher = existing_book[3]  # Publisher
                book.isbn = existing_book[4]  # ISBN
                book.total_count = existing_book[6]  # TotalCount
                book.available_count = existing_book[7]  # AvailableCount
                book.price = existing_book[8]  # Price
                if book_manager.update_book(book):
                    success_count += 1
        
        if success_count > 0:
            return jsonify({'success': True, 'message': f'成功修改 {success_count} 本图书的类别！'})
        return jsonify({'success': False, 'message': '修改失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/books/low_stock', methods=['GET'])
def api_get_low_stock_books():
    try:
        # 获取库存不足的图书（可借数量小于3）
        rows = book_manager.get_all_books()
        low_stock_books = [dict(row) for row in rows if row[7] < 3]  # AvailableCount < 3
        return jsonify({'success': True, 'data': low_stock_books})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/borrow/overdue', methods=['GET'])
def api_get_overdue_books():
    try:
        # 获取逾期的借阅记录
        rows = borrow_manager.search_borrow_records(status=2)  # 2表示逾期
        overdue_records = [dict(row) for row in rows]
        return jsonify({'success': True, 'data': overdue_records})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/reader_categories', methods=['GET'])
def api_get_reader_categories():
    try:
        rows = reader_category_manager.get_all_categories()
        data = [dict(row) for row in rows]
        return jsonify({'success': True, 'data': data})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/reader_categories', methods=['POST'])
def api_add_reader_category():
    try:
        data = request.get_json()
        category = ReaderCategory()
        category.category_name = data.get('category_name', '')
        category.max_borrow_count = int(data.get('max_borrow_count', 0))
        category.borrow_days = int(data.get('borrow_days', 0))
        category.late_fee_per_day = float(data.get('late_fee_per_day', 0))
        if reader_category_manager.add_category(category):
            return jsonify({'success': True, 'message': '添加成功！'})
        return jsonify({'success': False, 'message': '添加失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/reader_categories/<int:category_id>', methods=['PUT'])
def api_update_reader_category(category_id):
    try:
        data = request.get_json()
        category = ReaderCategory()
        category.category_id = category_id
        category.category_name = data.get('category_name', '')
        category.max_borrow_count = int(data.get('max_borrow_count', 0))
        category.borrow_days = int(data.get('borrow_days', 0))
        category.late_fee_per_day = float(data.get('late_fee_per_day', 0))
        if reader_category_manager.update_category(category):
            return jsonify({'success': True, 'message': '修改成功！'})
        return jsonify({'success': False, 'message': '修改失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/reader_categories/<int:category_id>', methods=['DELETE'])
def api_delete_reader_category(category_id):
    try:
        if reader_category_manager.delete_category(category_id):
            return jsonify({'success': True, 'message': '删除成功！'})
        return jsonify({'success': False, 'message': '删除失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/readers', methods=['GET'])
def api_get_readers():
    try:
        rows = reader_manager.get_all_readers()
        data = [dict(row) for row in rows]
        return jsonify({'success': True, 'data': data})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/readers', methods=['POST'])
def api_add_reader():
    try:
        data = request.get_json()
        reader = Reader()
        reader.reader_name = data.get('reader_name', '')
        reader.gender = data.get('gender', '')
        reader.reader_category_id = int(data.get('reader_category_id', 0))
        reader.phone = data.get('phone', '')
        reader.email = data.get('email', '')
        reader.address = data.get('address', '')
        reader.status = int(data.get('status', 0))
        reader.remark = data.get('remark', '')
        if reader_manager.add_reader(reader):
            return jsonify({'success': True, 'message': '添加成功！'})
        return jsonify({'success': False, 'message': '添加失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/readers/<int:reader_id>', methods=['PUT'])
def api_update_reader(reader_id):
    try:
        data = request.get_json()
        reader = Reader()
        reader.reader_id = reader_id
        reader.reader_name = data.get('reader_name', '')
        reader.gender = data.get('gender', '')
        reader.reader_category_id = int(data.get('reader_category_id', 0))
        reader.phone = data.get('phone', '')
        reader.email = data.get('email', '')
        reader.address = data.get('address', '')
        reader.status = int(data.get('status', 0))
        reader.remark = data.get('remark', '')
        if reader_manager.update_reader(reader):
            return jsonify({'success': True, 'message': '修改成功！'})
        return jsonify({'success': False, 'message': '修改失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/readers/<int:reader_id>', methods=['DELETE'])
def api_delete_reader(reader_id):
    try:
        if reader_manager.delete_reader(reader_id):
            return jsonify({'success': True, 'message': '删除成功！'})
        return jsonify({'success': False, 'message': '删除失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/readers/<int:reader_id>/password', methods=['PUT'])
def api_change_password(reader_id):
    try:
        data = request.get_json()
        old_password = data.get('old_password', '')
        new_password = data.get('new_password', '')
        
        # 获取读者对应的用户ID
        readers = reader_manager.get_reader_by_id(reader_id)
        if not readers or len(readers) == 0:
            return jsonify({'success': False, 'message': '读者不存在！'})
        
        reader = readers[0]
        user_id = reader.get('UserId', 0) if isinstance(reader, dict) else (reader[1] if len(reader) > 1 else 0)
        if not user_id:
            return jsonify({'success': False, 'message': '读者用户ID不存在！'})
        
        # 修改密码
        if user_manager.change_password(user_id, old_password, new_password):
            return jsonify({'success': True, 'message': '密码修改成功！'})
        return jsonify({'success': False, 'message': '旧密码错误！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/borrow/book', methods=['POST'])
def api_borrow_book():
    try:
        data = request.get_json()
        reader_id = int(data.get('reader_id', 0))
        book_id = int(data.get('book_id', 0))
        borrow_date = data.get('borrow_date', '')
        due_date = data.get('due_date', '')
        if borrow_manager.borrow_book(reader_id, book_id, borrow_date, due_date):
            return jsonify({'success': True, 'message': '借阅成功！'})
        return jsonify({'success': False, 'message': '借阅失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/borrow/records', methods=['GET'])
def api_get_borrow_records():
    try:
        reader_id = request.args.get('reader_id', type=int)
        isbn = request.args.get('isbn', '')
        page_index = request.args.get('page_index', 1, type=int)
        page_size = request.args.get('page_size', 10, type=int)

        # 处理reader_id为0的情况
        if reader_id == 0:
            return jsonify({'success': True, 'data': [], 'total': 0})

        rows, total = borrow_manager.search_all_borrow_records(
            reader_id=reader_id,
            page_index=page_index,
            page_size=page_size
        )
        data = [dict(row) for row in rows]
        return jsonify({'success': True, 'data': data, 'total': total})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/borrow/active', methods=['GET'])
def api_get_active_borrows():
    try:
        reader_id = request.args.get('reader_id', type=int)
        if reader_id:
            rows = borrow_manager.search_borrow_records(reader_id=reader_id)
        else:
            rows = borrow_manager.search_borrow_records()
        data = [dict(row) for row in rows]
        return jsonify({'success': True, 'data': data})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/return/book', methods=['POST'])
def api_return_book():
    try:
        data = request.get_json()
        borrow_id = int(data.get('borrow_id', 0))
        return_date = data.get('return_date', '')
        late_fee = float(data.get('late_fee', 0))
        remark = data.get('remark', '')
        success, message, fee = borrow_manager.return_book(borrow_id, return_date, late_fee, remark)
        if success:
            return jsonify({'success': True, 'message': f'{message}！逾期费用：{fee:.2f}元'})
        return jsonify({'success': False, 'message': message})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


@app.route('/api/calculate_late_fee', methods=['POST'])
def api_calculate_late_fee():
    try:
        data = request.get_json()
        borrow_id = int(data.get('borrow_id', 0))
        return_date = data.get('return_date', '')
        fee = borrow_manager.calculate_late_fee(borrow_id, return_date)
        return jsonify({'success': True, 'data': {'late_fee': fee}})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})


# 全局变量，用于存储借阅申请记录（实际项目中应该使用数据库）
borrow_applications = []
application_id_counter = 1

@app.route('/api/borrow/apply', methods=['POST'])
def api_borrow_apply():
    # 确保请求数据是 JSON 格式
    if not request.is_json:
        return jsonify({'success': False, 'message': '请求数据必须是 JSON 格式！'})
    
    data = request.get_json()
    
    # 安全获取参数
    reader_id = data.get('reader_id', 0)
    books = data.get('books', [])
    borrow_date = data.get('borrow_date', '')
    due_date = data.get('due_date', '')
    
    # 验证参数类型
    try:
        reader_id = int(reader_id)
    except (ValueError, TypeError):
        return jsonify({'success': False, 'message': '读者ID必须是数字！'})
    
    if not isinstance(books, list):
        return jsonify({'success': False, 'message': '图书列表必须是数组！'})
    
    # 验证参数值
    if not reader_id:
        return jsonify({'success': False, 'message': '读者ID不能为空！'})
    if not books or len(books) == 0:
        return jsonify({'success': False, 'message': '请选择要借阅的图书！'})
    if not borrow_date:
        return jsonify({'success': False, 'message': '借阅日期不能为空！'})
    if not due_date:
        return jsonify({'success': False, 'message': '应还日期不能为空！'})
    
    # 检查读者是否存在
    try:
        readers = reader_manager.get_reader_by_id(reader_id)
        if not readers or len(readers) == 0:
            return jsonify({'success': False, 'message': '读者不存在！'})
        
        # 检查读者状态
        reader = readers[0]
        reader_name = reader.get('RealName', '') if isinstance(reader, dict) else (reader[1] if len(reader) > 1 else '')
        status = reader.get('Status', 0) if isinstance(reader, dict) else (reader[9] if len(reader) > 9 else 0)
        if status != 0:
            return jsonify({'success': False, 'message': '读者状态异常，无法借阅！'})
        
        # 检查读者可借数量上限
        max_borrow_count = 10  # 默认值
        
        # 检查当前借阅数量
        current_borrows = borrow_manager.search_borrow_records(reader_id=reader_id, status=0)  # 0表示借出中
        if len(current_borrows) >= max_borrow_count:
            return jsonify({'success': False, 'message': f'已达到借阅上限 {max_borrow_count} 本！'})
        
        # 检查所选图书是否都可借
        for book_id in books:
            try:
                book_id = int(book_id)
            except (ValueError, TypeError):
                return jsonify({'success': False, 'message': f'图书ID {book_id} 必须是数字！'})
            
            book_rows = book_manager.get_book_by_id(book_id)
            if not book_rows or len(book_rows) == 0:
                return jsonify({'success': False, 'message': f'图书ID {book_id} 不存在！'})
            book = book_rows[0]
            available_count = book.get('AvailableCount', 0) if isinstance(book, dict) else (book[7] if len(book) > 7 else 0)
            if available_count <= 0:
                return jsonify({'success': False, 'message': f'图书ID {book_id} 暂无可借数量！'})
        
        # 创建借阅申请记录
        global application_id_counter
        for book_id in books:
            try:
                book_id = int(book_id)
                
                # 从数据库获取最新的图书信息
                book_rows = book_manager.get_book_by_id(book_id)
                if not book_rows or len(book_rows) == 0:
                    continue
                book = book_rows[0]
                book_name = book.get('BookName', '') if isinstance(book, dict) else (book[1] if len(book) > 1 else '')
                isbn = book.get('ISBN', '') if isinstance(book, dict) else (book[4] if len(book) > 4 else '')
                
                # 从数据库获取最新的读者信息
                readers = reader_manager.get_reader_by_id(reader_id)
                if readers and len(readers) > 0:
                    reader = readers[0]
                    reader_name = reader.get('ReaderName', '') if isinstance(reader, dict) else (reader[1] if len(reader) > 1 else '')
                
                # 计算预计借阅天数
                import datetime
                borrow_date_obj = datetime.datetime.strptime(borrow_date, '%Y-%m-%d')
                due_date_obj = datetime.datetime.strptime(due_date, '%Y-%m-%d')
                expected_borrow_days = (due_date_obj - borrow_date_obj).days
                
                # 创建申请记录
                application = {
                    'ApplicationId': application_id_counter,
                    'ReaderId': reader_id,
                    'ReaderName': reader_name,
                    'BookId': book_id,
                    'BookName': book_name,
                    'ISBN': isbn,
                    'ApplicationTime': datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
                    'ExpectedBorrowDays': expected_borrow_days,
                    'Status': 0,  # 0: 待审核
                    'BorrowDate': borrow_date,
                    'DueDate': due_date
                }
                borrow_applications.append(application)
                application_id_counter += 1
            except Exception as ex:
                continue
        
        if len(borrow_applications) > 0:
            return jsonify({'success': True, 'message': f'借阅申请提交成功！等待管理员审核。'})
        return jsonify({'success': False, 'message': '借阅申请提交失败！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': f'处理借阅请求时出错：{str(ex)}'})

@app.route('/api/borrow/applications', methods=['GET'])
def api_get_borrow_applications():
    try:
        # 从数据库获取最新的读者和图书信息，更新申请记录
        updated_applications = []
        for application in borrow_applications:
            # 从数据库获取最新的读者信息
            readers = reader_manager.get_reader_by_id(application['ReaderId'])
            if readers and len(readers) > 0:
                reader = readers[0]
                application['ReaderName'] = reader.get('ReaderName', '') if isinstance(reader, dict) else (reader[1] if len(reader) > 1 else '')
                # 可以添加更多读者信息字段
            
            # 从数据库获取最新的图书信息
            book_rows = book_manager.get_book_by_id(application['BookId'])
            if book_rows and len(book_rows) > 0:
                book = book_rows[0]
                application['BookName'] = book.get('BookName', '') if isinstance(book, dict) else (book[1] if len(book) > 1 else '')
                application['ISBN'] = book.get('ISBN', '') if isinstance(book, dict) else (book[4] if len(book) > 4 else '')
            
            updated_applications.append(application)
        
        return jsonify({'success': True, 'data': updated_applications})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})

@app.route('/api/borrow/applications/<int:application_id>/approve', methods=['POST'])
def api_approve_borrow_application(application_id):
    try:
        global borrow_applications
        for application in borrow_applications:
            if application['ApplicationId'] == application_id:
                # 更新申请状态
                application['Status'] = 1  # 1: 审核通过
                
                # 执行借阅操作
                borrow_manager.borrow_book(
                    application['ReaderId'],
                    application['BookId'],
                    application['BorrowDate'],
                    application['DueDate']
                )
                return jsonify({'success': True, 'message': '审核通过成功！'})
        return jsonify({'success': False, 'message': '申请记录不存在！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})

@app.route('/api/borrow/applications/<int:application_id>/reject', methods=['POST'])
def api_reject_borrow_application(application_id):
    try:
        data = request.get_json()
        reason = data.get('reason', '')
        if not reason:
            return jsonify({'success': False, 'message': '请输入驳回原因！'})
        
        global borrow_applications
        for application in borrow_applications:
            if application['ApplicationId'] == application_id:
                # 更新申请状态
                application['Status'] = 2  # 2: 已驳回
                application['RejectReason'] = reason
                return jsonify({'success': True, 'message': '驳回成功！'})
        return jsonify({'success': False, 'message': '申请记录不存在！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})

@app.route('/api/borrow/applications/reader/<int:reader_id>', methods=['GET'])
def api_get_reader_applications(reader_id):
    try:
        global borrow_applications
        reader_applications = [app for app in borrow_applications if app['ReaderId'] == reader_id]
        return jsonify({'success': True, 'data': reader_applications})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})





if __name__ == '__main__':
    print('Registered routes:')
    for rule in app.url_map.iter_rules():
        print(f'{rule.rule} -> {rule.endpoint}')
    app.run(debug=True, host='0.0.0.0', port=8080)
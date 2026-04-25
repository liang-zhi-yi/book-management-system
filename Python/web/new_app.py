from flask import Flask, jsonify, request
from flask_cors import CORS

app = Flask(__name__)
app.secret_key = 'book_management_secret_key_2024'
app.config['JSON_AS_ASCII'] = False

# 添加CORS支持
CORS(app)

# 测试路由
@app.route('/test', methods=['GET'])
def test():
    return 'Test route works!'

# 借阅申请审核API
@app.route('/borrow/applications', methods=['GET'])
def get_borrow_applications():
    try:
        # 模拟数据，实际应该从数据库获取
        applications = [
            {
                'ApplicationId': 1,
                'ReaderId': 1,
                'ReaderName': '张三',
                'BookId': 1,
                'BookName': 'Python编程从入门到精通',
                'ISBN': '9787115428028',
                'ApplicationTime': '2024-04-20 10:30:00',
                'ExpectedBorrowDays': 30,
                'Status': 0  # 0: 待审核, 1: 审核通过, 2: 已驳回
            },
            {
                'ApplicationId': 2,
                'ReaderId': 2,
                'ReaderName': '李四',
                'BookId': 2,
                'BookName': 'Java核心技术',
                'ISBN': '9787111647400',
                'ApplicationTime': '2024-04-19 14:20:00',
                'ExpectedBorrowDays': 15,
                'Status': 1
            },
            {
                'ApplicationId': 3,
                'ReaderId': 3,
                'ReaderName': '王五',
                'BookId': 3,
                'BookName': 'C++ Primer',
                'ISBN': '9787111429231',
                'ApplicationTime': '2024-04-18 09:15:00',
                'ExpectedBorrowDays': 20,
                'Status': 2
            }
        ]
        return jsonify({'success': True, 'data': applications})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})

@app.route('/borrow/applications/<int:application_id>/approve', methods=['POST'])
def approve_borrow_application(application_id):
    try:
        # 模拟审核通过操作
        # 实际应该更新数据库中的申请状态，并执行借阅操作
        return jsonify({'success': True, 'message': '审核通过成功！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})

@app.route('/borrow/applications/<int:application_id>/reject', methods=['POST'])
def reject_borrow_application(application_id):
    try:
        data = request.get_json()
        reason = data.get('reason', '')
        if not reason:
            return jsonify({'success': False, 'message': '请输入驳回原因！'})
        
        # 模拟驳回操作
        # 实际应该更新数据库中的申请状态，并记录驳回原因
        return jsonify({'success': True, 'message': '驳回成功！'})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})

if __name__ == '__main__':
    # 打印所有注册的路由
    print('Registered routes:')
    for rule in app.url_map.iter_rules():
        print(f'{rule.rule} -> {rule.endpoint}')
    app.run(debug=True, host='0.0.0.0', port=5002)
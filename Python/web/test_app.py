from flask import Flask, jsonify, request

app = Flask(__name__)
app.secret_key = 'book_management_secret_key_2024'
app.config['JSON_AS_ASCII'] = False

# 测试路由
@app.route('/api/test', methods=['GET'])
def api_test():
    return jsonify({'success': True, 'message': 'Test route works!'})

# 借阅申请审核API
@app.route('/api/borrow/applications', methods=['GET'])
def api_get_borrow_applications():
    try:
        # 模拟数据
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
            }
        ]
        return jsonify({'success': True, 'data': applications})
    except Exception as ex:
        return jsonify({'success': False, 'message': str(ex)})

if __name__ == '__main__':
    # 打印所有注册的路由
    print('Registered routes:')
    for rule in app.url_map.iter_rules():
        print(f'{rule.rule} -> {rule.endpoint}')
    app.run(debug=True, host='0.0.0.0', port=5001)
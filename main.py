#!/usr/bin/env python3
import os
import sys

current_dir = os.path.dirname(os.path.abspath(__file__))
python_dir = os.path.join(current_dir, 'Python')
web_dir = os.path.join(python_dir, 'web')

sys.path.insert(0, python_dir)
sys.path.insert(0, web_dir)

os.environ['APP_DIR'] = web_dir

from app import app

if __name__ == '__main__':
    print('Starting Book Management System...')
    print(f'Python path: {python_dir}')
    print(f'Web path: {web_dir}')
    app.run(debug=False, host='0.0.0.0', port=8080)
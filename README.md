# 图书管理系统

## 项目介绍

图书管理系统是一个基于Flask和SQLite的Web应用，用于管理图书馆的图书、读者和借阅记录。系统分为管理端和读者端两个部分，提供了完整的图书管理功能。

## 功能说明

### 管理端功能
- **图书管理**：添加、编辑、删除图书，批量操作，库存管理
- **读者管理**：添加、编辑、删除读者信息，读者类别管理
- **借阅管理**：图书借阅、归还、续借，逾期管理
- **借阅申请审核**：审核读者的借阅申请
- **数据统计**：仪表盘数据展示，借阅趋势分析
- **通知系统**：逾期提醒，库存预警

### 读者端功能
- **图书浏览与搜索**：支持按书名、作者、出版社搜索，支持网格和列表视图切换
- **在线借阅申请**：提交借阅申请，查看申请记录
- **我的借阅记录**：查看借阅历史，筛选记录，展开/收起已还记录
- **个人信息管理**：查看和修改个人信息，修改密码
- **借阅统计**：查看总借阅次数、当前借阅、逾期数量、总逾期费

## 技术栈

- **后端**：Python 3.13, Flask 3.1.3, SQLite
- **前端**：HTML5, CSS3, JavaScript, Font Awesome, Chart.js
- **开发环境**：Windows 10

## 项目结构

```
BookManagement程序/
├── Python/
│   ├── web/
│   │   ├── static/
│   │   │   ├── css/
│   │   │   │   └── style.css
│   │   │   └── js/
│   │   │       ├── app.js        # 管理端JavaScript
│   │   │       └── reader_app.js  # 读者端JavaScript
│   │   ├── templates/
│   │   │   ├── index.html        # 管理端主页面
│   │   │   ├── login.html        # 登录/注册页面
│   │   │   └── reader_index.html # 读者端主页面
│   │   ├── app.py                # 后端主应用
│   │   └── data/
│   │       └── bookmanager.db    # SQLite数据库
│   ├── bll/                      # 业务逻辑层
│   ├── dal/                      # 数据访问层
│   └── models/                   # 数据模型
├── README.md
├── .gitignore
└── LICENSE
```

## 使用方法

### 本地运行

1. **安装依赖**
   ```bash
   cd Python/web
   pip install flask
   ```

2. **启动服务器**
   ```bash
   python app.py
   ```

3. **访问系统**
   - 登录页面：http://127.0.0.1:5000
   - 管理端：使用系统管理员账号登录
   - 读者端：使用读者账号登录

### 登录账号

- **系统管理员**
  - 用户名：admin
  - 密码：123456

- **读者**
  - 无默认用户名及密码，直接注册即可

## 在线访问

该项目已部署到GitHub Pages，可以通过以下地址访问：
- 在线地址：[https://liang-zhi-yi.github.io/book-management-system](https://liang-zhi-yi.github.io/book-management-system)

## 注意事项

1. 首次运行时，系统会自动初始化数据库
2. 数据库文件位于 `Python/web/data/bookmanager.db`
3. 系统使用SQLite数据库，无需额外配置
4. 建议使用现代浏览器访问系统，如Chrome、Firefox、Edge

## 项目特点

- **响应式设计**：适配不同屏幕尺寸
- **用户友好**：简洁美观的界面，操作流程清晰
- **功能完整**：涵盖图书管理的主要功能
- **数据安全**：密码加密存储，用户权限管理
- **易于维护**：模块化设计，代码结构清晰

## 许可证

本项目采用MIT许可证，详见LICENSE文件。

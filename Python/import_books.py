import random
import requests
from dal.db_helper import DBHelper
from models.book import Book
from dal.book_service import BookService

class BookImporter:
    def __init__(self):
        self.db = DBHelper()
        self.book_service = BookService()
    
    def clear_existing_data(self):
        """清除现有的图书相关数据"""
        print("开始清除现有数据...")
        
        # 清除借阅记录
        self.db.execute_non_query("DELETE FROM BorrowRecord")
        print("已清除借阅记录")
        
        # 清除图书数据
        self.db.execute_non_query("DELETE FROM Book")
        print("已清除图书数据")
        
        # 清除图书类别
        self.db.execute_non_query("DELETE FROM BookCategory")
        print("已清除图书类别")
        
        # 重新创建图书类别
        self.db.execute_non_query("INSERT INTO BookCategory (CategoryName, Description) VALUES ('文学类', '文学作品书籍')")
        self.db.execute_non_query("INSERT INTO BookCategory (CategoryName, Description) VALUES ('科技类', '科学技术书籍')")
        self.db.execute_non_query("INSERT INTO BookCategory (CategoryName, Description) VALUES ('历史类', '历史书籍')")
        self.db.execute_non_query("INSERT INTO BookCategory (CategoryName, Description) VALUES ('艺术类', '艺术书籍')")
        self.db.execute_non_query("INSERT INTO BookCategory (CategoryName, Description) VALUES ('教育类', '教育书籍')")
        print("已重新创建图书类别")
    
    def search_books(self, query, limit=10):
        """使用Open Library API搜索图书"""
        url = f"http://openlibrary.org/search.json"
        params = {
            'q': query,
            'limit': limit
        }
        
        try:
            response = requests.get(url, params=params)
            response.raise_for_status()
            data = response.json()
            return data.get('docs', [])
        except Exception as e:
            print(f"搜索图书失败: {e}")
            return []
    
    def import_books(self, total=1000):
        """导入指定数量的图书"""
        print(f"开始导入{total}本图书...")
        
        # 搜索关键词列表
        keywords = ["python", "java", "javascript", "history", "art", "science", "education", "literature", "business", "technology"]
        
        imported = 0
        
        while imported < total:
            for keyword in keywords:
                if imported >= total:
                    break
                
                print(f"搜索关键词: {keyword}")
                books = self.search_books(keyword, limit=50)
                
                for book in books:
                    if imported >= total:
                        break
                    
                    try:
                        # 提取图书信息
                        title = book.get('title', '未知标题')
                        authors = book.get('author_name', [])
                        author = authors[0] if authors else '未知作者'
                        publisher = book.get('publisher', ['未知出版社'])[0] if book.get('publisher') else '未知出版社'
                        isbn = book.get('isbn', [''])[0] if book.get('isbn') else ''
                        
                        # 随机生成类别（1-5）
                        category_id = random.randint(1, 5)
                        
                        # 随机生成数量（1-20）
                        total_count = random.randint(1, 20)
                        available_count = total_count
                        
                        # 随机生成单价（10-100元）
                        price = round(random.uniform(10, 100), 2)
                        
                        # 创建图书对象
                        book_obj = Book()
                        book_obj.book_name = title
                        book_obj.author = author
                        book_obj.publisher = publisher
                        book_obj.isbn = isbn
                        book_obj.category_id = category_id
                        book_obj.total_count = total_count
                        book_obj.available_count = available_count
                        book_obj.price = price
                        
                        # 添加图书
                        result = self.book_service.add_book(book_obj)
                        if result > 0:
                            imported += 1
                            print(f"已导入 {imported}/{total}: {title}")
                    except Exception as e:
                        print(f"导入图书失败: {e}")
                        continue
        
        print(f"图书导入完成，共导入{imported}本图书")

if __name__ == "__main__":
    importer = BookImporter()
    importer.clear_existing_data()
    importer.import_books(total=1000)

using BookManagement.Model;
using BookManagement.DAL;
using System.Data;

namespace BookManagement.BLL
{
    public class BookManager
    {
        
        public DataTable GetBookById(int bookId)
        {
            return bookService.GetBookById(bookId);
        }

        public DataTable GetBookByISBN(string isbn)
        {
            return bookService.GetBookByISBN(isbn);
        }
        private BookService bookService = new BookService();

        public DataTable GetAllBooks()
        {
            return bookService.GetAllBooks();
        }

        public DataTable GetAllCategories()
        {
            return bookService.GetAllCategoriesForComboBox();
            return bookService.GetAllCategories();
        }

        public bool AddBook(Book book)
        {
            if (string.IsNullOrEmpty(book.BookName))
                throw new System.Exception("书名不能为空！");
            if (book.CategoryId <= 0)
                throw new System.Exception("请选择图书类别！");
            if (book.TotalCount <= 0)
                throw new System.Exception("图书总数必须大于0！");

            return bookService.AddBook(book) > 0;
        }

        public bool UpdateBook(Book book)
        {
            if (string.IsNullOrEmpty(book.BookName))
                throw new System.Exception("书名不能为空！");
            if (book.CategoryId <= 0)
                throw new System.Exception("请选择图书类别！");

            return bookService.UpdateBook(book) > 0;
        }

        public bool DeleteBook(int bookId)
        {
            // 实际开发中，这里应检查是否有未归还的借阅记录
            return bookService.DeleteBook(bookId) > 0;
        }
    }
}

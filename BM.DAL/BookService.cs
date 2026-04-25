using BookManagement.DAL.Utility;
using BookManagement.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BookManagement.DAL
{
    public class BookService
    {
        public DataTable SearchAllBorrowRecords(int? readerId, string readerName, string bookName,
    string isbn, int? status, string category,
    DateTime? borrowFrom, DateTime? borrowTo,
    DateTime? returnFrom, DateTime? returnTo,
    int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;

            try
            {
                // 构建基础查询
                string sql = @"
            SELECT 
                br.BorrowId,
                br.BorrowDate,
                br.DueDate,
                br.ReturnDate,
                ISNULL(br.LateFee, 0) as LateFee,
                br.Status,
                ISNULL(br.Remark, '') as Remark,
                CASE 
                    WHEN br.Status = 0 THEN '借出中'
                    WHEN br.Status = 1 THEN '已归还'
                    WHEN br.Status = 2 THEN '逾期'
                    ELSE '未知'
                END as StatusText,
                b.BookName,
                b.ISBN,
                b.BookId,
                r.ReaderName,
                r.ReaderId
            FROM BorrowRecord br
            INNER JOIN Book b ON br.BookId = b.BookId
            INNER JOIN Reader r ON br.ReaderId = r.ReaderId
            WHERE 1=1";

                // 添加查询条件
                if (readerId.HasValue)
                    sql += $" AND r.ReaderId = {readerId.Value}";

                if (!string.IsNullOrEmpty(readerName))
                    sql += $" AND r.ReaderName LIKE '%{readerName}%'";

                if (!string.IsNullOrEmpty(bookName))
                    sql += $" AND b.BookName LIKE '%{bookName}%'";

                if (!string.IsNullOrEmpty(isbn))
                    sql += $" AND b.ISBN LIKE '%{isbn}%'";

                if (status.HasValue)
                    sql += $" AND br.Status = {status.Value}";

                if (!string.IsNullOrEmpty(category) && category != "全部")
                    sql += $" AND b.CategoryId IN (SELECT CategoryId FROM BookCategory WHERE CategoryName = '{category}')";

                if (borrowFrom.HasValue)
                    sql += $" AND br.BorrowDate >= '{borrowFrom.Value:yyyy-MM-dd}'";

                if (borrowTo.HasValue)
                    sql += $" AND br.BorrowDate <= '{borrowTo.Value:yyyy-MM-dd}'";

                if (returnFrom.HasValue)
                    sql += $" AND br.ReturnDate >= '{returnFrom.Value:yyyy-MM-dd}'";

                if (returnTo.HasValue)
                    sql += $" AND br.ReturnDate <= '{returnTo.Value:yyyy-MM-dd}'";

                // 获取总记录数
                string countSql = "SELECT COUNT(*) FROM (" + sql + ") as t";
                object countResult = SQLHelper.ExecuteScalar(countSql);
                totalRecords = countResult != null ? Convert.ToInt32(countResult) : 0;

                // 添加排序和分页
                sql += " ORDER BY br.BorrowDate DESC";

                if (pageSize > 0 && pageIndex > 0)
                {
                    int offset = (pageIndex - 1) * pageSize;
                    sql += $" OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
                }

                DataSet ds = SQLHelper.ExecuteDataSet(sql);
                return ds?.Tables?[0];
            }
            catch (Exception ex)
            {
                throw new Exception($"查询借阅记录失败：{ex.Message}");
            }
        }


        public DataTable GetBookByISBN(string isbn)
        {
            string sql = $"SELECT * FROM Book WHERE ISBN = '{isbn}'";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }

        /// <summary>
        /// 获取所有图书信息（联表查询，获取类别名称）
        /// </summary>
        public DataTable GetAllBooks()
        {
            string sql = @"
                SELECT b.BookId, b.BookName, b.Author, b.Publisher, b.ISBN, 
                       b.CategoryId, c.CategoryName, 
                       b.TotalCount, b.AvailableCount, b.Price
                FROM Book b
                LEFT JOIN BookCategory c ON b.CategoryId = c.CategoryId
                ORDER BY b.BookId";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 新增图书
        /// </summary>
        public int AddBook(Book book)
        {
            string sql = $@"
                INSERT INTO Book (BookName, Author, Publisher, ISBN, CategoryId, TotalCount, AvailableCount, Price) 
                VALUES ('{book.BookName}', '{book.Author}', '{book.Publisher}', '{book.ISBN}', 
                        {book.CategoryId}, {book.TotalCount}, {book.AvailableCount}, {book.Price})";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改图书信息
        /// </summary>
        public DataTable GetBookById(int bookId)
        {
            string sql = $"SELECT * FROM Book WHERE BookId = {bookId}";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }
        public int UpdateBook(Book book)
        {
            string sql = $@"
                UPDATE Book 
                SET BookName='{book.BookName}', Author='{book.Author}', Publisher='{book.Publisher}', 
                    ISBN='{book.ISBN}', CategoryId={book.CategoryId}, 
                    TotalCount={book.TotalCount}, AvailableCount={book.AvailableCount}, Price={book.Price}
                WHERE BookId={book.BookId}";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除图书
        /// </summary>
        public int DeleteBook(int bookId)
        {
            // 注意：实际项目中，删除前应检查是否有未归还的借阅记录
            string sql = $"DELETE FROM Book WHERE BookId={bookId}";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取所有图书类别（用于下拉框）
        /// </summary>
        public DataTable GetAllCategoriesForComboBox()
        {
            string sql = "SELECT CategoryId, CategoryName FROM BookCategory ORDER BY CategoryId";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }/// 获取所有图书类别
         /// </summary>
        public DataTable GetAllCategories()
        {
            try
            {
                string sql = "SELECT CategoryId, CategoryName FROM BookCategory ORDER BY CategoryName";
                DataSet ds = SQLHelper.ExecuteDataSet(sql);
                return ds?.Tables?[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取图书类别失败：{ex.Message}", "错误");
                return null;
            }
        }
    }
}

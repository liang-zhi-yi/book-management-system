using System;
using System.Data;
using System.Data.SqlClient;
using BookManagement.DAL.Utility;
using BookManagement.Model;
namespace BookManagement.DAL
{
    public class BorrowService
    {
        /// <summary>
        /// 借书操作（需要事务处理）
        /// </summary>
        public bool BorrowBook(int readerId, int bookId, DateTime borrowDate, DateTime dueDate)
        {
            // 注意：这是一个需要事务的操作，涉及多个表的更新
            using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. 检查读者状态
                    string checkReaderSql = $"SELECT Status FROM Reader WHERE ReaderId = {readerId}";
                    SqlCommand cmdCheckReader = new SqlCommand(checkReaderSql, conn, transaction);
                    object readerStatus = cmdCheckReader.ExecuteScalar();
                    if (readerStatus == null || Convert.ToInt32(readerStatus) != 0) // 0:正常
                    {
                        transaction.Rollback();
                        return false;
                    }
                    string checkBookSql = $"SELECT AvailableCount FROM Book WHERE BookId = {bookId}";
                    SqlCommand cmdCheckBook = new SqlCommand(checkBookSql, conn, transaction);
                    object availableCount = cmdCheckBook.ExecuteScalar();
                    if (availableCount == null || Convert.ToInt32(availableCount) <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    // 3. 插入借阅记录
                    string insertBorrowSql = $@"
                        INSERT INTO BorrowRecord (ReaderId, BookId, BorrowDate, DueDate, Status) 
                        VALUES ({readerId}, {bookId}, '{borrowDate:yyyy-MM-dd}', '{dueDate:yyyy-MM-dd}', 0)";
                    SqlCommand cmdInsertBorrow = new SqlCommand(insertBorrowSql, conn, transaction);
                    int rows = cmdInsertBorrow.ExecuteNonQuery();
                    if (rows <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    string updateBookSql = $"UPDATE Book SET AvailableCount = AvailableCount - 1 WHERE BookId = {bookId}";
                    SqlCommand cmdUpdateBook = new SqlCommand(updateBookSql, conn, transaction);
                    rows = cmdUpdateBook.ExecuteNonQuery();
                    if (rows <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public DataTable GetReaderBorrowInfo(int readerId)
        {
            string sql = $@"
                SELECT 
                    br.BorrowId,  -- 注意：使用别名
                    br.BorrowDate, 
                    br.DueDate, 
                    b.BookName, 
                    b.ISBN,
                    r.ReaderName,
                    r.ReaderCategoryId
                FROM BorrowRecord br
                INNER JOIN Book b ON br.BookId = b.BookId
                INNER JOIN Reader r ON br.ReaderId = r.ReaderId
                WHERE br.ReaderId = {readerId} AND br.Status IN (0, 2)  -- 0:借出中, 2:逾期
                ORDER BY br.DueDate";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }
        public DataTable SearchBorrowRecords(int? readerId, string isbn, int? borrowId)
        {
            string sql = @"
                SELECT 
                    br.BorrowId,  -- 注意：使用别名
                    br.BorrowDate,
                    br.DueDate,
                    br.ReturnDate,
                    br.Status,
                    b.BookName,
                    b.ISBN,
                    r.ReaderName,
                    r.ReaderId
                FROM BorrowRecord br
                INNER JOIN Book b ON br.BookId = b.BookId
                INNER JOIN Reader r ON br.ReaderId = r.ReaderId
                WHERE br.Status IN (0, 2)";  // 0:借出中, 2:逾期

            if (readerId.HasValue)
                sql += $" AND r.ReaderId = {readerId.Value}";

            if (!string.IsNullOrEmpty(isbn))
                sql += $" AND b.ISBN LIKE '%{isbn}%'";

            if (borrowId.HasValue)
                sql += $" AND br.BorrowId = {borrowId.Value}";

            sql += " ORDER BY br.DueDate";

            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }
        public bool ReturnBook(int borrowId, DateTime returnDate, decimal lateFee, string remark)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. 获取图书ID
                    string getBookSql = $"SELECT BookId FROM BorrowRecord WHERE BorrowId= {borrowId}";
                    SqlCommand cmdGetBook = new SqlCommand(getBookSql, conn, transaction);
                    object bookIdObj = cmdGetBook.ExecuteScalar();

                    if (bookIdObj == null)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    int bookId = Convert.ToInt32(bookIdObj); string updateBorrowSql = $@"
                        UPDATE BorrowRecord 
                        SET ReturnDate = '{returnDate:yyyy-MM-dd}', 
                            LateFee = {lateFee}, 
                            Status = 1,  -- 1:已归还
                            Remark = '{remark}'
                        WHERE BorrowId = {borrowId}";

                    SqlCommand cmdUpdateBorrow = new SqlCommand(updateBorrowSql, conn, transaction);
                    int rows = cmdUpdateBorrow.ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    // 3. 更新图书库存
                    string updateBookSql = $"UPDATE Book SET AvailableCount = AvailableCount + 1 WHERE BookId = {bookId}";
                    SqlCommand cmdUpdateBook = new SqlCommand(updateBookSql, conn, transaction);
                    rows = cmdUpdateBook.ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public DataTable GetBorrowRecordById(int borrowId)
        {
            string sql = $@"
                SELECT 
                    br.*,
                    b.BookName,
                    b.ISBN,
                    r.ReaderName,
                    r.ReaderCategoryId
                FROM BorrowRecord br
                INNER JOIN Book b ON br.BookId = b.BookId
                INNER JOIN Reader r ON br.ReaderId = r.ReaderId
                WHERE br.BorrowId = {borrowId}";

            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }/// <summary>
         /// 多条件查询所有借阅记录（支持分页）
         /// </summary>
        public DataTable SearchAllBorrowRecords(int? readerId, string readerName, string bookName,
            string isbn, int? status,string category, DateTime? borrowFrom, DateTime? borrowTo,
            DateTime? returnFrom, DateTime? returnTo, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;

            // 构建基础查询
            string sql = @"
        SELECT 
            br.BorrowId,
            br.BorrowDate,
            br.DueDate,
            br.ReturnDate,
            br.Status,
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

            if (pageSize > 0)
            {
                int offset = (pageIndex - 1) * pageSize;
                sql += $" OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }

            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }

    }
}
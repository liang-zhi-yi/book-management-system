using System;
using System.Data;
using BookManagement.DAL;
using BookManagement.Model;

namespace BookManagement.BLL
{
    public class BorrowManager
    {
        private BorrowService borrowService = new BorrowService();
        private ReaderService readerService = new ReaderService();
        private BookService bookService = new BookService();

        /// <summary>
        /// 验证借阅条件
        /// </summary>
        public (bool canBorrow, string message) ValidateBorrow(int readerId, int bookId)
        {
            try
            {
                // 1. 检查读者是否存在
                DataTable readerDt = readerService.GetReaderById(readerId);
                if (readerDt == null || readerDt.Rows.Count == 0)
                    return (false, "读者不存在！");

                // 2. 检查读者状态
                int readerStatus = Convert.ToInt32(readerDt.Rows[0]["Status"]);
                if (readerStatus != 0) // 0:正常
                    return (false, "读者状态异常，无法借书！");

                // 3. 检查图书是否存在
                DataTable bookDt = bookService.GetBookById(bookId);
                if (bookDt == null || bookDt.Rows.Count == 0)
                    return (false, "图书不存在！");

                // 4. 检查图书库存
                int availableCount = Convert.ToInt32(bookDt.Rows[0]["AvailableCount"]);
                if (availableCount <= 0)
                    return (false, "该图书已全部借出！");

                // 5. 检查读者借阅上限
                int categoryId = Convert.ToInt32(readerDt.Rows[0]["ReaderCategoryId"]);
                DataTable categoryDt = readerService.GetReaderCategoryById(categoryId);
                if (categoryDt != null && categoryDt.Rows.Count > 0)
                {
                    int maxBorrow = Convert.ToInt32(categoryDt.Rows[0]["MaxBorrowCount"]);

                    // 获取读者当前借阅数量
                    DataTable borrowDt = borrowService.GetReaderBorrowInfo(readerId);
                    int currentBorrowCount = borrowDt?.Rows.Count ?? 0;

                    if (currentBorrowCount >= maxBorrow)
                        return (false, $"已达到最大借阅数量（{maxBorrow}本）！");
                }

                return (true, "可以借阅");
            }
            catch (Exception ex)
            {
                return (false, $"验证失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 执行借书操作
        /// </summary>
        public bool BorrowBook(int readerId, int bookId, DateTime borrowDate, DateTime dueDate)
        {
            // 先验证
            var validation = ValidateBorrow(readerId, bookId);
            if (!validation.canBorrow)
                throw new Exception(validation.message);

            // 执行借书
            return borrowService.BorrowBook(readerId, bookId, borrowDate, dueDate);
        }

        /// <summary>
        /// 获取读者借阅信息
        /// </summary>
        public DataTable GetReaderBorrowInfo(int readerId)
        {
            return borrowService.GetReaderBorrowInfo(readerId);
        }
        /// <summary>
        /// 查询借阅记录
        /// </summary>
        public DataTable SearchBorrowRecords(int? readerId, string isbn, int? borrowId)
        {
            return borrowService.SearchBorrowRecords(readerId, isbn, borrowId);
        }
        /// <summary>
        /// 多条件查询所有借阅记录（支持分页）
        /// </summary>
        /// <summary>
        /// 多条件查询所有借阅记录(支持分页)
        /// </summary>
        public DataTable SearchAllBorrowRecords(int? readerId, string readerName, string bookName,
            string isbn, int? status, string category,
            DateTime? borrowFrom, DateTime? borrowTo,
            DateTime? returnFrom, DateTime? returnTo,
            int pageIndex, int pageSize, out int totalRecords)
        {
            return borrowService.SearchAllBorrowRecords(
                readerId, readerName, bookName, isbn,
                status, category, borrowFrom, borrowTo,
                returnFrom, returnTo, pageIndex, pageSize,
                out totalRecords
            );
        }


        /// <summary>
        /// 还书操作
        /// </summary>
        public decimal CalculateLateFee(int borrowId, DateTime actualReturnDate)
        {
            try
            {
                // 获取借阅记录详情
                DataTable borrowDt = borrowService.GetBorrowRecordById(borrowId);
                if (borrowDt == null || borrowDt.Rows.Count == 0)
                    throw new Exception("借阅记录不存在");

                // 获取应还日期
                DateTime dueDate = Convert.ToDateTime(borrowDt.Rows[0]["DueDate"]);

                // 计算逾期天数
                int overdueDays = 0;
                if (actualReturnDate > dueDate)
                {
                    overdueDays = (actualReturnDate - dueDate).Days;
                }

                // 获取读者类别ID
                int readerCategoryId = Convert.ToInt32(borrowDt.Rows[0]["ReaderCategoryId"]);

                // 获取读者类别的逾期费率
                DataTable categoryDt = readerService.GetReaderCategoryById(readerCategoryId);
                if (categoryDt == null || categoryDt.Rows.Count == 0)
                    throw new Exception("读者类别不存在");

                decimal lateFeePerDay = Convert.ToDecimal(categoryDt.Rows[0]["LateFeePerDay"]);

                return overdueDays * lateFeePerDay;
            }
            catch (Exception ex)
            {
                throw new Exception($"计算逾期费用失败：{ex.Message}");
            }
        }

        public (bool success, string message, decimal lateFee) ReturnBook(int borrowId, DateTime returnDate, decimal lateFee, string remark)
        {
            try
            {
                // 计算逾期费用
                decimal calculatedLateFee = CalculateLateFee(borrowId, returnDate);

                // 如果传入的逾期费用为0，使用计算值
                if (lateFee == 0 && calculatedLateFee > 0)
                {
                    lateFee = calculatedLateFee;
                }

                // 执行还书
                bool result = borrowService.ReturnBook(borrowId, returnDate, lateFee, remark);

                if (result)
                {
                    return (true, "还书成功", lateFee);
                }
                else
                {
                    return (false, "还书失败", 0);
                }
            }
            catch (Exception ex)
            {
                return (false, $"还书失败：{ex.Message}", 0);
            }
        }
        public DataTable GetBorrowRecordById(int borrowId)
        {
            return borrowService.GetBorrowRecordById(borrowId);
        }

    }
}

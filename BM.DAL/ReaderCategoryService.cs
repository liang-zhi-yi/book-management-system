using BookManagement.DAL.Utility;
using BookManagement.Model;
using System.Data;

namespace BookManagement.DAL
{
    public class ReaderCategoryService
    {
        public DataTable GetAllCategories()
        {
            // 注意：这里假设列名是 CategoryId, CategoryName, MaxBorrowCount, BorrowDays, LateFeePerDay
            string sql = "SELECT CategoryId, CategoryName, MaxBorrowCount, BorrowDays, LateFeePerDay FROM ReaderCategory ORDER BY CategoryId";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }

        public int AddCategory(ReaderCategory category)
        {
            string sql = $@"
                INSERT INTO ReaderCategory (CategoryName, MaxBorrowCount, BorrowDays, LateFeePerDay) 
                VALUES ('{category.CategoryName}', {category.MaxBorrowCount}, {category.BorrowDays}, {category.LateFeePerDay})";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        public int UpdateCategory(ReaderCategory category)
        {
            string sql = $@"
                UPDATE ReaderCategory 
                SET CategoryName='{category.CategoryName}', 
                    MaxBorrowCount={category.MaxBorrowCount}, 
                    BorrowDays={category.BorrowDays}, 
                    LateFeePerDay={category.LateFeePerDay}
                WHERE CategoryId={category.CategoryId}";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        public int DeleteCategory(int categoryId)
        {
            string sql = $"DELETE FROM ReaderCategory WHERE CategoryId={categoryId}";
            return SQLHelper.ExecuteNonQuery(sql);
        }
    }
}

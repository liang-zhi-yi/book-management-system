using System.Data;
using System.Data.SqlClient;
using BookManagement.Model;
using BookManagement.DAL.Utility;

namespace BookManagement.DAL
{
    public class BookCategoryService
    {
        /// <summary>
        /// 获取所有图书类别
        /// </summary>
        public DataTable GetAllCategories()
        {
            string sql = "SELECT CategoryId, CategoryName, Description FROM BookCategory ORDER BY CategoryId";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 添加图书类别
        /// </summary>
        public int AddCategory(BookCategory category)
        {
            string sql = $"INSERT INTO BookCategory (CategoryName, Description) VALUES ('{category.CategoryName}', '{category.Description}')";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新图书类别
        /// </summary>
        public int UpdateCategory(BookCategory category)
        {
            string sql = $"UPDATE BookCategory SET CategoryName='{category.CategoryName}', Description='{category.Description}' WHERE CategoryId={category.CategoryId}";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除图书类别
        /// </summary>
        public int DeleteCategory(int categoryId)
        {
            // 注意：在实际项目中，删除前应检查该类别下是否有图书
            string sql = $"DELETE FROM BookCategory WHERE CategoryId={categoryId}";
            return SQLHelper.ExecuteNonQuery(sql);
        }
    }
}

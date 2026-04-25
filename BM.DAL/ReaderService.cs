using System.Data;
using System.Data.SqlClient;
using BookManagement.Model;
using BookManagement.DAL.Utility;

namespace BookManagement.DAL
{
    public class ReaderService
    {
        /// <summary>
        /// 获取所有读者信息（联表查询获取读者类别名称）
        /// </summary>
        public DataTable GetReaderById(int readerId)
        {
            string sql = $"SELECT * FROM Reader WHERE ReaderId = {readerId}";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }

        public DataTable GetReaderCategoryById(int categoryId)
        {
            string sql = $"SELECT * FROM ReaderCategory WHERE CategoryId = {categoryId}";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }

        

       

        public DataTable GetAllReaders()
        {
            string sql = @"
                SELECT r.*, rc.CategoryName as ReaderCategoryName
                FROM Reader r
                LEFT JOIN ReaderCategory rc ON r.ReaderCategoryId = rc.CategoryId
                ORDER BY r.ReaderId";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }

        /// <summary>
        /// 获取所有读者类别（用于下拉框）
        /// </summary>
        public DataTable GetAllReaderCategoriesForComboBox()
        {
            string sql = "SELECT CategoryId, CategoryName FROM ReaderCategory ORDER BY CategoryId";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            return ds?.Tables?[0];
        }

        /// <summary>
        /// 新增读者
        /// </summary>
        public int AddReader(Reader reader)
        {
            string sql = $@"
                INSERT INTO Reader (ReaderName, Gender, ReaderCategoryId, Phone, Email, Address, RegistrationDate, ExpiryDate, Status, Remark) 
                VALUES ('{reader.ReaderName}', '{reader.Gender}', {reader.ReaderCategoryId}, 
                        '{reader.Phone}', '{reader.Email}', '{reader.Address}', 
                        '{reader.RegistrationDate:yyyy-MM-dd}', '{reader.ExpiryDate:yyyy-MM-dd}', 
                        {reader.Status}, '{reader.Remark}')";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改读者信息
        /// </summary>
        public int UpdateReader(Reader reader)
        {
            string sql = $@"
                UPDATE Reader 
                SET ReaderName='{reader.ReaderName}', Gender='{reader.Gender}', 
                    ReaderCategoryId={reader.ReaderCategoryId}, Phone='{reader.Phone}', 
                    Email='{reader.Email}', Address='{reader.Address}', 
                    RegistrationDate='{reader.RegistrationDate:yyyy-MM-dd}', 
                    ExpiryDate='{reader.ExpiryDate:yyyy-MM-dd}', 
                    Status={reader.Status}, Remark='{reader.Remark}'
                WHERE ReaderId={reader.ReaderId}";
            return SQLHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除读者
        /// </summary>
        public int DeleteReader(int readerId)
        {
            // 注意：实际项目中，删除前应检查是否有未归还的图书
            string sql = $"DELETE FROM Reader WHERE ReaderId={readerId}";
            return SQLHelper.ExecuteNonQuery(sql);
        }
    }
}

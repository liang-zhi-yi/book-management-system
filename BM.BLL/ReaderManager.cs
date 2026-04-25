using System.Data;
using BookManagement.Model;
using BookManagement.DAL;

namespace BookManagement.BLL
{
    public class ReaderManager
    {
        private ReaderService readerService = new ReaderService();
        public DataTable GetReaderById(int readerId)
        {
            return readerService.GetReaderById(readerId);
        }

        public DataTable GetReaderCategoryById(int categoryId)
        {
            return readerService.GetReaderCategoryById(categoryId);
        }

        public DataTable GetAllReaders()
        {
            return readerService.GetAllReaders();
        }

        public DataTable GetAllReaderCategories()
        {
            return readerService.GetAllReaderCategoriesForComboBox();
        }

        public bool AddReader(Reader reader)
        {
            // 业务规则验证
            if (string.IsNullOrEmpty(reader.ReaderName))
                throw new System.Exception("读者姓名不能为空！");
            if (reader.ReaderCategoryId <= 0)
                throw new System.Exception("请选择读者类别！");
            if (string.IsNullOrEmpty(reader.Phone))
                throw new System.Exception("联系电话不能为空！");
            if (reader.ExpiryDate <= reader.RegistrationDate)
                throw new System.Exception("有效期必须晚于注册日期！");

            return readerService.AddReader(reader) > 0;
        }

        public bool UpdateReader(Reader reader)
        {
            if (string.IsNullOrEmpty(reader.ReaderName))
                throw new System.Exception("读者姓名不能为空！");
            if (reader.ReaderCategoryId <= 0)
                throw new System.Exception("请选择读者类别！");
            if (string.IsNullOrEmpty(reader.Phone))
                throw new System.Exception("联系电话不能为空！");

            return readerService.UpdateReader(reader) > 0;
        }

        public bool DeleteReader(int readerId)
        {
            return readerService.DeleteReader(readerId) > 0;
        }
    }
}

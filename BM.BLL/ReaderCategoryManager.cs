using System.Data;
using BookManagement.Model;
using BookManagement.DAL;

namespace BookManagement.BLL
{
    public class ReaderCategoryManager
    {
        private ReaderCategoryService categoryService = new ReaderCategoryService();

        public DataTable GetAllCategories()
        {
            return categoryService.GetAllCategories();
        }

        public bool AddCategory(ReaderCategory category)
        {
            // 业务规则验证
            if (string.IsNullOrEmpty(category.CategoryName))
                throw new System.Exception("类别名称不能为空！");
            if (category.MaxBorrowCount <= 0)
                throw new System.Exception("最大借阅数量必须大于0！");
            if (category.BorrowDays <= 0)
                throw new System.Exception("借阅天数必须大于0！");
            if (category.LateFeePerDay < 0)
                throw new System.Exception("逾期费用不能为负数！");

            return categoryService.AddCategory(category) > 0;
        }

        public bool UpdateCategory(ReaderCategory category)
        {
            // 同样验证
            if (string.IsNullOrEmpty(category.CategoryName))
                throw new System.Exception("类别名称不能为空！");
            // ... 其他验证
            return categoryService.UpdateCategory(category) > 0;
        }

        public bool DeleteCategory(int categoryId)
        {
            return categoryService.DeleteCategory(categoryId) > 0;
        }
    }
}

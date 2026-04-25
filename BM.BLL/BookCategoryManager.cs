using BookManagement.Model;
using BookManagement.DAL;

namespace BookManagement.BLL
{
    public class BookCategoryManager
    {
        private BookCategoryService categoryService = new BookCategoryService();

        public System.Data.DataTable GetAllCategories()
        {
            return categoryService.GetAllCategories();
        }

        public bool AddCategory(BookCategory category)
        {
            // 业务规则验证
            if (string.IsNullOrEmpty(category.CategoryName))
            {
                throw new System.Exception("类别名称不能为空！");
            }
            return categoryService.AddCategory(category) > 0;
        }

        public bool UpdateCategory(BookCategory category)
        {
            if (string.IsNullOrEmpty(category.CategoryName))
            {
                throw new System.Exception("类别名称不能为空！");
            }
            return categoryService.UpdateCategory(category) > 0;
        }

        public bool DeleteCategory(int categoryId)
        {
            return categoryService.DeleteCategory(categoryId) > 0;
        }
    }
}

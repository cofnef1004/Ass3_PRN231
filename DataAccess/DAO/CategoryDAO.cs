using BusinessObjects.Models;

namespace DataAccess
{
    public class CategoryDAO
    {
        public static List<Category> GetCategories()
        {
            var listCategories = new List<Category>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listCategories = context.Categories.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listCategories;
        }

        public static Category FindCategoryById(int categoryID)
        {
            var category = new Category();
            try
            {
                using (var context = new MyDbContext())
                {
                    category = context.Categories.SingleOrDefault(c => c.CategoryId == categoryID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;
        }

        public static void SaveCategory(Category category)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Categories.Add(category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateCategory(Category category)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry(category).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteCategory(Category category)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var categoryToDelete = context
                        .Categories
                        .SingleOrDefault(c => c.CategoryId == category.CategoryId);
                    context.Categories.Remove(categoryToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
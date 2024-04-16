using LibraryManagement.Core.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddCategoryAsync(Category newCategory)
        {
            try
            {
                string upperCategoryName = newCategory.CategoryName.ToUpper();
                Category categoryExist = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName.ToUpper().Equals(upperCategoryName));
                if (categoryExist != null) return false;
                await _dbContext.AddAsync(newCategory);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _dbContext.Categories.OrderBy(c => c.CategoryName).AsNoTracking().ToListAsync();
            return categories;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string searchString)
        {
            var categories = await _dbContext.Categories
                .Where(c => c.CategoryName.Contains(searchString))
                .OrderBy(c => c.CategoryName)
                .AsNoTracking()
                .ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(string categoryId)
        {
            Category category = await _dbContext.Categories.FindAsync(categoryId);
            return category;
        }

        public async Task<bool> RemoveCategoryAsync(Category category)
        {
            try
            {
                category?.Books?.Clear();
                _dbContext.Remove(category);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> UpdateCategoryAsync(Category categoryExist, string newCategoryName)
        {
            try
            {
                categoryExist.CategoryName = newCategoryName;
                _dbContext.Update(categoryExist);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckDuplicateCategoryAsync(string currentCategoryId, string newCategoryName)
        {
            var checkNameExist = await _dbContext.Categories.AnyAsync(c =>
                    c.CategoryName.ToUpper().Equals(newCategoryName.ToUpper()) &&
                    !c.CategoryId.Equals(currentCategoryId));
            if (checkNameExist) return false;
            return true;
        }
    }
}

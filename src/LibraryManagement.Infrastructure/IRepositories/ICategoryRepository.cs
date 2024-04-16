using LibraryManagement.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace LibraryManagement.Infrastructure.IRepositories
{
    public interface ICategoryRepository
    {
      
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task<IEnumerable<Category>> GetAllCategoriesAsync(string searchString);
        public Task<Category> GetCategoryByIdAsync(string categoryId);
        public Task<bool> AddCategoryAsync(Category newCategory);
        public Task<bool> UpdateCategoryAsync(Category categoryExist, string newCategoryName);
        public Task<bool> RemoveCategoryAsync(Category category);
        public Task<bool> CheckDuplicateCategoryAsync(string currentCategoryId, string newCategoryName);


    }
}

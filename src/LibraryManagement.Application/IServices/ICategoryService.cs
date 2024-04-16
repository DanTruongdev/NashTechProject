using LibraryManagement.Application.Dtos;
using LibraryManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
namespace LibraryManagement.Application.IServices
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task<IEnumerable<Category>> GetAllCategoriesAsync(string searchString);
        public Task<Category> GetCategoryByIdAsync(string categoryId);
        public Task<Result> AddCategoryAsync(CategoryDto form);
        public Task<Result> UpdateCategoryAsync(Category category);
        public Task<Result> RemoveCategoryAsync(string categoryId);
    }
}

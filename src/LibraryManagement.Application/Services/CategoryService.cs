using LibraryManagement.Application.Dtos;
using LibraryManagement.Application.IServices;
using LibraryManagement.Core.Entities;
using LibraryManagement.Infrastructure.Extentions;
using LibraryManagement.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;
using LibraryManagement.Infrastructure.Constants;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ILogger<CategoryService> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string searchString)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(searchString);
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(string categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            return category;
        }

        public async Task<Result> RemoveCategoryAsync(string categoryId)
        {
            Category category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null) return new Result(false, "The category does not exist");
            var result = await _categoryRepository.RemoveCategoryAsync(category);
            return new Result("The category is removed");
        }

        public async Task<Result>  AddCategoryAsync(CategoryDto form)
        {
            Category newCategory = new Category()
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryName = form.CategoryName
            };
            var result = await _categoryRepository.AddCategoryAsync(newCategory);
            if (!result) return new Result(false, "The category name already exist!");
            return new Result("The category is added");
        }

        public async Task<Result>  UpdateCategoryAsync(Category category)
        {
            Category categoryExist = await _categoryRepository.GetCategoryByIdAsync(category.CategoryId);
            if (categoryExist == null) return new Result(false, "The category does not exist");
            var checkDuplicateResult = await _categoryRepository.CheckDuplicateCategoryAsync(categoryExist.CategoryId, category.CategoryName);
            if (!checkDuplicateResult) return new Result(false, "The category name already exist");
            var result = await _categoryRepository.UpdateCategoryAsync(categoryExist, category.CategoryName);
            if (!result) return new Result(false, "Failed to update the category name");
            return new Result("Update category successfully");
        }
    }
}

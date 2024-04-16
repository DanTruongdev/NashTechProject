using LibraryManagement.Application.Dtos;
using LibraryManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
namespace LibraryManagement.Application.IServices
{
    public interface IAuthorService
    {
        public Task<IEnumerable<Author>> GetAllAuthorsAsync();
        public Task<Author> GetAuthorByIdAsync(string categoryId);
        public Task<IEnumerable<Author>> GetAllAuthorsAsync(string searchString);
        public Task<Result> AddAuthorAsync(AuthorDto form);
        public Task<Result> UpdateAuthorAsync(Author category);
        public Task<Result> RemoveAuthorAsync(string categoryId);
    }
}

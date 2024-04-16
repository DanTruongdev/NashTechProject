using LibraryManagement.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace LibraryManagement.Infrastructure.IRepositories
{
    public interface IAuthorRepository
    {
      
        public Task<IEnumerable<Author>> GetAllAuthorsAsync();
        public Task<IEnumerable<Author>> GetAllAuthorsAsync(string searchString);
        public Task<Author> GetAuthorByIdAsync(string authorId);
        public Task<bool> AddAuthorAsync(Author newAuthor);
        public Task<bool> UpdateAuthorAsync(Author authorExist, string newAuthorName);
        public Task<bool> RemoveAuthorAsync(Author author);
        public Task<bool> CheckDuplicateAuthorAsync(string currentAuthorId, string newAuthorName);


    }
}

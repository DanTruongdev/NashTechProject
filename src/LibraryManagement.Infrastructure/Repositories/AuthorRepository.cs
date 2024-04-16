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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddAuthorAsync(Author newAuthor)
        {
            try
            {
                string upperAuthorName = newAuthor.AuthorName.ToUpper();
                Author authorExist = await _dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorName.ToUpper().Equals(upperAuthorName));
                if (authorExist != null) return false;
                await _dbContext.AddAsync(newAuthor);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            var authors = await _dbContext.Authors.OrderBy(a => a.AuthorName).AsNoTracking().ToListAsync();
            return authors;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(string searchString)
        {
            var authors = await _dbContext.Authors
                .Where(a => a.AuthorName.Contains(searchString))
                .OrderBy(a => a.AuthorName)
                .AsNoTracking()
                .ToListAsync();
            return authors;
        }

        public async Task<Author> GetAuthorByIdAsync(string authorId)
        {
            Author author = await _dbContext.Authors.FindAsync(authorId);
            return author;
        }

        public async Task<bool> RemoveAuthorAsync(Author author)
        {
            try
            {
                author?.AuthorBooks?.Clear();
                _dbContext.Remove(author);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> UpdateAuthorAsync(Author authorExist, string newAuthorName)
        {
            try
            {
                authorExist.AuthorName = newAuthorName;
                _dbContext.Update(authorExist);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckDuplicateAuthorAsync(string currentAuthorId, string newAuthorName)
        {
            var checkNameExist = await _dbContext.Authors.AnyAsync(a =>
                    a.AuthorName.ToUpper().Equals(newAuthorName.ToUpper()) &&
                    !a.AuthorId.Equals(currentAuthorId));
            if (checkNameExist) return false;
            return true;
        }
    }
}

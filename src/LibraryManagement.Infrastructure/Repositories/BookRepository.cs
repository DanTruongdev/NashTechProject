using LibraryManagement.Core.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Book>> GetAllBookAsync()
        {
            var books = await _dbContext.Books
                .Include(b => b.Category)
                .Include(b => b.AuthorBooks)
                .ThenInclude(ab => ab.Author)
                .OrderBy(b => b.BookName)
                .ToListAsync();
            return books;
        }
        public async Task<IEnumerable<Book>> GetAllBookAsync(int order, int skip, int take)
        {
            var books = await _dbContext.Books
                .Include(b => b.Category)
                .Include(b => b.AuthorBooks)
                .ThenInclude(ab => ab.Author)
                .Skip(skip).Take(take)
                .OrderBy(b => b.CreationDate)
                .ToListAsync();
            if (order == 1) books.Reverse();
            return books;
        }
        public async Task<Book> GetBookByIdAsync(string bookId)
        {
            Book book = await _dbContext.Books
                .Include(b => b.Category)
                .Include(b => b.AuthorBooks)
                .ThenInclude(ab => ab.Author)
                .FirstOrDefaultAsync(b => b.BookId.Equals(bookId));
            return book;
        }
        public async  Task<bool> AddBookAsync(Book newBook)
        {
            try
            {
                var bookExist = await _dbContext.Books.FirstOrDefaultAsync(b => b.BookCode.Equals(newBook.BookCode));
                if (bookExist != null) return false;
                await _dbContext.AddAsync(newBook);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddAuthorBookAsync(string bookId, string authorId)
        {
            try
            {
                AuthorBook newAuthorBook = new AuthorBook()
                {
                    BookId = bookId,
                    AuthorId = authorId
                };
                await _dbContext.AddAsync(newAuthorBook);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> RemoveBookAsync(Book book)
        {
            try
            {
                _dbContext.Remove(book);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateBookAsync(Book bookEdited)
        {
            try
            {
                _dbContext.Update(bookEdited);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> IsBookCodeUniqueAsync(Book currentBook, string newBookCode)
        {
            var bookExist = await _dbContext.Books.AnyAsync(b => !b.BookId.Equals(currentBook.BookId) &&
                                                                 b.BookCode.Equals(newBookCode));
          
            return !bookExist;
        }
    }

}


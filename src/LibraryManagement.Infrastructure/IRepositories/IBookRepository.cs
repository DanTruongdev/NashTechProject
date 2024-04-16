using LibraryManagement.Core.Entities;

namespace LibraryManagement.Infrastructure.IRepositories
{
    public interface IBookRepository
    {
      
        public Task<IEnumerable<Book>> GetAllBookAsync();
        public Task<IEnumerable<Book>> GetAllBookAsync(int order, int skip, int take);
        public Task<Book> GetBookByIdAsync(string bookId);
        public Task<bool> AddBookAsync(Book newBook);
        public Task<bool> AddAuthorBookAsync(string bookId, string authorId);
        public Task<bool> UpdateBookAsync(Book bookEdited);
        public Task<bool> RemoveBookAsync(Book book);
        public Task<bool> IsBookCodeUniqueAsync(Book currentBook, string newBookCode);
    }
}

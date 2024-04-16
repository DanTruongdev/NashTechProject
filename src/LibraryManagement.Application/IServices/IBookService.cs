using LibraryManagement.Application.Dtos;
using LibraryManagement.Core.Entities;
namespace LibraryManagement.Application.IServices
{
    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetAllBooksAsync();
        public Task<IEnumerable<Book>> GetAllBookAsync(int order, int skip, int take);
        public Task<IEnumerable<Book>> GetAllBookAsync(string categoryId, int orderName, string keywords);
        public Task<int> CountTotalPageAsync(IEnumerable<Book> bookList);
        public Task<Book> GetBookByIdAsync(string bookId);
        public Task<Result> AddBookAsync(BookDto form);
        public Task<Result> UpdateBookAsync(BookDto form);
        public Task<Result> RemoveBookAsync(string bookId);
        public Task<FileResult> DownloadBookAsync(string bookId);
    }
}

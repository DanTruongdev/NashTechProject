using LibraryManagement.Application.Dtos;
using LibraryManagement.Application.IServices;
using LibraryManagement.Core.Entities;
using LibraryManagement.Infrastructure.Constants;
using LibraryManagement.Infrastructure.Extentions;
using LibraryManagement.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.IO;


namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllBookAsync();
            return books;
        }
        public async Task<IEnumerable<Book>> GetAllBookAsync(int order, int skip, int take)
        {
            var books = await _bookRepository.GetAllBookAsync(order, skip, take);
            return books;
        }
        public async Task<IEnumerable<Book>> GetAllBookAsync(string categoryId, int orderName, string keywords)
        {
            var books = await _bookRepository.GetAllBookAsync();
            if (categoryId != null && !categoryId.Equals("All")) books = books.Where(b => b.CategoryId.Equals(categoryId));
            if (keywords != null && !keywords.Equals(""))
            {
                keywords = keywords.ToUpper();
                books = books.Where(b => b.BookName.ToUpper().Contains(keywords) ||
                                         b.BookCode.ToUpper().Equals(keywords) ||
                                         b.AuthorBooks.Any(ab => ab.Author.AuthorName.ToUpper().Contains(keywords)));
            }
            if (orderName == 1) books = books.OrderByDescending(b => b.BookName);
            return books.ToList();
        }
        public async Task<int> CountTotalPageAsync(IEnumerable<Book> bookList)
        {
            int total = bookList.Count();
            int totalPage = total / 9;
            if (total % 9 != 0) totalPage++;
            return totalPage;
        }

        public async Task<Book> GetBookByIdAsync(string bookId)
        {
            Book book = await _bookRepository.GetBookByIdAsync(bookId);
            return book;

        }
        public async Task<Result> AddBookAsync(BookDto form)
        {

            Book newBook = new Book()
            {
                BookId = Guid.NewGuid().ToString(),
                BookName = form.BookName,
                BookCode = form.BookCode.ToUpper(),
                Image = "",
                CategoryId = form.CategoryId,
                Available = form.Quantity,
                BookDescription = form.BookDescription != null ? form.BookDescription : "",
                CreationDate = DateTime.Now,
                LatestUpdate = DateTime.Now,
            };
            if (form.Image != null)
            {
                string imagePath = FileManager.UploadFile(form.Image);
                newBook.Image = imagePath;
            }
            if (form.Source != null)
            {
                string sourcePath = FileManager.UploadFile(form.Source);
                newBook.UrlDownLoad = sourcePath;
            }
            var result = await _bookRepository.AddBookAsync(newBook);
            if (!result) return new Result(false, "The book code already exist");
            foreach (var authorId in form.AuthorId)
            {
                bool addAuthorResult = await _bookRepository.AddAuthorBookAsync(newBook.BookId, authorId);
                if (!addAuthorResult) return new Result(false, "Failed to add author book");
            }
            return new Result("The book is added");
        }

        public async Task<Result> UpdateBookAsync(BookDto form)
        {
            Book bookExist = await _bookRepository.GetBookByIdAsync(form.BookId);
            if (bookExist == null) return new Result(false, "The book does not exist");
            var isBookCodeUnique = await _bookRepository.IsBookCodeUniqueAsync(bookExist, form.BookCode);
            if (!isBookCodeUnique) return new Result(false, "The book code already exist");

            bookExist.BookCode = form.BookCode;
            bookExist.BookName = form.BookName;
            bookExist.CategoryId = form.CategoryId;
            bookExist.BookName = form.BookName;
            bookExist.Available = form.Quantity;
            bookExist.BookDescription = form.BookDescription != null ? form.BookDescription : bookExist.BookDescription;
            bookExist.LatestUpdate = DateTime.Now;
            if (form.Image != null)
            {
                if (bookExist.Image != null) FileManager.RemoveFile(bookExist.Image);
                var imagePath = FileManager.UploadFile(form.Image);
                bookExist.Image = imagePath;
            }
            if (form.Source != null)
            {
                if (bookExist.UrlDownLoad != null) FileManager.RemoveFile(bookExist.UrlDownLoad);
                string sourcePath = FileManager.UploadFile(form.Source);
                bookExist.UrlDownLoad = sourcePath;
            }
            bookExist.AuthorBooks.Clear();
            var result = await _bookRepository.UpdateBookAsync(bookExist);
            foreach (var authorId in form.AuthorId)
            {
                bool addAuthorResult = await _bookRepository.AddAuthorBookAsync(bookExist.BookId, authorId);
                if (!addAuthorResult) return new Result(false, "Failed to add author book");
            }
            if (!result) return new Result(false, "Failed to update the author name");
            return new Result("Update category successfully");
        }

        public async Task<Result> RemoveBookAsync(string bookId)
        {
            Book book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null) return new Result(false, "The book does not exist");
            if (book.Image != null) FileManager.RemoveFile(book.Image);
            if (book.UrlDownLoad != null) FileManager.RemoveFile(book.UrlDownLoad);
            book?.AuthorBooks?.Clear();
            var result = await _bookRepository.RemoveBookAsync(book);
            return new Result("The book is removed");
        }

        public async Task<FileResult> DownloadBookAsync(string bookId)
        {
            Book book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book.UrlDownLoad == null) return null;
            if (book.Available == 0) return null;
            book.Available--;
            book.DownloadCount++;
            await _bookRepository.UpdateBookAsync(book);
            var file = FileManager.GetFile(book.UrlDownLoad);
            return file;
        }
    }
}

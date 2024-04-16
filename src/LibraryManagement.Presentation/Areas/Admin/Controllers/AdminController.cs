using LibraryManagement.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.IServices;
using Microsoft.AspNetCore.Identity;
using LibraryManagement.Core.Entities;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.Infrastructure.Constants;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryManagement.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Librarian")]
    public class AdminController : Controller
    {
        public readonly IUserService _userService;
        public readonly ICategoryService _categoryService;
        public readonly IBookService _bookService;
        public readonly IAuthorService _authorService;
        public readonly SignInManager<User> _signInManager; 
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUserService userService, ICategoryService categoryService, 
            IAuthorService authorService, SignInManager<User> signInManager, 
            IBookService bookService, ILogger<AdminController> logger)
        {
            _userService = userService;
            _categoryService = categoryService;
            _authorService = authorService;
            _bookService = bookService;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            var books = await _bookService.GetAllBooksAsync();
            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewData["TotalUser"] = users != null ? users.Count() : 0;
            ViewData["TotalBook"] = books != null ? books.Count() : 0;
            ViewData["TotalAuthor"] = authors != null ? authors.Count() : 0;
            ViewData["TotalCategory"] = categories != null ? categories.Count() : 0;
            return View();
        }

        //User
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string? searchString)
        {
            IEnumerable<UserDto> users = new List<UserDto>();
            if (searchString != null && searchString?.Length != 0) users = await _userService.SearchUserByName(searchString);
            else users = await _userService.GetAllUsersAsync();
            ViewData["SearchString"] = searchString;
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> ChangeUserStatus(string userId)
        {
            var result = await _userService.ChangeUserStatus(userId);
            return RedirectToAction(nameof(GetAllUsers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
          
            return View();
        }
        //Category
        [HttpGet]
        public async Task<IActionResult> AddCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(CategoryDto category)
        {
           
            if (ModelState.IsValid)
            {
                Result result = await _categoryService.AddCategoryAsync(category);
                if (result.IsSucceed) return RedirectToAction(nameof(GetAllCategories));
                ViewBag.AddCateMsg = result.Message;
                return View();

            }
          
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(string categoryId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category category)
        {
           
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategoryAsync(category);
                if (result.IsSucceed) return RedirectToAction(nameof(GetAllCategories));
                ViewBag.EditCateMsg = result.Message;
                return View(category);
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveCategory(string categoryId)
        {
            var result = await _categoryService.RemoveCategoryAsync(categoryId);
            return RedirectToAction(nameof(GetAllCategories)); ;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(string searchString)
        {
            IEnumerable<Category> categories = new List<Category>();
            if (searchString != null && searchString?.Length != 0) categories = await _categoryService.GetAllCategoriesAsync(searchString!);
            else categories = await _categoryService.GetAllCategoriesAsync();
            ViewData["SearchString"] = searchString;
            return View(categories);
        }
        //Authors
        [HttpGet]
        public async Task<IActionResult> AddAuthor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAuthor(AuthorDto author)
        {

            if (ModelState.IsValid)
            {
                var result = await _authorService.AddAuthorAsync(author);
                if (result.IsSucceed) return RedirectToAction(nameof(GetAllAuthors));
                ViewBag.AddAuthMsg = result.Message;
                return View();

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditAuthor(string authorId)
        {
            var category = await _authorService.GetAuthorByIdAsync(authorId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAuthor(Author author)
        {

            if (ModelState.IsValid)
            {
                var result = await _authorService.UpdateAuthorAsync(author);
                if (result.IsSucceed) return RedirectToAction(nameof(GetAllAuthors));
                ViewBag.EditAuthMsg = result.Message;
                return View(author);
            }
            return View(author);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveAuthor(string authorId)
        {
            var result = await _authorService.RemoveAuthorAsync(authorId);
            return RedirectToAction(nameof(GetAllAuthors)); ;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors(string searchString)
        {
            IEnumerable<Author> authors = new List<Author>();
            if (searchString != null && searchString?.Length != 0) authors = await _authorService.GetAllAuthorsAsync(searchString!);
            else authors = await _authorService.GetAllAuthorsAsync();
            ViewData["SearchString"] = searchString;
            return View(authors);
        }
        //Book
        [HttpGet]
        public async Task<IActionResult> GetAllBooks(string? searchString)
        {
            IEnumerable<Book> books = new List<Book>();
            if (searchString != null && searchString?.Length != 0) books = await _bookService.GetAllBookAsync("All", 0, searchString!);
            else books = await _bookService.GetAllBooksAsync();
            ViewData["SearchString"] = searchString;
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            ViewData["Categories"] = new List<Category>();
            ViewData["Authors"] = new List<Author>();
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories.Any()) ViewData["Categories"] = categories;
            var authors = await _authorService.GetAllAuthorsAsync();
            if (authors.Any()) ViewData["Authors"] = authors;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(BookDto book)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookService.AddBookAsync(book);
                if (result.IsSucceed) return RedirectToAction(nameof(GetAllBooks));
                ViewBag.AddBookMsg = result.Message;
            }
            ViewData["Categories"] = new List<Category>();
            ViewData["Authors"] = new List<Author>();
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories.Any()) ViewData["Categories"] = categories;
            var authors = await _authorService.GetAllAuthorsAsync();
            if (authors.Any()) ViewData["Authors"] = authors;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditBook(string bookId)
        {
            ViewData["Categories"] = new List<Category>();
            ViewData["Authors"] = new List<Author>();
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories.Any()) ViewData["Categories"] = categories;
            var authors = await _authorService.GetAllAuthorsAsync();
            if (authors.Any()) ViewData["Authors"] = authors;
            var currentBook = await _bookService.GetBookByIdAsync(bookId);
            ViewData["CurrentBook"] = currentBook;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(BookDto book)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookService.UpdateBookAsync(book);
                if (result.IsSucceed) return RedirectToAction(nameof(GetAllBooks));
                ViewBag.EditBookMsg = result.Message;
            }
            ViewData["Categories"] = new List<Category>();
            ViewData["Authors"] = new List<Author>();
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories.Any()) ViewData["Categories"] = categories;
            var authors = await _authorService.GetAllAuthorsAsync();
            if (authors.Any()) ViewData["Authors"] = authors;
            var currentBook = await _bookService.GetBookByIdAsync(book.BookId);
            ViewData["CurrentBook"] = currentBook;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveBook(string bookId)
        {
            var result = await _bookService.RemoveBookAsync(bookId);
            return RedirectToAction(nameof(GetAllBooks)); ;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Authentication", new { area = "Authentication" });
        }
    }
}

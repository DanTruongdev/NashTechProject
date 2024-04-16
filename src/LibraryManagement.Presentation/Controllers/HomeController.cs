using LibraryManagement.Application.Dtos;
using LibraryManagement.Application.IServices;
using LibraryManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LibraryManagement.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, IUserService userService,
            IBookService bookService, ICategoryService categoryService)
        {
            _logger = logger;
            _userService = userService;
            _bookService = bookService;
            _categoryService = categoryService;
          
        }

        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBookAsync(1,0,9);
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> AllBooks(string categoryId, string orderByName, string keyword, string page)
        {
            int currentPage = int.Parse(page);
            ViewData["Categories"] = new List<Category>();
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories.Any()) ViewData["Categories"] = categories;
            int order = orderByName.StartsWith("A") ? 0 : 1;
            var books = await _bookService.GetAllBookAsync(categoryId, order, keyword);
            ViewData["CurrentPage"] = currentPage;
            ViewData["TotalPage"] = await _bookService.CountTotalPageAsync(books);
            ViewData["CategoryId"] = categoryId;
            ViewData["OrderByName"] = orderByName;
            ViewData["Keyword"] = keyword == null ? "" : keyword;
            books = books.Skip(9 * (currentPage - 1)).Take(9).ToList();
            return View(books);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DownloadBook(string bookId, string returnUrl)
        {
            var fileDownload = await _bookService.DownloadBookAsync(bookId);
            if (fileDownload == null) return RedirectPermanent(returnUrl);
            return File(fileDownload.MemoryStream, fileDownload.MimeType, fileDownload.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookDetail(string bookId)
        {
            Book book = await _bookService.GetBookByIdAsync(bookId);
            return View(book);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UserInfo()
        {
            User currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null) return RedirectToAction(nameof(Index));
            ViewData["CurrentUser"] = currentUser;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UserInfo(UpdateUserDto user)
        {
             if (ModelState.IsValid)
             {
                var result = await _userService.UpdateUserAsync(user);
                ViewBag.EditUserMsg = result.Message;
             }
            User currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null) return RedirectToAction(nameof(Index));
            ViewData["CurrentUser"] = currentUser;
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AboutUs()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ContactUs()
        {
            return View();
        }
    }
}
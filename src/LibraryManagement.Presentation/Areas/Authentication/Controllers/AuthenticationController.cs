using LibraryManagement.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.IServices;
using Microsoft.AspNetCore.Identity;
using LibraryManagement.Core.Entities;
using LibraryManagement.Infrastructure.Constants;

namespace LibraryManagement.Presentation.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    public class AuthenticationController : Controller
    {
        public readonly IUserService _userService;
        public readonly SignInManager<User> _signInManager; 
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IUserService userService, SignInManager<User> signInManager, ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateAsync(model, RoleName.Customer);
                if (!result.IsSucceed)
                {
                    ViewBag.SignUpError = result.Message;
                    return View();
                }
                return RedirectToAction(nameof(Login));
            }
            ViewBag.SignUpError = "Failed to create account";
            return View();
        }
       
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginAsync(model);
                if (!result.IsSucceed) 
                {
                    _logger.LogWarning(result.Message);
                    ViewBag.LoginError = result.Message;
                    return View();
                }
                _logger.LogInformation("User logged in");
                if (result.Message.Equals(RoleName.Librarian))
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignInGoogle()
        {
            var redirectUri = Url.Action(nameof(HandleGoogleCallback), "Authentication");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUri);
            return new ChallengeResult("Google", properties);
        }

        [HttpGet]
        public async Task<IActionResult> HandleGoogleCallback()
        {
           var result = await _userService.CreateWithGoogleAsync();
           if (result.IsSucceed) return RedirectToAction("Index", "Home", new { area = "" });
           ViewBag.LoginError = result.Message;
           return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dtos
{
    public class LoginDto
    {   
        [EmailAddress]
        [Required(ErrorMessage = "The email field is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The password field is required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}

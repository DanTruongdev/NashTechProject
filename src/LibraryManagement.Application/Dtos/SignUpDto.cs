using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "The first name field is required")]
        public string FirstName{ get; set; }
        [Required(ErrorMessage = "The last name field is required")]
        public string LastName{ get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "The email field is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The password field is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The repeate password field is required")]
        public string ConfirmPass { get; set; }
    }
}

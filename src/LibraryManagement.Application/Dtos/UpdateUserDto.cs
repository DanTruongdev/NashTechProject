using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Dtos
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "The first name field is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The first name must contain between 2 and 50 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The last name field is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The last name must contain between 2 and 50 characters")]
        public string LastName { get; set; }
        [AllowNull]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The phone number must contain 10 characters")]
        public string? PhoneNumber { get; set; }
        [AllowNull]
        public bool Gender { get; set; }
        [AllowNull]
        public DateTime Dob { get; set; }
        public string? Address { get; set; }
        
    }
}

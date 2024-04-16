using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Entities
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "The first name field is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The first name must contain between 2 and 50 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The first name field is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The last name must contain between 2 and 50 characters")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The first name field is required")]
        //[EmailAddress]
        //public override string Email { get; set; }
        //[AllowNull]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "The phone number must contain 10 characters")]
        //public string? PhoneNumber { get; set; }
        [AllowNull]
        public bool Gender { get; set; }
        [AllowNull]
        public DateTime Dob { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        [Required]
        public bool IsActivated { get; set; }
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}

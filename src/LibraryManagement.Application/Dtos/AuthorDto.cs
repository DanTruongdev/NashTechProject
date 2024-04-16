using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Dtos
{
    public class AuthorDto
    {
        [Required(ErrorMessage = "The author name field is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The author name must contain between 2 and 100 characters")]
        public string AuthorName { get; set; }
    }
}

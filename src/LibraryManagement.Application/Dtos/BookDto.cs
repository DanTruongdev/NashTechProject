using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dtos
{
    public class BookDto
    {
        public string? BookId { get; set; }
        [Required(ErrorMessage = "The book code field is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The book code must contain 10 characters")]
        public string BookCode { get; set; }
        [Required(ErrorMessage = "The book name field is required")]

        [StringLength(500, MinimumLength = 2, ErrorMessage = "The book name must contain between 2 and 500 characters")]
        public string BookName { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Source { get; set; }
        [Required(ErrorMessage = "The category field is required")]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "The author field is required")]
        public List<string> AuthorId { get; set; }
        public string? BookDescription { get; set; }
        [Required(ErrorMessage = "The quantity field is required")]
        [Range(0, 1000, ErrorMessage = "The quantity must be between 0 and 1000")]
        public int Quantity { get; set; }
    }
}

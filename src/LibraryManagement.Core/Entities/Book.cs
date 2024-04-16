using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LibraryManagement.Core.Entities
{
    public class Book
    {
        [Key]
        public string BookId { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The book code must contain 10 characters")]
        public string BookCode { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "The book name must contain between 2 and 500 characters")]
        public string BookName { get; set; }
        public string? Image { get; set; }
        public string? UrlDownLoad { get; set; }
        [Required]
        public int DownloadCount { get; set; } = 0;
        [Required]
        public string CategoryId { get; set; }
        [AllowNull]
        public string BookDescription { get; set; }
        [Required]
        public int Available { get; set; } = 0;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime LatestUpdate { get; set; } = DateTime.Now;
        //
        public virtual Category? Category { get; set; }
        public virtual ICollection<AuthorBook>? AuthorBooks { get; set; }

    }
}

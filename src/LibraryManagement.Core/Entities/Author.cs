using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Core.Entities
{
    public class Author
    {
        [Key]
        public string AuthorId { get; set; }
        [Required(ErrorMessage = "The author name field is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The author name must contain between 2 and 100 characters")]
        public string AuthorName { get; set; }
        //
        public virtual ICollection<AuthorBook>? AuthorBooks { get; set; }
    }
}

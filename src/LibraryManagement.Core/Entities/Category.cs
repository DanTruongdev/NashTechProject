using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Core.Entities
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "The category name field is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The category name must contain between 2 and 100 characters")]
        public string CategoryName { get; set; }
        //
        public virtual ICollection<Book>? Books { get; set; }
    }
}

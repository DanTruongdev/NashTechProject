using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Entities
{
    public class AuthorBook
    {
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string BookId { get; set; }
        //
        public virtual Author? Author { get; set; }
        public virtual Book? Book { get; set; }

    }
}

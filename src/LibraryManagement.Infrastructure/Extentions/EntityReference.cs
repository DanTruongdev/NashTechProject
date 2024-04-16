using LibraryManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Extentions
{
    public static class EntityReference
    {
        public static void DuplicateIndex(ModelBuilder builder)
        {
            builder.Entity<AuthorBook>().HasKey(a => new {a.AuthorId, a.BookId});
        }
    }
}

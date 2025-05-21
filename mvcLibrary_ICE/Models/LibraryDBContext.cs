using Microsoft.EntityFrameworkCore;

namespace mvcLibrary_ICE.Models
{
    public class LibraryDBContext : DbContext
    {
        public DbSet<BookType> BookType { get; set; }

        public DbSet<Book> Book { get; set; }

        public DbSet<Loan> Loan { get; set; }

        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options)
        {
        }


    }
}

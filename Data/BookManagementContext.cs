using BookManagementPrj.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementPrj.Data
{
    public class BookManagementContext : DbContext
    {
        public BookManagementContext(DbContextOptions<BookManagementContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}

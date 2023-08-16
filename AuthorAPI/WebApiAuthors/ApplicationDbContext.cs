using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Models;

namespace WebApiAuthors
{
	public class ApplicationDbContext : DbContext
	{
        protected readonly IConfiguration Configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("defaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Creating a complex key with both IDs
            modelBuilder.Entity<AuthorBook>()
                .HasKey(ab => new { ab.AuthorId, ab.BookId });
        }

        // Table
        public DbSet<Author> Authors { get; set; }

        // Optional (but needed to create queries with Books
        public DbSet<Book> Books { get; set; }

        public DbSet<AuthorBook> AuthorsBooks { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}


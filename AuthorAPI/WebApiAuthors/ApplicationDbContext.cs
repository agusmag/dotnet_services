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

        // Table
        public DbSet<Author> Authors { get; set; }
    }
}


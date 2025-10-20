using BulkyWeb.Models;
using BulkyWeb.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  //seeding the first 3 category records by using dbcontext override method
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFI", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 },
				 new Category { Id = 4, Name = "Comedy", DisplayOrder = 4 }
				);

        }
    }   
}

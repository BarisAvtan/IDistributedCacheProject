using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml.Linq;

namespace RedisExampleApp.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        //EF CONFIG
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
             new Product()
            {
                Id = 1,
                Name = "Test",
                Prices = 12
            }, 
             new Product()
            {
                Id = 2,
                Name = "Test2",
                Prices = 14
            },
             new Product()
            {
                Id = 3,
                Name = "Test3",
                Prices = 15
            }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}

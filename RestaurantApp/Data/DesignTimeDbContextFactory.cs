using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace RestaurantApp.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RestaurantDbContext>
    {
        public RestaurantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RestaurantDbContext>();
            optionsBuilder.UseSqlServer("Server=LAPTOP-5K3GC6BJ;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False");
            return new RestaurantDbContext(optionsBuilder.Options);
        }
    }
} 
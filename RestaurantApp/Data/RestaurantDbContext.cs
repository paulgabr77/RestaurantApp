using Microsoft.EntityFrameworkCore;
using RestaurantApp.Models;

namespace RestaurantApp.Data
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<DishImage> DishImages { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurare relație many-to-many între Dish și Allergen
            modelBuilder.Entity<Dish>()
                .HasMany(d => d.Allergens)
                .WithMany(a => a.Dishes)
                .UsingEntity(j => j.ToTable("DishAllergens"));

            // Configurare relație many-to-many între Menu și Dish
            modelBuilder.Entity<Menu>()
                .HasMany(m => m.Dishes)
                .WithMany(d => d.Menus)
                .UsingEntity(j => j.ToTable("MenuDishes"));

            // Configurare chei străine pentru OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Dish)
                .WithMany(d => d.OrderDetails)
                .HasForeignKey(od => od.DishId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Menu)
                .WithMany(m => m.OrderDetails)
                .HasForeignKey(od => od.MenuId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 
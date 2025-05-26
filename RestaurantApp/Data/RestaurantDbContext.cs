using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantApp.Models;

namespace RestaurantApp.Data
{
    public class RestaurantDbContext : DbContext
    {
        // Constructor pentru Dependency Injection (folosit în aplicație)
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        // Constructor fără parametri (OBLIGATORIU pentru EF Core Tools)
        public RestaurantDbContext()
        {
        }

        // DbSet-uri pentru toate entitățile
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configurație folosită DOAR pentru EF Core Tools (Add-Migration, etc.)
                optionsBuilder.UseSqlServer("Server=LAPTOP-5K3GC6BJ;Database=RestaurantDb;Trusted_Connection=True;")
                    .LogTo(Console.WriteLine, LogLevel.Information) // Logging detaliat
                    .EnableSensitiveDataLogging(); // Afișează valorile parametrilor
            }
        }

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

            // Configurare pentru tipurile de date decimal
            ConfigureDecimalProperties(modelBuilder);
        }

        private void ConfigureDecimalProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>()
                .Property(d => d.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Menu>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StockItem>()
                .Property(si => si.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StockItem>()
                .Property(si => si.MinimumQuantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Discount>()
                .Property(d => d.Percentage)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Discount>()
                .Property(d => d.MinimumOrderAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Delivery>()
                .Property(d => d.DeliveryFee)
                .HasColumnType("decimal(18,2)");
        }
    }
}
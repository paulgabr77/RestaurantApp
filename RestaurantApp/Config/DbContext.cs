using Microsoft.EntityFrameworkCore;
using RestaurantApp.Models;

namespace RestaurantApp.Config
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurare pentru Dish
            modelBuilder.Entity<Dish>()
                .Property(d => d.Price)
                .HasColumnType("decimal(18,2)");

            // Configurare pentru Menu
            modelBuilder.Entity<Menu>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            // Configurare pentru Order
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            // Configurare pentru OrderItem
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            // Configurare pentru StockItem
            modelBuilder.Entity<StockItem>()
                .Property(si => si.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StockItem>()
                .Property(si => si.MinimumQuantity)
                .HasColumnType("decimal(18,2)");

            // Configurare pentru Discount
            modelBuilder.Entity<Discount>()
                .Property(d => d.Percentage)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Discount>()
                .Property(d => d.MinimumOrderAmount)
                .HasColumnType("decimal(18,2)");

            // Configurare pentru Delivery
            modelBuilder.Entity<Delivery>()
                .Property(d => d.DeliveryFee)
                .HasColumnType("decimal(18,2)");
        }
    }
} 
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
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurare relatie many-to-many intre Dish si Allergen
            modelBuilder.Entity<Dish>()
                .HasMany(d => d.Allergens)
                .WithMany(a => a.Dishes)
                .UsingEntity(j => j.ToTable("DishAllergens"));

            // Configurare relatie many-to-many intre Menu si Dish
            modelBuilder.Entity<Menu>()
                .HasMany(m => m.Dishes)
                .WithMany(d => d.Menus)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuDishes",
                    j => j
                        .HasOne<Dish>()
                        .WithMany()
                        .HasForeignKey("DishesDishId")
                        .OnDelete(DeleteBehavior.NoAction),
                    j => j
                        .HasOne<Menu>()
                        .WithMany()
                        .HasForeignKey("MenusMenuId")
                        .OnDelete(DeleteBehavior.NoAction)
                );

            // Configurare chei straine pentru OrderDetail
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

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Weight)
                .HasColumnType("decimal(18,2)");
        }
    }
} 
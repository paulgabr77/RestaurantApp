using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApp.Repositories;
using RestaurantApp.Data;
using RestaurantApp.Services;
using RestaurantApp.ViewModels;
using RestaurantApp.Views;
using RestaurantApp.Extensions;
using System;

namespace RestaurantApp
{
    public partial class App : System.Windows.Application
    {
        private IServiceProvider _serviceProvider;

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configurare
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("Config/appsettings.json")
                .Build();

            // Database
            services.AddDbContextFactory<RestaurantDbContext>(options =>
            {
                options.UseSqlServer("Server=LAPTOP-5K3GC6BJ;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False");
            });

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Services
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<IDishService, DishService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IAllergenService, AllergenService>();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<IStockService, StockService>();
            services.AddSingleton<IDiscountService, DiscountService>();
            services.AddSingleton<IDeliveryService, DeliveryService>();
            services.AddSingleton<IAuthService, AuthService>();

            // ViewModels
            services.AddTransient<MenuViewModel>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<CartViewModel>();
            services.AddTransient<AddProductViewModel>();
            services.AddTransient<AuthViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<MenuWindow>();
            services.AddTransient<CartWindow>();
            services.AddTransient<AddProductWindow>();
            services.AddTransient<AuthWindow>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _serviceProvider = ConfigureServices();
            this.ConfigureServices(_serviceProvider);

            var authWindow = _serviceProvider.GetRequiredService<AuthWindow>();
            authWindow.Show();
        }

        public IServiceProvider Services => _serviceProvider;
    }
} 
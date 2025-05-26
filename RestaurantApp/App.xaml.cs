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

namespace RestaurantApp
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
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

            // Services
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAuthService, AuthService>();

            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<ReportViewModel>();
            services.AddTransient<StockViewModel>();
            services.AddTransient<DiscountViewModel>();
            services.AddTransient<DeliveryViewModel>();
            services.AddTransient<MenuViewModel>();
            services.AddTransient<AuthViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<OrderWindow>();
            services.AddTransient<ReportWindow>();
            services.AddTransient<StockWindow>();
            services.AddTransient<DiscountWindow>();
            services.AddTransient<DeliveryWindow>();
            services.AddTransient<AuthWindow>();

            // ServiceProvider
            services.AddSingleton<IServiceProvider>(sp => sp);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var authWindow = _serviceProvider.GetService<AuthWindow>();
            authWindow.Show();
        }
    }
} 
using System.Configuration;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApp.Data;
using RestaurantApp.Repositories;
using RestaurantApp.Services;
using RestaurantApp.ViewModels;
using RestaurantApp.Views;

namespace RestaurantApp
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            ServiceProvider = _serviceProvider;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine($"Calea curentă: {AppDomain.CurrentDomain.BaseDirectory}");
            // Configurare
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Database
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Connection string încărcat: {connectionString}");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string este null sau gol!");
            }

            services.AddDbContext<RestaurantDbContext>(options =>
            {
                Console.WriteLine($"Configurare DbContext cu: {connectionString}"); // Debug
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging().LogTo(Console.WriteLine); ;
            });

            services.AddScoped<IAuthService>(provider =>
            {
                var context = provider.GetRequiredService<RestaurantDbContext>();
                Console.WriteLine($"AuthService creat. Context valid: {context != null}");
                return new AuthService(context);
            });

            // Services
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IDishService, DishService>();
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
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
                    Console.WriteLine($"Verificare baza de date: {dbContext.Database.CanConnect()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la conectarea la baza de date: {ex.Message}");
                MessageBox.Show("Eroare la conectarea la baza de date. Verifică appsettings.json");
                Environment.Exit(1);
            }

            base.OnStartup(e);

            try
            {
                var authWindow = _serviceProvider.GetRequiredService<AuthWindow>();
                authWindow.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la pornire: {ex}");
                MessageBox.Show($"Eroare critică: {ex.Message}");
                Environment.Exit(1);
            }
            //base.OnStartup(e);
            //var authWindow = ServiceProvider.GetRequiredService<AuthWindow>();
            //authWindow.Show();
        }

    }
} 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApp.Data;
using RestaurantApp.Repositories;
using RestaurantApp.Services;
using RestaurantApp.ViewModels;
using RestaurantApp.Views;
using System.Windows;

namespace RestaurantApp
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Configurare
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton(configuration);

            // DbContext
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Services
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();

            // ViewModels
            services.AddTransient<MenuViewModel>();
            services.AddTransient<AuthViewModel>();
            services.AddTransient<OrderViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<AuthWindow>();
            services.AddTransient<OrderWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var authWindow = serviceProvider.GetService<AuthWindow>();
            authWindow.Show();
        }
    }
} 
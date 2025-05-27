using System.Windows;
using Microsoft.Extensions.DependencyInjection; // Add this using directive
using RestaurantApp.ViewModels;
using RestaurantApp.Services; // Ensure this namespace is included for IProductService and other services

namespace RestaurantApp.Views
{
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            var serviceProvider = (App.Current as App)?.Services; // Cast App.Current to your custom App class
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }

            DataContext = new MenuViewModel(
                serviceProvider.GetService<IDishService>(),
                serviceProvider.GetService<ICategoryService>(),
                serviceProvider.GetService<IProductService>(),
                serviceProvider.GetService<ICartService>()
            );
        }
    }
} 
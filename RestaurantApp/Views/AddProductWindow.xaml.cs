using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApp.ViewModels;
using RestaurantApp.Services;
using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            var serviceProvider = (App.Current as App)?.Services;
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }

            DataContext = new AddProductViewModel(
                serviceProvider.GetService<IProductService>(),
                serviceProvider.GetService<ICategoryService>(),
                serviceProvider.GetService<IAllergenService>()
            );
        }

        public AddProductWindow(Product product)
        {
            InitializeComponent();
            var serviceProvider = (App.Current as App)?.Services;
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            DataContext = new AddProductViewModel(
                serviceProvider.GetService<IProductService>(),
                serviceProvider.GetService<ICategoryService>(),
                serviceProvider.GetService<IAllergenService>(),
                product
            );
        }
    }
} 
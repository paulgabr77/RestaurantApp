using RestaurantApp.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(MenuViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            DataContext = viewModel;
        }

        private void OpenAccount_Click(object sender, RoutedEventArgs e)
        {
            var authWindow = _serviceProvider.GetService<AuthWindow>();
            authWindow.Show();
        }

        private void OpenDiscounts_Click(object sender, RoutedEventArgs e)
        {
            var discountWindow = _serviceProvider.GetService<DiscountWindow>();
            discountWindow.Show();
        }

        private void OpenDeliveries_Click(object sender, RoutedEventArgs e)
        {
            var deliveryWindow = _serviceProvider.GetService<DeliveryWindow>();
            deliveryWindow.Show();
        }
    }
} 
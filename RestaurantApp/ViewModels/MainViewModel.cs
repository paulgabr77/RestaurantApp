using RestaurantApp.Views;
using RestaurantApp.Commands;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private bool _isAuthenticated;

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public ICommand OpenOrdersCommand { get; }
        public ICommand OpenReportsCommand { get; }
        public ICommand OpenStockCommand { get; }
        public ICommand OpenDiscountsCommand { get; }
        public ICommand OpenDeliveriesCommand { get; }
        public ICommand OpenAccountCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            OpenOrdersCommand = new RelayCommand(OpenOrders);
            OpenReportsCommand = new RelayCommand(OpenReports);
            OpenStockCommand = new RelayCommand(OpenStock);
            OpenDiscountsCommand = new RelayCommand(OpenDiscounts);
            OpenDeliveriesCommand = new RelayCommand(OpenDeliveries);
            OpenAccountCommand = new RelayCommand(OpenAccount);
        }

        private void OpenOrders()
        {
            var orderWindow = _serviceProvider.GetService<OrderWindow>();
            orderWindow.Show();
        }

        private void OpenReports()
        {
            var reportWindow = _serviceProvider.GetService<ReportWindow>();
            reportWindow.Show();
        }

        private void OpenStock()
        {
            var stockWindow = _serviceProvider.GetService<StockWindow>();
            stockWindow.Show();
        }

        private void OpenDiscounts()
        {
            var discountWindow = _serviceProvider.GetService<DiscountWindow>();
            discountWindow.Show();
        }

        private void OpenDeliveries()
        {
            var deliveryWindow = _serviceProvider.GetService<DeliveryWindow>();
            deliveryWindow.Show();
        }

        private void OpenAccount()
        {
            IsAuthenticated = !IsAuthenticated;
        }
    }
} 
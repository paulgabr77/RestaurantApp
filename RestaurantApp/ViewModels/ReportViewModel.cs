using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantApp.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _selectedStatus;
        private decimal _totalRevenue;
        private ObservableCollection<Dish> _topDishes;
        private ObservableCollection<Menu> _topMenus;
        private ObservableCollection<User> _topCustomers;
        private Dictionary<string, int> _statusDistribution;
        private Dictionary<string, decimal> _categoryRevenue;
        private string _errorMessage;

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;
            _startDate = DateTime.Today.AddDays(-30);
            _endDate = DateTime.Today;

            LoadReportsCommand = new RelayCommand(async () => await LoadReports());
            ExportReportCommand = new RelayCommand(async () => await ExportReport());
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public string SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        public ObservableCollection<Dish> TopDishes
        {
            get => _topDishes;
            set => SetProperty(ref _topDishes, value);
        }

        public ObservableCollection<Menu> TopMenus
        {
            get => _topMenus;
            set => SetProperty(ref _topMenus, value);
        }

        public ObservableCollection<User> TopCustomers
        {
            get => _topCustomers;
            set => SetProperty(ref _topCustomers, value);
        }

        public Dictionary<string, int> StatusDistribution
        {
            get => _statusDistribution;
            set => SetProperty(ref _statusDistribution, value);
        }

        public Dictionary<string, decimal> CategoryRevenue
        {
            get => _categoryRevenue;
            set => SetProperty(ref _categoryRevenue, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadReportsCommand { get; }
        public ICommand ExportReportCommand { get; }

        private async Task LoadReports()
        {
            try
            {
                TotalRevenue = await _reportService.GetTotalRevenueByDateRangeAsync(StartDate, EndDate);

                var topDishes = await _reportService.GetTopSellingDishesAsync(5);
                TopDishes = new ObservableCollection<Dish>(topDishes);

                var topMenus = await _reportService.GetTopSellingMenusAsync(5);
                TopMenus = new ObservableCollection<Menu>(topMenus);

                var topCustomers = await _reportService.GetTopCustomersAsync(5);
                TopCustomers = new ObservableCollection<User>(topCustomers);

                StatusDistribution = await _reportService.GetOrdersByStatusDistributionAsync();
                CategoryRevenue = await _reportService.GetRevenueByCategoryAsync(StartDate, EndDate);

                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la incarcarea rapoartelor: {ex.Message}";
            }
        }

        private async Task ExportReport()
        {
            // TODO: Implementare export raport
            ErrorMessage = "Functionalitatea de export nu este implementata inca.";
        }
    }
} 
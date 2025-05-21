using RestaurantApp.ViewModels;
using System.Windows;

namespace RestaurantApp.Views
{
    public partial class StockWindow : Window
    {
        public StockWindow(StockViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 
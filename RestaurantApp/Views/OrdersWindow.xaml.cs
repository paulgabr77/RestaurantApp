using System.Windows;
using RestaurantApp.ViewModels;

namespace RestaurantApp.Views
{
    public partial class OrdersWindow : Window
    {
        public OrdersWindow(OrderViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.LoadOrdersCommand.Execute(null);
        }
    }
} 
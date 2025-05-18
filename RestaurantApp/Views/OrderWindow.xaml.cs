using RestaurantApp.ViewModels;
using System.Windows;

namespace RestaurantApp.Views
{
    public partial class OrderWindow : Window
    {
        public OrderWindow(OrderViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 
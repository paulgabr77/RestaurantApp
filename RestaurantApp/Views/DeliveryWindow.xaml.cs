using RestaurantApp.ViewModels;
using System.Windows;

namespace RestaurantApp.Views
{
    public partial class DeliveryWindow : Window
    {
        public DeliveryWindow(DeliveryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 
using RestaurantApp.ViewModels;
using System.Windows;

namespace RestaurantApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MenuViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 
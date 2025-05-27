using System.Windows;
using RestaurantApp.ViewModels;

namespace RestaurantApp.Views
{
    public partial class AccountWindow : Window
    {
        public AccountWindow()
        {
            InitializeComponent();
            DataContext = new AccountViewModel();
        }
    }
} 
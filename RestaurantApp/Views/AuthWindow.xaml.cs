using RestaurantApp.ViewModels;
using System.Windows;

namespace RestaurantApp.Views
{
    public partial class AuthWindow : Window
    {
        public AuthWindow(AuthViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Binding pentru PasswordBox
            PasswordBox.PasswordChanged += (s, e) => viewModel.Password = PasswordBox.Password;
            ConfirmPasswordBox.PasswordChanged += (s, e) => viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
        }
    }
} 
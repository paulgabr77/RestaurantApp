using RestaurantApp.ViewModels;
using System.Windows;

namespace RestaurantApp.Views
{
    public partial class AuthWindow : Window
    {
        private readonly AuthViewModel _viewModel;

        public AuthWindow(AuthViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.AuthenticationSuccessful += OnAuthenticationSuccessful;

            // Binding pentru PasswordBox
            PasswordBox.PasswordChanged += (s, e) => viewModel.Password = PasswordBox.Password;
            ConfirmPasswordBox.PasswordChanged += (s, e) => viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
        }

        private void OnAuthenticationSuccessful(object sender, Models.User user)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
} 
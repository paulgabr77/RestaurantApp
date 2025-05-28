using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApp.Services;
using RestaurantApp.ViewModels;

namespace RestaurantApp.Views
{
    public partial class AuthWindow : Window
    {
        private readonly AuthViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;

        public AuthWindow(AuthViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _serviceProvider = serviceProvider;
            DataContext = _viewModel;
            _viewModel.AuthenticationSuccessful += OnAuthenticationSuccessful;

            // Binding pentru PasswordBox
            PasswordBox.PasswordChanged += (s, e) => viewModel.Password = PasswordBox.Password;
            ConfirmPasswordBox.PasswordChanged += (s, e) => viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
        }

        private void OnAuthenticationSuccessful(object sender, Models.User user)
        {
            var dishService = _serviceProvider.GetService(typeof(IDishService)) as IDishService;
            var categoryService = _serviceProvider.GetService(typeof(ICategoryService)) as ICategoryService;
            var productService = _serviceProvider.GetService(typeof(IProductService)) as IProductService;
            var cartService = _serviceProvider.GetService(typeof(ICartService)) as ICartService;
            var allergenService = _serviceProvider.GetService(typeof(IAllergenService)) as IAllergenService;

            if (dishService == null)
            {
                throw new InvalidOperationException("IDishService is not registered in the service provider.");
            }
            if (categoryService == null)
            {
                throw new InvalidOperationException("ICategoryService is not registered in the service provider.");
            }
            if (productService == null)
            {
                throw new InvalidOperationException("IProductService is not registered in the service provider.");
            }
            if (cartService == null)
            {
                throw new InvalidOperationException("ICartService is not registered in the service provider.");
            }
            if (allergenService == null)
            {
                throw new InvalidOperationException("IAllergenService is not registered in the service provider.");
            }
    
            // Pass all required services to the MenuViewModel constructor
            var menuViewModel = new MenuViewModel(dishService, categoryService, productService, cartService, allergenService);
            var mainWindow = new MainWindow(menuViewModel, _serviceProvider);
            mainWindow.Show();
            Close();
        }
    }
} 
using System.Windows;
using RestaurantApp.ViewModels;
using RestaurantApp.Services;
using RestaurantApp.Extensions;

namespace RestaurantApp.Views
{
    public partial class CartWindow : Window
    {
        public CartWindow()
        {
            InitializeComponent();
            var cartService = ((App)Application.Current).GetService(typeof(ICartService)) as ICartService;
            DataContext = new CartViewModel(cartService);
        }
    }
} 
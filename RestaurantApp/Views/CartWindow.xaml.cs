using System.Windows;
using RestaurantApp.ViewModels;
using RestaurantApp.Services;
using RestaurantApp.Extensions;

namespace RestaurantApp.Views
{
    public partial class CartWindow : Window
    {
        public CartWindow(ICartService cartService, IOrderService orderService)
        {
            InitializeComponent();
            DataContext = new CartViewModel(cartService, orderService);
        }
    }
} 
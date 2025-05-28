using System.Windows;
using RestaurantApp.ViewModels;
using RestaurantApp.Services;
using RestaurantApp.Extensions;

namespace RestaurantApp.Views
{
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            var dishService = ((App)Application.Current).GetService(typeof(IDishService)) as IDishService;
            var categoryService = ((App)Application.Current).GetService(typeof(ICategoryService)) as ICategoryService;
            var productService = ((App)Application.Current).GetService(typeof(IProductService)) as IProductService;
            var cartService = ((App)Application.Current).GetService(typeof(ICartService)) as ICartService;
            var allergenService = ((App)Application.Current).GetService(typeof(IAllergenService)) as IAllergenService;
            DataContext = new MenuViewModel(dishService, categoryService, productService, cartService, allergenService);
        }
    }
} 
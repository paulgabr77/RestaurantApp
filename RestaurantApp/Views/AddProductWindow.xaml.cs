using System.Windows;
using RestaurantApp.ViewModels;
using RestaurantApp.Services;
using RestaurantApp.Extensions;
using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            var productService = ((App)Application.Current).GetService(typeof(IProductService)) as IProductService;
            var categoryService = ((App)Application.Current).GetService(typeof(ICategoryService)) as ICategoryService;
            var allergenService = ((App)Application.Current).GetService(typeof(IAllergenService)) as IAllergenService;
            DataContext = new AddProductViewModel(productService, categoryService, allergenService);
        }

        public AddProductWindow(Product product)
        {
            InitializeComponent();
            var productService = ((App)Application.Current).GetService(typeof(IProductService)) as IProductService;
            var categoryService = ((App)Application.Current).GetService(typeof(ICategoryService)) as ICategoryService;
            var allergenService = ((App)Application.Current).GetService(typeof(IAllergenService)) as IAllergenService;
            DataContext = new AddProductViewModel(productService, categoryService, allergenService, product);
        }
    }
} 
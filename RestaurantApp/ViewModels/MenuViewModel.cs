using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;  
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using RestaurantApp.Views;

namespace RestaurantApp.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IDishService _dishService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Dish> _dishes;
        private ObservableCollection<Product> _products;
        private string _searchTerm;
        private Category _selectedCategory;
        private Dish _selectedDish;
        private bool _isAuthenticated;

        public MenuViewModel(IDishService dishService, ICategoryService categoryService, IProductService productService, ICartService cartService)
        {
            _dishService = dishService;
            _categoryService = categoryService;
            _productService = productService;
            _cartService = cartService;
            LoadDataCommand = new RelayCommand(async () => await LoadData());
            SearchCommand = new RelayCommand(async () => await Search());
            AddDishCommand = new RelayCommand(AddDish);

            // Inițializare comenzi pentru meniu
            OpenMenuCommand = new RelayCommand(OpenMenu);
            OpenOrdersCommand = new RelayCommand(OpenOrders);
            OpenReportsCommand = new RelayCommand(OpenReports);
            OpenCartCommand = new RelayCommand(OpenCart);
            OpenAccountCommand = new RelayCommand(OpenAccount);
            AddProductCommand = new RelayCommand(OpenAddProduct);
            AddToCartCommand = new RelayCommand<Product>(AddToCart);
            EditProductCommand = new RelayCommand<Product>(EditProduct);
            DeleteProductCommand = new RelayCommand<Product>(DeleteProduct);
            
            Products = new ObservableCollection<Product>();
            
            // Încărcare produse
            LoadProducts();

            _ = LoadData();
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    LoadDishesByCategory();
                }
            }
        }

        public Dish SelectedDish
        {
            get => _selectedDish;
            set => SetProperty(ref _selectedDish, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddDishCommand { get; }

        // Comenzi pentru meniu
        public ICommand OpenMenuCommand { get; }
        public ICommand OpenOrdersCommand { get; }
        public ICommand OpenReportsCommand { get; }
        public ICommand OpenCartCommand { get; }
        public ICommand OpenAccountCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set => SetProperty(ref _isAuthenticated, value);
        }

        private async Task LoadData()
        {
            var categories = await _categoryService.GetAllCategoriesWithDishesAndMenusAsync();
            Categories = new ObservableCollection<Category>(categories);
        }

        private async Task LoadDishesByCategory()
        {
            if (SelectedCategory != null)
            {
                var dishes = await _dishService.GetDishesByCategoryAsync(SelectedCategory.CategoryId);
                Dishes = new ObservableCollection<Dish>(dishes);
            }
        }

        private async Task Search()
        {
            await LoadData();
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                foreach (var category in Categories)
                {
                    category.Dishes = category.Dishes?.Where(d => d.Name.Contains(SearchTerm)).ToList();
                    category.Menus = category.Menus?.Where(m => m.Name.Contains(SearchTerm)).ToList();
                }
            }
        }

        private void AddDish()
        {
            MessageBox.Show("Funcționalitatea de adăugare preparat nu este implementată încă.");
        }

        // Metode pentru comenzile de meniu
        private void OpenMenu()
        {
            var menuWindow = new MenuWindow();
            menuWindow.Show();
        }

        private void OpenOrders()
        {
            var ordersWindow = new OrdersWindow();
            ordersWindow.Show();
        }

        private void OpenReports()
        {
            var reportsWindow = new ReportsWindow();
            reportsWindow.Show();
        }

        private void OpenCart()
        {
            var cartWindow = new CartWindow();
            cartWindow.Show();
        }

        private void OpenAccount()
        {
            var accountWindow = new AccountWindow();
            accountWindow.Show();
        }

        private void OpenAddProduct()
        {
            var addProductWindow = new AddProductWindow();
            addProductWindow.ShowDialog();
            LoadData(); // Reîncarcă datele după adăugarea produsului
        }

        private async void LoadProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        private async void AddToCart(Product product)
        {
            if (product != null)
            {
                await _cartService.AddToCartAsync(product);
                // TODO: Adăugare notificare că produsul a fost adăugat în coș
            }
        }

        private void EditProduct(Product product)
        {
            if (product != null)
            {
                var editWindow = new RestaurantApp.Views.AddProductWindow(product); // presupunem că AddProductWindow acceptă un produs pentru editare
                editWindow.ShowDialog();
                LoadProducts(); // reîncarcă lista după editare
            }
        }

        private async void DeleteProduct(Product product)
        {
            if (product != null)
            {
                var result = MessageBox.Show($"Sigur vrei să ștergi produsul '{product.Name}'?", "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _productService.DeleteProductAsync(product.ProductId);
                    LoadProducts();
                }
            }
        }
    }
} 
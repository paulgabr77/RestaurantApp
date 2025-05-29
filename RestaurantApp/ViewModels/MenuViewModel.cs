using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;  
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using RestaurantApp.Views;
using System.Linq;
using RestaurantApp.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantApp.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IDishService _dishService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IAllergenService _allergenService;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Dish> _dishes;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Allergen> _allergens;
        private string _searchTerm;
        private Category _selectedCategory;
        private Allergen _selectedAllergen;
        private Dish _selectedDish;
        private bool _isAuthenticated;

        public MenuViewModel(
            IDishService dishService, 
            ICategoryService categoryService, 
            IProductService productService, 
            ICartService cartService,
            IAllergenService allergenService)
        {
            _dishService = dishService;
            _categoryService = categoryService;
            _productService = productService;
            _cartService = cartService;
            _allergenService = allergenService;

            LoadDataCommand = new RelayCommand(async () => await LoadData());
            SearchCommand = new RelayCommand(async () => await Search());
            AddDishCommand = new RelayCommand(AddDish);

            // Initializare comenzi pentru meniu
            OpenMenuCommand = new RelayCommand(OpenMenu);
            OpenOrdersCommand = new RelayCommand(OpenOrders);
            OpenReportsCommand = new RelayCommand(OpenReports);
            OpenCartCommand = new RelayCommand(OpenCart);
            OpenAccountCommand = new RelayCommand(OpenAccount);
            AddProductCommand = new RelayCommand(OpenAddProduct);
            AddToCartCommand = new RelayCommand<Product>(AddToCart);
            EditProductCommand = new RelayCommand<Product>(EditProduct);
            DeleteProductCommand = new RelayCommand<Product>(DeleteProduct);
            OpenStockCommand = new RelayCommand(OpenStock);
            
            Products = new ObservableCollection<Product>();
            Allergens = new ObservableCollection<Allergen>();
            
            // incarcare initiala a datelor
            _ = InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            try
            {
                // incarcam datele secvential pentru a evita probleme de concurenta
                await LoadData();
                await LoadProductsAsync();
                await LoadAllergensAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea datelor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        private async Task LoadAllergensAsync()
        {
            var allergens = await _allergenService.GetAllAllergensAsync();
            Allergens.Clear();
            foreach (var allergen in allergens)
            {
                Allergens.Add(allergen);
            }
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

        public ObservableCollection<Allergen> Allergens
        {
            get => _allergens;
            set => SetProperty(ref _allergens, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (SetProperty(ref _searchTerm, value))
                {
                    _ = Search();
                }
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    _ = Search();
                }
            }
        }

        public Allergen SelectedAllergen
        {
            get => _selectedAllergen;
            set
            {
                if (SetProperty(ref _selectedAllergen, value))
                {
                    _ = Search();
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
        public ICommand OpenStockCommand { get; }

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
            try
            {
                var products = await _productService.GetAllProductsAsync();
                var filteredProducts = products.AsQueryable();

                // Filtrare dupa nume
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    filteredProducts = filteredProducts.Where(p => 
                        p.Name.ToLower().Contains(SearchTerm.ToLower()));
                }

                // Filtrare dupa categorie
                if (SelectedCategory != null)
                {
                    filteredProducts = filteredProducts.Where(p => 
                        p.CategoryId == SelectedCategory.CategoryId);
                }

                // Filtrare dupa alergen
                if (SelectedAllergen != null)
                {
                    filteredProducts = filteredProducts.Where(p => 
                        p.Allergens.Any(a => a.AllergenId == SelectedAllergen.AllergenId));
                }

                Products.Clear();
                foreach (var product in filteredProducts)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la cautare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddDish()
        {
            MessageBox.Show("Functionalitatea de adaugare preparat nu este implementata inca.");
        }

        // Metode pentru comenzile de meniu
        private void OpenMenu()
        {
            var menuWindow = new MenuWindow();
            menuWindow.Show();
        }

        private void OpenOrders()
        {
            var app = (App)Application.Current;
            var orderService = app.Services.GetRequiredService<IOrderService>();
            var dishService = app.Services.GetRequiredService<IDishService>();
            var viewModel = new OrderViewModel(orderService, dishService);
            var ordersWindow = new OrdersWindow(viewModel);
            ordersWindow.Show();
        }

        private void OpenReports()
        {
            var user = AuthViewModel.CurrentUserStatic;
            if (user == null || !user.IsEmployee)
            {
                MessageBox.Show("Nu puteti accesa aceasta functie, nu sunteti angajat!", "Acces interzis", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var reportsWindow = new ReportsWindow();
            reportsWindow.Show();
        }

        private void OpenCart()
        {
            OpenCartWindow();
        }

        private void OpenAccount()
        {
            var accountWindow = new AccountWindow();
            accountWindow.Show();
        }

        private void OpenAddProduct()
        {
            var user = AuthViewModel.CurrentUserStatic;
            if (user == null || !user.IsEmployee)
            {
                MessageBox.Show("Nu puteti accesa aceasta functie, nu sunteti angajat!", "Acces interzis", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var addProductWindow = new AddProductWindow();
            addProductWindow.ShowDialog();
            _ = InitializeDataAsync(); // Reincarca datele dupa adaugarea produsului
        }

        private async void AddToCart(Product product)
        {
            if (product != null)
            {
                await _cartService.AddToCartAsync(product);
                // TODO: Adaugare notificare ca produsul a fost adaugat in cos
            }
        }

        private void EditProduct(Product product)
        {
            if (product != null)
            {
                var editWindow = new RestaurantApp.Views.AddProductWindow(product);
                editWindow.ShowDialog();
                _ = InitializeDataAsync(); // reincarca lista dupa editare
            }
        }

        private async void DeleteProduct(Product product)
        {
            if (product != null)
            {
                var result = MessageBox.Show($"Sigur vrei sa stergi produsul '{product.Name}'?", "Confirmare stergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _productService.DeleteProductAsync(product.ProductId);
                        await InitializeDataAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la stergerea produsului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void OpenCartWindow()
        {
            var app = (App)Application.Current;
            var cartService = app.Services.GetRequiredService<ICartService>();
            var orderService = app.Services.GetRequiredService<IOrderService>();
            var cartWindow = new CartWindow(cartService, orderService);
            cartWindow.Show();
        }

        private void OpenStock()
        {
            var user = AuthViewModel.CurrentUserStatic;
            if (user == null || !user.IsEmployee)
            {
                MessageBox.Show("Nu puteti accesa aceasta functie, nu sunteti angajat!", "Acces interzis", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var app = (App)Application.Current;
            var stockService = app.Services.GetRequiredService<IStockService>();
            var stockViewModel = new StockViewModel(stockService);
            var stockWindow = new StockWindow(stockViewModel);
            stockWindow.Show();
        }
    }
} 
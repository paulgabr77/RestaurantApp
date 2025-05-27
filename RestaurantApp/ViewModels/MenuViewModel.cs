using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;  
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace RestaurantApp.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IDishService _dishService;
        private readonly ICategoryService _categoryService;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Dish> _dishes;
        private string _searchTerm;
        private Category _selectedCategory;
        private Dish _selectedDish;
        private bool _isAuthenticated;

        public MenuViewModel(IDishService dishService, ICategoryService categoryService)
        {
            _dishService = dishService;
            _categoryService = categoryService;
            LoadDataCommand = new RelayCommand(async () => await LoadData());
            SearchCommand = new RelayCommand(async () => await Search());
            AddDishCommand = new RelayCommand(AddDish);

            // Inițializare comenzi pentru meniu
            OpenMenuCommand = new RelayCommand(OpenMenu);
            OpenOrdersCommand = new RelayCommand(OpenOrders);
            OpenReportsCommand = new RelayCommand(OpenReports);
            OpenStockCommand = new RelayCommand(OpenStock);
            OpenDiscountsCommand = new RelayCommand(OpenDiscounts);
            OpenDeliveriesCommand = new RelayCommand(OpenDeliveries);
            OpenAccountCommand = new RelayCommand(OpenAccount);
            
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
        public ICommand OpenStockCommand { get; }
        public ICommand OpenDiscountsCommand { get; }
        public ICommand OpenDeliveriesCommand { get; }
        public ICommand OpenAccountCommand { get; }

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
                    category.Dishes = category.Dishes?.Where(d => d.Name.Contains(SearchTerm) || (d.Description != null && d.Description.Contains(SearchTerm))).ToList();
                    category.Menus = category.Menus?.Where(m => m.Name.Contains(SearchTerm) || (m.Description != null && m.Description.Contains(SearchTerm))).ToList();
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
            MessageBox.Show("Funcționalitatea de meniu nu este implementată încă.");
        }
        private void OpenOrders()
        {
            MessageBox.Show("Funcționalitatea de comenzi nu este implementată încă.");
        }

        private void OpenReports()
        {
            MessageBox.Show("Funcționalitatea de rapoarte nu este implementată încă.");
        }

        private void OpenStock()
        {
            MessageBox.Show("Funcționalitatea de stoc nu este implementată încă.");
        }

        private void OpenDiscounts()
        {
            MessageBox.Show("Funcționalitatea de reduceri nu este implementată încă.");
        }

        private void OpenDeliveries()
        {
            MessageBox.Show("Funcționalitatea de livrări nu este implementată încă.");
        }

        private void OpenAccount()
        {
            MessageBox.Show("Funcționalitatea de cont nu este implementată încă.");
        }
    }
} 
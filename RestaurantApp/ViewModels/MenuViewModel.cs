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

        public MenuViewModel(IDishService dishService, ICategoryService categoryService)
        {
            _dishService = dishService;
            _categoryService = categoryService;
            LoadDataCommand = new RelayCommand(async () => await LoadData());
            SearchCommand = new RelayCommand(async () => await Search());
            AddDishCommand = new RelayCommand(AddDish);
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
    }
} 
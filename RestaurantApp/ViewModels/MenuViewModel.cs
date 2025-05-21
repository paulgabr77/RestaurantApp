using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;  
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantApp.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IDishService _dishService;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Dish> _dishes;
        private string _searchTerm;
        private Category _selectedCategory;
        private Dish _selectedDish;

        public MenuViewModel(IDishService dishService)
        {
            _dishService = dishService;
            LoadDataCommand = new RelayCommand(async () => await LoadData());
            SearchCommand = new RelayCommand(async () => await Search());
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

        private async Task LoadData()
        {
            var dishes = await _dishService.GetAllDishesAsync();
            Dishes = new ObservableCollection<Dish>(dishes);
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
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var dishes = await _dishService.SearchDishesAsync(SearchTerm);
                Dishes = new ObservableCollection<Dish>(dishes);
            }
        }
    }
} 
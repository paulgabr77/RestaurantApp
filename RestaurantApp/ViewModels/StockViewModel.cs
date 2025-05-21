using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace RestaurantApp.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        private readonly IStockService _stockService;
        private ObservableCollection<StockItem> _stockItems;
        private StockItem _selectedItem;
        private string _searchTerm;
        private string _errorMessage;

        public StockViewModel(IStockService stockService)
        {
            _stockService = stockService;
            _stockItems = new ObservableCollection<StockItem>();

            LoadStockCommand = new RelayCommand(async () => await LoadStock());
            AddStockCommand = new RelayCommand(async () => await AddStock());
            UpdateStockCommand = new RelayCommand(async () => await UpdateStock());
            DeleteStockCommand = new RelayCommand(async () => await DeleteStock());
            SearchCommand = new RelayCommand(async () => await Search());
        }

        public ObservableCollection<StockItem> StockItems
        {
            get => _stockItems;
            set => SetProperty(ref _stockItems, value);
        }

        public StockItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(ErrorVisibility));
            }
        }

        public Visibility ErrorVisibility => !string.IsNullOrEmpty(ErrorMessage) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand LoadStockCommand { get; }
        public ICommand AddStockCommand { get; }
        public ICommand UpdateStockCommand { get; }
        public ICommand DeleteStockCommand { get; }
        public ICommand SearchCommand { get; }

        private async Task LoadStock()
        {
            try
            {
                var items = await _stockService.GetAllStockItemsAsync();
                StockItems = new ObservableCollection<StockItem>(items);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la încărcarea stocurilor: {ex.Message}";
            }
        }

        private async Task AddStock()
        {
            try
            {
                var newItem = new StockItem
                {
                    DishId = 0, // Ar trebui selectat din UI
                    Quantity = 0,
                    MinimumQuantity = 0,
                    LastUpdated = DateTime.Now
                };

                var success = await _stockService.AddStockItemAsync(newItem);
                if (success)
                {
                    await LoadStock();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = "Nu s-a putut adăuga elementul în stoc.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la adăugarea în stoc: {ex.Message}";
            }
        }

        private async Task UpdateStock()
        {
            if (SelectedItem == null)
            {
                ErrorMessage = "Vă rugăm selectați un element din stoc.";
                return;
            }

            try
            {
                var success = await _stockService.UpdateStockItemAsync(SelectedItem);
                if (success)
                {
                    await LoadStock();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = "Nu s-a putut actualiza elementul din stoc.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la actualizarea stocului: {ex.Message}";
            }
        }

        private async Task DeleteStock()
        {
            if (SelectedItem == null)
            {
                ErrorMessage = "Vă rugăm selectați un element din stoc.";
                return;
            }

            try
            {
                var success = await _stockService.DeleteStockItemAsync(SelectedItem.StockItemId);
                if (success)
                {
                    await LoadStock();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = "Nu s-a putut șterge elementul din stoc.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la ștergerea din stoc: {ex.Message}";
            }
        }

        private async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                await LoadStock();
                return;
            }

            try
            {
                var items = await _stockService.GetAllStockItemsAsync();
                var filteredItems = items.Where(i => 
                    i.Dish.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                
                StockItems = new ObservableCollection<StockItem>(filteredItems);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la cautarea in stoc: {ex.Message}";
            }
        }
    }
} 
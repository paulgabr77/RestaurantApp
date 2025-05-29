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
    public class DiscountViewModel : ViewModelBase
    {
        private readonly IDiscountService _discountService;
        private ObservableCollection<Discount> _discounts;
        private Discount _selectedDiscount;
        private string _errorMessage;

        public DiscountViewModel(IDiscountService discountService)
        {
            _discountService = discountService;
            _discounts = new ObservableCollection<Discount>();

            LoadDiscountsCommand = new RelayCommand(async () => await LoadDiscounts());
            CreateDiscountCommand = new RelayCommand(async () => await CreateDiscount());
            UpdateDiscountCommand = new RelayCommand(async () => await UpdateDiscount());
            DeleteDiscountCommand = new RelayCommand(async () => await DeleteDiscount());
        }

        public ObservableCollection<Discount> Discounts
        {
            get => _discounts;
            set => SetProperty(ref _discounts, value);
        }

        public Discount SelectedDiscount
        {
            get => _selectedDiscount;
            set => SetProperty(ref _selectedDiscount, value);
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

        public ICommand LoadDiscountsCommand { get; }
        public ICommand CreateDiscountCommand { get; }
        public ICommand UpdateDiscountCommand { get; }
        public ICommand DeleteDiscountCommand { get; }

        private async Task LoadDiscounts()
        {
            try
            {
                var discounts = await _discountService.GetActiveDiscountsAsync();
                Discounts = new ObservableCollection<Discount>(discounts);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la incarcarea reducerilor: {ex.Message}";
            }
        }

        private async Task CreateDiscount()
        {
            try
            {
                var newDiscount = new Discount
                {
                    Code = GenerateDiscountCode(),
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1),
                    IsActive = true,
                    CurrentUses = 0
                };

                var success = await _discountService.CreateDiscountAsync(newDiscount);
                if (success!=null)
                {
                    await LoadDiscounts();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = "Nu s-a putut crea reducerea.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la crearea reducerii: {ex.Message}";
            }
        }

        private async Task UpdateDiscount()
        {
            if (SelectedDiscount == null)
            {
                ErrorMessage = "Va rugam selectati o reducere.";
                return;
            }

            try
            {
                var success = await _discountService.UpdateDiscountAsync(SelectedDiscount);
                if (success!=null)
                {
                    await LoadDiscounts();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = "Nu s-a putut actualiza reducerea.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la actualizarea reducerii: {ex.Message}";
            }
        }

        private async Task DeleteDiscount()
        {
            if (SelectedDiscount == null)
            {
                ErrorMessage = "Va rugam selectati o reducere.";
                return;
            }

            try
            {
                SelectedDiscount.IsActive = false;
                var success = await _discountService.UpdateDiscountAsync(SelectedDiscount);
                if (success != null)
                {
                    await LoadDiscounts();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = "Nu s-a putut sterge reducerea.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la stergerea reducerii: {ex.Message}";
            }
        }

        private string GenerateDiscountCode()
        {
            // Generare simpla de cod - in practica ar trebui sa fie mai complex
            return $"DISCOUNT{DateTime.Now.Ticks % 10000:D4}";
        }
    }
} 
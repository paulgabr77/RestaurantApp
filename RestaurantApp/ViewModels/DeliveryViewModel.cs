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
    public class DeliveryViewModel : ViewModelBase
    {
        private readonly IDeliveryService _deliveryService;
        private ObservableCollection<Delivery> _deliveries;
        private Delivery _selectedDelivery;
        private string _errorMessage;
        private string _address;
        private decimal _deliveryFee;
        private TimeSpan _estimatedTime;

        public DeliveryViewModel(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
            _deliveries = new ObservableCollection<Delivery>();

            LoadDeliveriesCommand = new RelayCommand(async () => await LoadDeliveries());
            CreateDeliveryCommand = new RelayCommand(async () => await CreateDelivery());
            UpdateDeliveryStatusCommand = new RelayCommand<string>(async (status) => await UpdateDeliveryStatus(status));
            CalculateDeliveryFeeCommand = new RelayCommand(async () => await CalculateDeliveryFee());
        }

        public ObservableCollection<Delivery> Deliveries
        {
            get => _deliveries;
            set => SetProperty(ref _deliveries, value);
        }

        public Delivery SelectedDelivery
        {
            get => _selectedDelivery;
            set => SetProperty(ref _selectedDelivery, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public decimal DeliveryFee
        {
            get => _deliveryFee;
            set => SetProperty(ref _deliveryFee, value);
        }

        public TimeSpan EstimatedTime
        {
            get => _estimatedTime;
            set => SetProperty(ref _estimatedTime, value);
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

        public ICommand LoadDeliveriesCommand { get; }
        public ICommand CreateDeliveryCommand { get; }
        public ICommand UpdateDeliveryStatusCommand { get; }
        public ICommand CalculateDeliveryFeeCommand { get; }

        private async Task LoadDeliveries()
        {
            try
            {
                var deliveries = await _deliveryService.GetActiveDeliveriesAsync();
                Deliveries = new ObservableCollection<Delivery>(deliveries);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la incarcarea livrarilor: {ex.Message}";
            }
        }

        private async Task CreateDelivery()
        {
            if (string.IsNullOrWhiteSpace(Address))
            {
                ErrorMessage = "Va rugam introduceti adresa de livrare.";
                return;
            }

            try
            {
                var newDelivery = new Delivery
                {
                    Address = Address,
                    Status = "Pending",
                    RequestedTime = DateTime.Now,
                    DeliveryFee = DeliveryFee
                };

                var success = await _deliveryService.CreateDeliveryAsync(newDelivery);
                if (success)
                {
                    await LoadDeliveries();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = "Nu s-a putut crea livrarea.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la crearea livrarii: {ex.Message}";
            }
        }

        private async Task UpdateDeliveryStatus(string status)
        {
            if (SelectedDelivery == null)
            {
                ErrorMessage = "Va rugam selectati o livrare.";
                return;
            }

            try
            {
                var success = await _deliveryService.UpdateDeliveryStatusAsync(SelectedDelivery.DeliveryId, status);
                if (success)
                {
                    await LoadDeliveries();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = "Nu s-a putut actualiza statusul livrarii.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la actualizarea statusului livrarii: {ex.Message}";
            }
        }

        private async Task CalculateDeliveryFee()
        {
            if (string.IsNullOrWhiteSpace(Address))
            {
                ErrorMessage = "Va rugam introduceti adresa de livrare.";
                return;
            }

            try
            {
                DeliveryFee = await _deliveryService.CalculateDeliveryFeeAsync(Address);
                EstimatedTime = await _deliveryService.EstimateDeliveryTimeAsync(Address);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la calcularea taxei de livrare: {ex.Message}";
            }
        }
    }
} 
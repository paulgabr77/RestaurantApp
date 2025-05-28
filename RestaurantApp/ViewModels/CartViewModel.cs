using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Linq;

namespace RestaurantApp.ViewModels
{
    public class CartViewModel : ViewModelBase
    {
        private readonly ICartService _cartService;
        private ObservableCollection<CartItem> _cartItems;
        private decimal _totalAmount;

        public CartViewModel(ICartService cartService)
        {
            _cartService = cartService;
            _cartItems = new ObservableCollection<CartItem>();

            LoadCartCommand = new RelayCommand(async () => await LoadCart());
            PlaceOrderCommand = new RelayCommand(async () => await PlaceOrder());
            RemoveItemCommand = new RelayCommand<CartItem>(async (item) => await RemoveItem(item));
            IncreaseQuantityCommand = new RelayCommand<CartItem>(async (item) => await UpdateQuantity(item, 1));
            DecreaseQuantityCommand = new RelayCommand<CartItem>(async (item) => await UpdateQuantity(item, -1));

            _ = LoadCart();
        }

        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
        }

        public ICommand LoadCartCommand { get; }
        public ICommand PlaceOrderCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }

        private async Task LoadCart()
        {
            var items = await _cartService.GetCartItemsAsync();
            CartItems.Clear();
            foreach (var item in items)
            {
                if (item != null && item.Product != null)
                {
                    CartItems.Add(item);
                }
            }
            await UpdateTotal();
        }

        private async Task RemoveItem(CartItem item)
        {
            if (item != null && item.Product != null)
            {
                await _cartService.RemoveFromCartAsync(item.Product.ProductId);
                await LoadCart();
            }
        }

        private async Task UpdateQuantity(CartItem item, int change)
        {
            if (item != null && item.Product != null)
            {
                var newQuantity = item.Quantity + change;
                if (newQuantity > 0)
                {
                    await _cartService.UpdateQuantityAsync(item.Product.ProductId, newQuantity);
                    await LoadCart();
                }
                else
                {
                    await RemoveItem(item);
                }
            }
        }

        private async Task UpdateTotal()
        {
            TotalAmount = await _cartService.GetTotalAsync();
        }

        private async Task PlaceOrder()
        {
            if (CartItems.Count == 0)
            {
                MessageBox.Show("Coșul este gol!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Implementare plasare comandă
            MessageBox.Show("Funcționalitatea de plasare comandă va fi implementată în curând!", "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
} 
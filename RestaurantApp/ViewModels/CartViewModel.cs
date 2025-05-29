using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System.Collections.Generic;

namespace RestaurantApp.ViewModels
{
    public class CartViewModel : ViewModelBase
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private ObservableCollection<CartItem> _cartItems;
        private decimal _totalAmount;
        private const decimal TRANSPORT_COST = 20.0m;

        public CartViewModel(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
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

        public decimal TransportCost => TRANSPORT_COST;

        public decimal GrandTotal => TotalAmount + TransportCost;

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
            OnPropertyChanged(nameof(GrandTotal));
        }

        private async Task PlaceOrder()
        {
            if (CartItems.Count == 0)
            {
                MessageBox.Show("Coșul este gol!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var user = RestaurantApp.ViewModels.AuthViewModel.CurrentUserStatic;
                if (user == null)
                {
                    MessageBox.Show("Nu sunteți autentificat!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var orderDetails = CartItems.Select(item => new OrderDetail
                {
                    ProductId = item.Product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price,
                    TotalPrice = item.TotalPrice
                }).ToList();

                var order = await _orderService.CreateOrderAsync(user.UserId, orderDetails);

                if (order != null)
                {
                    // Actualizăm statusul comenzii la "în curs de livrare"
                    await _orderService.UpdateOrderStatusAsync(order.OrderId, "în curs de livrare");

                    // Golim coșul
                    await _cartService.ClearCartAsync();
                    await LoadCart();

                    MessageBox.Show($"Comanda a fost plasată cu succes!\nCod comandă: {order.OrderCode}", 
                        "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("A apărut o eroare la plasarea comenzii.", 
                        "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"A apărut o eroare: {ex.Message}\n{ex.InnerException?.Message}", 
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
} 
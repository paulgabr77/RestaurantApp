using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace RestaurantApp.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly IDishService _dishService;
        private ObservableCollection<Order> _orders;
        private ObservableCollection<OrderDetail> _cartItems;
        private Order _selectedOrder;
        private string _searchTerm;
        private decimal _cartTotal;
        private string _errorMessage;

        public OrderViewModel(IOrderService orderService, IDishService dishService)
        {
            _orderService = orderService;
            _dishService = dishService;
            _cartItems = new ObservableCollection<OrderDetail>();

            LoadOrdersCommand = new RelayCommand(async () => await LoadOrders());
            PlaceOrderCommand = new RelayCommand(async () => await PlaceOrder());
            CancelOrderCommand = new RelayCommand(async () => await CancelOrder());
            SearchCommand = new RelayCommand(async () => await Search());
            AddToCartCommand = new RelayCommand<Dish>(async (dish) => await AddToCart(dish));
            RemoveFromCartCommand = new RelayCommand<OrderDetail>(async (item) => await RemoveFromCart(item));
            DeleteOrderCommand = new RelayCommand<Order>(async (order) => await DeleteOrder(order));
            FinalizeOrderCommand = new RelayCommand<Order>(async (order) => await FinalizeOrder(order));
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public ObservableCollection<OrderDetail> CartItems
        {
            get => _cartItems;
            set
            {
                SetProperty(ref _cartItems, value);
                OnPropertyChanged(nameof(CanPlaceOrder));
            }
        }


        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public decimal CartTotal
        {
            get => _cartTotal;
            set => SetProperty(ref _cartTotal, value);
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


        public bool CanPlaceOrder => CartItems.Count > 0;
        public Visibility ErrorVisibility => !string.IsNullOrEmpty(ErrorMessage) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand LoadOrdersCommand { get; }
        public ICommand PlaceOrderCommand { get; }
        public ICommand CancelOrderCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand DeleteOrderCommand { get; }
        public ICommand FinalizeOrderCommand { get; }

        private async Task LoadOrders()
        {
            var user = AuthViewModel.CurrentUserStatic;
            if (user == null)
            {
                Orders = new ObservableCollection<Order>();
                return;
            }
            var orders = await _orderService.GetUserOrdersAsync(user.UserId);
            Orders = new ObservableCollection<Order>(orders);
        }

        private async Task PlaceOrder()
        {
            if (CartItems.Count == 0)
            {
                ErrorMessage = "Coșul este gol.";
                return;
            }

            var user = AuthViewModel.CurrentUserStatic;
            if (user == null)
            {
                ErrorMessage = "Nu sunteți autentificat!";
                return;
            }
            var order = await _orderService.CreateOrderAsync(user.UserId, new List<OrderDetail>(CartItems));
            
            if (order != null)
            {
                CartItems.Clear();
                CartTotal = 0;
                await LoadOrders();
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = "A apărut o eroare la plasarea comenzii.";
            }
        }

        private async Task CancelOrder()
        {
            if (SelectedOrder == null)
            {
                ErrorMessage = "Vă rugăm selectați o comandă.";
                return;
            }

            var success = await _orderService.CancelOrderAsync(SelectedOrder.OrderId);
            if (success)
            {
                await LoadOrders();
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = "Nu s-a putut anula comanda.";
            }
        }

        private async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                await LoadOrders();
                return;
            }

            var orders = await _orderService.GetAllOrdersAsync();
            var filteredOrders = orders.Where(o => 
                o.OrderCode.Contains(SearchTerm) || 
                o.Status.Contains(SearchTerm));
            
            Orders = new ObservableCollection<Order>(filteredOrders);
        }

        private async Task AddToCart(Dish dish)
        {
            var existingItem = CartItems.FirstOrDefault(i => i.DishId == dish.DishId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CartItems.Add(new OrderDetail
                {
                    DishId = dish.DishId,
                    Quantity = 1,
                    UnitPrice = dish.Price,
                    TotalPrice = dish.Price
                });
            }

            await UpdateCartTotal();
        }

        private async Task RemoveFromCart(OrderDetail item)
        {
            CartItems.Remove(item);
            await UpdateCartTotal();
        }

        private async Task UpdateCartTotal()
        {
            CartTotal = await _orderService.CalculateOrderTotalAsync(new List<OrderDetail>(CartItems));
        }

        private async Task DeleteOrder(Order order)
        {
            if (order == null) return;
            var result = MessageBox.Show($"Sigur vrei sa stergi comanda {order.OrderCode}?", "Confirmare stergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                await _orderService.CancelOrderAsync(order.OrderId);
                await LoadOrders();
            }
        }

        private async Task FinalizeOrder(Order order)
        {
            if (order == null) return;
            var user = AuthViewModel.CurrentUserStatic;
            if (user == null || !user.IsEmployee)
            {
                MessageBox.Show("Nu puteti accesa aceasta functie, nu sunteti angajat!", "Acces interzis", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            await _orderService.UpdateOrderStatusAsync(order.OrderId, "finalizata");
            await LoadOrders();
        }
    }
} 
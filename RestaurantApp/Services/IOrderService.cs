using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId, List<OrderDetail> orderDetails);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> CancelOrderAsync(int orderId);
        Task<decimal> CalculateOrderTotalAsync(List<OrderDetail> orderDetails);
        Task<string> GenerateOrderCodeAsync();
        Task<DateTime> CalculateEstimatedDeliveryTimeAsync(Order order);
    }
} 
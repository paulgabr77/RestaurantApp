using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IReportService
    {
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Dish>> GetTopSellingDishesAsync(int count);
        Task<IEnumerable<Menu>> GetTopSellingMenusAsync(int count);
        Task<IEnumerable<User>> GetTopCustomersAsync(int count);
        Task<Dictionary<string, int>> GetOrdersByStatusDistributionAsync();
        Task<Dictionary<string, decimal>> GetRevenueByCategoryAsync(DateTime startDate, DateTime endDate);
    }
} 
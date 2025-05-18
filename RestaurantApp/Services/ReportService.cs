using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class ReportService : IReportService
    {
        private readonly RestaurantDbContext _context;

        public ReportService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Dish)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Menu)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Dish)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Menu)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<IEnumerable<Dish>> GetTopSellingDishesAsync(int count)
        {
            return await _context.OrderDetails
                .Where(od => od.DishId.HasValue)
                .GroupBy(od => od.DishId)
                .Select(g => new
                {
                    DishId = g.Key.Value,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(count)
                .Join(_context.Dishes,
                    x => x.DishId,
                    d => d.DishId,
                    (x, d) => d)
                .ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetTopSellingMenusAsync(int count)
        {
            return await _context.OrderDetails
                .Where(od => od.MenuId.HasValue)
                .GroupBy(od => od.MenuId)
                .Select(g => new
                {
                    MenuId = g.Key.Value,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(count)
                .Join(_context.Menus,
                    x => x.MenuId,
                    m => m.MenuId,
                    (x, m) => m)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetTopCustomersAsync(int count)
        {
            return await _context.Orders
                .GroupBy(o => o.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalSpent = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(count)
                .Join(_context.Users,
                    x => x.UserId,
                    u => u.UserId,
                    (x, u) => u)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetOrdersByStatusDistributionAsync()
        {
            var orders = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return orders.ToDictionary(x => x.Status, x => x.Count);
        }

        public async Task<Dictionary<string, decimal>> GetRevenueByCategoryAsync(DateTime startDate, DateTime endDate)
        {
            var revenue = await _context.OrderDetails
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .GroupBy(od => od.Dish.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Revenue = g.Sum(od => od.TotalPrice)
                })
                .ToListAsync();

            return revenue.ToDictionary(x => x.Category, x => x.Revenue);
        }
    }
} 
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly RestaurantDbContext _context;
        private readonly Random _random;

        public OrderService(RestaurantDbContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task<Order> CreateOrderAsync(int userId, List<OrderDetail> orderDetails)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderCode = await GenerateOrderCodeAsync(),
                Status = "Plasată",
                TotalAmount = await CalculateOrderTotalAsync(orderDetails),
                DeliveryFee = 10.00m, // Ar trebui citit din configurare
                DiscountAmount = 0.00m,
                EstimatedDeliveryTime = DateTime.Now.AddMinutes(45) // Ar trebui calculat dinamic
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var detail in orderDetails)
            {
                detail.OrderId = order.OrderId;
                await _context.OrderDetails.AddAsync(detail);
            }

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Dish)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Menu)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Dish)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Menu)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Dish)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Menu)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.Status == "Livrată" || order.Status == "Anulată")
                return false;

            order.Status = "Anulată";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalculateOrderTotalAsync(List<OrderDetail> orderDetails)
        {
            decimal total = 0;
            foreach (var detail in orderDetails)
            {
                if (detail.DishId.HasValue)
                {
                    var dish = await _context.Dishes.FindAsync(detail.DishId);
                    if (dish != null)
                    {
                        detail.UnitPrice = dish.Price;
                        detail.TotalPrice = dish.Price * detail.Quantity;
                        total += detail.TotalPrice;
                    }
                }
                else if (detail.MenuId.HasValue)
                {
                    var menu = await _context.Menus.FindAsync(detail.MenuId);
                    if (menu != null)
                    {
                        detail.UnitPrice = menu.Dishes.Sum(d => d.Price) * (1 - menu.DiscountPercentage / 100);
                        detail.TotalPrice = detail.UnitPrice * detail.Quantity;
                        total += detail.TotalPrice;
                    }
                }
            }
            return total;
        }

        public async Task<string> GenerateOrderCodeAsync()
        {
            string code;
            do
            {
                code = $"ORD{DateTime.Now:yyyyMMdd}{_random.Next(1000, 9999)}";
            } while (await _context.Orders.AnyAsync(o => o.OrderCode == code));

            return code;
        }

        public async Task<DateTime> CalculateEstimatedDeliveryTimeAsync(Order order)
        {
            // Logica simplă pentru calcularea timpului estimat de livrare
            // Ar trebui să ia în considerare distanța, traficul, etc.
            return DateTime.Now.AddMinutes(45);
        }
    }
} 
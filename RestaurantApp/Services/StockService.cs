using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class StockService : IStockService
    {
        private readonly RestaurantDbContext _context;

        public StockService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<StockItem> GetStockItemByIdAsync(int id)
        {
            return await _context.StockItems
                .Include(s => s.Dish)
                .FirstOrDefaultAsync(s => s.StockItemId == id);
        }

        public async Task<IEnumerable<StockItem>> GetAllStockItemsAsync()
        {
            return await _context.StockItems
                .Include(s => s.Dish)
                .OrderBy(s => s.Dish.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockItem>> GetLowStockItemsAsync()
        {
            return await _context.StockItems
                .Include(s => s.Dish)
                .Where(s => s.Quantity <= s.MinimumQuantity)
                .OrderBy(s => s.Quantity)
                .ToListAsync();
        }

        public async Task<bool> UpdateStockLevelAsync(int id, int quantity)
        {
            var stockItem = await _context.StockItems.FindAsync(id);
            if (stockItem == null)
                return false;

            stockItem.Quantity = quantity;
            stockItem.LastUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddStockItemAsync(StockItem item)
        {
            if (item == null)
                return false;

            item.LastUpdated = DateTime.Now;
            await _context.StockItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStockItemAsync(StockItem item)
        {
            if (item == null)
                return false;

            var existingItem = await _context.StockItems.FindAsync(item.StockItemId);
            if (existingItem == null)
                return false;

            existingItem.Quantity = item.Quantity;
            existingItem.MinimumQuantity = item.MinimumQuantity;
            existingItem.LastUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStockItemAsync(int id)
        {
            var stockItem = await _context.StockItems.FindAsync(id);
            if (stockItem == null)
                return false;

            _context.StockItems.Remove(stockItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckStockAvailabilityAsync(int dishId, int quantity)
        {
            var stockItem = await _context.StockItems
                .FirstOrDefaultAsync(s => s.DishId == dishId);

            return stockItem != null && stockItem.Quantity >= quantity;
        }

        public async Task<bool> ReserveStockAsync(int dishId, int quantity)
        {
            var stockItem = await _context.StockItems
                .FirstOrDefaultAsync(s => s.DishId == dishId);

            if (stockItem == null || stockItem.Quantity < quantity)
                return false;

            stockItem.Quantity -= quantity;
            stockItem.LastUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReleaseStockAsync(int dishId, int quantity)
        {
            var stockItem = await _context.StockItems
                .FirstOrDefaultAsync(s => s.DishId == dishId);

            if (stockItem == null)
                return false;

            stockItem.Quantity += quantity;
            stockItem.LastUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
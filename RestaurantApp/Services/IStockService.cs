using RestaurantApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IStockService
    {
        Task<StockItem> GetStockItemByIdAsync(int id);
        Task<IEnumerable<StockItem>> GetAllStockItemsAsync();
        Task<IEnumerable<StockItem>> GetLowStockItemsAsync();
        Task<bool> UpdateStockLevelAsync(int id, int quantity);
        Task<bool> AddStockItemAsync(StockItem item);
        Task<bool> UpdateStockItemAsync(StockItem item);
        Task<bool> DeleteStockItemAsync(int id);
        Task<bool> CheckStockAvailabilityAsync(int dishId, int quantity);
        Task<bool> ReserveStockAsync(int dishId, int quantity);
        Task<bool> ReleaseStockAsync(int dishId, int quantity);
    }
} 
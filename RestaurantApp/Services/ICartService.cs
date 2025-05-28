using RestaurantApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(Product product);
        Task<bool> RemoveFromCartAsync(int productId);
        Task<bool> UpdateQuantityAsync(int productId, int quantity);
        Task<decimal> GetTotalAsync();
        Task ClearCartAsync();
        Task<IEnumerable<CartItem>> GetCartItemsAsync();
    }
} 
using RestaurantApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IDiscountService
    {
        Task<bool> CreateDiscountAsync(Discount discount);
        Task<Discount> GetDiscountByIdAsync(int id);
        Task<Discount> GetDiscountByCodeAsync(string code);
        Task<IEnumerable<Discount>> GetActiveDiscountsAsync();
        Task<bool> UpdateDiscountAsync(Discount discount);
        Task<bool> DeleteDiscountAsync(int id);
        Task<bool> ApplyDiscountAsync(int orderId, string discountCode);
        Task<bool> ValidateDiscountCodeAsync(string code);
    }
} 
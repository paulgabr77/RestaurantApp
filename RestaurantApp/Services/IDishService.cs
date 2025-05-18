using RestaurantApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IDishService
    {
        Task<IEnumerable<Dish>> GetAllDishesAsync();
        Task<Dish> GetDishByIdAsync(int id);
        Task<IEnumerable<Dish>> GetDishesByCategoryAsync(int categoryId);
        Task<IEnumerable<Dish>> SearchDishesAsync(string searchTerm);
        Task<IEnumerable<Dish>> GetDishesByAllergenAsync(int allergenId, bool exclude = false);
        Task AddDishAsync(Dish dish);
        Task UpdateDishAsync(Dish dish);
        Task DeleteDishAsync(int id);
        Task<IEnumerable<Dish>> GetLowStockDishesAsync();
    }
} 
using RestaurantApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesWithDishesAndMenusAsync();
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> CreateCategoryAsync(Category category);
    }
} 
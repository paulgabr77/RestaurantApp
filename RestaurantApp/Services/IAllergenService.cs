using RestaurantApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IAllergenService
    {
        Task<IEnumerable<Allergen>> GetAllAllergensAsync();
        Task<Allergen> GetAllergenByIdAsync(int id);
        Task<Allergen> CreateAllergenAsync(Allergen allergen);
        Task<Allergen> UpdateAllergenAsync(Allergen allergen);
        Task<bool> DeleteAllergenAsync(int id);
    }
} 
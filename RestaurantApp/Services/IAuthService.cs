using RestaurantApp.Models;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string email, string password);
        Task<User> RegisterAsync(User user, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> UpdateUserProfileAsync(User user);
        Task<bool> IsEmailUniqueAsync(string email);
    }
} 
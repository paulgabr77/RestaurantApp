using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly RestaurantDbContext _context;

        public CategoryService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesWithDishesAndMenusAsync()
        {
            return await _context.Categories
                .Include(c => c.Dishes)
                    .ThenInclude(d => d.Allergens)
                .Include(c => c.Dishes)
                    .ThenInclude(d => d.Images)
                .Include(c => c.Menus)
                    .ThenInclude(m => m.Dishes)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
} 
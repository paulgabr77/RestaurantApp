using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class DishService : IDishService
    {
        private readonly IRepository<Dish> _dishRepository;
        private readonly RestaurantDbContext _context;

        public DishService(IRepository<Dish> dishRepository, RestaurantDbContext context)
        {
            _dishRepository = dishRepository;
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .ToListAsync();
        }

        public async Task<Dish> GetDishByIdAsync(int id)
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .FirstOrDefaultAsync(d => d.DishId == id);
        }

        public async Task<IEnumerable<Dish>> GetDishesByCategoryAsync(int categoryId)
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .Where(d => d.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Dish>> SearchDishesAsync(string searchTerm)
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .Where(d => d.Name.Contains(searchTerm) || d.Description.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Dish>> GetDishesByAllergenAsync(int allergenId, bool exclude = false)
        {
            var dishes = await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .ToListAsync();

            if (exclude)
            {
                return dishes.Where(d => !d.Allergens.Any(a => a.AllergenId == allergenId));
            }
            else
            {
                return dishes.Where(d => d.Allergens.Any(a => a.AllergenId == allergenId));
            }
        }

        public async Task AddDishAsync(Dish dish)
        {
            await _dishRepository.AddAsync(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            _dishRepository.Update(dish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDishAsync(int id)
        {
            var dish = await GetDishByIdAsync(id);
            if (dish != null)
            {
                _dishRepository.Remove(dish);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Dish>> GetLowStockDishesAsync()
        {
            var lowStockThreshold = 5.00m; // Ar trebui citit din configurare
            return await _context.Dishes
                .Where(d => d.TotalQuantity <= lowStockThreshold)
                .ToListAsync();
        }
    }
} 
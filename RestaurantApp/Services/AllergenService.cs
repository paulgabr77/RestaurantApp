using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class AllergenService : IAllergenService
    {
        private readonly RestaurantDbContext _context;

        public AllergenService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Allergen>> GetAllAllergensAsync()
        {
            return await _context.Allergens
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Allergen> GetAllergenByIdAsync(int id)
        {
            return await _context.Allergens
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AllergenId == id);
        }

        public async Task<Allergen> CreateAllergenAsync(Allergen allergen)
        {
            await _context.Allergens.AddAsync(allergen);
            await _context.SaveChangesAsync();
            return allergen;
        }

        public async Task<Allergen> UpdateAllergenAsync(Allergen allergen)
        {
            var existingAllergen = await _context.Allergens.FindAsync(allergen.AllergenId);
            if (existingAllergen == null)
                return null;

            existingAllergen.Name = allergen.Name;
            
            await _context.SaveChangesAsync();
            return existingAllergen;
        }

        public async Task<bool> DeleteAllergenAsync(int id)
        {
            var allergen = await _context.Allergens.FindAsync(id);
            if (allergen == null)
                return false;

            _context.Allergens.Remove(allergen);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
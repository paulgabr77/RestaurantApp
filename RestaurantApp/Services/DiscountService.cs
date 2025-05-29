using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly RestaurantDbContext _context;

        public DiscountService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<Discount> GetDiscountByCodeAsync(string code)
        {
            return await _context.Discounts
                .FirstOrDefaultAsync(d => d.Code == code && d.IsActive);
        }

        public async Task<IEnumerable<Discount>> GetActiveDiscountsAsync()
        {
            var now = DateTime.Now;
            return await _context.Discounts
                .Where(d => d.IsActive && d.StartDate <= now && d.EndDate >= now)
                .ToListAsync();
        }

        public async Task<bool> ApplyDiscountAsync(int orderId, string discountCode)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            var discount = await GetDiscountByCodeAsync(discountCode);
            if (discount == null) return false;

            // Verifica daca reducerea este valabila
            if (!IsDiscountValid(discount, order.TotalAmount))
                return false;

            // Aplica reducerea
            decimal discountValue = order.TotalAmount * discount.Percentage / 100;
            order.DiscountAmount = discountValue;
            order.TotalAmount = order.TotalAmount - discountValue;

            // Incrementeaza numarul de utilizari
            discount.CurrentUses++;
            if (discount.MaximumUses.HasValue && discount.CurrentUses >= discount.MaximumUses)
            {
                discount.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateDiscountAsync(Discount discount)
        {
            if (discount == null) return false;

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDiscountAsync(Discount discount)
        {
            if (discount == null) return false;

            var existingDiscount = await _context.Discounts.FindAsync(discount.DiscountId);
            if (existingDiscount == null) return false;

            _context.Entry(existingDiscount).CurrentValues.SetValues(discount);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Discount> GetDiscountByIdAsync(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }

        public async Task<bool> DeleteDiscountAsync(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null) return false;

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ValidateDiscountCodeAsync(string code)
        {
            var discount = await GetDiscountByCodeAsync(code);
            return discount != null && IsDiscountValid(discount, 0);
        }

        private bool IsDiscountValid(Discount discount, decimal orderAmount)
        {
            if (!discount.IsActive) return false;
            if (DateTime.Now < discount.StartDate || DateTime.Now > discount.EndDate) return false;
            if (discount.MinimumOrderAmount.HasValue && orderAmount < discount.MinimumOrderAmount) return false;
            if (discount.MaximumUses.HasValue && discount.CurrentUses >= discount.MaximumUses) return false;
            return true;
        }

        private decimal CalculateDiscountedAmount(decimal originalAmount, decimal discountPercentage)
        {
            return originalAmount * (1 - discountPercentage / 100);
        }
    }
} 
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class CartService : ICartService
    {
        private readonly RestaurantDbContext _context;
        private readonly Dictionary<int, int> _cartItems; // ProductId -> Quantity

        public CartService(RestaurantDbContext context)
        {
            _context = context;
            _cartItems = new Dictionary<int, int>();
        }

        public async Task<bool> AddToCartAsync(Product product)
        {
            if (product == null || !product.IsAvailable)
                return false;

            if (_cartItems.ContainsKey(product.ProductId))
            {
                _cartItems[product.ProductId]++;
            }
            else
            {
                _cartItems[product.ProductId] = 1;
            }

            return true;
        }

        public async Task<bool> RemoveFromCartAsync(int productId)
        {
            if (_cartItems.ContainsKey(productId))
            {
                _cartItems.Remove(productId);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateQuantityAsync(int productId, int quantity)
        {
            if (quantity <= 0)
                return await RemoveFromCartAsync(productId);

            if (_cartItems.ContainsKey(productId))
            {
                _cartItems[productId] = quantity;
                return true;
            }
            return false;
        }

        public async Task<decimal> GetTotalAsync()
        {
            decimal total = 0;
            foreach (var item in _cartItems)
            {
                var product = await _context.Products.FindAsync(item.Key);
                if (product != null)
                {
                    total += product.Price * item.Value;
                }
            }
            return total;
        }

        public async Task ClearCartAsync()
        {
            _cartItems.Clear();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            var cartItems = new List<CartItem>();
            foreach (var item in _cartItems)
            {
                var product = await _context.Products.FindAsync(item.Key);
                if (product != null)
                {
                    cartItems.Add(new CartItem
                    {
                        Product = product,
                        Quantity = item.Value,
                        TotalPrice = product.Price * item.Value
                    });
                }
            }
            return cartItems;
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
} 
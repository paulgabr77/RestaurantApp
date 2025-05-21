using System;

namespace RestaurantApp.Models
{
    public class StockItem
    {
        public int StockItemId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation properties
        public virtual Dish Dish { get; set; }
    }
} 
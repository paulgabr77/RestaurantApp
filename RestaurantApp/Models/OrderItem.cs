using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? DishId { get; set; }
        public int? MenuId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [StringLength(500)]
        public string? Notes { get; set; }

        public virtual Order Order { get; set; }
        public virtual Dish Dish { get; set; }
        public virtual Menu Menu { get; set; }
    }
} 
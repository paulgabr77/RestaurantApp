using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Menu
    {
        public int MenuId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string Price { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; }

        // Navigation properties
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
} 
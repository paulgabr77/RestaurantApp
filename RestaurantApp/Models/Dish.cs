using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Dish
    {
        public int DishId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PortionQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalQuantity { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Allergen> Allergens { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<DishImage> Images { get; set; }

        public Dish()
        {
            Allergens = new HashSet<Allergen>();
            Menus = new HashSet<Menu>();
            OrderDetails = new HashSet<OrderDetail>();
            Images = new HashSet<DishImage>();
        }
    }
} 
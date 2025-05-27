using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }

        public Category()
        {
            Dishes = new HashSet<Dish>();
            Menus = new HashSet<Menu>();
        }
    }
} 
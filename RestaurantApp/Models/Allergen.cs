using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class Allergen
    {
        public int AllergenId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<Dish> Dishes { get; set; }

        public Allergen()
        {
            Dishes = new HashSet<Dish>();
        }
    }
} 
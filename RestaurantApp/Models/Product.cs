using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RestaurantApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public string ImageUrl { get; set; }
        
        public virtual ICollection<Allergen> Allergens { get; set; }
        
        public string Ingredients { get; set; }
        
        [Required]
        public decimal Weight { get; set; }
        
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        
        public bool IsAvailable { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Product()
        {
            Allergens = new HashSet<Allergen>();
        }

        public string AllergensDisplay => Allergens != null && Allergens.Any() 
            ? string.Join(", ", Allergens.Select(a => a.Name)) 
            : "Fără alergeni";

        public string ImageFullPath => !string.IsNullOrEmpty(ImageUrl) ? System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageUrl.TrimStart('/', '\\')) : null;
    }
} 
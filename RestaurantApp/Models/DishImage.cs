using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class DishImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public int DishId { get; set; }
        public virtual Dish? Dish { get; set; }

        [Required]
        [StringLength(200)]
        public string ImagePath { get; set; }

        public bool IsMain { get; set; }
    }
} 
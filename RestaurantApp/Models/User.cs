using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        public string DeliveryAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool IsEmployee { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
    }
} 
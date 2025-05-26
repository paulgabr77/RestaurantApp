using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Pending, InProgress, Delivered, Cancelled
        public DateTime RequestedTime { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal DeliveryFee { get; set; }
        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
    }
} 
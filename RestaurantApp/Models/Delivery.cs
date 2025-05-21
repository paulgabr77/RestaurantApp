using System;

namespace RestaurantApp.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; } // Pending, InProgress, Delivered, Cancelled
        public DateTime RequestedTime { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public decimal DeliveryFee { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
    }
} 
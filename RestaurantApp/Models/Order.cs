using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderCode { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal DeliveryFee { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountAmount { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }

        // Navigation properties
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
} 
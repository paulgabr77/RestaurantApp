using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? MinimumOrderAmount { get; set; }
        public int? MaximumUses { get; set; }
        public int CurrentUses { get; set; }
        public bool IsForDelivery { get; set; }
    }
} 
using System;

namespace RestaurantApp.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string Code { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? MinimumOrderAmount { get; set; }
        public int? MaximumUses { get; set; }
        public int CurrentUses { get; set; }
        public bool IsForDelivery { get; set; }
    }
} 
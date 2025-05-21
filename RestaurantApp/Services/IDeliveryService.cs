using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IDeliveryService
    {
        Task<Delivery> GetDeliveryByIdAsync(int id);
        Task<IEnumerable<Delivery>> GetActiveDeliveriesAsync();
        Task<bool> CreateDeliveryAsync(Delivery delivery);
        Task<bool> UpdateDeliveryStatusAsync(int deliveryId, string status);
        Task<bool> UpdateDeliveryDetailsAsync(Delivery delivery);
        Task<decimal> CalculateDeliveryFeeAsync(string address);
        Task<TimeSpan> EstimateDeliveryTimeAsync(string address);
    }
} 
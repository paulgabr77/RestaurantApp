using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly RestaurantDbContext _context;

        public DeliveryService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<Delivery> GetDeliveryByIdAsync(int id)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .FirstOrDefaultAsync(d => d.DeliveryId == id);
        }

        public async Task<IEnumerable<Delivery>> GetActiveDeliveriesAsync()
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Where(d => d.Status != "Delivered" && d.Status != "Cancelled")
                .OrderBy(d => d.RequestedTime)
                .ToListAsync();
        }

        public async Task<bool> CreateDeliveryAsync(Delivery delivery)
        {
            if (delivery == null) return false;

            delivery.Status = "Pending";
            delivery.RequestedTime = DateTime.Now;
            delivery.EstimatedDeliveryTime = DateTime.Now.AddMinutes(30); // Exemplu simplu

            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDeliveryStatusAsync(int deliveryId, string status)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery == null) return false;

            delivery.Status = status;
            if (status == "Delivered")
            {
                delivery.ActualDeliveryTime = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDeliveryDetailsAsync(Delivery delivery)
        {
            if (delivery == null) return false;

            var existingDelivery = await _context.Deliveries.FindAsync(delivery.DeliveryId);
            if (existingDelivery == null) return false;

            _context.Entry(existingDelivery).CurrentValues.SetValues(delivery);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalculateDeliveryFeeAsync(string address)
        {
            // Implementare simpla - in practica ar trebui sa foloseasca un serviciu de geocodare
            // si sa calculeze distanta pana la adresa clientului
            return 10.0m; // Taxa fixa de 10 lei
        }

        public async Task<TimeSpan> EstimateDeliveryTimeAsync(string address)
        {
            // Implementare simpla - in practica ar trebui sa foloseasca un serviciu de geocodare
            // si sa calculeze timpul estimat pana la adresa clientului
            return TimeSpan.FromMinutes(30); // Timp estimat fix de 30 minute
        }
    }
} 
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(RestaurantDbContext context)
        {
            // Asigura-te ca baza de date este creata
            await context.Database.EnsureCreatedAsync();

            // Verifica daca exista deja categorii
            if (!await context.Categories.AnyAsync())
            {
                var categories = new Category[]
                {
                    new Category { Name = "Feluri principale" },
                    new Category { Name = "Aperitive" },
                    new Category { Name = "Deserturi" },
                    new Category { Name = "Bauturi" },
                    new Category { Name = "Salate" },
                    new Category { Name = "Supe" }
                };

                await context.Categories.AddRangeAsync(categories);
            }

            // Verifica daca exista deja alergeni
            if (!await context.Allergens.AnyAsync())
            {
                var allergens = new Allergen[]
                {
                    new Allergen { Name = "Gluten" },
                    new Allergen { Name = "Lactoza" },
                    new Allergen { Name = "Oua" },
                    new Allergen { Name = "Arahide" },
                    new Allergen { Name = "Nuci" },
                    new Allergen { Name = "Soia" },
                    new Allergen { Name = "Peste" },
                    new Allergen { Name = "Moluste" },
                    new Allergen { Name = "Sulfiti" },
                    new Allergen { Name = "Sesam" }
                };

                await context.Allergens.AddRangeAsync(allergens);
            }

            // Salveaza modificarile
            await context.SaveChangesAsync();
        }
    }
} 
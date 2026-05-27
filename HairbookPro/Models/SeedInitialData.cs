using System;
using System.Linq;
using System.Data.Entity.Migrations;

namespace Hairbookpro.Models
{
    public static class SeedInitialData
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Service categories
            context.ServiceCategories.AddOrUpdate(sc => sc.Name,
                new ServiceCategory { Name = "Šišanje" },
                new ServiceCategory { Name = "Farbanje" },
                new ServiceCategory { Name = "Njega kose" },
                new ServiceCategory { Name = "Stilizovanje" }
            );

            // Stylists
            context.Stylists.AddOrUpdate(s => s.FullName,
                new Stylist { FullName = "Ana Petrović", Specialization = "Šišanje i feniranje", Bio = "Senior frizer sa 8 godina iskustva.", IsActive = true },
                new Stylist { FullName = "Marko Jovanović", Specialization = "Farbanje", Bio = "Specijalista za koloraciju i balayage.", IsActive = true },
                new Stylist { FullName = "Milica Kovač", Specialization = "Njega kose", Bio = "Tretmani i obnova oštećene kose.", IsActive = true }
            );

            context.SaveChanges();

            // Services (attach to categories if available)
            var sisanje = context.ServiceCategories.FirstOrDefault(sc => sc.Name == "Šišanje");
            var farbanje = context.ServiceCategories.FirstOrDefault(sc => sc.Name == "Farbanje");
            var njega = context.ServiceCategories.FirstOrDefault(sc => sc.Name == "Njega kose");
            var stilizovanje = context.ServiceCategories.FirstOrDefault(sc => sc.Name == "Stilizovanje");

            if (sisanje != null)
            {
                context.Services.AddOrUpdate(s => s.Name,
                    new Service { Name = "Žensko šišanje", Description = "Profesionalno žensko šišanje.", Price = 20M, DurationMinutes = 45, IsActive = true, ServiceCategoryId = sisanje.Id },
                    new Service { Name = "Muško šišanje", Description = "Klasično i moderno muško šišanje.", Price = 12M, DurationMinutes = 30, IsActive = true, ServiceCategoryId = sisanje.Id }
                );
            }

            if (farbanje != null)
            {
                context.Services.AddOrUpdate(s => s.Name,
                    new Service { Name = "Balayage", Description = "Moderna tehnika farbanja.", Price = 80M, DurationMinutes = 180, IsActive = true, ServiceCategoryId = farbanje.Id }
                );
            }

            if (njega != null)
            {
                context.Services.AddOrUpdate(s => s.Name,
                    new Service { Name = "Keratin tretman", Description = "Obnova i zaglađivanje kose.", Price = 60M, DurationMinutes = 120, IsActive = true, ServiceCategoryId = njega.Id }
                );
            }

            if (stilizovanje != null)
            {
                context.Services.AddOrUpdate(s => s.Name,
                    new Service { Name = "Feniranje", Description = "Stilizovanje i feniranje.", Price = 15M, DurationMinutes = 35, IsActive = true, ServiceCategoryId = stilizovanje.Id }
                );
            }

            // Blog categories and posts
            context.BlogCategories.AddOrUpdate(bc => bc.Name,
                new BlogCategory { Name = "Savjeti" },
                new BlogCategory { Name = "Trendovi" },
                new BlogCategory { Name = "Njega" }
            );

            context.SaveChanges();

            var savjeti = context.BlogCategories.FirstOrDefault(bc => bc.Name == "Savjeti");
            var trendovi = context.BlogCategories.FirstOrDefault(bc => bc.Name == "Trendovi");

            if (savjeti != null)
            {
                context.BlogPosts.AddOrUpdate(p => p.Title,
                    new BlogPost { Title = "Kako njegovati kosu tokom ljeta", Content = "Koristite hidratantne maske i zaštitu od sunca...", CreatedAt = DateTime.Now, BlogCategoryId = savjeti.Id }
                );
            }

            if (trendovi != null)
            {
                context.BlogPosts.AddOrUpdate(p => p.Title,
                    new BlogPost { Title = "Top frizure ove sezone", Content = "Ove sezone dominiraju prirodni talasi i slojevito šišanje...", CreatedAt = DateTime.Now, BlogCategoryId = trendovi.Id }
                );
            }

            // Sample contact message
            context.ContactMessages.AddOrUpdate(c => new { c.Email, c.FullName },
                new ContactMessage { FullName = "Test User", Email = "test@example.com", Message = "Interested in balayage prices.", CreatedAt = DateTime.Now }
            );

            context.SaveChanges();
        }
    }
}

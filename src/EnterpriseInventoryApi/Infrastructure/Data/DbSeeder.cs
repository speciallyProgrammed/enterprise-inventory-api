using EnterpriseInventoryApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseInventoryApi.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IPasswordHasher<User> hasher)
    {
        if (!await db.Users.AnyAsync(u => u.Role == Roles.Admin))
        {
            var admin = new User
            {
                Email = "admin@local",
                Name = "Admin",
                Role = Roles.Admin
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");
            db.Users.Add(admin);
        }

        if (!await db.Products.AnyAsync())
        {
            db.Products.AddRange(
                new Product { Sku = "SKU-001", Name = "Laptop", Price = 1299.99m },
                new Product { Sku = "SKU-002", Name = "Keyboard", Price = 79.99m },
                new Product { Sku = "SKU-003", Name = "Mouse", Price = 39.99m }
            );
        }

        if (!await db.Stores.AnyAsync())
        {
            db.Stores.AddRange(
                new Store { Code = "STORE-001", Name = "Main Warehouse" },
                new Store { Code = "STORE-002", Name = "Retail Outlet" }
            );
        }

        await db.SaveChangesAsync();

        if (!await db.Stocks.AnyAsync())
        {
            var store = await db.Stores.FirstAsync();
            var products = await db.Products.Take(2).ToListAsync();

            db.Stocks.AddRange(
                new Stock { StoreId = store.Id, ProductId = products[0].Id, Quantity = 50 },
                new Stock { StoreId = store.Id, ProductId = products[1].Id, Quantity = 150 }
            );
        }

        await db.SaveChangesAsync();
    }
}

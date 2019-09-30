using System;
using Audacia.DataAccess.Tests.Helpers.Entities;

namespace Audacia.DataAccess.Tests.Helpers.Database
{
    public static class DummyDbContextExtensions
    {
        public static void Seed(this DummyDbContext dbContext)
        {
            dbContext.Customers.AddRange(
                new Customer { CustomerId = 1, EmailAddress = "bob@example.com", FirstName = "Bob", LastName = "Smith" },
                new Customer { CustomerId = 2, EmailAddress = "alice@example.com", FirstName = "Alice", LastName = "Smith" });

            dbContext.Products.AddRange(
                new Product { ProductId = 1, Description = "Mug", Price = 4.99m },
                new Product { ProductId = 2, Description = "Lunchbox", Price = 2.99m },
                new Product { ProductId = 3, Description = "Pencil", Price = 1.50m },
                new Product { ProductId = 4, Description = "Headphones", Price = 244.99m },
                new Product { ProductId = 5, Description = "Desk", Price = 74.99m });

            dbContext.Orders.AddRange(
                new Order { OrderId = 1, CustomerId = 1, DatePlaced = DateTime.Now },
                new Order { OrderId = 2, CustomerId = 1, DatePlaced = DateTime.Now },
                new Order { OrderId = 3, CustomerId = 2, DatePlaced = DateTime.Now });

            dbContext.OrderItems.AddRange(
                new OrderItem { OrderItemId = 1, OrderId = 1, ProductId = 1 },
                new OrderItem { OrderItemId = 2, OrderId = 2, ProductId = 2 },
                new OrderItem { OrderItemId = 3, OrderId = 2, ProductId = 3 },
                new OrderItem { OrderItemId = 4, OrderId = 2, ProductId = 5 },
                new OrderItem { OrderItemId = 5, OrderId = 3, ProductId = 4 },
                new OrderItem { OrderItemId = 6, OrderId = 3, ProductId = 3 });

            dbContext.SaveChanges();
        }
    }
}
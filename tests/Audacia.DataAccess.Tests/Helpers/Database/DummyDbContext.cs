using Audacia.DataAccess.Tests.Helpers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.Tests.Helpers.Database
{
    public class DummyDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }


        public DummyDbContext(DbContextOptions<DummyDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);
            modelBuilder.Entity<Customer>()
                .Property(c => c.EmailAddress)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(c => c.LastName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);
            modelBuilder.Entity<Product>()
                .Property(c => c.Description)
                .HasMaxLength(50)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Database
{
  public class AppDbContext : DbContext
  {
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<OrderItem>()
          .HasKey(oi => new { oi.OrderId, oi.ProductId });

      modelBuilder.Entity<Customer>()
          .HasMany(c => c.Addresses)
          .WithOne()
          .HasForeignKey("CustomerId");

      modelBuilder.Entity<CustomerAddress>().ToTable("CustomerAddresses");
      modelBuilder.Entity<LockerAddress>().ToTable("LockerAddresses");
      modelBuilder.Entity<DeliveryPointAddress>().ToTable("DeliveryPointAddresses");
    }
  }
}

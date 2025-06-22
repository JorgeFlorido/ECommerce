using ECommerce.Domain.Common.Models;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Order;
using ECommerce.Domain.Models.Product;
using ECommerce.Domain.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Database
{
  public class AppDbContext : DbContext
  {
    private readonly IPublisher _publisher;
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public AppDbContext(IPublisher publisher)
    {
      _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) : base(options)
    {
      _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    }

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      var entitiesWithEvents = ChangeTracker.Entries<Entity>()
          .Select(e => e.Entity)
          .Where(e => e.DomainEvents.Count != 0)
          .ToArray();

      foreach (var entity in entitiesWithEvents)
      {
        var events = entity.DomainEvents.ToArray();

        foreach (var domainEvent in events)
        {
          await _publisher.Publish(domainEvent, cancellationToken);
        }
      }

      return await base.SaveChangesAsync(cancellationToken);
    }
  }
}

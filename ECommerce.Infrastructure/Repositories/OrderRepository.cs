using ECommerce.Database;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
  public class OrderRepository : IOrderRepository
  {
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
      await _context.Orders.AddAsync(order, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
      await _context.Orders
        .Where(o => o.Id == orderId)
        .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
      return await _context.Orders
        .AsNoTracking()
        .Include(o => o.Items)
        .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
      return await _context.Orders
        .AsNoTracking()
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
      return await _context.Orders
        .AsNoTracking()
        .Where(o => o.CustomerId == customerId)
        .Include(o => o.Items)
        .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
      return await _context.Orders
        .AsNoTracking()
        .Where(o => o.Status == status)
        .Include(o => o.Items)
        .ToListAsync(cancellationToken);
    }

    public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
      await _context.Orders
        .Where(o => o.Id == order.Id)
        .ExecuteUpdateAsync(u => u
          .SetProperty(o => o.Status, order.Status)
          .SetProperty(o => o.TotalAmount, order.TotalAmount)
          .SetProperty(o => o.Payment, order.Payment), cancellationToken);
    }
  }
}

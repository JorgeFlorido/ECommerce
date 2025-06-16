using ECommerce.Database;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
  public class CustomerRepository : ICustomerRepository
  {
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
      await _context.Customers.AddAsync(customer, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
      await _context.Customers
        .Where(c => c.Id == customerId)
        .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
      return await _context.Customers
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
      return await _context.Customers
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
    }

    public async Task UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
      await _context.Customers
        .Where(c => c.Id == customer.Id)
        .ExecuteUpdateAsync(u => u
          .SetProperty(c => c.Name, customer.Name)
          .SetProperty(c => c.Surname, customer.Surname)
          .SetProperty(c => c.Email, customer.Email)
          .SetProperty(c => c.PhoneNumber, customer.PhoneNumber), cancellationToken);
    }
  }
}

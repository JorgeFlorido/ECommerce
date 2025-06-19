using ECommerce.Domain.Models;
using ECommerce.Domain.Models.User;

namespace ECommerce.Domain.Abstractions
{
  public interface ICustomerRepository
  {
    Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<CustomerAddress?> GetCustomerAddressByIdAsync(Guid addressId, CancellationToken cancellationToken = default);
    Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
    Task UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
    Task UpdateCustomerAddressAsync(CustomerAddress address, CancellationToken cancellationToken = default);
    Task DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
  }
}

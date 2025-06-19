using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Addresses
{
  public class GetCustomerAddressesQuery : IRequest<IEnumerable<CustomerAddress>>
  {
    public Guid CustomerId { get; set; }
  }
} 
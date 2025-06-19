using ECommerce.Application.Requests.Queries.Addresses;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.Handlers.Addresses
{
  internal class GetCustomerAddressesHandler : IRequestHandler<GetCustomerAddressesQuery, IEnumerable<CustomerAddress>>
  {
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerAddressesHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task<IEnumerable<CustomerAddress>> Handle(GetCustomerAddressesQuery request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);
      return customer == null ?
        throw new ArgumentException($"Customer with ID {request.CustomerId} not found.")
        : (IEnumerable<CustomerAddress>)customer.Addresses;
    }
  }
}
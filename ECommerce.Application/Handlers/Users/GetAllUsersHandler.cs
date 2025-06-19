using ECommerce.Application.Requests.Queries.Users;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.User;
using MediatR;

namespace ECommerce.Application.Handlers.Users
{
  internal class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<Customer>>
  {
    private readonly ICustomerRepository _customerRepository;
    
    public GetAllUsersHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task<IEnumerable<Customer>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
      var customers = await _customerRepository.GetAllCustomersAsync(cancellationToken);
      return customers;
    }
  }
} 
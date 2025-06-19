using ECommerce.Application.Requests.Queries.Users;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.User;
using MediatR;

namespace ECommerce.Application.Handlers.Users
{
  internal class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Customer?>
  {
    private readonly ICustomerRepository _customerRepository;
    
    public GetUserByIdHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task<Customer?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetCustomerByIdAsync(request.Id, cancellationToken);
      return customer;
    }
  }
} 
using ECommerce.Application.Requests.Commands.Users;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.User;
using MediatR;
using AutoMapper;

namespace ECommerce.Application.Handlers.Users
{
  internal class AddUserHandler : IRequestHandler<AddUserCommand, Guid>
  {
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    
    public AddUserHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
      var customer = _mapper.Map<Customer>(request);
      await _customerRepository.AddCustomerAsync(customer, cancellationToken);
      return customer.Id;
    }
  }
} 
using ECommerce.Application.Requests.Commands.Users;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.User;
using MediatR;
using AutoMapper;

namespace ECommerce.Application.Handlers.Users
{
  internal class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Customer>
  {
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    
    public UpdateUserHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<Customer> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
      var existingCustomer = await _customerRepository.GetCustomerByIdAsync(request.UserId, cancellationToken);
      if (existingCustomer == null)
      {
        throw new ArgumentException($"Customer with ID {request.UserId} not found.");
      }
      
      _mapper.Map(request, existingCustomer);
      
      await _customerRepository.UpdateCustomerAsync(existingCustomer, cancellationToken);
      return existingCustomer;
    }
  }
} 
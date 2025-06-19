using ECommerce.Application.Requests.Commands.Users;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.User;
using MediatR;

namespace ECommerce.Application.Handlers.Users
{
  internal class AddUserHandler : IRequestHandler<AddUserCommand, Guid>
  {
    private readonly ICustomerRepository _customerRepository;
    
    public AddUserHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
      var customer = new Customer
      {
        Email = request.Email,
        PasswordHash = request.Password, // In real app, this should be hashed
        Name = request.Name,
        Surname = request.Surname,
        PhoneNumber = request.PhoneNumber
      };
      
      await _customerRepository.AddCustomerAsync(customer, cancellationToken);
      return customer.Id;
    }
  }
} 
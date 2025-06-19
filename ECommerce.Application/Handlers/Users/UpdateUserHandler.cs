using ECommerce.Application.Requests.Commands.Users;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.User;
using MediatR;

namespace ECommerce.Application.Handlers.Users
{
  internal class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Customer>
  {
    private readonly ICustomerRepository _customerRepository;
    
    public UpdateUserHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task<Customer> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
      var existingCustomer = await _customerRepository.GetCustomerByIdAsync(request.UserId, cancellationToken);
      if (existingCustomer == null)
      {
        throw new ArgumentException($"Customer with ID {request.UserId} not found.");
      }
      
      existingCustomer.Email = request.Email ?? existingCustomer.Email;
      existingCustomer.Name = request.Name ?? existingCustomer.Name;
      existingCustomer.Surname = request.Surname ?? existingCustomer.Surname;
      existingCustomer.PhoneNumber = request.PhoneNumber ?? existingCustomer.PhoneNumber;
      
      await _customerRepository.UpdateCustomerAsync(existingCustomer, cancellationToken);
      return existingCustomer;
    }
  }
} 
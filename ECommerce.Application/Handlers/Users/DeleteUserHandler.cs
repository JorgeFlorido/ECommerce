using ECommerce.Application.Requests.Commands.Users;
using ECommerce.Domain.Abstractions;
using MediatR;

namespace ECommerce.Application.Handlers.Users
{
  internal class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
  {
    private readonly ICustomerRepository _customerRepository;
    
    public DeleteUserHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
      var existingCustomer = await _customerRepository.GetCustomerByIdAsync(request.UserId, cancellationToken);
      if (existingCustomer == null)
      {
        throw new ArgumentException($"Customer with ID {request.UserId} not found.");
      }
      
      await _customerRepository.DeleteCustomerAsync(request.UserId, cancellationToken);
    }
  }
} 
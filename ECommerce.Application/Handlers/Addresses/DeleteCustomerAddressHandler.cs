using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Abstractions;
using MediatR;

namespace ECommerce.Application.Handlers.Addresses
{
  internal class DeleteCustomerAddressHandler : IRequestHandler<DeleteCustomerAddressCommand>
  {
    private readonly ICustomerRepository _customerRepository;
    
    public DeleteCustomerAddressHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task Handle(DeleteCustomerAddressCommand request, CancellationToken cancellationToken)
    {
      var allCustomers = await _customerRepository.GetAllCustomersAsync(cancellationToken);

      var customerWithAddress = allCustomers.FirstOrDefault(c => c.Addresses.Any(a => a.Id == request.AddressId)) 
        ?? throw new ArgumentException($"Address with ID {request.AddressId} not found.");

      var addressToRemove = customerWithAddress.Addresses.First(a => a.Id == request.AddressId);
      customerWithAddress.Addresses.Remove(addressToRemove);
      
      await _customerRepository.UpdateCustomerAsync(customerWithAddress, cancellationToken);
    }
  }
} 
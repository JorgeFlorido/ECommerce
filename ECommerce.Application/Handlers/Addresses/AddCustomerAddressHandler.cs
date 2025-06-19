using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.Handlers.Addresses
{
  internal class AddCustomerAddressHandler : IRequestHandler<AddCustomerAddressCommand, CustomerAddress>
  {
    private readonly ICustomerRepository _customerRepository;
    
    public AddCustomerAddressHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task<CustomerAddress> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken) 
        ?? throw new ArgumentException($"Customer with ID {request.CustomerId} not found.");
      
      var address = new CustomerAddress
      {
        Id = Guid.NewGuid(),
        CustomerId = request.CustomerId,
        Street = request.Street,
        City = request.City,
        State = request.State,
        PostalCode = request.PostalCode,
        Country = request.Country,
        IsPrimary = request.IsPrimary
      };
      
      if (request.IsPrimary)
      {
        foreach (var existingAddress in customer.Addresses)
        {
          existingAddress.IsPrimary = false;
        }
      }
      
      customer.Addresses.Add(address);
      await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);
      
      return address;
    }
  }
} 
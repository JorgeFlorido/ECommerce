using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.Handlers.Addresses
{
  internal class UpdateCustomerAddressHandler : IRequestHandler<UpdateCustomerAddressCommand, CustomerAddress>
  {
    private readonly ICustomerRepository _customerRepository;
    
    public UpdateCustomerAddressHandler(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task<CustomerAddress> Handle(UpdateCustomerAddressCommand request, CancellationToken cancellationToken)
    {
      // Get the address directly by ID
      var addressToUpdate = await _customerRepository.GetCustomerAddressByIdAsync(request.AddressId, cancellationToken);
      
      if (addressToUpdate == null)
      {
        throw new ArgumentException($"Address with ID {request.AddressId} not found.");
      }
      
      // Update address properties
      addressToUpdate.Street = request.Street ?? addressToUpdate.Street;
      addressToUpdate.City = request.City ?? addressToUpdate.City;
      addressToUpdate.State = request.State ?? addressToUpdate.State;
      addressToUpdate.PostalCode = request.PostalCode ?? addressToUpdate.PostalCode;
      addressToUpdate.Country = request.Country;
      
      // Handle primary address logic
      if (request.IsPrimary && !addressToUpdate.IsPrimary)
      {
        // Get the customer to unset other primary addresses
        var customer = await _customerRepository.GetCustomerByIdAsync(addressToUpdate.CustomerId, cancellationToken);
        if (customer != null)
        {
          foreach (var address in customer.Addresses)
          {
            address.IsPrimary = false;
            await _customerRepository.UpdateCustomerAddressAsync(address, cancellationToken);
          }
        }
        addressToUpdate.IsPrimary = true;
      }
      else if (!request.IsPrimary && addressToUpdate.IsPrimary)
      {
        addressToUpdate.IsPrimary = false;
      }
      
      // Update the address directly
      await _customerRepository.UpdateCustomerAddressAsync(addressToUpdate, cancellationToken);
      
      return addressToUpdate;
    }
  }
} 
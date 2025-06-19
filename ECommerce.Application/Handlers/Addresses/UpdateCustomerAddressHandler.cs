using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;
using MediatR;
using AutoMapper;

namespace ECommerce.Application.Handlers.Addresses
{
  internal class UpdateCustomerAddressHandler : IRequestHandler<UpdateCustomerAddressCommand, CustomerAddress>
  {
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    
    public UpdateCustomerAddressHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<CustomerAddress> Handle(UpdateCustomerAddressCommand request, CancellationToken cancellationToken)
    {
      // Get the address directly by ID
      var addressToUpdate = await _customerRepository.GetCustomerAddressByIdAsync(request.AddressId, cancellationToken);
      
      if (addressToUpdate == null)
      {
        throw new ArgumentException($"Address with ID {request.AddressId} not found.");
      }
      
      _mapper.Map(request, addressToUpdate);
      
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
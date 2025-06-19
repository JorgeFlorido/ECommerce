using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;
using MediatR;
using AutoMapper;

namespace ECommerce.Application.Handlers.Addresses
{
  internal class AddCustomerAddressHandler : IRequestHandler<AddCustomerAddressCommand, CustomerAddress>
  {
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    
    public AddCustomerAddressHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<CustomerAddress> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken) 
        ?? throw new ArgumentException($"Customer with ID {request.CustomerId} not found.");
      
      var address = _mapper.Map<CustomerAddress>(request);
      address.Id = Guid.NewGuid();
      
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
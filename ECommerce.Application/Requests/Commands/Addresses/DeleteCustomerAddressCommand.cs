using MediatR;

namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class DeleteCustomerAddressCommand : IRequest
  {
    public Guid AddressId { get; set; }
  }
} 
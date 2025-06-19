using ECommerce.Domain.Enums;

namespace ECommerce.Application.Requests.Commands.Addresses
{
  // Discriminated union for shipping addresses
  public abstract class OrderShippingAddressCommand
  {
    public abstract AddressType Type { get; }
  }
} 
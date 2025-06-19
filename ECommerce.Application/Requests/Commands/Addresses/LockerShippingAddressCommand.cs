using ECommerce.Domain.Enums;

namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class LockerShippingAddressCommand : OrderShippingAddressCommand
  {
    public override AddressType Type => AddressType.Locker;
    public LockerAddressCommand Address { get; set; } = null!;
  }
} 
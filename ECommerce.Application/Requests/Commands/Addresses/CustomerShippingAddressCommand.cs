using ECommerce.Domain.Enums;

namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class CustomerShippingAddressCommand : OrderShippingAddressCommand
  {
    public override AddressType Type => AddressType.CustomerAddress;
    public CustomerAddressCommand Address { get; set; } = null!;
  }
} 
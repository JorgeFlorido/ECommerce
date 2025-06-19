using ECommerce.Domain.Enums;

namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class DeliveryPointShippingAddressCommand : OrderShippingAddressCommand
  {
    public override AddressType Type => AddressType.DeliveryPoint;
    public DeliveryPointAddressCommand Address { get; set; } = null!;
  }
} 
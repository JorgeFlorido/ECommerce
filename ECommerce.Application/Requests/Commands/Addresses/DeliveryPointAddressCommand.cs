namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class DeliveryPointAddressCommand : AddressCommand
  {
    public string? ShopName { get; set; }
    public string? ContactNumber { get; set; }
  }
} 
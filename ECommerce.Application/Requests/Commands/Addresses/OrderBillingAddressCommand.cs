namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class OrderBillingAddressCommand
  {
    public CustomerAddressCommand CustomerAddress { get; set; } = null!;
  }
} 
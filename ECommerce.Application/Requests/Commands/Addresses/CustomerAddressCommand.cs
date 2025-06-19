namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class CustomerAddressCommand : AddressCommand
  {
    public Guid CustomerId { get; set; }
    public bool IsPrimary { get; set; }
  }
} 
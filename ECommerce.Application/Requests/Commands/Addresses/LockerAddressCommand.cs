namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class LockerAddressCommand : AddressCommand
  {
    public string? LockerId { get; set; }
    public string? Provider { get; set; }
  }
} 
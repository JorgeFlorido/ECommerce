using ECommerce.Domain.Enums;

namespace ECommerce.Application.Requests.Commands.Addresses
{
  public class AddressCommand
  {
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public Country Country { get; set; }
  }
} 
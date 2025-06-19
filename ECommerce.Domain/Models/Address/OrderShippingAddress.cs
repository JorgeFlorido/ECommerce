using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Models
{
  public class OrderShippingAddress
  {
    public Guid Id { get; set; }
    public AddressType Type { get; set; }
    public Address Address { get; set; } = null!;
  }
} 
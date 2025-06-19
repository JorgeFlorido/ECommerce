using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Models
{
  public abstract class Address
  {
    public Guid Id { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public PostalCode? PostalCode { get; set; }
    public Country Country { get; set; }
  }
}

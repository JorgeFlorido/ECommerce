namespace ECommerce.Domain.Models
{
  public class OrderBillingAddress
  {
    public Guid Id { get; set; }
    public CustomerAddress CustomerAddress { get; set; } = null!;
  }
} 
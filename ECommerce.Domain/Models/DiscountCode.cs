namespace ECommerce.Domain.Models
{
  public class DiscountCode
  {
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal Amount { get; set; }
  }
}

using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Models
{
  public class Payment
  {
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
  }
}

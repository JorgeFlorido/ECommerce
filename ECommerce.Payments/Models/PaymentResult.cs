using ECommerce.Payments.Enums;

namespace ECommerce.Domain.Models.Payment
{
  public class PaymentResult
  {
    public PaymentStatus Status { get; set; }
    public string TransactionId { get; set; }
    public string RedirectUrl { get; set; }
    public string ProviderMessage { get; set; }
  }
}

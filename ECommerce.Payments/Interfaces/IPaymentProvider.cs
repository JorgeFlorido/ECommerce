using ECommerce.Domain.Models.Payment;
using ECommerce.Payments.Models;

namespace ECommerce.Domain.Abstractions
{
  public interface IPaymentProvider
  {
    public Task<PaymentResult> AuthorizeAsync(PaymentRequest request);
    public Task<PaymentResult> CaptureAsync(PaymentRequest request);
    public Task<PaymentResult> RefundAsync(PaymentRequest request);
  }
}

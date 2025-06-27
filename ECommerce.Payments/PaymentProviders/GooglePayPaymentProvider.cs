using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Payment;
using ECommerce.Payments.Models;

namespace ECommerce.Payments.PaymentProviders
{
  public class GooglePayPaymentProvider : IPaymentProvider
  {
    public Task<PaymentResult> AuthorizeAsync(PaymentRequest request)
    {
      throw new NotImplementedException();
    }

    public Task<PaymentResult> CaptureAsync(PaymentRequest request)
    {
      throw new NotImplementedException();
    }

    public Task<PaymentResult> RefundAsync(PaymentRequest request)
    {
      throw new NotImplementedException();
    }
  }
}

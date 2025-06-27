using ECommerce.Domain.Abstractions;
using ECommerce.Payments.Enums;
using ECommerce.Payments.Interfaces;
using ECommerce.Payments.PaymentProviders;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Payments.Router
{
  public class PaymentRouter : IPaymentRouter
  {
    private readonly IServiceProvider _serviceProvider;

    public PaymentRouter(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IPaymentProvider GetPaymentProvider(PaymentMethod paymentMethod) => paymentMethod switch
    {
      PaymentMethod.PayPal => _serviceProvider.GetRequiredService<PaypalPaymentProvider>(),
      PaymentMethod.GooglePay => _serviceProvider.GetRequiredService<GooglePayPaymentProvider>(),
      PaymentMethod.Klarna => _serviceProvider.GetRequiredService<KlarnaPaymentProvider>(),
      PaymentMethod.AmazonPay => _serviceProvider.GetRequiredService<AmazonPayPaymentProvider>(),
      PaymentMethod.CreditCard => _serviceProvider.GetRequiredService<CreditCardPaymentProvider>(),
      _ => throw new NotSupportedException($"Payment method {paymentMethod} is not supported.")
    };
  }
}

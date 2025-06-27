using ECommerce.Domain.Abstractions;
using ECommerce.Payments.Enums;

namespace ECommerce.Payments.Interfaces
{
  internal interface IPaymentRouter
  {
    IPaymentProvider GetPaymentProvider(PaymentMethod paymentMethod);
  }
}

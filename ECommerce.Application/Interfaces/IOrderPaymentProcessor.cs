using ECommerce.Domain.Models.Order;

namespace ECommerce.Application.Interfaces
{
  public interface IOrderPaymentProcessor
  {
    Task<bool> ProcessPaymentAsync(Order order, CancellationToken ct);
    Task<bool> RefundPaymentAsync(Order order, CancellationToken ct);
  }
}

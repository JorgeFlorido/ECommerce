using ECommerce.Domain.Models;

namespace ECommerce.Application.Interfaces
{
  public interface IShippingService
  {
    Task<decimal> CalculateShippingCostAsync(OrderShippingAddress orderShippingAddress, CancellationToken cancellationToken = default);

    Task<string> GenerateShippingLabelAsync(Guid orderId, CancellationToken cancellationToken = default);
  }
}

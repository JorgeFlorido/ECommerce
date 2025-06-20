using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services
{
  public class ShippingService : IShippingService
  {
    public Task<decimal> CalculateShippingCostAsync(OrderShippingAddress orderShippingAddress, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<string> GenerateShippingLabelAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
  }
}

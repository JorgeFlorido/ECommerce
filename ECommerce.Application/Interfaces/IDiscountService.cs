using ECommerce.Domain.Models;

namespace ECommerce.Application.Interfaces
{
  public interface IDiscountService
  {
    Task<DiscountCode?> GetDiscountCodeAsync(string code, CancellationToken cancellationToken = default);
  }
}

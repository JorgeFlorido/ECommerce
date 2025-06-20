using ECommerce.Domain.Enums;

namespace ECommerce.Application.Interfaces
{
  public interface ITaxService
  {
    Task<decimal> CalculateTaxAsync(decimal amount, CancellationToken cancellationToken = default);

    Task<decimal> GetTaxRateAsync(Country country, CancellationToken cancellationToken = default);
  }
}

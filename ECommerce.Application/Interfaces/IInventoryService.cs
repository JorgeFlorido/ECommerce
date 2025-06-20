namespace ECommerce.Application.Interfaces
{
  public interface IInventoryService
  {
    Task<bool> IsProductInStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);

    Task UpdateProductStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
  }
}

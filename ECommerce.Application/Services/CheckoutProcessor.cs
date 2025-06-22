using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Domain.Models.Order;

namespace ECommerce.Application.Services
{
  public class CheckoutProcessor : ICheckoutProcessor
  {
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    private readonly IShippingService _shippingService;
    private readonly IInventoryService _inventoryService;

    public CheckoutProcessor(
        ITaxService taxService,
        IDiscountService discountService,
        IShippingService shippingService,
        IInventoryService inventoryService)
    {
      _taxService = taxService;
      _discountService = discountService;
      _shippingService = shippingService;
      _inventoryService = inventoryService;
    }

    public async Task<OrderCostCalculationResult> CalculateOrderCostAsync(OrderCostCalculationQuery query, CancellationToken cancellationToken)
    {
      var gross = query.Items.Sum(i => i.TotalPrice);
      var taxRate = await _taxService.GetTaxRateAsync(query.BillingAddress.CustomerAddress.Country, cancellationToken);
      var shipping = await _shippingService.CalculateShippingCostAsync(query.ShippingAddress, cancellationToken);
      var discount = await _discountService.GetDiscountCodeAsync(query.DiscountCode, cancellationToken);

      return new OrderCostCalculationResult
      {
        GrossAmount = gross,
        TaxAmount = gross * taxRate,
        ShippingCost = shipping,
        DiscountCode = discount
      };
    }

    public async Task<List<Guid>> GetOutOfStockItemsAsync(IEnumerable<OrderItem> items, CancellationToken ct)
    {
      var outOfStock = new List<Guid>();
      foreach (var item in items)
      {
        var inStock = await _inventoryService.IsProductInStockAsync(item.Id, item.Quantity, ct);
        if (!inStock) outOfStock.Add(item.Id);
      }
      return outOfStock;
    }
  }
}

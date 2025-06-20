using ECommerce.Domain.Models;

namespace ECommerce.Application.Models
{
  public class OrderCostCalculationResult
  {
    public decimal GrossAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TaxAmount { get; set; }
    public DiscountCode? DiscountCode { get; set; }
    public decimal OtherFees { get; set; } = 0.0m;
    public decimal TotalAmount => GrossAmount + ShippingCost + TaxAmount - (DiscountCode?.Amount ?? 0.0m) + OtherFees;
  }
}

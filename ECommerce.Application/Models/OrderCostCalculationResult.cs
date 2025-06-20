namespace ECommerce.Application.Models
{
  public class OrderCostCalculationResult
  {
    public decimal GrossAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal OtherFees { get; set; } = 0.0m;
    public decimal TotalAmount => GrossAmount + ShippingCost + TaxAmount - DiscountAmount + OtherFees;
  }
}

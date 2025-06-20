namespace ECommerce.API.Models.Responses.Order
{
  public class OrderCostCalculationResponse
  {
    public decimal GrossAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal OtherFees { get; set; }
    public decimal NetAmount { get; set; }
    public decimal TotalAmount { get; set; }
  }
}

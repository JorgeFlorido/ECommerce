using ECommerce.Domain.Common.Models;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Models.Order
{
  public class Order : Entity
  {
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal ShippingCost { get; set; } = 0.0m;
    public decimal TaxAmount { get; set; } = 0.0m;
    public decimal OtherFees { get; set; } = 0.0m;
    public decimal TotalAmount => GrossAmount + ShippingCost + TaxAmount + OtherFees - (DiscountCode?.Amount ?? 0.0m);
    public List<OrderItem> Items { get; set; } = [];
    public Payment? Payment { get; set; }
    public OrderShippingAddress? ShippingAddress { get; set; }
    public OrderBillingAddress? BillingAddress { get; set; }
    public DiscountCode? DiscountCode { get; set; } 
  }
}

using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Models.Orders
{
  public class Order
  {
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public Payment? Payment { get; set; }
  }
}

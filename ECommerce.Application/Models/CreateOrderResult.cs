namespace ECommerce.Application.Models
{
  public class CreateOrderResult
  {
    public bool Success { get; set; }
    public Guid? OrderId { get; set; }
    public List<Guid> OutOfStockItemIds { get; set; } = [];
    public string? Message { get; set; }
  }
}
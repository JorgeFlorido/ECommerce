namespace ECommerce.API.Models.Requests.Order
{
  public class OrderItemRequest
  {
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
  }
}

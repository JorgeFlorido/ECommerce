namespace ECommerce.Domain.Models
{
  public class CartItem
  {
    public Guid ProductId { get; set; } 
    public int Quantity { get; set; }
  }
}

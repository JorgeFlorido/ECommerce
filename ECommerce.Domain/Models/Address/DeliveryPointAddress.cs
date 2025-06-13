namespace ECommerce.Domain.Models
{
  public class DeliveryPointAddress : Address
  {
    public string? ShopName { get; set; }
    public string? ContactNumber { get; set; }
  }
}

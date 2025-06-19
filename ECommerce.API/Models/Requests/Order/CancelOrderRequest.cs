namespace ECommerce.API.Models.Requests.Order
{
  public class CancelOrderRequest
  {
    public string? Reason { get; set; }
    public bool RefundPayment { get; set; } = true;
  }
} 
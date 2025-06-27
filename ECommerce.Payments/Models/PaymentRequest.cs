namespace ECommerce.Payments.Models
{
  public class PaymentRequest
  {
    public string OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PaymentMethod { get; set; }
    public string ReturnUrl { get; set; } 
    public string CancelUrl { get; set; }
  }
}

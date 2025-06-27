namespace ECommerce.Payments.Options
{
  public class PaypalOptions
  {
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string ApiBaseUrl { get; set; } = "https://api-m.sandbox.paypal.com";
  }
}

namespace ECommerce.Payments.Options
{
  public class AmazonPayOptions
  {
    public string ApiBaseUrl { get; set; } = "https://pay-api.amazon.com";
    public string PublicKeyId { get; set; }
    public string PrivateKey { get; set; } // PEM format
    public string MerchantId { get; set; }
    public string Region { get; set; } = "us";
  }
}

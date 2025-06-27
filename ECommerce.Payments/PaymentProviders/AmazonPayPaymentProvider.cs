using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Payment;
using ECommerce.Payments.Enums;
using ECommerce.Payments.Helpers;
using ECommerce.Payments.Models;
using ECommerce.Payments.Options;
using System.Text;
using System.Text.Json;

namespace ECommerce.Payments.PaymentProviders
{
  public class AmazonPayPaymentProvider : IPaymentProvider
  {
    private readonly AmazonPayOptions _options;
    private readonly HttpClient _httpClient;

    public AmazonPayPaymentProvider(AmazonPayOptions options, HttpClient httpClient)
    {
      _options = options ?? throw new ArgumentNullException(nameof(options));
      _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<PaymentResult> AuthorizeAsync(PaymentRequest request)
    {
      var payload = new
      {
        chargeAmount = new
        {
          amount = request.Amount.ToString("F2"),
          currencyCode = request.Currency
        },
        captureNow = false,
        merchantMetadata = new
        {
          merchantReferenceId = Guid.NewGuid().ToString()
        }
      };

      var jsonPayload = JsonSerializer.Serialize(payload);
      var endpoint = "/sandbox/charges";

      var requestMessage = CreateSignedRequest(HttpMethod.Post, endpoint, jsonPayload);
      var response = await _httpClient.SendAsync(requestMessage);

      return await HandlePaymentResponse(response, PaymentStatus.Authorized);
    }

    public async Task<PaymentResult> CaptureAsync(PaymentRequest request)
    {
      var payload = new
      {
        chargeAmount = new
        {
          amount = request.Amount.ToString("F2"),
          currencyCode = request.Currency
        }
      };

      var jsonPayload = JsonSerializer.Serialize(payload);
      var endpoint = $"/sandbox/charges/{request.OrderId}/capture";

      var requestMessage = CreateSignedRequest(HttpMethod.Post, endpoint, jsonPayload);
      var response = await _httpClient.SendAsync(requestMessage);

      return await HandlePaymentResponse(response, PaymentStatus.Captured);
    }

    public async Task<PaymentResult> RefundAsync(PaymentRequest request)
    {
      var payload = new
      {
        refundAmount = new
        {
          amount = request.Amount.ToString("F2"),
          currencyCode = request.Currency
        }
      };

      var jsonPayload = JsonSerializer.Serialize(payload);
      var endpoint = $"/sandbox/charges/{request.OrderId}/refund";

      var requestMessage = CreateSignedRequest(HttpMethod.Post, endpoint, jsonPayload);
      var response = await _httpClient.SendAsync(requestMessage);

      return await HandlePaymentResponse(response, PaymentStatus.Refunded);
    }

    private HttpRequestMessage CreateSignedRequest(HttpMethod method, string path, string payload)
    {
      var uri = new Uri(_options.ApiBaseUrl + path);
      var request = new HttpRequestMessage(method, uri)
      {
        Content = new StringContent(payload, Encoding.UTF8, "application/json")
      };

      var signer = new AmazonPaySignatureHelper(_options);
      signer.SignRequest(request, payload);

      return request;
    }

    private async Task<PaymentResult> HandlePaymentResponse(HttpResponseMessage response, PaymentStatus successStatus)
    {
      var content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        return new PaymentResult
        {
          Status = PaymentStatus.Failed,
          ProviderMessage = content
        };
      }

      var json = JsonSerializer.Deserialize<JsonElement>(content);
      var id = json.GetProperty("chargeId").GetString();

      return new PaymentResult
      {
        Status = successStatus,
        TransactionId = id
      };
    }
  }
}

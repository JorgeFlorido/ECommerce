using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Payment;
using ECommerce.Payments.Enums;
using ECommerce.Payments.Models;
using ECommerce.Payments.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ECommerce.Payments.PaymentProviders
{
  public class PaypalPaymentProvider : IPaymentProvider
  {
    private readonly PaypalOptions _options;
    private readonly HttpClient _httpClient;

    public PaypalPaymentProvider(PaypalOptions options, HttpClient httpClient)
    {
      _options = options ?? throw new ArgumentNullException(nameof(options));
      _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<PaymentResult> AuthorizeAsync(PaymentRequest request)
    {
      var accessToken = await GetAccessTokenAsync();

      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

      var body = new
      {
        intent = "AUTHORIZE",
        purchase_units = new[]
          {
            new
            {
                amount = new
                {
                    currency_code = request.Currency,
                    value = request.Amount.ToString("F2")
                }
            }
        }
      };

      var response = await _httpClient.PostAsJsonAsync($"{_options.ApiBaseUrl}/v2/checkout/orders", body);

      if (!response.IsSuccessStatusCode)
      {
        return new PaymentResult
        {
          Status = PaymentStatus.Failed,
          ProviderMessage = await response.Content.ReadAsStringAsync()
        };
      }

      var json = await response.Content.ReadFromJsonAsync<JsonElement>();
      var orderId = json.GetProperty("id").GetString();

      return new PaymentResult
      {
        Status = PaymentStatus.Authorized,
        TransactionId = orderId
      };
    }

    public async Task<PaymentResult> CaptureAsync(PaymentRequest request)
    {
      var accessToken = await GetAccessTokenAsync();

      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

      var url = $"{_options.ApiBaseUrl}/v2/checkout/orders/{request.OrderId}/capture";

      var response = await _httpClient.PostAsync(url, null);

      if (!response.IsSuccessStatusCode)
      {
        return new PaymentResult
        {
          Status = PaymentStatus.Failed,
          ProviderMessage = await response.Content.ReadAsStringAsync()
        };
      }

      var json = await response.Content.ReadFromJsonAsync<JsonElement>();

      var captureId = json
          .GetProperty("purchase_units")[0]
          .GetProperty("payments")
          .GetProperty("captures")[0]
          .GetProperty("id")
          .GetString();

      return new PaymentResult
      {
        Status = PaymentStatus.Captured,
        TransactionId = captureId
      };
    }

    public async Task<PaymentResult> RefundAsync(PaymentRequest request)
    {
      var accessToken = await GetAccessTokenAsync();

      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

      var url = $"{_options.ApiBaseUrl}/v2/payments/captures/{request.OrderId}/refund";

      var body = new
      {
        amount = new
        {
          value = request.Amount.ToString("F2"),
          currency_code = request.Currency
        }
      };

      var response = await _httpClient.PostAsJsonAsync(url, body);

      if (!response.IsSuccessStatusCode)
      {
        return new PaymentResult
        {
          Status = PaymentStatus.Failed,
          ProviderMessage = await response.Content.ReadAsStringAsync()
        };
      }

      var json = await response.Content.ReadFromJsonAsync<JsonElement>();
      var refundId = json.GetProperty("id").GetString();

      return new PaymentResult
      {
        Status = PaymentStatus.Refunded,
        TransactionId = refundId
      };
    }

    private async Task<string> GetAccessTokenAsync()
    {
      var clientId = _options.ClientId;
      var secret = _options.ClientSecret;

      var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

      var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

      var response = await _httpClient.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", content);
      response.EnsureSuccessStatusCode();

      var result = await response.Content.ReadFromJsonAsync<JsonElement>();
      return result.GetProperty("access_token").GetString();
    }
  }
}

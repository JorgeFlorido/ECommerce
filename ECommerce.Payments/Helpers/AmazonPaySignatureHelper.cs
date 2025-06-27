using ECommerce.Payments.Options;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Payments.Helpers
{
  public class AmazonPaySignatureHelper
  {
    private readonly AmazonPayOptions _options;

    public AmazonPaySignatureHelper(AmazonPayOptions options)
    {
      _options = options;
    }

    public void SignRequest(HttpRequestMessage request, string body)
    {
      var host = request.RequestUri.Host;
      var method = request.Method.Method;
      var path = request.RequestUri.AbsolutePath;

      var headersToSign = new SortedDictionary<string, string>
      {
        ["content-type"] = "application/json",
        ["x-amz-pay-date"] = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ"),
        ["x-amz-pay-host"] = host
      };

      string canonicalRequest = BuildCanonicalRequest(method, path, headersToSign, body);
      string stringToSign = BuildStringToSign(headersToSign["x-amz-pay-date"], canonicalRequest);
      string signature = Sign(stringToSign, _options.PrivateKey);

      request.Headers.Add("x-amz-pay-host", headersToSign["x-amz-pay-host"]);
      request.Headers.Add("x-amz-pay-date", headersToSign["x-amz-pay-date"]);
      request.Headers.Add("x-amz-pay-region", _options.Region);
      request.Headers.Add("x-amz-pay-idempotency-key", Guid.NewGuid().ToString());
      request.Headers.Add("authorization", $"AMZN-PAY-RSASSA-PSS PublicKeyId={_options.PublicKeyId}, Signature={signature}, SignedHeaders=content-type;x-amz-pay-date;x-amz-pay-host");

      request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    }

    private static string BuildCanonicalRequest(string method, string path, IDictionary<string, string> headers, string body)
    {
      var signedHeaders = string.Join(";", headers.Keys);
      var headerString = string.Join("\n", headers.Select(h => $"{h.Key}:{h.Value}"));

      var payloadHash = SHA256Hash(body);

      return $"{method}\n{path}\n\n{headerString}\n\n{signedHeaders}\n{payloadHash}";
    }

    private static string BuildStringToSign(string date, string canonicalRequest)
    {
      var hashedRequest = SHA256Hash(canonicalRequest);
      return $"AMZN-PAY-RSASSA-PSS\n{date}\n{hashedRequest}";
    }

    private static string SHA256Hash(string data)
    {
      var hash = SHA256.HashData(Encoding.UTF8.GetBytes(data));
      return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    private static string Sign(string stringToSign, string privateKeyPem)
    {
      using var rsa = RSA.Create();
      rsa.ImportFromPem(privateKeyPem.ToCharArray());

      var signatureBytes = rsa.SignData(Encoding.UTF8.GetBytes(stringToSign), HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
      return Convert.ToBase64String(signatureBytes);
    }
  }
}

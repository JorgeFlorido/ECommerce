using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ECommerce.Infrastructure.Services
{
  public class TwilioSmsService : ISmsService
  {
    private readonly IConfiguration _configuration;

    public TwilioSmsService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public async Task SendSmsAsync(string to, string message)
    {
      var twilioSettings = _configuration.GetSection("TwilioSettings");
      var accountSid = twilioSettings["AccountSid"];
      var authToken = twilioSettings["AuthToken"];
      var fromPhoneNumber = twilioSettings["FromPhoneNumber"];

      TwilioClient.Init(accountSid, authToken);

      await MessageResource.CreateAsync(
          to: new PhoneNumber(to),
          from: new PhoneNumber(fromPhoneNumber),
          body: message
      );
    }
  }
} 
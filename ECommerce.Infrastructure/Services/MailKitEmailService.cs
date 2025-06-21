using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
  public class MailKitEmailService : IEmailService
  {
    private readonly IConfiguration _configuration;

    public MailKitEmailService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
      var emailSettings = _configuration.GetSection("EmailSettings");

      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
      message.To.Add(new MailboxAddress(to, to));
      message.Subject = subject;

      message.Body = new TextPart("html")
      {
        Text = body
      };

      using (var client = new SmtpClient())
      {
        if (int.TryParse(emailSettings["SmtpPort"], out int smtpPort))
        {
          await client.ConnectAsync(emailSettings["SmtpServer"], smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
          await client.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
          await client.SendAsync(message);
          await client.DisconnectAsync(true);
        }
      }
    }
  }
} 
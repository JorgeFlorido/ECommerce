using ECommerce.Application.Interfaces;
using ECommerce.Domain.Abstractions;

namespace ECommerce.Application.Services
{
  public class NotificationService : INotificationService
  {
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly ICustomerRepository _customerRepository;

    public NotificationService(IEmailService emailService, ISmsService smsService, ICustomerRepository customerRepository)
    {
      _emailService = emailService;
      _smsService = smsService;
      _customerRepository = customerRepository;
    }

    public async Task SendNotificationAsync(Guid userId, string message)
    {
      var customer = await _customerRepository.GetCustomerByIdAsync(userId);

      if (customer != null)
      {
        if (!string.IsNullOrEmpty(customer.Email))
        {
          await _emailService.SendEmailAsync(customer.Email, "Notification", message);
        }

        if (!string.IsNullOrEmpty(customer.PhoneNumber))
        {
          await _smsService.SendSmsAsync(customer.PhoneNumber, message);
        }
      }
    }

    public async Task SendBatchNotificationsAsync(IEnumerable<Guid> userIds, string message)
    {
      foreach (var userId in userIds)
      {
        await SendNotificationAsync(userId, message);
      }
    }
  }
} 
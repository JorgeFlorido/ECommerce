namespace ECommerce.Application.Interfaces
{
  public interface INotificationService
  {
    Task SendNotificationAsync(Guid userId, string message);

    Task SendBatchNotificationsAsync(IEnumerable<Guid> userIds, string message);
  }
}

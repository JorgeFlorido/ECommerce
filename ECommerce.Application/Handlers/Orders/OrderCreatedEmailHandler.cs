using ECommerce.Application.Interfaces;
using ECommerce.Domain.Events;

namespace ECommerce.Application.Handlers.Orders
{
    public class OrderCreatedEmailHandler : IDomainEventHandler<OrderCreatedEvent>
    {
        private readonly INotificationService _notificationService;

        public OrderCreatedEmailHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            try
            {
                var message = $"Your order {domainEvent.OrderId} has been created.";
                await _notificationService.SendNotificationAsync(domainEvent.CustomerId, message);
            }
            catch
            {
                // Notification failures should not stop order processing
            }
        }
    }
} 
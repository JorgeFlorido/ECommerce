using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Domain.Events
{
    public interface IDomainEventHandler<TEvent> where TEvent : DomainEvent
    {
        Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
    }
} 
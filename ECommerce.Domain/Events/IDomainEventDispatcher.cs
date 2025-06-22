using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Domain.Events
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken);
    }
} 
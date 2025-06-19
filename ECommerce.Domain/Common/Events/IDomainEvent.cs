using System;

namespace ECommerce.Domain.Common.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
} 
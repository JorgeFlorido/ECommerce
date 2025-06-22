namespace ECommerce.Domain.Events
{
  public class OrderCreatedEvent : DomainEvent
  {
    public Guid OrderId { get; }
    public Guid CustomerId { get; }

    public OrderCreatedEvent(Guid orderId, Guid customerId)
    {
      OrderId = orderId;
      CustomerId = customerId;
    }
  }
}

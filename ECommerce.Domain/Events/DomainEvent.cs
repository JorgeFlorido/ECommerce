namespace ECommerce.Domain.Events
{
  public abstract class DomainEvent
  {
    public DateTime OccurredOn { get; private set; }
    protected DomainEvent()
    {
      OccurredOn = DateTime.UtcNow;
    }
    public override string ToString()
    {
      return $"{GetType().Name} occurred on {OccurredOn:O}";
    }
  }
}

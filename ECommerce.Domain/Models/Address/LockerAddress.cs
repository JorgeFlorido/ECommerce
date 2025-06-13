namespace ECommerce.Domain.Models
{
  public class LockerAddress : Address
  {
    public string? LockerId { get; set; }
    public string? Provider { get; set; }
  }
}

namespace ECommerce.Domain.Models
{
  public class CustomerAddress : Address
  {
    public Guid CustomerId { get; set; }
    public bool IsPrimary { get; set; }
  }
}

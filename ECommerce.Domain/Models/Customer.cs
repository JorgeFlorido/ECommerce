namespace ECommerce.Domain.Models
{
  public class Customer
  {
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public List<Address> Addresses { get; set; } = [];
  }
}

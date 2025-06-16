namespace ECommerce.Domain.Models.User
{
  public class Customer : BaseUser
  {
    public string? PhoneNumber { get; set; }
    public List<Address> Addresses { get; set; } = [];
  }
}

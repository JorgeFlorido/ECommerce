namespace ECommerce.API.Models.Requests.User
{
  public class UpdateUserRequest
  {
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
  }
} 
namespace ECommerce.API.Models.Requests.User
{
  public class AddUserRequest
  {
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
  }
} 
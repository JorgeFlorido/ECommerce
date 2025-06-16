namespace ECommerce.Domain.Models.User
{
  public abstract class BaseUser
  {
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
  }
}

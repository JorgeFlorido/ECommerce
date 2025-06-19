using ECommerce.Domain.Models.User;
using MediatR;

namespace ECommerce.Application.Requests.Commands.Users
{
  public class AddUserCommand : IRequest<Guid>
  {
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
  }
} 
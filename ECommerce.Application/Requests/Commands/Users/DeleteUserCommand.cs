using MediatR;

namespace ECommerce.Application.Requests.Commands.Users
{
  public class DeleteUserCommand : IRequest
  {
    public Guid UserId { get; set; }
  }
} 
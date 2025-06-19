using ECommerce.Domain.Models.User;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Users
{
  public class GetUserByIdQuery : IRequest<Customer?>
  {
    public Guid Id { get; set; }
  }
} 
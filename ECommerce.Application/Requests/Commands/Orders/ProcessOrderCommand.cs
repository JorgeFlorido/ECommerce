using MediatR;

namespace ECommerce.Application.Requests.Commands.Orders
{
  public class ProcessOrderCommand : IRequest<bool>
  {
    public Guid OrderId { get; set; }
  }
} 
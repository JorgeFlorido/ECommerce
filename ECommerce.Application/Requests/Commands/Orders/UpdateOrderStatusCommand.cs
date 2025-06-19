using ECommerce.Domain.Enums;
using MediatR;

namespace ECommerce.Application.Requests.Commands.Orders
{
  public class UpdateOrderStatusCommand : IRequest<bool>
  {
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
  }
} 
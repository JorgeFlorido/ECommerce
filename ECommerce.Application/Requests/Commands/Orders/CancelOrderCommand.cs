using MediatR;

namespace ECommerce.Application.Requests.Commands.Orders
{
  public class CancelOrderCommand : IRequest<bool>
  {
    public Guid OrderId { get; set; }
    public string? Reason { get; set; }
    public bool RefundPayment { get; set; } = true;
  }
} 
using ECommerce.Domain.Enums;

namespace ECommerce.API.Models.Requests.Order
{
  public class UpdateOrderStatusRequest
  {
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
  }
} 
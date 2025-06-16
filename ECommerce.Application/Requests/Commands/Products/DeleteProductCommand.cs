using MediatR;

namespace ECommerce.Application.Requests.Commands.Products
{
  public class DeleteProductCommand : IRequest<bool>
  {
    public Guid ProductId { get; set; }
  }
}

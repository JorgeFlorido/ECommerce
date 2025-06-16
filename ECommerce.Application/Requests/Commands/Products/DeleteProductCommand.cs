using MediatR;

namespace ECommerce.Application.Requests.Commands.Products
{
  public class DeleteProductCommand : IRequest<Guid> 
  {
    public Guid ProductId { get; set; }
  }
}

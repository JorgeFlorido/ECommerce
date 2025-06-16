using ECommerce.Application.Requests.Commands.Products;
using MediatR;

namespace ECommerce.Application.Handlers.Products
{
  public class AddProductHandler : IRequestHandler<AddProductCommand, Guid>
  {
    public Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}

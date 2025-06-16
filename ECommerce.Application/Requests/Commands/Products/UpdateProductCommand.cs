using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.Requests.Commands.Products
{
  public class UpdateProductCommand : IRequest<Unit>
  {
    public required Product Product { get; set; }
  }
}

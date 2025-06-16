using MediatR;

namespace ECommerce.Application.Requests.Commands.Products
{
  public class UpdateProductCommand : IRequest<Unit>
  {
    public Guid ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
  }
}

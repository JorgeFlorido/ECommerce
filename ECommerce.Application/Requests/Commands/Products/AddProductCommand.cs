using MediatR;

namespace ECommerce.Application.Requests.Commands.Products
{
  public class AddProductCommand : IRequest<Guid>
  {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
  }
}

namespace ECommerce.API.Models.Requests.Product
{
  public class UpdateProductRequest
  {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
  }
}

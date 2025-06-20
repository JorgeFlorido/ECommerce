namespace ECommerce.Domain.Models.Product
{
  public class ProductType
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
  }
}

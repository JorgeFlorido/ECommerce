namespace ECommerce.Domain.Models.Product
{
  public class Product
  {
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    public Guid? ProductTypeId { get; set; }
    public ProductType? ProductType { get; set; }
  }
}

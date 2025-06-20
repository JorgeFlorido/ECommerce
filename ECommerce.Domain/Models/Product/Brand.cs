namespace ECommerce.Domain.Models.Product
{
  public class Brand
  {
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string? LogoUrl { get; set; }
  }
}

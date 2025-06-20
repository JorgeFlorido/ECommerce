namespace ECommerce.Domain.Models.Product
{
  public class Category
  {
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = [];
  }
}

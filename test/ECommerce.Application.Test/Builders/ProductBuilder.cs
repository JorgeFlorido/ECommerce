using ECommerce.Domain.Models.Product;
using System.Reflection;

namespace ECommerce.Application.Test.Builders
{
  public class ProductBuilder
  {
    private readonly Product _product;

    public ProductBuilder()
    {
      _product = (Product)Activator.CreateInstance(typeof(Product), true)!;
    }

    public ProductBuilder WithId(Guid id)
    {
      var prop = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
      if (prop != null && prop.CanWrite)
        prop.SetValue(_product, id);
      else
      {
        var field = typeof(Product).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        field?.SetValue(_product, id);
      }
      return this;
    }

    public Product Build() => _product;
  }
}

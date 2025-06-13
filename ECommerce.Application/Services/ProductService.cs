using ECommerce.Application.Interfaces;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task AddProductAsync(Product product)
    {
      if (product == null)
      {
        throw new ArgumentNullException(nameof(product), "Product cannot be null.");
      }
      if (string.IsNullOrWhiteSpace(product.Name))
      {
        throw new ArgumentException("Product name cannot be empty.", nameof(product));
      }
      if (product.Price <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(product), "Product price must be greater than zero.");
      }
      await _productRepository.AddProductAsync(product);
    }

    public async Task DeleteProductAsync(Guid id)
    {
      if (id == Guid.Empty)
      {
        throw new ArgumentException("Product ID cannot be empty.", nameof(id));
      }

      _ = await _productRepository.GetProductByIdAsync(id)
        ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

      await _productRepository.DeleteProductAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
      var products = await _productRepository.GetAllProductsAsync();
      if (products == null || !products.Any())
      {
        throw new KeyNotFoundException("No products found.");
      }
      return products;
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
      if (id == Guid.Empty)
      {
        throw new ArgumentException("Product ID cannot be empty.", nameof(id));
      }
      var product = await _productRepository.GetProductByIdAsync(id);
      return product ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
    }

    public async Task UpdateProductAsync(Product product)
    {
      if (product == null)
      {
        throw new ArgumentNullException(nameof(product), "Product cannot be null.");
      }
      if (product.Id == Guid.Empty)
      {
        throw new ArgumentException("Product ID cannot be empty.", nameof(product));
      }
      if (string.IsNullOrWhiteSpace(product.Name))
      {
        throw new ArgumentException("Product name cannot be empty.", nameof(product));
      }
      if (product.Price <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(product), "Product price must be greater than zero.");
      }
      _ = await _productRepository.GetProductByIdAsync(product.Id)
        ?? throw new KeyNotFoundException($"Product with ID {product.Id} not found.");

      await _productRepository.UpdateProductAsync(product);
    }
  }
}

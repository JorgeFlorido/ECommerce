using ECommerce.Database;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
  public class ProductRepository : IProductRepository
  {
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken = default)
    {
      await _context.Products.AddAsync(product, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
      await _context.Products
        .Where(p => p.Id == productId)
        .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
      return await _context.Products
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
      return await _context.Products
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
      await _context.Products
        .Where(p => p.Id == product.Id)
        .ExecuteUpdateAsync(u => u
          .SetProperty(p => p.Name, product.Name)
          .SetProperty(p => p.Description, product.Description)
          .SetProperty(p => p.Price, product.Price)
          .SetProperty(p => p.StockQuantity, product.StockQuantity)
          .SetProperty(p => p.ImageUrl, product.ImageUrl), cancellationToken);
    }
  }
}

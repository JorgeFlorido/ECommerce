using ECommerce.Database;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Common.Models;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public async Task<PagedList<Product>> GetAllProductsAsync(ProductFilterQuery filter, PaginationQuery pagination, CancellationToken cancellationToken = default)
    {
      IQueryable<Product> query = _context.Products
          .Include(p => p.Brand)
          .Include(p => p.Category)
          .AsNoTracking();

      if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
      {
        query = query.Where(p =>
            (p.Name != null && p.Name.Contains(filter.SearchTerm)) ||
            (p.Description != null && p.Description.Contains(filter.SearchTerm)));
      }

      if (filter.Brands != null && filter.Brands.Any())
      {
        query = query.Where(p => p.Brand != null && filter.Brands.Contains(p.Brand.Name!));
      }
      
      if (filter.Categories != null && filter.Categories.Any())
      {
        query = query.Where(p => p.Category != null && filter.Categories.Contains(p.Category.Name!));
      }

      if (filter.MinPrice.HasValue)
      {
        query = query.Where(p => p.Price >= filter.MinPrice.Value);
      }

      if (filter.MaxPrice.HasValue)
      {
        query = query.Where(p => p.Price <= filter.MaxPrice.Value);
      }

      if (filter.InStock.HasValue)
      {
        query = query.Where(p => p.StockQuantity > 0 == filter.InStock.Value);
      }

      var totalCount = await query.CountAsync(cancellationToken);

      var items = await query
          .Skip((pagination.PageNumber - 1) * pagination.PageSize)
          .Take(pagination.PageSize)
          .ToListAsync(cancellationToken);

      return new PagedList<Product>(items, totalCount, pagination.PageNumber, pagination.PageSize);
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

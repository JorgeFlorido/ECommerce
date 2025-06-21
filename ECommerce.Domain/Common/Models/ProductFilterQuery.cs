using System.Collections.Generic;

namespace ECommerce.Domain.Common.Models
{
  public class ProductFilterQuery
  {
    public string? SearchTerm { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<string>? Brands { get; set; }
    public List<string>? Categories { get; set; }
    public bool? InStock { get; set; }
  }
} 
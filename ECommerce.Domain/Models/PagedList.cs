using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Models
{
  public class PagedList<T>
  {
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
      TotalCount = count;
      PageSize = pageSize;
      PageNumber = pageNumber;
      TotalPages = (int)Math.Ceiling(count / (double)pageSize);
      Items = items;
    }
  }
} 
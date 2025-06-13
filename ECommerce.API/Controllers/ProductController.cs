using AutoMapper;
using ECommerce.API.Models.Requests.Product;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
  [Route("api/products")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
      _productService = productService;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
      var products = await _productService.GetAllProductsAsync();
      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
      var product = await _productService.GetProductByIdAsync(id);
      return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductRequest productRequest)
    {
      var product = _mapper.Map<Product>(productRequest);
      await _productService.AddProductAsync(product);
      return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
      await _productService.DeleteProductAsync(id);
      return NoContent();
    }
  }
}
using AutoMapper;
using ECommerce.API.Models.Requests.Product;
using ECommerce.Application.Requests.Commands.Products;
using ECommerce.Application.Requests.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
  [Route("api/products")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductController(IMediator mediator, IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsRequest request)
    {
      var query = _mapper.Map<GetAllProductsQuery>(request);
      var products = await _mediator.Send(query);
      return Ok(products);
    }

    [HttpGet("brands/{brandName}/products")]
    public async Task<IActionResult> GetProductsByBrand(string brandName, [FromQuery] GetAllProductsRequest request)
    {
      request.Filter.Brands = new List<string> { brandName };

      var query = _mapper.Map<GetAllProductsQuery>(request);
      var products = await _mediator.Send(query);
      return Ok(products);
    }

    [HttpGet("categories/{categoryName}/products")]
    public async Task<IActionResult> GetProductsByCategory(string categoryName, [FromQuery] GetAllProductsRequest request)
    {
      request.Filter.Categories = new List<string> { categoryName };

      var query = _mapper.Map<GetAllProductsQuery>(request);
      var products = await _mediator.Send(query);
      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
      var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
      return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductRequest productRequest)
    {
      var addProductCommand = _mapper.Map<AddProductCommand>(productRequest);
      var result = await _mediator.Send(addProductCommand);
      return CreatedAtAction(nameof(GetProductById), new { id = result }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
      var deleteProductCommand = new DeleteProductCommand { ProductId = id };
      await _mediator.Send(deleteProductCommand);
      return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest productRequest)
    {
      var updateProductCommand = _mapper.Map<UpdateProductCommand>(productRequest);
      updateProductCommand.ProductId = id;
      var result = await _mediator.Send(updateProductCommand);
      return Ok(result);
    }
  }
}
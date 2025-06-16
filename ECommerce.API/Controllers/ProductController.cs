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
    public async Task<IActionResult> GetAllProducts()
    {
      var products = await _mediator.Send(new GetAllProductsQuery());
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
  }
}
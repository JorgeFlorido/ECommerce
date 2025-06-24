using AutoMapper;
using ECommerce.API.Models.Requests.Order;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.API.Models.Responses.Order;
using ECommerce.Application.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
  [Authorize]
  [Route("api/orders")]
  [ApiController]
  public class OrderController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrderController(IMediator mediator, IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    [Authorize(Roles = nameof(AppRoles.Admin))]
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
      var orders = await _mediator.Send(new GetAllOrdersQuery());
      return Ok(orders);
    }

    [Authorize(Policy = "CustomerData")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
      HttpContext.Items["Resource"] = (await _mediator.Send(new GetOrderByIdQuery { Id = id }))?.CustomerId.ToString();
      var result = await _mediator.Send(new GetOrderByIdQuery { Id = id });
      return Ok(result);
    }

    [Authorize(Policy = "CustomerData")]
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetOrdersByCustomer(Guid customerId)
    {
      HttpContext.Items["Resource"] = customerId.ToString();
      var result = await _mediator.Send(new GetOrdersByCustomerQuery { CustomerId = customerId });
      return Ok(result);
    }

    [Authorize(Roles = $"{nameof(AppRoles.Customer)},{nameof(AppRoles.Admin)}")]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest orderRequest)
    {
      var createOrderCommand = _mapper.Map<CreateOrderCommand>(orderRequest);
      var result = await _mediator.Send(createOrderCommand);
      return CreatedAtAction(nameof(GetOrderById), new { id = result }, result);
    }

    [Authorize(Roles = nameof(AppRoles.Admin))]
    [HttpPost("{id}/process")]
    public async Task<IActionResult> ProcessOrder(Guid id)
    {
      var processOrderCommand = new ProcessOrderCommand { OrderId = id };
      var result = await _mediator.Send(processOrderCommand);
      return Ok(result);
    }

    [Authorize(Policy = "CustomerData")]
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid id, [FromBody] CancelOrderRequest cancelRequest)
    {
      HttpContext.Items["Resource"] = (await _mediator.Send(new GetOrderByIdQuery { Id = id }))?.CustomerId.ToString();
      var cancelOrderCommand = _mapper.Map<CancelOrderCommand>(cancelRequest);
      cancelOrderCommand.OrderId = id;
      var result = await _mediator.Send(cancelOrderCommand);
      return Ok(result);
    }

    [Authorize(Roles = nameof(AppRoles.Admin))]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequest statusRequest)
    {
      var updateStatusCommand = _mapper.Map<UpdateOrderStatusCommand>(statusRequest);
      updateStatusCommand.OrderId = id;
      var result = await _mediator.Send(updateStatusCommand);
      return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("calculate-cost")]
    public async Task<IActionResult> CalculateOrderCost([FromBody] OrderCostCalculationRequest request)
    {
      var query = _mapper.Map<OrderCostCalculationQuery>(request);
      var result = await _mediator.Send(query);
      var response = _mapper.Map<OrderCostCalculationResponse>(result);
      return Ok(response);
    }
  }
} 
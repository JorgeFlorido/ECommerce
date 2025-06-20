using AutoMapper;
using ECommerce.API.Models.Requests.Order;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.API.Models.Responses.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
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

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
      var orders = await _mediator.Send(new GetAllOrdersQuery());
      return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
      var result = await _mediator.Send(new GetOrderByIdQuery { Id = id });
      return Ok(result);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetOrdersByCustomer(Guid customerId)
    {
      var result = await _mediator.Send(new GetOrdersByCustomerQuery { CustomerId = customerId });
      return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest orderRequest)
    {
      var createOrderCommand = _mapper.Map<CreateOrderCommand>(orderRequest);
      var result = await _mediator.Send(createOrderCommand);
      return CreatedAtAction(nameof(GetOrderById), new { id = result }, result);
    }

    [HttpPost("{id}/process")]
    public async Task<IActionResult> ProcessOrder(Guid id)
    {
      var processOrderCommand = new ProcessOrderCommand { OrderId = id };
      var result = await _mediator.Send(processOrderCommand);
      return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid id, [FromBody] CancelOrderRequest cancelRequest)
    {
      var cancelOrderCommand = _mapper.Map<CancelOrderCommand>(cancelRequest);
      cancelOrderCommand.OrderId = id;
      var result = await _mediator.Send(cancelOrderCommand);
      return Ok(result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequest statusRequest)
    {
      var updateStatusCommand = _mapper.Map<UpdateOrderStatusCommand>(statusRequest);
      updateStatusCommand.OrderId = id;
      var result = await _mediator.Send(updateStatusCommand);
      return Ok(result);
    }

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
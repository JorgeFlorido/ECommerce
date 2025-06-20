using AutoMapper;
using ECommerce.API.Models.Requests.Address;
using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Application.Requests.Queries.Addresses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
  [Route("api/addresses")]
  [ApiController]
  public class AddressController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AddressController(IMediator mediator, IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCustomerAddresses(Guid customerId)
    {
      var addresses = await _mediator.Send(new GetCustomerAddressesQuery { CustomerId = customerId });
      return Ok(addresses);
    }

    [HttpPost("customer")]
    public async Task<IActionResult> AddCustomerAddress([FromBody] AddCustomerAddressRequest request)
    {
      var command = _mapper.Map<AddCustomerAddressCommand>(request);
      var result = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetCustomerAddresses), new { customerId = result.CustomerId }, result);
    }

    [HttpPut("customer/{id}")]
    public async Task<IActionResult> UpdateCustomerAddress(Guid id, [FromBody] UpdateCustomerAddressRequest request)
    {
      var command = _mapper.Map<UpdateCustomerAddressCommand>(request);
      command.AddressId = id;
      var result = await _mediator.Send(command);
      return Ok(result);
    }

    [HttpDelete("customer/{id}")]
    public async Task<IActionResult> DeleteCustomerAddress(Guid id)
    {
      await _mediator.Send(new DeleteCustomerAddressCommand { AddressId = id });
      return NoContent();
    }
  }
} 
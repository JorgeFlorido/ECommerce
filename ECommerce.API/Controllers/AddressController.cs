using AutoMapper;
using ECommerce.API.Models.Requests.Address;
using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Application.Requests.Queries.Addresses;
using ECommerce.Application.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
  [Authorize]
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

    [Authorize(Policy = "CustomerData")]
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCustomerAddresses(Guid customerId)
    {
      HttpContext.Items["Resource"] = customerId.ToString();
      var addresses = await _mediator.Send(new GetCustomerAddressesQuery { CustomerId = customerId });
      return Ok(addresses);
    }

    [Authorize(Roles = $"{nameof(AppRoles.Customer)},{nameof(AppRoles.Admin)}")]
    [HttpPost("customer")]
    public async Task<IActionResult> AddCustomerAddress([FromBody] AddCustomerAddressRequest request)
    {
      var command = _mapper.Map<AddCustomerAddressCommand>(request);
      var result = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetCustomerAddresses), new { customerId = result.CustomerId }, result);
    }

    [Authorize(Policy = "CustomerData")]
    [HttpPut("customer/{id}")]
    public async Task<IActionResult> UpdateCustomerAddress(Guid id, [FromBody] UpdateCustomerAddressRequest request)
    {
      HttpContext.Items["Resource"] = id.ToString();
      var command = _mapper.Map<UpdateCustomerAddressCommand>(request);
      command.AddressId = id;
      var result = await _mediator.Send(command);
      return Ok(result);
    }

    [Authorize(Policy = "CustomerData")]
    [HttpDelete("customer/{id}")]
    public async Task<IActionResult> DeleteCustomerAddress(Guid id)
    {
      await _mediator.Send(new DeleteCustomerAddressCommand { AddressId = id });
      return NoContent();
    }
  }
} 
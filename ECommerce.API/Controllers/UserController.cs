using AutoMapper;
using ECommerce.API.Models.Requests.User;
using ECommerce.Application.Requests.Commands.Users;
using ECommerce.Application.Requests.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
  [Route("api/users")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await _mediator.Send(new GetAllUsersQuery());
      return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
      var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
      return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest userRequest)
    {
      var addUserCommand = _mapper.Map<AddUserCommand>(userRequest);
      var result = await _mediator.Send(addUserCommand);
      return CreatedAtAction(nameof(GetUserById), new { id = result }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
      var deleteUserCommand = new DeleteUserCommand { UserId = id };
      await _mediator.Send(deleteUserCommand);
      return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest userRequest)
    {
      var updateUserCommand = _mapper.Map<UpdateUserCommand>(userRequest);
      updateUserCommand.UserId = id;
      var result = await _mediator.Send(updateUserCommand);
      return Ok(result);
    }
  }
}

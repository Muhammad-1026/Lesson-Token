using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenLesson1.Dtos.Users;
using TokenLesson1.Services.Users;

namespace TokenLesson1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("Admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CreateUserDto>> PostUserAsync([FromBody] CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        CreateUserDto addUser = await _userService.AddUserAsync(createUserDto, cancellationToken);

        return Ok(addUser);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserDto>>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userService.GetAllAsync(cancellationToken);

        return Ok(users);
    }
}
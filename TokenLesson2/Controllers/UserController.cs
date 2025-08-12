using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenLesson2.Dtos.Request;
using TokenLesson2.Dtos.Response;
using TokenLesson2.Interface.Services;

namespace TokenLesson2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.GetAllAsync(cancellationToken));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        return await _userService.CreateUserAsync(createUserDto, cancellationToken);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        return await _authService.LoginAsync(loginDto, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
    {
        return await _userService.UpdateUserAsync(updateUserDto, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return Ok(true);
    }
}
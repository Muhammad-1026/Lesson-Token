using Microsoft.AspNetCore.Mvc;
using TokenLesson1.Dtos.Users;
using TokenLesson1.Services.Accaunts;

namespace TokenLesson1.Controllers;

[ApiController]
public class AccauntsController : ControllerBase
{
    private readonly IAccauntService _accauntService;

    public AccauntsController(IAccauntService accauntService)
    {
        _accauntService = accauntService;
    }

    [HttpPost("api/login")]
    public async ValueTask<ActionResult<TokenDto>> LoginAsync([FromBody] LoginDto loginDto)
    {
        TokenDto userToken = await _accauntService.LoginAsync(loginDto);

        return Ok(userToken);
    }
}

using Microsoft.AspNetCore.Mvc;
using TokenLesson1.Models.UserCredential;
using TokenLesson1.Models.UserToken;
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
    public async ValueTask<ActionResult<UserToken>> LoginAsync([FromBody] UserCredential userCredential)
    {
        UserToken userToken = await _accauntService.LoginAsncy(userCredential);

        return Ok(userToken);
    }
}

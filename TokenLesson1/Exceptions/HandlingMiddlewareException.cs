using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;
using TokenLesson1.Exeptions;

namespace TokenLesson1.Exceptions;

public class HandlingMiddlewareException
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HandlingMiddlewareException> _logger;

    public HandlingMiddlewareException(RequestDelegate next, ILogger<HandlingMiddlewareException> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SecurityTokenExpiredException ex) 
        {
            _logger.LogWarning("Token expired: {Message}", ex.Message);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { code = 401, message = "Срок действия токена истек." });
            await context.Response.WriteAsync(result);
        }
        catch (BusinessLogicException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { code = 400, message = ex.Message });
        }
        catch (ResourceNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { code = 404, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError("Unhandled exception: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { code = 500, message = "Внутренняя ошибка сервера." });
        }
    }
}
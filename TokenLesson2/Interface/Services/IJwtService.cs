using TokenLesson2.Dtos.Response;
using TokenLesson2.Models.User;

namespace TokenLesson2.Interface.Services;

public interface IJwtService
{
    Task<TokenDto> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);

    string GenerateRefreshToken();
}
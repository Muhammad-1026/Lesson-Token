using TokenLesson1.Dtos.Users;
using TokenLesson1.Models.User;

namespace TokenLesson1.Services.Jwt;

public interface IJwtService
{
    TokenDto GenerateToken(User user);
    string GenerateRefreshToken();
}


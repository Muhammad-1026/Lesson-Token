using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TokenLesson1.Exeptions;
using TokenLesson1.Models.User;
using TokenLesson1.Models.UserCredential;
using TokenLesson1.Models.UserToken;
using TokenLesson1.Repositories.Users;

namespace TokenLesson1.Services.Accaunts;

public class AccauntService : IAccauntService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AccauntService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<UserToken> LoginAsncy(UserCredential userCredential)
    {
        //var maybeUser = await _userRepository.GetUserByCredentialsAsync(
        //    userCredential.UserName, userCredential.UserPassword);

        //if (maybeUser is null)
        //    throw new ResourceNotFoundException("Неправильное имя пользователя или пароль.");

        //return GenerateUserToken(maybeUser);

        var user = await _context.Users
       .FirstOrDefaultAsync(x => x.UserName == userCredential.UserName);

        if (user == null || !BCrypt.Net.BCrypt.Verify(userCredential.UserPassword, user.UserPassword))
            throw new UnauthorizedAccessException("Неправильное имя пользователя или пароль.");

        // 1. Access token (JWT) яратиш
        string accessToken = _jwtService.GenerateToken(user); // бу сизнинг JWT яратиш методингиз
        DateTime accessTokenExpiration = DateTime.UtcNow.AddMinutes(5);

        // 2. Refresh токен яратиш
        string refreshToken = GenerateRefreshToken();
        DateTime refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        // 3. Refresh токенни базада сақлаш
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration = refreshTokenExpiration;

        await _context.SaveChangesAsync();

        // 4. Client'га қайтариладиган маълумот
        return new UserToken
        {
            AccessToken = accessToken,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshTokenExpiration
        };
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    private UserToken GenerateUserToken(User user)
    {
        string secretKey = _configuration["AuthConfiguration:Key"];
        string issuer = _configuration["AuthConfiguration:Issuer"];
        string audience = _configuration["AuthConfiguration:Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("FullName", $"{user.FirstName} {user.LastName}")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new UserToken
        {
            AccessToken = tokenString,
            AccessTokenExpiration = DateTime.Now.AddMinutes(30)
        };
    }
}

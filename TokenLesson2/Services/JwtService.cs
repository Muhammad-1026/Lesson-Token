using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TokenLesson2.Common;
using TokenLesson2.Dtos.Request;
using TokenLesson2.Dtos.Response;
using TokenLesson2.Interface.Repository;
using TokenLesson2.Interface.Services;
using TokenLesson2.Models.User;

namespace TokenLesson2.Services;

public class JwtService : IJwtService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly IJwtRepository _jwtRepository;

    public JwtService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings, IJwtRepository jwtRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
        _jwtRepository = jwtRepository;
    }

    public async Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken = default)
    {
        var storedToken = await _jwtRepository.GetByTokenAsync(refreshTokenDto.RefreshToken, cancellationToken);

        if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked)
            throw new SecurityTokenException("Invalid refresh token");

        if (storedToken.Expires < DateTime.UtcNow)
            throw new SecurityTokenException("Refresh token expired");

        var user = await _userRepository.GetByIdAsync(storedToken.UserId, cancellationToken);

        if (user == null)
            throw new SecurityTokenException("User not found");

        // Помечаем старый refresh токен как использованный
        storedToken.IsUsed = true;
        await _jwtRepository.UpdateRefreshTokenAsync(storedToken);

        // Генерируем новый access и refresh токен
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(30);
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(10);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: accessTokenExpiration,
            signingCredentials: creds
        );

        var newRefreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(),
            Expires = refreshTokenExpiration,
            UserId = user.Id,
            IsUsed = false,
            IsRevoked = false,
        };

        await _jwtRepository.AddRefreshTokenAsync(newRefreshToken, cancellationToken);

        return new TokenDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            AccessTokenExpiration = accessTokenExpiration,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpiration = refreshTokenExpiration
        };
    }

    public async Task<TokenDto> GenerateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key ?? throw new InvalidOperationException("JWT Key is not configured.")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(60);
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(30);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: accessTokenExpiration,
            signingCredentials: creds
        );

        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(),
            Expires = refreshTokenExpiration,
            UserId = user.Id,
            IsUsed = false,
            IsRevoked = false,
        };

        await _jwtRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

        return new TokenDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            AccessTokenExpiration = accessTokenExpiration,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiration = refreshTokenExpiration
        };
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
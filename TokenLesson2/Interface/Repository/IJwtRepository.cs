using TokenLesson2.Models.User;

namespace TokenLesson2.Interface.Repository;

public interface IJwtRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    Task UpdateRefreshTokenAsync(RefreshToken storedToken);
}
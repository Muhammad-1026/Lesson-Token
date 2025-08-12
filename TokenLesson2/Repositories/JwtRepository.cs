using Microsoft.EntityFrameworkCore;
using TokenLesson2.DataContext;
using TokenLesson2.Interface.Repository;
using TokenLesson2.Models.User;

namespace TokenLesson2.Repositories
{
    public class JwtRepository: IJwtRepository
    {
        private readonly AplicationDbContext _context;

        public JwtRepository(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            if (refreshToken is null)
                throw new ArgumentNullException(nameof(refreshToken), "Refresh token cannot be null");

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
            
            if (refreshToken is null)
                throw new KeyNotFoundException("Refresh token not found");

            return refreshToken;
        }
    }
}
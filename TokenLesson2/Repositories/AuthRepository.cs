using Microsoft.EntityFrameworkCore;
using TokenLesson2.DataContext;
using TokenLesson2.Dtos.Request;
using TokenLesson2.Interface.Repository;
using TokenLesson2.Models.User;

namespace TokenLesson2.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AplicationDbContext _context;

    public AuthRepository(AplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByCredentialsAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var userName = loginDto.UserName;
        var userPassword = loginDto.UserPassword;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken)
            ?? throw new KeyNotFoundException("Неправильное имя пользователя или пароль.");

        if (!BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword))
            throw new KeyNotFoundException("Неправильное имя пользователя или пароль.");

        return user;
    }
}
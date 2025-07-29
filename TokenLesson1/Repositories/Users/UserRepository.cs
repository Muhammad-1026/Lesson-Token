using Microsoft.EntityFrameworkCore;
using TokenLesson1.DataContext;
using TokenLesson1.Dtos.Users;
using TokenLesson1.Exeptions;
using TokenLesson1.Models.User;

namespace TokenLesson1.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly AplicationDbContext _context;

    public UserRepository(AplicationDbContext context)
    {
        _context = context;
    }
    public IQueryable<User> Users => _context.Users;

    public async Task<CreateUserDto> InsertUserAsync(CreateUserDto createUserDto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            UserName = createUserDto.UserName,
            UserPassword = HashPassword(createUserDto.UserPassword),
            PhoneNumber = createUserDto.PhoneNumber,
            Role = createUserDto.Role,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new CreateUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName,
            UserPassword = user.UserPassword,
            Role = user.Role,
        };
    }

    public async Task<User?> GetUserByCredentialsAsync(string userName, string userPassword)
    {
        var user = await _context.Users
             .FirstOrDefaultAsync(user => user.UserName == userName)
             ?? throw new ResourceNotFoundException("Неправильное имя пользователя или пароль.");
         
        if (string.IsNullOrEmpty(user.UserPassword) || !BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword))
            throw new BusinessLogicException("Неправильное имя пользователя или пароль.");

        return user;
    }

    private static string HashPassword(string UserPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(UserPassword);
    }

    public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var userDto = await _context.Users.Select(user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName,
            CreatedAt = user.CreatedAt,
            Role = user.Role,
            UpdatedAt = user.UpdatedAt ?? DateTimeOffset.MinValue,
        }).ToListAsync(cancellationToken);

        return userDto;
    }
}
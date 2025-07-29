using TokenLesson1.Dtos.Users;

namespace TokenLesson1.Services.Users;

public interface IUserService
{
    Task<CreateUserDto> AddUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

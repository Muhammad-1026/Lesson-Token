using TokenLesson2.Dtos.Request;
using TokenLesson2.Dtos.Response;

namespace TokenLesson2.Interface.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

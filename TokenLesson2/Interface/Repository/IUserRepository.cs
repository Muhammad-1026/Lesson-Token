using TokenLesson2.Dtos.Request;
using TokenLesson2.Models.User;

namespace TokenLesson2.Interface.Repository;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> UpdateUserAsync(UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
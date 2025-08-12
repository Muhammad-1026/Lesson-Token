using TokenLesson2.Dtos.Request;
using TokenLesson2.Models.User;

namespace TokenLesson2.Interface.Repository;

public interface IAuthRepository
{
    Task<User?> GetUserByCredentialsAsync(LoginDto loginDto, CancellationToken cancellation = default);
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TokenLesson1.Dtos.Users;
using TokenLesson1.Models.User;

namespace TokenLesson1.Repositories.Users;

public interface IUserRepository
{
    Task<CreateUserDto> InsertUserAsync(CreateUserDto createUserDto);
    Task<User?> GetUserByCredentialsAsync(string username, string userPassword);
    Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

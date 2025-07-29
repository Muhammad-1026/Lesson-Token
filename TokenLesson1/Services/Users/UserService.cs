using TokenLesson1.Dtos.Users;
using TokenLesson1.Repositories.Users;

namespace TokenLesson1.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CreateUserDto> AddUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
        {
            return await _userRepository.InsertUserAsync(createUserDto);
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }
    }
}

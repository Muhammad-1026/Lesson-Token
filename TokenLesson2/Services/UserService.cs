using AutoMapper;
using TokenLesson2.Dtos.Request;
using TokenLesson2.Dtos.Response;
using TokenLesson2.Interface.Repository;
using TokenLesson2.Interface.Services;

namespace TokenLesson2.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByUserNameAsync(createUserDto.UserName, cancellationToken);

        if (existingUser is not null)
            throw new ArgumentException("Пользователь с таким UserName уже существует.");

        createUserDto.UserPassword = HashPassword(createUserDto.UserPassword);

        return _mapper.Map<UserDto>(await _userRepository.CreateUserAsync(createUserDto, cancellationToken));
    }

    public async Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByIdAsync(updateUserDto.Id, cancellationToken);

        if (existingUser is null)
            throw new KeyNotFoundException("User не найден");

        return _mapper.Map<UserDto>(await _userRepository.UpdateUserAsync(updateUserDto, cancellationToken));
    }

    public async Task<bool> DeleteUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.DeleteUserByIdAsync(id, cancellationToken);

        if (existingUser is false)
            throw new KeyNotFoundException("User не найден");

        return true;
    }

    private static string HashPassword(string UserPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(UserPassword);
    }
}
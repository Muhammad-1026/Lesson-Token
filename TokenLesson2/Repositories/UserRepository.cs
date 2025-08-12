using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TokenLesson2.DataContext;
using TokenLesson2.Dtos.Request;
using TokenLesson2.Interface.Repository;
using TokenLesson2.Models.User;

namespace TokenLesson2.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(AplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users.Select(u => _mapper.Map<User>(u)).ToListAsync(cancellationToken);
    }

    public async Task<User> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(createUserDto);

        _context.Users.Add(user);

        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User> UpdateUserAsync(UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateUserDto.Id, cancellationToken);

        if (user == null)
            throw new KeyNotFoundException("User не найден");

        if (!string.IsNullOrWhiteSpace(updateUserDto.FirstName))
            user.FirstName = updateUserDto.FirstName;

        if (!string.IsNullOrWhiteSpace(updateUserDto.LastName))
            user.LastName = updateUserDto.LastName;

        if (!string.IsNullOrWhiteSpace(updateUserDto.PhoneNumber))
            user.PhoneNumber = updateUserDto.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(updateUserDto.UserName))
            user.UserName = updateUserDto.UserName;

        if (updateUserDto.Role.HasValue)
            user.Role = updateUserDto.Role.Value;

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken); 

        return _mapper.Map<User>(user);
    }

    public async Task<bool> DeleteUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User не найден");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

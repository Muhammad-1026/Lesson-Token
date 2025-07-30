using TokenLesson1.Dtos.Users;

namespace TokenLesson1.Services.Accaunts
{
    public interface IAccauntService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
    }
}

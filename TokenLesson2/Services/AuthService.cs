using TokenLesson2.Dtos.Request;
using TokenLesson2.Dtos.Response;
using TokenLesson2.Interface.Repository;
using TokenLesson2.Interface.Services;

namespace TokenLesson2.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IAuthRepository authRepository, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto, CancellationToken cancellation = default)
        {
            var userName = loginDto.UserName;
            var userPassword = loginDto.UserPassword;

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Имя не может быть пустым. Пожалуйста, введите его!");

            if (string.IsNullOrWhiteSpace(userPassword))
                throw new ArgumentException("Пароль не может быть пустым. Пожалуйста, введите его!");

            var user = await _authRepository.GetUserByCredentialsAsync(loginDto, cancellation);

            if (user is null || !BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword))
                throw new UnauthorizedAccessException("Неправильное имя или пароль");

           
            var token = _jwtService.GenerateTokenAsync(user);
            
            if (token is null)
                throw new InvalidOperationException("Не удалось сгенерировать токен. Пожалуйста, попробуйте позже.");

            return await token;
        }
    }
}
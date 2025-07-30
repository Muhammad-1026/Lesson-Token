using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TokenLesson1.Dtos.Users;
using TokenLesson1.Exeptions;
using TokenLesson1.Models.User;
using TokenLesson1.Repositories.Users;
using TokenLesson1.Services.Accaunts;
using TokenLesson1.Services.Jwt;

namespace TokenLesson1.Services.Accaunts  
{
    public class AccauntService : IAccauntService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AccauntService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var userName = loginDto.UserName;
            var userPassword = loginDto.UserPassword;

            var user = await _userRepository.GetUserByCredentialsAsync(userName, userPassword);

            var token = _jwtService.GenerateToken(user);

            return token;
        }
    }
}

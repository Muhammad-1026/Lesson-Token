using Microsoft.EntityFrameworkCore;
using TokenLesson1.Models.User;
using TokenLesson1.Models.UserCredential;
using TokenLesson1.Models.UserToken;

namespace TokenLesson1.Services.Accaunts
{
    public interface IAccauntService
    {
        Task<UserToken> LoginAsncy(UserCredential userCredential);
    }
}

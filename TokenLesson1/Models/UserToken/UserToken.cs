namespace TokenLesson1.Models.UserToken;

public class UserToken
{
    //public string Token { get; set; } = default!;

    //public DateTime ExpirationDate { get; set; }
    public string AccessToken { get; set; } = default!;
    public DateTime AccessTokenExpiration { get; set; }

    public string? RefreshToken { get; set; } = default!;
    public DateTime? RefreshTokenExpiration { get; set; }
}
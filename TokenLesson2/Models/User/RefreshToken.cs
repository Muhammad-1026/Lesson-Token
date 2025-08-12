namespace TokenLesson2.Models.User;

public class RefreshToken
{
    public Guid Id { get; set; }

    public required string Token { get; set; }

    public required Guid UserId { get; set; }

    public required DateTime Expires { get; set; }

    public bool IsUsed { get; set; } = false;

    public bool IsRevoked { get; set; } = false;

    public User User { get; set; } = default!;
}

using TokenLesson1.Models.User;

namespace TokenLesson1.Dtos.Users;

public class UserDto
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string UserName { get; set; }

    public required string PhoneNumber { get; set; }

    public required Role Role { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}

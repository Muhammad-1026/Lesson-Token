using TokenLesson2.Models.User;

namespace TokenLesson2.Dtos.Response
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public required string FirstName { get; init; }

        public required string LastName { get; init; }

        public required string PhoneNumber { get; init; }

        public required string UserName { get; init; }

        public required Role? Role { get; init; }

        public DateTimeOffset CreatedAt { get; init; }

        public DateTimeOffset UpdatedAt { get; init; }
    }
}

using System.ComponentModel.DataAnnotations;
using TokenLesson1.Models.User;

namespace TokenLesson1.Dtos.Users;

public class UpdateUserDto
{
    public required Guid Id { get; set; }

    [MaxLength(20)]
    public string? LastName { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [EnumDataType(typeof(Role))]
    public Role? Role { get; set; }
}

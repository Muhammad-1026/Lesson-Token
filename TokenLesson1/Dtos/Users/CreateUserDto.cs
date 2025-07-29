using System.ComponentModel.DataAnnotations;
using TokenLesson1.Models.User;

namespace TokenLesson1.Dtos.Users;

public class CreateUserDto
{
    [MaxLength(25)]
    public required string FirstName { get; set; }

    [MaxLength(25)]
    public required string LastName { get; set; }

    [Phone]
    public required string PhoneNumber { get; set; }
    
    [MinLength(4)]
    public required string UserName { get; set; }
    
    [MinLength(6)]
    public required string UserPassword { get; set; }

    [EnumDataType(typeof(Role))]
    public required Role Role { get; set; }
}

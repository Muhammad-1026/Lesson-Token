using System.ComponentModel.DataAnnotations;
using TokenLesson2.Models.User;

namespace TokenLesson2.Dtos.Request;

public class CreateUserDto
{
    [Required] 
    [MaxLength(20)]
    public string FirstName { get; set; } = default!;

    [Required]
    [MaxLength(20)]
    public string LastName { get; set; } = default!;

    [Required]
    [MaxLength(20)]
    [Phone]
    public string PhoneNumber { get; set; } = default!;

    [Required]
    [MinLength(4)]
    [MaxLength(12)]
    public string UserName { get; set; } = default!;

    [Required]
    [MinLength(4)]
    [MaxLength(12)]
    public string UserPassword { get; set; } = default!;

    [EnumDataType(typeof(Role))]
    public Role Role { get; set; }
}

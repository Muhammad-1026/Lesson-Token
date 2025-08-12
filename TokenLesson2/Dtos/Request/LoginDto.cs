using System.ComponentModel.DataAnnotations;
using TokenLesson2.Dtos.Response;

namespace TokenLesson2.Dtos.Request;

public class LoginDto
{
    [Required]
    [MaxLength(12)]
    [MinLength(4)]
    public string UserName { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    [MaxLength(12)]
    [MinLength(4)]
    public string UserPassword { get; set; } = default!;
}
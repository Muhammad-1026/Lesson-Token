using System.ComponentModel.DataAnnotations;
using TokenLesson1.Common;

namespace TokenLesson1.Models.User;

public class User : Entity
{
    [MaxLength(20)]
    public required string FirstName { get; set; }

    [MaxLength(20)]
    public required string LastName { get; set; }

    public required string UserName { get; set; }

    public required string UserPassword { get; set; }

    [Phone]
    public required string PhoneNumber { get; set; }
         
    public required Role Role { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }
}

public enum Role
{
    Admin,
    User
}
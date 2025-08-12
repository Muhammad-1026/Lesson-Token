using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TokenLesson2.Common;

namespace TokenLesson2.Models.User;

[Index(nameof(UserName), IsUnique = true)]
public class User : Entity
{
    [MaxLength(20)]
    public required string FirstName { get; set; }

    [MaxLength(20)]
    public required string LastName { get; set; }

    [Phone]
    public required string PhoneNumber { get; set; }

    public required string UserName { get; set; }

    public required string UserPassword { get; set; }


    public required Role Role { get; set; }
}

public enum Role
{
    Admin, //0
    User  //1
}
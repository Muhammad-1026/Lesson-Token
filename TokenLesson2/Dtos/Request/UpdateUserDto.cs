using System.ComponentModel.DataAnnotations;
using TokenLesson2.Models.User;

namespace TokenLesson2.Dtos.Request
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? UserName { get; set; }


        [EnumDataType(typeof(Role))]
        public Role? Role { get; set; }
    }
}

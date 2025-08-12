using System.ComponentModel.DataAnnotations;

namespace TokenLesson2.Common;

public class Entity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
}

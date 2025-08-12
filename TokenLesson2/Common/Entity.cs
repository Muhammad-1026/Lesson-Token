using System.ComponentModel.DataAnnotations;

namespace TokenLesson2.Common;

public class Entity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTime.UtcNow;
}

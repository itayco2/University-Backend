using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace University_backend;

public class Progress
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid LessonId { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? WatchedAt { get; set; }

    // Navigation properties
    public Users User { get; set; }
    public Lessons Lesson { get; set; }
}
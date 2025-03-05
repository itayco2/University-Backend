using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace University_backend;

public class Progress
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage ="Missing user id")]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required(ErrorMessage ="Missing lessonid")]
    public Guid LessonId { get; set; }

    [ForeignKey("LessonId")]
    public virtual Lesson Lesson { get; set; } = null!;

    public DateTime? WatchedAt { get; set; } 
}

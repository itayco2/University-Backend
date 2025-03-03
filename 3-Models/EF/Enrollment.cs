using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace University_backend;

public class Enrollment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage ="Missing user id")]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required(ErrorMessage ="Missing course id")]
    public Guid CourseId { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course Course { get; set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}

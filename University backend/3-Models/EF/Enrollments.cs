using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace University_backend;

public class Enrollments
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid CourseId { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime EnrolledAt { get; set; } = DateTime.Now;

    // Navigation properties
    public Users User { get; set; }
    public Courses Course { get; set; }
}
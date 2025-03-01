using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace University_backend;

public class Lessons
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CourseId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(500)]
    public string VideoUrl { get; set; } // Video URL (e.g., YouTube/Vimeo)

    // Navigation property
    public Courses Course { get; set; }
    public ICollection<Progress> ProgressRecords { get; set; }
}
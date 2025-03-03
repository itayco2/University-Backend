using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace University_backend;

public class Lesson
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CourseId { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; } = null!;

    [Required(ErrorMessage ="Missing title")]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage ="Missing VideoUrl")]
    [StringLength(150)]
    public string VideoUrl { get; set; } = null!;

    public virtual ICollection<Progress> ProgressRecords { get; set; } = new List<Progress>();
}

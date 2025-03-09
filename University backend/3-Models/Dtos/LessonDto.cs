using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace University_backend;

public class LessonDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CourseId { get; set; }
    public virtual Course? Course { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;

}

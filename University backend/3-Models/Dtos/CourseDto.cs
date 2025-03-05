using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_backend;

public class CourseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
    }
}
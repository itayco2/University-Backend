using AutoMapper;

namespace University_backend;

public class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<Lesson, LessonDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id != Guid.Empty ? src.Id : Guid.NewGuid()))
            .ForMember(dest => dest.CourseId,
                opt => opt.MapFrom(src => src.CourseId != Guid.Empty ? src.CourseId : Guid.Empty))
            .ForMember(dest => dest.Course,
                opt => opt.MapFrom(src => src.Course)) // Direct mapping for navigation property
            .ReverseMap()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id != Guid.Empty ? src.Id : Guid.NewGuid()));
    }
}
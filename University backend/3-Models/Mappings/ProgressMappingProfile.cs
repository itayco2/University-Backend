using AutoMapper;

namespace University_backend;

public class ProgressProfile : Profile
{
    public ProgressProfile()
    {
        CreateMap<Progress, ProgressDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
            .ForMember(dest => dest.WatchedAt, opt => opt.MapFrom(src => src.WatchedAt))
            .ReverseMap(); 
    }
}

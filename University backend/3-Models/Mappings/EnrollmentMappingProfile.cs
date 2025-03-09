using AutoMapper;

namespace University_backend
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id != Guid.Empty ? src.Id : Guid.NewGuid()))
                .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => src.EnrolledAt != default ? src.EnrolledAt : DateTime.Now));

            CreateMap<EnrollmentDto, Enrollment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id != Guid.Empty ? src.Id : Guid.NewGuid()))
                .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => src.EnrolledAt != default ? src.EnrolledAt : DateTime.Now));
        }
    }
}

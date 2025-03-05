using AutoMapper;

namespace University_backend._3_Models;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Mapping from User to UserDto
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.Name) ? string.Empty : src.Name))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                if (string.IsNullOrWhiteSpace(dest.Name) && !string.IsNullOrEmpty(src.Email))
                {
                    dest.Name = src.Email.Split('@')[0];
                }
            });

        // Reverse mapping from UserDto to User
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.Name) ? string.Empty : src.Name))
            .AfterMap((src, dest) =>
            {
                // Same fallback logic for reverse mapping
                if (string.IsNullOrWhiteSpace(dest.Name) && !string.IsNullOrEmpty(src.Email))
                {
                    dest.Name = src.Email.Split('@')[0];
                }
            });
    }
}

using AutoMapper;

namespace University_backend;

public class UserProfileMapping : Profile // Renamed to avoid conflicts
{
    public UserProfileMapping()
    {
        // Mapping from User to UserDto
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.Name) ? string.Empty : src.Name))
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Don't map the password
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId)) // Map RoleId
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName)) // Map RoleName
            .AfterMap((src, dest) =>
            {
                if (string.IsNullOrWhiteSpace(dest.Name) && !string.IsNullOrEmpty(src.Email))
                {
                    dest.Name = src.Email.Split('@')[0];
                }
            });

        // Reverse mapping from UserDto to User
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Don't map the password
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.Name) ? string.Empty : src.Name))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId)) // Map RoleId
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

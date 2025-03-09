using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace University_backend;

public class UserService : IDisposable
{
    private readonly UniversityContext _db;
    private readonly IMapper _mapper;

    public enum RoleType
    {
        Student = 1,
        Professor = 2,
        Admin = 3
    }

    public UserService(UniversityContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<string> Register(RegisterCredentials credentials)
    {
        ValidationContext validationContext = new ValidationContext(credentials);
        Validator.ValidateObject(credentials, validationContext, validateAllProperties: true);

        credentials.Email = credentials.Email.ToLowerInvariant();

        if (await IsEmailTaken(credentials.Email))
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var role = await _db.Roles.FindAsync(credentials.RoleId);
        if (role == null)
        {
            throw new InvalidOperationException("Invalid role selected.");
        }

        User user = new User
        {
            Name = credentials.Name,
            Email = credentials.Email,
            Password = Cyber.HashPassword(credentials.Password),
            RoleId = credentials.RoleId
        };

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return JwtHelper.GetNewToken(user);
    }

    public async Task<string?> Login(Credentials credentials)
    {
        // Validate the credentials
        ValidationContext validationContext = new ValidationContext(credentials);
        Validator.ValidateObject(credentials, validationContext, validateAllProperties: true);

        credentials.Email = credentials.Email.ToLowerInvariant();
        string hashedPassword = Cyber.HashPassword(credentials.Password);

        User? user = await _db.Users
            .AsNoTracking()
            .Include(u => u.Role) 
            .SingleOrDefaultAsync(u =>
                u.Email == credentials.Email &&
                u.Password == hashedPassword);

        if (user == null)
        {
            return null;
        }

        return JwtHelper.GetNewToken(user);
    }


    public async Task<UserDto?> GetOneUser(Guid id)
    {
        return await _db.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await _db.Users.AnyAsync(u => u.Email == email.ToLowerInvariant());
    }

    public void Dispose()
    {
        _db.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}

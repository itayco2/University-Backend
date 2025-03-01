using Microsoft.EntityFrameworkCore;

namespace University_backend;

public enum RoleType
{
    Admin = 1,
    Student = 2,
    Instructor = 3
}

public class UserService : IDisposable
{
    private readonly UniversityContext _db;

    public UserService(UniversityContext db)
    {
        _db = db;
    }

    public string Register(Users user)
    {
        user.Email = user.Email.ToLower();
        user.Password = Cyber.HashPassword(user.Password);
        _db.Users.Add(user);
        _db.SaveChanges();
        user.Role = _db.Roles.Single(r => r.RoleId == user.RoleId); // Populate the Role navigation property.
        return JwtHelper.GetNewToken(user); // JWT = Json Web Token
    }

    public string? Login(Credentials credentials)
    {
        credentials.Email = credentials.Email.ToLower();
        credentials.Password = Cyber.HashPassword(credentials.Password);
        Users? user = _db.Users.AsNoTracking().Include(u => u.Role).SingleOrDefault(u => u.Email == credentials.Email && u.Password == credentials.Password);
        if (user == null) return null;
        return JwtHelper.GetNewToken(user);
    }

    public Users? GetOneUser(Guid id)
    {
        Users? user = _db.Users.AsNoTracking().SingleOrDefault(u => u.Id == id);
        if (user == null) return null;
        user.Password = null!; // Remove password!
        return user;
    }

    public bool IsEmailTaken(string email)
    {
        return _db.Users.Any(u => u.Email == email);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}


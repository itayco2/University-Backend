using System;
using System.ComponentModel.DataAnnotations;

namespace University_backend;

public class UserDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int RoleId { get; set; }  
    public string RoleName { get; set; } = string.Empty;  
}

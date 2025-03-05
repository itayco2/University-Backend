using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace University_backend;

public class UserDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty; 
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace University_backend;

public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 8)]
    public string Password { get; set; } // Hashed

    [Required]
    public int RoleId { get; set; }

    // Navigation properties
    public Roles Role { get; set; }
    public ICollection<Enrollments> Enrollments { get; set; } = new List<Enrollments>();
    public ICollection<Progress> ProgressRecords { get; set; } = new List<Progress>();

}
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using University_backend;
using System.Text.Json.Serialization;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Missing name")]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Missing Email")]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Missing password")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(500)]
    public string Password { get; set; } = string.Empty;

    [Column("RoleID")]
    public int RoleId { get; set; }

    [InverseProperty("Users")]
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;



    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Progress> ProgressRecords { get; set; } = new List<Progress>();
}

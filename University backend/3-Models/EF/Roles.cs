using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_backend;

public class Roles
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("RoleID")]
    public int RoleId { get; set; }

    [Required]
    [StringLength(20)]
    public string RoleName { get; set; } = null!;

    // Navigation property
    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}

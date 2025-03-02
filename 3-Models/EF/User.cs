﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace University_backend;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage ="Missing name")]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage ="Missing Email")]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage ="Missing password")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(500)]
    public string Password { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Progress> ProgressRecords { get; set; } = new List<Progress>();
}
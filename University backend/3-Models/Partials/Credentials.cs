using System.ComponentModel.DataAnnotations;

namespace University_backend;

public class Credentials
{
    [Required(ErrorMessage = "Missing Email.")]
    [MinLength(2, ErrorMessage = "Email must be minimum 2 character long.")]
    [MaxLength(100, ErrorMessage = "Email can't exceed 100 chars.")]
    [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid Email.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(100, ErrorMessage = "Password can't exceed 100 characters.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Password must contain at least one uppercase letter, one digit, and one special character.")]
    public string Password { get; set; } = null!;
}

public class RegisterCredentials
{
    [Required(ErrorMessage = "The Name field is required.")]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "The Password field is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; } = null!;
}

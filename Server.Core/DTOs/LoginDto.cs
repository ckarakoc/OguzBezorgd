using System.ComponentModel.DataAnnotations;

namespace Server.Core.DTOs;

public class LoginDto
{
    /// <example>superuser</example>
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = null!;
    
    /// <example>Pa$$w0rd</example>
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
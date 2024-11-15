using System.ComponentModel.DataAnnotations;
using Server.Core.Validations;

namespace Server.Core.DTOs;

public class RegisterDto
{
    // todo: implement
    [Required(ErrorMessage = "Username is required")]
    [AllowedUserNameCharactersValidation]
    public string UserName { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "You need to assign a role of Customer, Partner or Deliverer")]
    public string Role { get; set; } = null!;
}
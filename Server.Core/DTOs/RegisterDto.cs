using System.ComponentModel.DataAnnotations;
using Server.Core.Validations;

namespace Server.Core.DTOs;

public class RegisterDto
{
    // todo: implement
    [Required(ErrorMessage = "Username is required")]
    [AllowedUserNameCharactersValidation]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
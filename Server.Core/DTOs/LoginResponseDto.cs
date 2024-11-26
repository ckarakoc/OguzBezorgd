using System.ComponentModel.DataAnnotations;
using Server.Core.Validations;

namespace Server.Core.DTOs;

public class LoginResponseDto
{
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
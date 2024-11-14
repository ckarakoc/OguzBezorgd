namespace Server.Core.DTOs;

public class UserDto
{
    public required int Id { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
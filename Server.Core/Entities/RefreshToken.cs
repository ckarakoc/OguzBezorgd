using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities;

public class RefreshToken
{
    public required string Token { get; set; }
    public required DateTime ExpiryDate { get; set; }
    
    [Key, ForeignKey("User")]
    public int UserId { get; set; }

    public required User User { get; set; }
}
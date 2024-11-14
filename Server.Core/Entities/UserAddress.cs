using System.ComponentModel.DataAnnotations;

namespace Server.Entities;

public class UserAddress
{
    // [Fields]
    public int Id { get; set; }

    [MaxLength(255)]
    public required string StreetAddress { get; set; }

    [MaxLength(100)]
    public required string City { get; set; }

    [MaxLength(100)]
    public required string State { get; set; }

    [MaxLength(100)]
    public required string Country { get; set; }

    [MaxLength(20)]
    public required string ZipCode { get; set; }

    // [Parent] -> Has -> UserAddress
    public int UserId { get; set; }
    public required User User { get; set; }
}
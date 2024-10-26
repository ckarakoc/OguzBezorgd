using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities;

public class Store
{
    // [Fields]
    public int Id { get; set; }
    public required string StoreName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public TimeOnly? OpeningTime { get; set; }
    public TimeOnly? ClosingTime { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // [Parent] -> Has -> Store
    public int PartnerId { get; set; }
    public required User Partner { get; set; }

    // Store -> Has -> [Child]
    public required StoreAddress Address { get; set; }
    public List<StoreProduct> Products { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
    
}
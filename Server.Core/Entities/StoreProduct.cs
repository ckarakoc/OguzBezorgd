using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities;

public class StoreProduct
{
    // [Fields]
    public int Id { get; set; }
    public required string ProductName { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; } = decimal.Zero;

    // [Parent] -> Has -> StoreProduct
    [ForeignKey("Store")]
    public int StoreId { get; set; }

    public required Store Store { get; set; }
}
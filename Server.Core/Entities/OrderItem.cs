namespace Server.Core.Entities;

public class OrderItem
{
    // [Fields]
    public int Id { get; set; }
    public required string ProductName { get; set; }
    public string? Description { get; set; }
    public required decimal UnitPrice { get; set; } = decimal.Zero;
    public required int Quantity { get; set; } = 1;

    // [Parent] -> Has -> OrderItem
    public int OrderId { get; set; }
    public required Order Order { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Server.Core.Entities;

public class Order
{
    // [Fields]
    public int Id { get; set; }
    public required DateTime OrderDate { get; set; } = DateTime.Now;
    public required decimal TotalAmount { get; set; } = decimal.Zero;
    public required string Status { get; set; } = "Pending";
    public required string FromAddress { get; set; }
    public required string ToAddress { get; set; }
    public string? PaymentMethod { get; set; }

    // Order -> Has -> [Child]
    public List<OrderItem> OrderItems { get; set; } = [];

    // [Parent] -> Has -> Order
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public required User Customer { get; set; }

    public int StoreId { get; set; }

    [ForeignKey("StoreId")]
    public required Store Store { get; set; }
}
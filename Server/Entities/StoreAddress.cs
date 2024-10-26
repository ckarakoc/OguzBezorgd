using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities;

public class StoreAddress
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

    [ForeignKey("Store")]
    public int StoreId { get; set; }

    public required Store Store { get; set; }
}
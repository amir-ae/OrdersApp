namespace OrdersAppLibrary.Models;

public class OrderItemModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.Decimal128)]
    [Range(0, (double)decimal.MaxValue)]
    public decimal Quantity { get; set; }

    public string? Unit { get; set; } = string.Empty;
}

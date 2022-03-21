namespace OrdersAppLibrary.Models;

public class OrderItem
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(0, (double)decimal.MaxValue)]
    public decimal Quantity { get; set; }

    public string? Unit { get; set; } = string.Empty;
}

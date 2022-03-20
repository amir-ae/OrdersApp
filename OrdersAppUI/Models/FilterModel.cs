namespace OrdersAppUI.Models;

public class FilterModel
{
    public string? OrderNumber { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? ProviderId { get; set; }

    public string? ProviderName { get; set; }

    public string? OrderItemName { get; set; }

    public string? OrderItemUnit { get; set; }
}
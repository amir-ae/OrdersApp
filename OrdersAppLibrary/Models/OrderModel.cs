namespace OrdersAppLibrary.Models;

public class OrderModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public ProviderModel Provider { get; set; } = new();

    public List<OrderItemModel> OrderItems { get; set; } = new();
}
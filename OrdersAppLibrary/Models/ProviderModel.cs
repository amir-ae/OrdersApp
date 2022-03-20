namespace OrdersAppLibrary.Models;

public class ProviderModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    //[Required]
    //[Display(Name = "Provider name")]
    public string? Name { get; set; }
}

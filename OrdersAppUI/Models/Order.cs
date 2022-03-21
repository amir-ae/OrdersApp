namespace OrdersAppUI.Models;

public class Order
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "The Number field is required")]
    [Display(Name = "Order number")]
    public string? Number { get; set; }

    [Required(ErrorMessage = "Please add one or more items to order")]
    public List<OrderItem>? OrderItems { get; set; }

    [Required(ErrorMessage = "Please select a provider")]
    public ProviderModel? Provider { get; set; }

    public virtual void AddItem(OrderItem item)
    {
        if (OrderItems is null) {
            OrderItems = new();
        }
        OrderItems?.Add(item);
    }
    public virtual void RemoveItem(OrderItem item) {
        if (OrderItems is null) {
            OrderItems = new();
        }
        OrderItem? orderIem = OrderItems?.Find(
                        k => k.Name == item.Name
                        && k.Quantity == item.Quantity
                        && k.Unit == item.Unit);
        if (orderIem is not null) {
            OrderItems?.Remove(orderIem);
        }
    }
    public virtual void Clear() => OrderItems?.Clear();

    public virtual void Save() { }
}
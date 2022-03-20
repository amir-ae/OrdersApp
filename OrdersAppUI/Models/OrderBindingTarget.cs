namespace OrdersAppUI.Models;

public class OrderBindingTarget
{
    [Required(ErrorMessage = "The Number field is required")]
    [Display(Name = "Order number")]
    public string? Number { get; set; }

    [Required(ErrorMessage = "Please add one or more items to order")]
    public List<OrderItemModel>? OrderItems { get; set; }

    [Required(ErrorMessage = "Please select a provider")]
    public ProviderModel? Provider { get; set; }
}
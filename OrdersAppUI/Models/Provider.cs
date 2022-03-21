namespace OrdersAppUI.Models;

public class Provider
{
    [Required(ErrorMessage = "The Name field is required")]
    [Display(Name = "Provider name")]
    public string? Name { get; set; }
}
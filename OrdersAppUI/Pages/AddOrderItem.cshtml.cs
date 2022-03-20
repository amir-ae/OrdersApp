namespace OrdersAppUI.Pages
{
    public class AddOrderItemModel : PageModel
    {
        [BindProperty]
        public OrderItemModel OrderItem { get; set; } = new();

        public IActionResult OnGetAsync()
        {
            string? orderItem = TempData["item"] as string;

            if (orderItem is not null) {
                var i = orderItem.Deserialize(OrderItem);
                if (i is not null) {
                    OrderItem = i;
                    TempData["item"] = OrderItem.Serialize();
                }
            }
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (ModelState.IsValid) {
                string? orderData = TempData["order"] as string;
                OrderBindingTarget order = new();
                if (orderData is not null) {
                    order = orderData.Deserialize(order) ?? new();
                }
                if (order.OrderItems is null) {
                    order.OrderItems = new();
                }
                string? itemData = TempData["item"] as string;
                if (itemData is not null) {
                    var i = itemData.Deserialize(new OrderItemModel()) ?? new();
                    var item = order.OrderItems?.Find(
                        k => k.Name == i.Name
                        && k.Quantity == i.Quantity
                        && k.Unit == i.Unit);
                    if (item is not null) {
                        order.OrderItems?.Remove(item);
                    }
                }
                order.OrderItems?.Add(OrderItem);
                TempData["order"] = order.Serialize();
                return RedirectToPage("CreateOrder", new { Id = TempData["id"] as string });
            }
            return Page();
        }
    }
}

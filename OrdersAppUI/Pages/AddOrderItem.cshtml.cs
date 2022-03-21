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
                var item = orderItem.Deserialize(OrderItem);
                if (item is not null) {
                    OrderItem = item;
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
                string? itemData = TempData["item"] as string;
                if (itemData is not null) {
                    var item = itemData.Deserialize(new OrderItemModel()) ?? new();
                    order.RemoveItem(item);
                }
                order.AddItem(OrderItem);
                TempData["order"] = order.Serialize();
                return RedirectToPage("CreateOrder", new { Id = TempData["id"] as string });
            }
            return Page();
        }
    }
}

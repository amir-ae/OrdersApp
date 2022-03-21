namespace OrdersAppUI.Pages
{
    public class AddOrderItemModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Order { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Item { get; set; }

        [BindProperty]
        public OrderItem NewItem { get; set; } = new();

        public IActionResult OnGetAsync()
        {
            if (Item is not null) {
                var orderItem = Item.Deserialize(new OrderItem());
                if (orderItem is not null) {
                    NewItem = orderItem;
                }
            }

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(Order)) {
                Order order = Order.Deserialize(new Order()) ?? new();

                if (Item is not null) {
                    var orderItem = Item.Deserialize(new OrderItem());
                    if (orderItem is not null) {
                        order.RemoveItem(orderItem);
                    }
                }
                order.AddItem(NewItem);
                return RedirectToPage("CreateOrder", new { order = order.Serialize() });
            }
            return Page();
        }
    }
}

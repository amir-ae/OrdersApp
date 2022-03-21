namespace OrdersAppUI.Pages
{
    public class CreateOrderModel : PageModel
    {
        private readonly IOrderData _orderData;
        private readonly IProviderData _providerData;
        private readonly IMapper _mapper;

        public CreateOrderModel(IOrderData orderData, IProviderData providerData, IMapper mapper)
        {
            _orderData = orderData;
            _providerData = providerData;
            _mapper = mapper;
        }

        [BindProperty]
        public Order Order { get; set; } = new();

        [BindProperty]
        public string ProviderId {
            get => Order.Provider?.Id ?? string.Empty;
            set {
                GetProviders().GetAwaiter().GetResult();
                Order.Provider = Providers.Find(p => p.Id == value) ?? new();
            }
        }

        [BindProperty]
        public string? OrderItems {
            get => Order.OrderItems?.SerializeHTML();
            set => Order.OrderItems = value?.DeserializeHTML(Order.OrderItems) ?? new();
        }

        public List<ProviderModel> Providers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string? id, string? order)
        {
            if (id is not null) {
                OrderModel? r = await _orderData.GetOrder(id);
                if (r is not null) {
                    Order = _mapper.Map<Order>(r);
                    OrderItems = Order.OrderItems?.SerializeHTML();
                }
            }
            else if (!string.IsNullOrEmpty(order)) {
                Order = order.Deserialize(Order) ?? new();
            }
            await GetProviders();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Order.Provider?.Id)) {
                ModelState.Remove("Order.Provider");
            }
            if (Order.OrderItems?.Count > 0) {
                ModelState.Remove("Order.OrderItems");
            }
            if (ModelState.IsValid) {
                var order = _mapper.Map<OrderModel>(Order);
                if (string.IsNullOrEmpty(order.Id)) {
                    var result = await _orderData.CreateOrder(order);
                    if (result) {
                        TempData["message"] = "Order Created";
                    }
                    else {
                        TempData["message"] = "Error Creating Order";
                    }
                }
                else {
                    var result = await _orderData.UpdateOrder(order);
                    if (result) {
                        TempData["message"] = "Order Updated";
                    }
                    else {
                        TempData["message"] = "Error Updating Order";
                    }
                };
                RedirectToPage();
            }
            await GetProviders();
            return Page();
        }

        public IActionResult OnPostAddItemAsync()
        {
            return RedirectToPage("AddOrderItem", new { order = Order.Serialize() });
        }

        public IActionResult OnPostEditItemAsync(string item)
        {
            return RedirectToPage("AddOrderItem", new { order = Order.Serialize(), item });
        }

        public IActionResult OnPostRemoveItemAsync(string item)
        {
            OrderItem? orderItem = item.Deserialize(new OrderItem());
            if (orderItem is not null) {
                Order.RemoveItem(orderItem);
            }
            return RedirectToPage(new { order = Order.Serialize() });
        }

        private async Task GetProviders()
        {
            string? providerData = TempData["providers"] as string;
            if (providerData is not null) {
                var p = providerData.Deserialize(Providers);
                if (p is not null) {
                    Providers = p;
                    return;
                }
            }
            var q = await _providerData.GetAllProviders();
            if (q is not null) {
                TempData["providers"] = q.Serialize();
                Providers = q;
            }
            else {
                TempData["message"] = "Error Retrieving Providers";
            }
        }
    }
}

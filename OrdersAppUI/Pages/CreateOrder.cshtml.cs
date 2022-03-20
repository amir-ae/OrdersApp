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
        public OrderBindingTarget Order { get; set; } = new();

        [BindProperty]
        public string ProviderId {
            get => Order.Provider?.Id ?? string.Empty;
            set {
                GetProviders().GetAwaiter().GetResult();
                Order.Provider = Providers.Find(p => p.Id == value) ?? new();
            }
        }

        [BindProperty]
        public string OrderItems {
            get => Order.OrderItems?.SerializeHTML() ?? string.Empty;
            set => Order.OrderItems = value.DeserializeHTML(Order.OrderItems) ?? new();
        }

        public List<ProviderModel> Providers { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string? orderData = TempData["order"] as string;

            if (orderData is not null) {
                Order = orderData.Deserialize(Order) ?? new();
            }
            else if (Id is not null) {
                OrderModel? r = await _orderData.GetOrder(Id);
                if (r is not null) {
                    Order = _mapper.Map<OrderBindingTarget>(r);
                    OrderItems = Order.OrderItems?.SerializeHTML() ?? string.Empty;
                }
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
                if (string.IsNullOrEmpty(Id)) {
                    var result = await _orderData.CreateOrder(order);
                    if (result) {
                        TempData["message"] = "Order Created";
                    }
                    else {
                        TempData["message"] = "Error Creating Order";
                    }
                }
                else {
                    order.Id = Id;
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
            TempData["id"] = Id;
            TempData["order"] = Order.Serialize();
            return RedirectToPage("AddOrderItem");
        }

        public IActionResult OnPostEditItemAsync(string item)
        {
            TempData["id"] = Id;
            TempData["item"] = item;
            TempData["order"] = Order.Serialize();
            return RedirectToPage("AddOrderItem");
        }

        public IActionResult OnPostRemoveItemAsync(string item)
        {
            OrderItemModel? i = item.Deserialize(new OrderItemModel());
            if (i is not null) {
                var orderItem = Order.OrderItems?.Find(
                    k => k.Name == i.Name 
                    && k.Quantity == i.Quantity 
                    && k.Unit == i.Unit);

                if (orderItem is not null) {
                    Order.OrderItems?.Remove(orderItem);
                }
            }
            TempData["order"] = Order.Serialize();
            return RedirectToPage(new { Id });
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

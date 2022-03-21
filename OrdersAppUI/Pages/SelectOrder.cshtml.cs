namespace OrdersAppUI.Pages
{
    public class SelectOrderModel : PageModel
    {
        private readonly IOrderData _orderData;
        private readonly IProviderData _providerData;

        public SelectOrderModel(IOrderData orderData, IProviderData providerData)
        { 
            _orderData = orderData;
            _providerData = providerData;
        }

        public List<OrderModel> Orders { get; set; } = new();

        public List<ProviderModel> Providers { get; set; } = new();

        [BindProperty]
        public Filter Filter { get; set; } = new();

        [BindProperty]
        public bool Clear { get; set; } = false;

        public bool AnyFilterValue {
            get => Filter.GetType().GetProperties()
                .Select(p => p.GetValue(Filter)?.ToString())
                .Any(value => !string.IsNullOrEmpty(value));
        }

        public async Task OnGetAsync()
        {
            if (!AnyFilterValue && TempData.ContainsKey("filter"))
            {
                GetFilter();
            }
            await GetProviders();
            await GetOrders();
        }

        public async Task<IActionResult> OnPostAsync(string? id, string? task)
        {
            TempData["id"] = id;
            TempData["task"] = task;
            TempData["modal"] = true;

            GetFilter();
            await GetOrders();

            return RedirectToPage(new { Filter });
        }

        public IActionResult OnPostFilter()
        {
            TempData["filter"] = Filter.Serialize();
            return RedirectToPage();
        }

        public IActionResult OnPostClear()
        {
            Filter = new();
            TempData.Remove("filter");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(
            [FromForm] OrderModel order,
            [FromForm(Name = "ProviderId")] string? providerId)
        {
            if (string.IsNullOrEmpty(providerId)) {
                ModelState.AddModelError("Order.Provider", "Please select a provider");
            }
            if (ModelState.IsValid)
            {
                await GetProviders();
                order.Provider = Providers.Find(p => p.Id == providerId) ?? new();
                order.Date = order.Date.ToLocalTime();
                if (await _orderData.UpdateOrder(order)) {
                    TempData["message"] = $"Order \"{order.Number}\" Updated";
                }
                else {
                    TempData["message"] = "Error Updating Order";
                }
                return RedirectToPage();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromForm]OrderModel order)
        {
            if (await _orderData.DeleteOrder(order.Id)) {
                TempData["message"] = $"Order \"{order.Number}\" Deleted";
                TempData.Remove("orders");
                return RedirectToPage();
            }
            return Page();
        }

        private void GetFilter()
        {
            string? filterData = TempData["filter"] as string;
            if (filterData is not null) {
                Filter = filterData.Deserialize(Filter) ?? new();
            }
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

        private async Task GetOrders(bool cache = false)
        {
            if (cache) {
                string? orderData = TempData["orders"] as string;
                if (orderData is not null) {
                    var p = orderData.Deserialize(Orders);
                    if (p is not null) {
                        Orders = p;
                        return;
                    }
                }
            }
            if (AnyFilterValue) {
                var r = await _orderData.GetFilteredOrders(
                    Filter.OrderNumber,
                    Filter.StartDate, Filter.EndDate,
                    Filter.ProviderId, Filter.ProviderName,
                    Filter.OrderItemName, Filter.OrderItemUnit
                    );
                if (r is not null) {
                    TempData["orders"] = r.Serialize();
                    Orders = r;
                    return;
                }
            }
            var q = await _orderData.GetAllOrders();
            if (q is not null) {
                TempData["orders"] = q.Serialize();
                Orders = q;
            }
            else {
                TempData["message"] = "Error Retrieving Orders";
            }
        }
    }
}
namespace OrdersAppUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProviderData _providerData;
        private readonly IOrderData _orderData;

        public IndexModel(IProviderData providerData, IOrderData orderData)
        {
            _providerData = providerData;
            _orderData = orderData;
        }

        public int ProvidersNumber { get; set; }

        public int OrdersNumber { get; set; }

        public async Task OnGetAsync()
        {
            var Providers = await _providerData.GetAllProviders();
            ProvidersNumber = Providers?.Count() ?? 0;

            var Orders = await _orderData.GetAllOrders();
            OrdersNumber = Orders?.Count() ?? 0;
        }
    }
}
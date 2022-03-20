namespace OrdersAppUI.Models.ViewModels
{
    public class OrderViewModel
    {
        public OrderModel Order { get; set; } = new();
        public List<ProviderModel> Providers { get; set; } = new();
        public string ProviderId {
            get => Order.Provider?.Id ?? string.Empty; set { }
        }
        public string Action { get; set; } = string.Empty;
        public bool ReadOnly { get; set; }
        public string Theme { get; set; } = string.Empty;
        public bool ShowId { get; set; }
        public bool ShowAction { get; set; }
        public string CancelLabel { get; set; } = string.Empty;
    }
}

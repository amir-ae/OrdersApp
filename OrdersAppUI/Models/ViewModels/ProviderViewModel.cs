namespace OrdersAppUI.Models.ViewModels
{
    public class ProviderViewModel
    {
        public ProviderModel Provider { get; set; } = new();
        public string Action { get; set; } = string.Empty;
        public bool ReadOnly { get; set; }
        public string Theme { get; set; } = string.Empty;
        public bool ShowId { get; set; }
        public bool ShowAction { get; set; }
        public string CancelLabel { get; set; } = string.Empty;
    }
}

namespace OrdersAppUI.Models.ViewModels
{
    public class ViewModel<T>
    {
        public T? ModelData { get; set; }
        public string Action { get; set; } = "Create";
        public bool ReadOnly { get; set; } = false;
        public string? Theme { get; set; } = "primary";
        public bool ShowId { get; set; } = true;
        public bool ShowAction { get; set; } = true;
        public string CancelLabel { get; set; } = "Cancel";
    }
}

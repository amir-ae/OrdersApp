namespace OrdersAppUI.Models.ViewModels
{
    public static class ViewModelFactory
    {
        public static ViewModel<T> Create<T>(T data)
        {
            return new ViewModel<T>
            {
                ModelData = data,
                Action = "Create",
                Theme = "primary",
                ShowId = false
            };
        }

        public static ViewModel<T> View<T>(T data)
        {
            return new ViewModel<T>
            {
                ModelData = data,
                Action = "View",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                CancelLabel = "Close"
            };
        }

        public static ViewModel<T> Edit<T>(T data)
        {
            return new ViewModel<T> 
            {
                ModelData = data,
                Action = "Edit",
                Theme = "warning"
            };
        }

        public static ViewModel<T> Delete<T>(T data)
        {
            return new ViewModel<T> 
            {
                ModelData = data,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger"
            };
        }
    }
}
namespace OrdersAppUI.Components;

public class OrderEditorViewComponent : ViewComponent
{
    private readonly IOrderData _orderData;
    private readonly IMapper _mapper;

    public OrderEditorViewComponent(IOrderData orderData, IMapper mapper)
    {
        _orderData = orderData;
        _mapper = mapper;
    }

    public OrderViewModel OrderViewModel { get; set; } = new();

    public async Task<IViewComponentResult> InvokeAsync(string? id, string? task)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(task))
        {
            OrderViewModel = _mapper.Map<OrderViewModel>(ViewModelFactory.Create(new OrderModel()));
        }
        else {
            OrderModel? r = await _orderData.GetOrder(id);
            if (r is not null && !string.IsNullOrEmpty(task))
            {
                switch (task.ToUpperInvariant())
                {
                    case "VIEW":
                        OrderViewModel = _mapper.Map<OrderViewModel>(ViewModelFactory.View(r));
                        break;
                    case "EDIT":
                        OrderViewModel = _mapper.Map<OrderViewModel>(ViewModelFactory.Edit(r));
                        break;
                    case "DELETE":
                        OrderViewModel = _mapper.Map<OrderViewModel>(ViewModelFactory.Delete(r));
                        break;
                };
            }
        }
        return View(OrderViewModel);
    }
}

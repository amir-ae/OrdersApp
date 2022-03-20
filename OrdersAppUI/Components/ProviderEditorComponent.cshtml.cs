namespace OrdersAppUI.Components;

public class ProviderEditorViewComponent : ViewComponent
{
    private readonly IProviderData _providerData;
    private readonly IMapper _mapper;

    public ProviderEditorViewComponent(IProviderData providerData, IMapper mapper)
    {
        _providerData = providerData;
        _mapper = mapper;
    }

    public ProviderViewModel ProviderViewModel { get; set; } = new();

    public async Task<IViewComponentResult> InvokeAsync(string? id, string? task)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(task))
        {
            ProviderViewModel = _mapper.Map<ProviderViewModel>(ViewModelFactory.Create(new ProviderModel()));
        }
        else {
            ProviderModel? p = await _providerData.GetProvider(id);
            if (p is not null && !string.IsNullOrEmpty(task))
            {
                switch (task.ToUpperInvariant())
                {
                    case "VIEW":
                        ProviderViewModel = _mapper.Map<ProviderViewModel>(ViewModelFactory.View(p));
                        break;
                    case "EDIT":
                        ProviderViewModel = _mapper.Map<ProviderViewModel>(ViewModelFactory.Edit(p));
                        break;
                    case "DELETE":
                        ProviderViewModel = _mapper.Map<ProviderViewModel>(ViewModelFactory.Delete(p));
                        break;
                };
            }
        }
        return View(ProviderViewModel);
    }
}

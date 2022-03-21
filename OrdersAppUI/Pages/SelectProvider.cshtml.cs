namespace OrdersAppUI.Pages
{
    public class SelectProviderModel : PageModel
    {
        private readonly IProviderData _providerData;
        private readonly IMapper _mapper;

        public SelectProviderModel(IProviderData providerData, IMapper mapper)
        { 
            _providerData = providerData;
            _mapper = mapper;
        }

        public List<ProviderModel> Providers { get; set; } = new();

        public async Task OnGetAsync(string? task)
        {
            bool create = task?.ToUpper() == "CREATE";
            await GetProviders(create);
        }

        public async Task<IActionResult> OnPostAsync(string? id, string? task)
        {
            TempData["id"] = id;
            TempData["task"] = task;
            TempData["modal"] = true;

            await GetProviders();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCreateAsync(
            [FromForm(Name = "Provider")]Provider target)
        {
            if (ModelState.IsValid)
            {
                var provider = _mapper.Map<ProviderModel>(target);
                var result = await _providerData.CreateProvider(provider);
                if (!string.IsNullOrEmpty(result)) {
                    TempData["message"] = $"Provider \"{target.Name}\" Created";
                }
                else {
                    TempData["message"] = "Error Creating Provider";
                }
                return RedirectToPage();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromForm]ProviderModel provider)
        {
            if (await _providerData.DeleteProvider(provider.Id)) {
                TempData["message"] = $"Provider \"{provider.Name}\" Deleted";
                TempData.Remove("providers");
                return RedirectToPage();
            }
            return Page();
        }

        private async Task GetProviders(bool cache = false)
        {
            if (cache) {
                string? providerData = TempData["providers"] as string;
                if (providerData is not null) {
                    var p = providerData.Deserialize(Providers);
                    if (p is not null) {
                        Providers = p;
                        return;
                    }
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
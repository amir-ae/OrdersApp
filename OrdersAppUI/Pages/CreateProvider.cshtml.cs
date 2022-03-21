namespace OrdersAppUI.Pages
{
    public class CreateProviderModel : PageModel
    {
        private readonly IProviderData _providerData;
        private readonly IMapper _mapper;

        public CreateProviderModel(IProviderData providerData, IMapper mapper) 
        { 
            _providerData = providerData;
            _mapper = mapper;
        }

        public Provider Provider { get; set; } = new Provider();

        public async Task<IActionResult> OnPostAsync(
            [FromForm(Name = "Provider")] Provider target)
        {
            if (ModelState.IsValid)
            {
                var provider = _mapper.Map<ProviderModel>(target);
                var result = await _providerData.CreateProvider(provider);
                if (!string.IsNullOrEmpty(result))
                {
                    TempData["message"] = "Provider Created";
                }
                else 
                {
                    TempData["message"] = "Error Creating Provider";
                }
                return RedirectToPage();
            }
            return Page();
        }
    }
}

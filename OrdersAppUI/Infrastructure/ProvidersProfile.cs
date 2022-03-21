namespace OrdersAppUI.Infrastructure
{
    public class ProvidersProfile : Profile
    {
        public ProvidersProfile()
        {
            CreateMap<Provider, ProviderModel>();

            CreateMap<ViewModel<ProviderModel>, ProviderViewModel>()
                .ForMember(p => p.Provider, op => op.MapFrom(v => v.ModelData ?? new ProviderModel()));
        }
    }
}

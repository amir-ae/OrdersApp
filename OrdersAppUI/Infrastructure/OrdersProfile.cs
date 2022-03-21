namespace OrdersAppUI.Infrastructure
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<Order, OrderModel>();

            CreateMap<OrderModel, Order>();

            CreateMap<OrderItem, OrderItemModel>();

            CreateMap<OrderItemModel, OrderItem>();

            CreateMap<ViewModel<OrderModel>, OrderViewModel>()
                .ForMember(p => p.Order, op => op.MapFrom(v => v.ModelData ?? new OrderModel()))
                .ForMember(p => p.Providers, op => op.MapFrom<ProvidersResolver>());
        }

        public class ProvidersResolver : IValueResolver<ViewModel<OrderModel>, OrderViewModel, List<ProviderModel>>
        {
            private readonly IProviderData _providerData;

            public ProvidersResolver(IProviderData providerData)
            {
                _providerData = providerData;
            }
            public List<ProviderModel> Resolve(ViewModel<OrderModel> source, OrderViewModel dest, List<ProviderModel> member, ResolutionContext context)
            {
                return _providerData.GetAllProviders().GetAwaiter().GetResult() ?? new();
            }
        }
    }
}

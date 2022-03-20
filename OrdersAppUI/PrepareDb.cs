namespace OrdersAppUI;

public static class PrepareDb
{
    public static void EnsurePopulated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        SeedProviders(scope).GetAwaiter().GetResult();
        SeedOrders(scope).GetAwaiter().GetResult();
    }

    private async static Task SeedProviders(IServiceScope scope)
    {
        var providerData = scope.ServiceProvider.GetRequiredService<IProviderData>();
        var providers = await providerData.GetAllProviders();

        if (providers is not null && !providers.Any()) {
            Console.WriteLine("---> Seeding providers data...");
            List<ProviderModel> p = new() {
                new ProviderModel { Name = "Klocko Group" },
                new ProviderModel { Name = "Herzog PLC" },
                new ProviderModel { Name = "Steuber, Considine and Hermann" },
                new ProviderModel { Name = "Considine-Bauch" },
                new ProviderModel { Name = "Stracke Group" }
            };
            await providerData.CreateManyProviders(p);
            Console.WriteLine("---> Providers have been created");
        }
    }

    private async static Task SeedOrders(IServiceScope scope)
    {
        var orderData = scope.ServiceProvider.GetRequiredService<IOrderData>();
        var orders = await orderData.GetAllOrders();

        if (orders is not null && !orders.Any()) {
            Console.WriteLine("---> Seeding orders data...");
            var providerData = scope.ServiceProvider.GetRequiredService<IProviderData>();
            var providers = await providerData.GetAllProviders();

            if (providers is null || providers.Count() < 5) {
                Console.WriteLine("---> Cannot seed orders without providers");
                return;
            }
            List<OrderModel> r = new() {
                new OrderModel {
                    Number = "abc-123",
                    Provider = providers[0],
                    OrderItems = new() { 
                        new OrderItemModel { Name = "Apples", Quantity = 1, Unit = "Kilogram" },
                        new OrderItemModel { Name = "Lifejacket", Quantity = 1 },
                        new OrderItemModel { Name = "Soccer Ball", Quantity = 2 },
                    }
                },
                new OrderModel {
                    Number = "ijk-456",
                    Provider = providers[1],
                    OrderItems = new() {
                        new OrderItemModel { Name = "Chess Board", Quantity = 1 },
                        new OrderItemModel { Name = "Red Fabric", Quantity = 5, Unit = "Meter" },
                    }
                },
                new OrderModel {
                    Number = "xyz-789",
                    Provider = providers[3],
                    OrderItems = new() {
                        new OrderItemModel { Name = "Corner Flags", Quantity = 4 },
                        new OrderItemModel { Name = "Organic Milk", Quantity = 3, Unit = "Liter" },
                    }
                },
            };
            await orderData.CreateManyOrders(r);
            Console.WriteLine("---> Orders have been created");
        }
    }
}

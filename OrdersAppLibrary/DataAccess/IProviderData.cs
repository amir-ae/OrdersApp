
namespace OrdersAppLibrary.DataAccess;

public interface IProviderData
{
    Task<string?> CreateProvider(ProviderModel provider);
    Task<bool> CreateManyProviders(IEnumerable<ProviderModel> providers);
    Task<ProviderModel?> GetProvider(string id);
    Task<List<ProviderModel>?> GetAllProviders();
    Task<bool> DeleteProvider(string id);
}
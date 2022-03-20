namespace OrdersAppLibrary.DataAccess;

public class ProviderData : IProviderData
{
    private const string CacheName = "ProviderData";
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<ProviderModel> _providers;

    public ProviderData(IDbConnection db, IMemoryCache cache)
    {
        _providers = db.ProviderCollection;
        _cache = cache;
    }

    public async Task<ProviderModel?> GetProvider(string id)
    {
        var results = await _providers.FindAsync(r => r.Id == id);
        if (results == null) return null;
        return results.FirstOrDefault();
    }

    public async Task<List<ProviderModel>?> GetAllProviders()
    {
        var providers = _cache.Get<List<ProviderModel>>(CacheName);
        if (providers is null) {
            var results = await _providers.FindAsync(_ => true);
            if (results == null) return null;

            providers = results.ToList();
            _cache.Set(CacheName, providers, TimeSpan.FromHours(1));
        }

        return providers;
    }

    public async Task<string?> CreateProvider(ProviderModel provider)
    {
        try {
            await _providers.InsertOneAsync(provider);
        }
        catch (Exception) {
            return null;
        }
        _cache.Remove(CacheName);
        return provider.Id;
    }

    public async Task<bool> CreateManyProviders(IEnumerable<ProviderModel> providers)
    {
        try {
            await _providers.InsertManyAsync(providers);
        }
        catch (Exception) {
            return false;
        }
        _cache.Remove(CacheName);
        return true;
    }

    public async Task<bool> DeleteProvider(string id)
    {
        try {
            var filter = Builders<ProviderModel>.Filter.Eq("Id", id);
            await _providers.DeleteOneAsync(filter);
        }
        catch (Exception) {
            return false;
        }
        _cache.Remove(CacheName);
        return true;
    }
}

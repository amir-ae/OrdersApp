using Microsoft.Extensions.Configuration;

namespace OrdersAppLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private string _connectionId = "MongoDB";
    public string DbName { get; private set; }
    public string ProviderCollectionName { get; private set; } = "providers";
    public string OrderCollectionName { get; private set; } = "orders";

    public MongoClient Client { get; private set; }
    public IMongoCollection<ProviderModel> ProviderCollection { get; private set; }
    public IMongoCollection<OrderModel> OrderCollection { get; private set; }

    public DbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        ProviderCollection = _db.GetCollection<ProviderModel>(ProviderCollectionName);
        OrderCollection = _db.GetCollection<OrderModel>(OrderCollectionName);
    }
}

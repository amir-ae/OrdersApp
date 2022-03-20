namespace OrdersAppLibrary.DataAccess;

public interface IDbConnection
{
    MongoClient Client { get; }
    string DbName { get; }
    IMongoCollection<OrderModel> OrderCollection { get; }
    string OrderCollectionName { get; }
    IMongoCollection<ProviderModel> ProviderCollection { get; }
    string ProviderCollectionName { get; }
}

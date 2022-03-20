namespace OrdersAppLibrary.DataAccess;

public class OrderData : IOrderData
{
    private const string CacheName = "OrderData";
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<OrderModel> _orders;

    public OrderData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _orders = db.OrderCollection;
    }

    public async Task<List<OrderModel>?> GetAllOrders()
    {
        var orders = _cache.Get<List<OrderModel>>(CacheName);
        if (orders is null) {
            var results = await _orders.FindAsync(_ => true);
            if (results == null) return null;

            orders = results.ToList();
            _cache.Set(CacheName, orders, TimeSpan.FromMinutes(1));
        }

        return orders;
    }

    public async Task<OrderModel?> GetOrder(string id)
    {
        var results = await _orders.FindAsync(r => r.Id == id);
        if (results == null) return null;
        return results.FirstOrDefault();
    }

    public async Task<OrderModel?> GetOrderFromNumber(string number)
    {
        var results = await _orders.FindAsync(r => r.Number == number);
        if (results == null) return null;
        return results.FirstOrDefault();
    }

    public async Task<List<OrderModel>?> GetFilteredOrders(
        string? orderNumber,
        DateTime? startDate, DateTime? endDate,
        string? providerId, string? providerName,
        string? orderItemName, string? orderItemUnit)
    {
        FilterDefinition<OrderModel> combinedFilters = Builders<OrderModel>.Filter.Empty;

        if (orderNumber is not null) {
            var orderNumberFilter = Builders<OrderModel>.Filter.Regex("Number", $"/{orderNumber}/");
            combinedFilters = Builders<OrderModel>.Filter.And(combinedFilters, orderNumberFilter);
        }
        if (startDate is not null) {
            var startDateFilter = Builders<OrderModel>.Filter.Gte("Date", startDate);
            combinedFilters = Builders<OrderModel>.Filter.And(combinedFilters, startDateFilter);
        }
        if (endDate is not null) {
            var endDateFilter = Builders<OrderModel>.Filter.Lte("Date", endDate);
            combinedFilters = Builders<OrderModel>.Filter.And(combinedFilters, endDateFilter);
        }
        if (providerName is not null) {
            var providerNameFilter = Builders<OrderModel>.Filter.Regex("Provider.Name", $"/{providerName}/");
            combinedFilters = Builders<OrderModel>.Filter.And(combinedFilters, providerNameFilter);
        }
        if (orderItemName is not null) {
            var orderItemNameFilter = Builders<OrderModel>.Filter.Regex("OrderItems.Name", $"/{orderItemName}/");
            combinedFilters = Builders<OrderModel>.Filter.And(combinedFilters, orderItemNameFilter);
        }
        if (orderItemUnit is not null) {
            var orderItemUnitFilter = Builders<OrderModel>.Filter.Regex("OrderItems.Unit", $"/{orderItemUnit}/");
            combinedFilters = Builders<OrderModel>.Filter.And(combinedFilters, orderItemUnitFilter);
        }

        var query = await _orders.FindAsync(combinedFilters);

        var orders = query.ToList();

        if (providerId is not null) {
            orders = orders.Where(r => r.Provider.Id.Contains(providerId)).ToList();
        }
        return orders;
    }

    public async Task<bool> CreateManyOrders(IEnumerable<OrderModel> orders)
    {
        try {
            await _orders.InsertManyAsync(orders);
        }
        catch (Exception) {
            return false;
        }
        _cache.Remove(CacheName);
        return true;
    }

    public async Task<bool> CreateOrder(OrderModel order)
    {
        try {
            await _orders.InsertOneAsync(order);
        }
        catch (Exception) {
            return false;
        }
        _cache.Remove(CacheName);
        return true;
    }

    public async Task<bool> UpdateOrder(OrderModel order)
    {
        try {
            var filter = Builders<OrderModel>.Filter.Eq("Id", order.Id);
            await _orders.ReplaceOneAsync(filter, order);
        }
        catch (Exception) {
            return false;
        }

        _cache.Remove(CacheName);
        return true;
    }

    public async Task<bool> DeleteOrder(string id)
    {
        try {
            var filter = Builders<OrderModel>.Filter.Eq("Id", id);
            await _orders.DeleteOneAsync(filter);
        }
        catch (Exception) {
            return false;
        }
        _cache.Remove(CacheName);
        return true;
    }
}


namespace OrdersAppLibrary.DataAccess;

public interface IOrderData
{
    Task<bool> CreateOrder(OrderModel order);
    Task<bool> CreateManyOrders(IEnumerable<OrderModel> orders);
    Task<List<OrderModel>?> GetAllOrders();
    Task<List<OrderModel>?> GetFilteredOrders(
        string? orderNumber,
        DateTime? startDate, DateTime? endDate, 
        string? providerId, string? providerName, 
        string? orderItemName, string? orderItemUnit);
    Task<OrderModel?> GetOrder(string id);
    Task<OrderModel?> GetOrderFromNumber(string number);
    Task<bool> UpdateOrder(OrderModel order);
    Task<bool> DeleteOrder(string id);
}
using UXComex.Domain.Abstract;
using UXComex.Domain.Entities;
using UXComex.Domain.Enums;

namespace UXComex.Domain.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<OrderItem> AddNewOrderItemAsync(OrderItem orderItem);
    Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId);
    Task<bool> RemoveOrderItemAsync(Guid orderItemId);
}
using Dapper;
using System.Text;
using UXComex.Domain.Entities;
using UXComex.Domain.Enums;
using UXComex.Domain.Repositories;
using UXComex.Infra.Connection;

namespace UXComex.Infra.Repositories;

public class OrderRepository(ISqlDbConnection sqlDbConnection) : IOrderRepository
{
    private readonly ISqlDbConnection _sqlDbConnection = sqlDbConnection;

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var query = "SELECT" +
                    "   a.Id, a.ClientId, a.OrderDate, a.TotalAmount, a.Status, " +
                    "   b.ProductId, b.Id, b.OrderId, b.Quantity, b.UnitPrice, " +
                    "   p.Name, p.Id," +
                    "   c.Email, c.Name, c.Id, c.Phone, c.RegisterDate " +
                    "FROM Orders a" +
                    "   LEFT JOIN OrderItems b ON b.OrderId = a.Id" +
                    "   LEFT JOIN Products p ON b.ProductId = p.Id" +
                    "   LEFT JOIN Clients c ON c.Id = a.ClientId" +
                    "   WHERE a.Id = @Id";


        Order orderResult = null;
        var orderItemList = new List<OrderItem>();

        var connection = await _sqlDbConnection.GetConnectionAsync();
        var result = await connection.QueryAsync<Order, OrderItem, Product, Client, Order>(
            sql: query,
            map: (order, orderItem, product, client) =>
            {
                order.RegisterClient(client);
                if (orderResult is null)
                    orderResult = order;

                if (orderItem?.Id is not null)
                {
                    orderItem.RegisterProduct(product);
                    orderItemList.Add(orderItem);
                }
                return order;
            },
            param: new { Id = id },
            splitOn: "ProductId, Name, Email");

        if (orderResult is not null)
            orderResult.OrderItems.AddRange(orderItemList.Where(item => item.OrderId == orderResult.Id));
        
        return orderResult;
    }

    public async Task<IEnumerable<Order>> GetPaginatedAsync(int pageNumber = 1, int itemsPerPage = 10, string? search = "", string? field = "")
    {
        var query = new StringBuilder(
            "SELECT" +
            "   a.Id, a.ClientId, a.OrderDate, a.TotalAmount, a.Status, " +
            "   c.Email, c.Name, c.Id, c.Phone, c.RegisterDate " +
            "FROM Orders a" +
            "   LEFT JOIN Clients c ON c.Id = a.ClientId");

        if (string.IsNullOrWhiteSpace(search) is false)
        {
            query.Append(" WHERE ");
            query.AppendFormat("{0} LIKE CONCAT(@Search,'%') ", field);
        }

        query.Append($" ORDER BY CURRENT_TIMESTAMP OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY ");

        var connection = await _sqlDbConnection.GetConnectionAsync();
        return await connection.QueryAsync<Order, Client, Order>(
            sql: query.ToString(),
            map: (order, client) =>
            {
                order.RegisterClient(client);
                return order;
            },
            param: new { Search = search },
            splitOn: "Email");
    }

    public async Task<int> CountAsync(string? search = "", string? field = "")
    {
        var query = new StringBuilder(
            "SELECT" +
            "   a.Id, a.ClientId, a.OrderDate, a.TotalAmount, a.Status, " +
            "   c.Email, c.Name, c.Id, c.Phone, c.RegisterDate " +
            "FROM Orders a" +
            "   LEFT JOIN Clients c ON c.Id = a.ClientId");

        if (string.IsNullOrWhiteSpace(search) is false)
        {
            query.Append(" WHERE ");
            query.AppendFormat("{0} LIKE CONCAT(@Search,'%') ", field);
        }

        var connection = await _sqlDbConnection.GetConnectionAsync();
        var result = await connection.QueryAsync<Order, Client, Order>(
            sql: query.ToString(),
            map: (order, client) =>
            {
                order.RegisterClient(client);
                return order;
            },
            param: new { Search = search },
            splitOn: "Email");

        return result.Count();
    }

    public async Task<Order> CreateAsync(Order entity)
    {
        var query = "INSERT INTO Orders (ClientId, OrderDate, TotalAmount, Status) " +
                    "OUTPUT Inserted.Id " +
                    "VALUES (@ClientId, @OrderDate, @TotalAmount, @Status)";

        var connection = await _sqlDbConnection.GetConnectionAsync();
        var orderId = await connection.ExecuteScalarAsync(query, entity);

        return await GetByIdAsync(Guid.Parse(orderId.ToString()));
    }

    public async Task<Order> UpdateAsync(Order entity, Guid id)
    {
        var query = "UPDATE Orders SET TotalAmount = @TotalAmount, Status = @Status " +
                    "WHERE Id = @Id";
        var result = await _sqlDbConnection.ExecuteAsync<Order>(query, new
        {
            TotalAmount = entity.TotalAmount,
            Status = entity.Status,
            Id = id
        });
        return result ? entity : null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var query = "DELETE FROM Orders WHERE Id = @Id";
        return await _sqlDbConnection.ExecuteAsync<Order>(query, new { Id = id });
    }

    public async Task<OrderItem> AddNewOrderItemAsync(OrderItem orderItem)
    {
        var query = "INSERT INTO OrderItems (ProductId, OrderId, Quantity, UnitPrice) " +
                    "VALUES (@ProductId, @OrderId, @Quantity, @UnitPrice)";
        var result = await _sqlDbConnection.ExecuteAsync<Order>(query, orderItem);
        return result ? orderItem : null;
    }

    public async Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId)
    {
        var query = "SELECT Id, ProductId, OrderId, Quantity, UnitPrice FROM OrderItems WHERE Id = @Id";
        var result = await _sqlDbConnection.QueryAsync<OrderItem>(query, new { Id = orderItemId });
        return result.FirstOrDefault();
    }

    public async Task<bool> RemoveOrderItemAsync(Guid orderItemId)
    {
        var query = "DELETE FROM OrderItems WHERE Id = @Id";
        return await _sqlDbConnection.ExecuteAsync<OrderItem>(query, new { Id = orderItemId });
    }
}
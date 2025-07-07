using System.Text;
using UXComex.Domain.Entities;
using UXComex.Domain.Repositories;
using UXComex.Infra.Connection;

namespace UXComex.Infra.Repositories;

public class NotificationRepository(ISqlDbConnection sqlDbConnection) : INotificationRepository
{
    private readonly ISqlDbConnection _sqlDbConnection = sqlDbConnection;

    public async Task<Notification?> GetByIdAsync(Guid id)
    {
        var query = "SELECT Id, RegisterDate, Message FROM Notifications WHERE Id = @Id";
        var result = await _sqlDbConnection.QueryAsync<Notification>(query, new { Id = id });
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Notification>> GetPaginatedAsync(int pageNumber = 1, int itemsPerPage = 10, string? search = "", string? field = "")
    {
        var query = new StringBuilder("SELECT Id, RegisterDate, Message FROM Notifications");

        if (string.IsNullOrWhiteSpace(search) is false)
        {
            query.Append(" WHERE ");
            query.AppendFormat("{0} LIKE CONCAT(@Search,'%') ", field);
        }

        query.Append($" ORDER BY CURRENT_TIMESTAMP OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY ");

        var result = await _sqlDbConnection.QueryAsync<Notification>(query.ToString(), new { Search = search });
        return result;
    }

    public Task<int> CountAsync(string? search = "", string? field = "")
    {
        throw new NotImplementedException();
    }

    public async Task<Notification> CreateAsync(Notification entity)
    {
        var query = "INSERT INTO Notifications (RegisterDate, Message) " +
                    "VALUES (@RegisterDate, @Message)";
        var result = await _sqlDbConnection.ExecuteAsync<Notification>(query, entity);
        return result ? entity : null;
    }

    public Task<Notification> UpdateAsync(Notification entity, Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
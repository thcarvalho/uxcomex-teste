using System.Text;
using UXComex.Domain.Entities;
using UXComex.Domain.Repositories;
using UXComex.Infra.Connection;

namespace UXComex.Infra.Repositories;

public class ClientRepository(ISqlDbConnection sqlDbConnection) : IClientRepository
{
    private readonly ISqlDbConnection _sqlDbConnection = sqlDbConnection;

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        var query = "SELECT Id, Name, Email, Phone, RegisterDate FROM Clients WHERE Id = @Id";
        var result = await _sqlDbConnection.QueryAsync<Client>(query, new { Id = id });
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Client>> GetPaginatedAsync(int pageNumber = 1, int itemsPerPage = 10, string? search = "", string? field = "")
    {
        var query = new StringBuilder("SELECT Id, Name, Email, Phone, RegisterDate FROM Clients");

        if (string.IsNullOrWhiteSpace(search) is false)
        {
            query.Append(" WHERE ");
            query.AppendFormat("{0} LIKE CONCAT(@Search,'%') ", field);
        }

        query.Append($" ORDER BY CURRENT_TIMESTAMP OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY ");

        var result = await _sqlDbConnection.QueryAsync<Client>(query.ToString(), new { Search = search });
        return result;
    }

    public async Task<int> CountAsync(string? search = "", string? field = "")
    {
        var query = new StringBuilder("SELECT Id, Name, Email, Phone, RegisterDate FROM Clients");
        if (string.IsNullOrWhiteSpace(search) is false)
        {
            query.Append(" WHERE ");
            query.AppendFormat("{0} LIKE CONCAT(@Search,'%') ", field);
        }
        var result = await _sqlDbConnection.QueryAsync<Client>(query.ToString(), new { Search = search });
        return result.Count();
    }

    public async Task<Client> CreateAsync(Client entity)
    {
        var query = "INSERT INTO Clients (Name, Email, Phone, RegisterDate) " +
                    "VALUES (@Name, @Email, @Phone, @RegisterDate)";
        var result = await _sqlDbConnection.ExecuteAsync<Client>(query, entity);
        return result ? entity : null;
    }

    public async Task<Client> UpdateAsync(Client entity, Guid id)
    {
        var query = "UPDATE Clients SET Name = @Name, Email = @Email, Phone = @Phone " +
                    "WHERE Id = @Id";
        var result = await _sqlDbConnection.ExecuteAsync<Client>(query, new
        {
            Name = entity.Name,
            Email = entity.Email,
            Phone = entity.Phone,
            Id = id
        });
        return result ? entity : null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var query = "DELETE FROM Clients WHERE Id = @Id";
        return await _sqlDbConnection.ExecuteAsync<Client>(query, new { Id = id });
    }

    public async Task<bool> ExistsEmailAsync(string email, string? except = "")
    {
        var query = new StringBuilder("SELECT Email FROM Clients WHERE Email = @Email ");

        if (string.IsNullOrWhiteSpace(except) is false)
            query.Append(" AND (Email <> @Except) ");

        var result = await _sqlDbConnection.QueryAsync<Client>(query.ToString(), new { Email = email, Except = except });
        return result.Any();
    }
}
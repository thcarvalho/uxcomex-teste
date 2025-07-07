using System.Text;
using UXComex.Domain.Entities;
using UXComex.Domain.Repositories;
using UXComex.Infra.Connection;

namespace UXComex.Infra.Repositories;

public class ProductRepository(ISqlDbConnection sqlDbConnection) : IProductRepository
{
    private readonly ISqlDbConnection _sqlDbConnection = sqlDbConnection;
    
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var query = "SELECT Id, Name, Description, Price, QuantityInStock FROM Products WHERE Id = @Id";
        var result = await _sqlDbConnection.QueryAsync<Product>(query, new { Id = id });
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Product>> GetPaginatedAsync(int pageNumber = 1, int itemsPerPage = 10, string? search = "", string? field = "")
    {
        var query = new StringBuilder("SELECT Id, Name, Description, Price, QuantityInStock FROM Products");

        if (string.IsNullOrWhiteSpace(search) is false)
        {
            query.Append(" WHERE ");
            query.AppendFormat("{0} LIKE CONCAT(@Search,'%') ", field);
        }

        query.Append($" ORDER BY CURRENT_TIMESTAMP OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY ");

        var result = await _sqlDbConnection.QueryAsync<Product>(query.ToString(), new { Search = search });
        return result;
    }

    public async Task<int> CountAsync(string? search = "", string? field = "")
    {
        var query = new StringBuilder("SELECT Id, Name, Description, Price, QuantityInStock FROM Products");

        if (string.IsNullOrWhiteSpace(search) is false)
        {
            query.Append(" WHERE ");
            query.AppendFormat("{0} LIKE CONCAT(@Search,'%') ", field);
        }

        var result = await _sqlDbConnection.QueryAsync<Product>(query.ToString(), new { Search = search });
        return result.Count();
    }

    public async Task<Product> CreateAsync(Product entity)
    {
        var query = "INSERT INTO Products (Name, Description, Price, QuantityInStock) " +
                    "VALUES (@Name, @Description, @Price, @QuantityInStock)";
        var result = await _sqlDbConnection.ExecuteAsync<Product>(query, entity);
        return result ? entity : null;
    }

    public async Task<Product> UpdateAsync(Product entity, Guid id)
    {
        var query = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, QuantityInStock = @QuantityInStock " +
                    "WHERE Id = @Id";
        var result = await _sqlDbConnection.ExecuteAsync<Product>(query, new
        {
            Name = entity.Name, 
            Description = entity.Description, 
            Price = entity.Price, 
            QuantityInStock = entity.QuantityInStock, 
            Id = id
        });
        return result ? entity : null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var query = "DELETE FROM Products WHERE Id = @Id";
        return await _sqlDbConnection.ExecuteAsync<Product>(query, new { Id = id });
    }
}
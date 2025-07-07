using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace UXComex.Infra.Connection;

public class SqlDbConnection(IConfiguration configuration) : ISqlDbConnection, IDisposable, IAsyncDisposable
{
    private readonly IConfiguration _configuration = configuration;
    private SqlConnection _connection;

    public async Task<SqlConnection> GetConnectionAsync()
    {
        if (_connection == null)
        {
            var connectionString = _configuration.GetConnectionString("SqlConnection");
            var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();
            _connection = connection;
        }

        return _connection;
    }

    public async Task<bool> ExecuteAsync<T>(string sqlQuery, object param)
    {
        var connection = await GetConnectionAsync();
        return await connection.ExecuteAsync(sqlQuery, param) > 0;
    }

    public async Task<T> QueryFirstAsync<T>(string sqlQuery)
    {
        var connection = await GetConnectionAsync();
        return await connection.QueryFirstAsync<T>(sqlQuery);
    }

    public async Task<T> QueryFirstAsync<T>(string sqlQuery, object param)
    {
        var connection = await GetConnectionAsync();
        return await connection.QueryFirstAsync<T>(sqlQuery, param);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sqlQuery)
    {
        var connection = await GetConnectionAsync();
        return await connection.QueryAsync<T>(sqlQuery);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sqlQuery, object param)
    {
        var connection = await GetConnectionAsync();
        return await connection.QueryAsync<T>(sqlQuery, param);
    }


    public void Dispose()
    {
        if (_connection != null)
        {
            _connection.Dispose();
            _connection = null;
        }

        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }

        GC.SuppressFinalize(this);
    }
}
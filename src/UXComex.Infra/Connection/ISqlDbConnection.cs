using Microsoft.Data.SqlClient;

namespace UXComex.Infra.Connection;

public interface ISqlDbConnection
{
    Task<SqlConnection> GetConnectionAsync();
    Task<bool> ExecuteAsync<T>(string sqlQuery, object param);
    Task<T> QueryFirstAsync<T>(string sqlQuery);
    Task<T> QueryFirstAsync<T>(string sqlQuery, object param);
    Task<IEnumerable<T>> QueryAsync<T>(string sqlQuery);
    Task<IEnumerable<T>> QueryAsync<T>(string sqlQuery, object param);
}
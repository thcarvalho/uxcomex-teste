namespace UXComex.Domain.Abstract;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetPaginatedAsync(int pageNumber = 1, int itemsPerPage = 10, string? search = "", string? field = "");
    Task<int> CountAsync(string? search = "", string? field = "");
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity, Guid id);
    Task<bool> DeleteAsync(Guid id);
}
namespace UXComex.Domain.DTOs.Shared;

public class PaginatedResponse<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; }
    public int ItemsPerPage { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
}
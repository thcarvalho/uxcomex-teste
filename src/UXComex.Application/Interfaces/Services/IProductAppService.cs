using UXComex.Domain.DTOs.Product;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Enums;

namespace UXComex.Application.Interfaces.Services;

public interface IProductAppService
{
    Task<PaginatedResponse<ProductResponseDTO>> GetPaginatedAsync(
        int pageNumber = 1, int itemsPerPage = 10, string? search = "", ProductSearchField? field = null);
    Task<ProductResponseDTO> GetByIdAsync(Guid id);
    Task<ProductResponseDTO> CreateAsync(ProductRequestDTO product);
    Task<ProductResponseDTO> UpdateAsync(ProductRequestDTO product, Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task<ProductResponseDTO> ChangeProductStockAsync(Guid id, int quantity);
}
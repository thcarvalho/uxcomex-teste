using UXComex.Domain.DTOs.Client;
using UXComex.Domain.DTOs.Shared;
using UXComex.Domain.Enums;

namespace UXComex.Application.Interfaces.Services;

public interface IClientAppService
{
    Task<PaginatedResponse<ClientResponseDto>> GetPaginatedAsync(
        int pageNumber = 1, int itemsPerPage = 10, string? search = "", ClientSearchField? field = null);
    Task<ClientResponseDto> GetByIdAsync(Guid id);
    Task<ClientResponseDto> CreateAsync(ClientRequestDTO product);
    Task<ClientResponseDto> UpdateAsync(ClientRequestDTO product, Guid id);
    Task<bool> DeleteAsync(Guid id);
}